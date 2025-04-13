
using System.Net.Http;
using System.Text;
using System.Text.Json;
using ChatClient.Base.Models;

namespace ChatClient.Base.Services;

public class LLMApiClient
{
    private readonly HttpClient _httpClient = new();
    private const string Endpoint = "http://localhost:1234/v1/chat/completions";

    public async Task<(string Message, string Context)> SendMessageAsync(List<ChatMessage> context)
    {
        var messages = context.Select(m => new
        {
            role = m.Speaker switch
            {
                "user" => "user",
                "ai" => "assistant",
                _ => "user"
            },
            content = m.Speaker == "user" ? m.Visible : m.ContextSummary
        }).ToList();

        var payload = new
        {
            model = "mistral-7b",
            messages = messages,
            temperature = 0.3,
            max_tokens = 300,
            response_format = new
            {
                type = "json_schema",
                json_schema = new
                {
                    name = "structured_reply",
                    strict = true,
                    schema = new
                    {
                        type = "object",
                        properties = new
                        {
                            message = new { type = "string" },
                            context = new { type = "string" }
                        },
                        required = new[] { "message", "context" }
                    }
                }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(Endpoint, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseJson);

        //Console.WriteLine($"DEBUG: {responseJson}");

        if (doc.RootElement.TryGetProperty("choices", out var choicesElement) &&
            choicesElement.GetArrayLength() > 0 &&
            choicesElement[0].TryGetProperty("message", out var messageElement) &&
            messageElement.TryGetProperty("content", out var contentElement))
        {
            var innerJson = contentElement.GetString();
            return TryExtractJson(innerJson ?? "");
        }

        return ("[Error: Unexpected response structure]", "");
    }

    private (string Message, string Context) TryExtractJson(string content)
    {
        content = content.Trim();

        if (content.Contains("\n") && content.Contains("\""))
        {
            try
            {
                content = JsonSerializer.Deserialize<string>($"{ content}") ?? content;
            }
            catch
            {
                // use raw string
            }
        }

        int start = content.IndexOf('{');
        int end = content.LastIndexOf('}');
        if (start != -1 && end != -1 && end > start)
        {
            string json = content.Substring(start, end - start + 1);
            try
            {
                var doc = JsonDocument.Parse(json);
                var message = doc.RootElement.GetProperty("message").GetString();
                var context = doc.RootElement.GetProperty("context").GetString();
                return (message ?? "", context ?? "");
            }
            catch (JsonException)
            {
                return ($"[Warning: Invalid JSON extracted]\n{json}", "");
            }
        }

        return ($"[Warning: No JSON block found in response]\n{content}", "");
    }
}

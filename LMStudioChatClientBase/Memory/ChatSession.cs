
using System.Text.Json;
using ChatClient.Base.Models;

namespace ChatClient.Base.Memory;

public class ChatSession
{
    public string SessionId { get; set; } = DateTime.UtcNow.ToString("s").Replace(":", "-");
    public string Model { get; set; } = "mistral-7b";
    public List<ChatMessage> History { get; set; } = new();

    private const string SYSTEM_PROMPT = """
You are a JSON-only assistant.

Every reply you generate MUST be valid JSON. Nothing else.

Your output must contain:
- "message": a full natural language reply to the user
- "context": a short factual summary of your message's core content

Respond ONLY with the JSON object. Do NOT include any:
- greetings
- explanations
- labels like "Response:"
- Markdown or formatting
- extra commentary

✅ GOOD:
{
  "message": "Elephants can run up to 30 km/h.",
  "context": "Elephants reach 30 km/h."
}

❌ BAD:
Hello! Here’s your answer:
{
  "message": "...",
  "context": "..."
}

NO TEXT OUTSIDE THE JSON.
""";


    public ChatSession()
    {
        SessionId = DateTime.UtcNow.ToString("s").Replace(":", "-");
        Model = "mistral-7b";

        History = new List<ChatMessage>
        {
            new ChatMessage("user", SYSTEM_PROMPT)
        };
    }

    public void AddMessage(string speaker, string visible, string contextSummary = "")
    {
        History.Add(new ChatMessage(speaker, visible, contextSummary));
    }

    public List<ChatMessage> GetContextMessages() => History;

    public void Save(string filePath = "last_session.json")
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }


    public static ChatSession Load(string filePath)
    {
        if (!File.Exists(filePath)) return new ChatSession();

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<ChatSession>(json)
            ?? new ChatSession(); // fallback safety
    }


    private string GenerateSummary(string text, string speaker)
    {
        // Placeholder: Replace with actual summarization logic later
        return speaker == "user" ? $"User said something about: {text.Substring(0, Math.Min(50, text.Length))}..." 
                                 : $"AI replied with: {text.Substring(0, Math.Min(50, text.Length))}...";
    }
}

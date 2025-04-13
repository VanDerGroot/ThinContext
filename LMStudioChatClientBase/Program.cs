
using System;
using ChatClient.Base.Services;
using ChatClient.Base.Models;
using ChatClient.Base.Memory;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Chat session started. Type 'exit' to quit.");
        var client = new LLMApiClient();
        var session = ChatSession.Load("last_session.json");

        while (true)
        {
            Console.Write("You: ");
            var userInput = Console.ReadLine();
            if (userInput?.ToLower() == "exit") break;

            session.AddMessage("user", userInput);

            var (replyText, summary) = await client.SendMessageAsync(session.GetContextMessages());
            Console.WriteLine($"AI: {replyText}");
            session.AddMessage("ai", replyText, summary);

        }

        session.Save("last_session.json");
        Console.WriteLine("Session saved. Goodbye!");
    }
}

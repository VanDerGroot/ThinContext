
namespace ChatClient.Base.Models;

public class ChatMessage
{
    public string Speaker { get; set; }
    public string Visible { get; set; }
    public string ContextSummary { get; set; }

    public ChatMessage(string speaker, string visible, string contextSummary = "")
    {
        Speaker = speaker;
        Visible = visible;
        ContextSummary = contextSummary;
    }
}

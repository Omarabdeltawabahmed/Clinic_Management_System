namespace MediCareClinic.ViewModels;
public class AIChatViewModel
{
    public string Message { get; set; } = string.Empty;
    public List<AIChatMessage> Messages { get; set; } = new();
}
public class AIChatMessage { public string Role { get; set; } = string.Empty; public string Content { get; set; } = string.Empty; }

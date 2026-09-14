using System.Text.Json;
using MediCareClinic.Services;
using MediCareClinic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCareClinic.Controllers;

[Authorize(Roles = "Patient,Admin")]
public class AIController : Controller
{
    private const string SessionKey = "MediCareAIChat";
    private readonly ClinicAssistantService _assistant;
    public AIController(ClinicAssistantService assistant) => _assistant = assistant;
    [HttpGet] public IActionResult Index() => View(new AIChatViewModel { Messages = Load() });
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Ask(AIChatViewModel model)
    {
        var messages = Load();
        if (!string.IsNullOrWhiteSpace(model.Message))
        {
            messages.Add(new AIChatMessage { Role = "user", Content = model.Message.Trim() });
            messages.Add(new AIChatMessage { Role = "assistant", Content = await _assistant.AskAsync(model.Message.Trim()) });
            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(messages));
        }
        return View("Index", new AIChatViewModel { Messages = messages });
    }
    [HttpPost] public IActionResult Clear() { HttpContext.Session.Remove(SessionKey); return RedirectToAction(nameof(Index)); }
    private List<AIChatMessage> Load() => JsonSerializer.Deserialize<List<AIChatMessage>>(HttpContext.Session.GetString(SessionKey) ?? "[]") ?? new();
}

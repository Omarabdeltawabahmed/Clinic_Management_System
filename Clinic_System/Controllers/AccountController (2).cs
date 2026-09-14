using MediCareClinic.Models;
using MediCareClinic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MediCareClinic.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) { _userManager = userManager; _signInManager = signInManager; }
    [HttpGet] public IActionResult Login(string? returnUrl = null) { ViewBag.ReturnUrl = returnUrl; return View(); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && (await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false)).Succeeded) return LocalRedirect(returnUrl ?? Url.Action("Index", "Home")!);
        ModelState.AddModelError(string.Empty, "Invalid email or password."); return View(model);
    }
    [HttpGet] public IActionResult Register() => View();
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName, BirthDate = model.BirthDate, BloodType = model.BloodType, ChronicConditions = model.ChronicConditions, Allergies = model.Allergies };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded) { await _userManager.AddToRoleAsync(user, "Patient"); await _signInManager.SignInAsync(user, false); TempData["Success"] = "Welcome to MediCare Clinic. Your account is ready."; return RedirectToAction("Index", "Home"); }
        foreach (var error in result.Errors) ModelState.AddModelError(nameof(model.Email), error.Description); return View(model);
    }
    [Authorize]
    [HttpGet] public IActionResult ChangePassword() => View(new ChangePasswordViewModel());
    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded) { await _signInManager.RefreshSignInAsync(user); TempData["Success"] = "Your password has been changed successfully."; return RedirectToAction("Index", "Home"); }
        foreach (var error in result.Errors) ModelState.AddModelError(nameof(model.CurrentPassword), error.Description); return View(model);
    }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Logout() { await _signInManager.SignOutAsync(); return RedirectToAction("Index", "Home"); }
}

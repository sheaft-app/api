using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
#pragma warning disable CS8618

namespace Sheaft.Web.Api.Pages;

[Authorize]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class Logout : PageModel
{
    public Logout()
    {
    }

    public string Message { get; set; }
    
    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == false)
            return RedirectToPage("Login");
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("Login");
    }
}
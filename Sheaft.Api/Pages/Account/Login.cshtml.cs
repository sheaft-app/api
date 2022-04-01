using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Api.Pages;

[AllowAnonymous]
public class Login : PageModel
{
    private readonly ISheaftMediator _mediator;
    private readonly IAccountRepository _accountRepository;

    public Login(ISheaftMediator mediator, IAccountRepository accountRepository)
    {
        _mediator = mediator;
        _accountRepository = accountRepository;
    }

    [BindProperty] 
    public string UserName { get; set; }

    [BindProperty, DataType(DataType.Password)]
    public string Password { get; set; }
    public string Message { get; set; }
    
    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToPage("/hangfire");
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await _mediator.Execute(new LoginUserCommand(UserName, Password), CancellationToken.None);
        if (result.IsFailure)
        {
            Message = result.Error.Message;
            return Page();
        }

        var accountResult = await _accountRepository.Get(new Username(UserName), CancellationToken.None);
        if(accountResult.IsFailure)
        {
            Message = accountResult.Error.Message;
            return Page();
        }
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, accountResult.Value.Identifier.Value),
            new Claim(ClaimTypes.Name, accountResult.Value.Username.Value),
            new Claim(ClaimTypes.Email, accountResult.Value.Email.Value),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(ClaimTypes.NameIdentifier, accountResult.Value.Identifier.Value),
        };
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
        
        return Redirect("/hangfire");
    }
}
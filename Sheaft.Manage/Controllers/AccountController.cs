using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Core.Extensions;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.ViewModels;

namespace Sheaft.Manage.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configurationProvider;

        public AccountController(
            IAppDbContext context,
            IMapper mapper,
            IConfigurationProvider configurationProvider,
            ILogger<AccountController> logger)
        {
            _context = context;
            _mapper = mapper;
            _configurationProvider = configurationProvider;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Impersonate(CancellationToken token)
        {
            var id = HttpContext.Request.ImpersonificationId();
            if (id.HasValue)
            {
                ViewBag.User = await _context.Users
                    .Where(c => c.Id == id.Value)
                    .ProjectTo<UserViewModel>(_configurationProvider)
                    .SingleOrDefaultAsync(token);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImpersonateByEmail(string email, CancellationToken token)
        {
            var users = await _context.Users
                .Where(c => 
                    (c.Email.Contains(email) && c.UserType == Interop.Enums.UserKind.Consumer) || 
                    (c.Company != null && c.Company.Email.Contains(email) && c.UserType == Interop.Enums.UserKind.Owner))
                .ProjectTo<UserViewModel>(_configurationProvider)
                .ToListAsync(token);

            return View("Impersonate", users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImpersonateById(Guid id, CancellationToken token)
        {
            var user = await _context.Users.SingleOrDefaultAsync(c => c.Id == id, token);
            AddImpersonate(user.Id, user.Company != null ? user.Company.Name : $"{user.FirstName} {user.LastName}");
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveImpersonification()
        {
            RemoveImpersonate();
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task Logout()
        {
            RemoveImpersonate();
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        private void AddImpersonate(Guid id, string name)
        {
            HttpContext.Response.Cookies.Append("Sheaft.Impersonating.Id", id.ToString("N"), new CookieOptions { Secure = true, HttpOnly = true, SameSite = SameSiteMode.Strict });
            HttpContext.Response.Cookies.Append("Sheaft.Impersonating.Name", name, new CookieOptions { Secure = true, HttpOnly = true, SameSite = SameSiteMode.Strict });
        }

        private void RemoveImpersonate()
        {
            HttpContext.Response.Cookies.Delete("Sheaft.Impersonating.Id");
            HttpContext.Response.Cookies.Delete("Sheaft.Impersonating.Name");
        }
    }
}

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
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Web.Manage.Extensions;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppDbContext _context;
        private readonly IConfigurationProvider _configurationProvider;

        public AccountController(
            IAppDbContext context,
            IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
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
                ViewBag.User = await _context.Users.OfType<User>()
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
                .Where(c => c.Email.Contains(email))
                .ProjectTo<UserViewModel>(_configurationProvider)
                .ToListAsync(token);

            if (users.Count == 1)
            {
                var user = users.First();
                AddImpersonate(user.Id, user.Name);
                var origin = Request.Headers.FirstOrDefault(c => c.Key.ToLower() == "referer").Value;
                return Redirect(origin);
            }

            return View("Impersonate", users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImpersonateById(Guid id, CancellationToken token)
        {
            var user = await _context.Users.OfType<User>().SingleOrDefaultAsync(c => c.Id == id, token);
            AddImpersonate(user.Id, user.Name);

            var origin = Request.Headers.FirstOrDefault(c => c.Key.ToLower() == "referer").Value;
            return Redirect(origin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveImpersonification()
        {
            RemoveImpersonate();
            var origin = Request.Headers.FirstOrDefault(c => c.Key.ToLower() == "referer").Value;
            return Redirect(origin);
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
            HttpContext.Response.Cookies.Append("Sheaft.Impersonating.Id", id.ToString("N"),
                new CookieOptions {Secure = true, HttpOnly = true, SameSite = SameSiteMode.Strict});
            HttpContext.Response.Cookies.Append("Sheaft.Impersonating.Name", name,
                new CookieOptions {Secure = true, HttpOnly = true, SameSite = SameSiteMode.Strict});
        }

        private void RemoveImpersonate()
        {
            HttpContext.Response.Cookies.Delete("Sheaft.Impersonating.Id");
            HttpContext.Response.Cookies.Delete("Sheaft.Impersonating.Name");
        }
    }
}
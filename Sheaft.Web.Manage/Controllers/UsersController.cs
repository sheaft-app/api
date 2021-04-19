using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Setting.Commands;
using Sheaft.Mediatr.UserSetting.Commands;
using Sheaft.Options;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Controllers
{
    public class UsersController : ManageController
    {
        public UsersController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider)
            : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Users.OfType<User>()
                .AsNoTracking()
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            var controller = string.Empty;
            switch (entity.Kind)
            {
                case ProfileKind.Consumer:
                    controller = "Consumers";
                    break;
                case ProfileKind.Producer:
                    controller = "Producers";
                    break;
                case ProfileKind.Store:
                    controller = "Stores";
                    break;
            }

            return RedirectToAction("Edit", controller, new {id});
        }

        [HttpGet]
        public IActionResult AddSetting(Guid userId, CancellationToken token)
        {
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSetting(Guid userId, SettingKind settingKind, string value, CancellationToken token)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Kind == settingKind, token);
            var result =
                await _mediatr.Process(
                    new AddUserSettingCommand(await GetRequestUserAsync(token))
                        {Value = value, SettingId = setting.Id, UserId = userId}, token);
            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id = userId});
        }

        [HttpGet]
        public async Task<IActionResult> EditSetting(Guid userId, Guid settingId, CancellationToken token)
        {
            ViewBag.UserId = userId;
            var entity = await _context.Users.SingleAsync(e => e.Id == userId, token);
            var userSetting = entity.GetSetting(settingId);

            return View(_mapper.Map<UserSettingViewModel>(userSetting));
        }

        [HttpPost]
        public async Task<IActionResult> EditSetting(Guid userId, Guid settingId, string value, CancellationToken token)
        {
            var result =
                await _mediatr.Process(
                    new UpdateUserSettingCommand(await GetRequestUserAsync(token))
                        {Value = value, SettingId = settingId, UserId = userId}, token);
            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id = userId});
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSetting(Guid userId, Guid settingId, CancellationToken token)
        {
            var result =
                await _mediatr.Process(
                    new RemoveUserSettingCommand(await GetRequestUserAsync(token))
                        {SettingId = settingId, UserId = userId}, token);
            if (!result.Succeeded)
                throw result.Exception;

            return RedirectToAction("Edit", new {id = userId});
        }
    }
}
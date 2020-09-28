using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Application.Interop;
using Sheaft.Manage.Models;
using Sheaft.Application.Models;
using Sheaft.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Enums;

namespace Sheaft.Manage.Controllers
{
    public class UsersController : ManageController
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<UsersController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Users.OfType<User>()
                .AsNoTracking()
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

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

            return RedirectToAction("Edit", controller, new { id = id });
        }
    }
}

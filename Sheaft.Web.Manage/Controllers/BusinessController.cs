using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Enums;
using Sheaft.Exceptions;

namespace Sheaft.Web.Manage.Controllers
{
    public class BusinessController : ManageController
    {
        private readonly ILogger<BusinessController> _logger;

        public BusinessController(
            IAppDbContext context,
            IMapper mapper,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IConfigurationProvider configurationProvider,
            ILogger<BusinessController> logger) : base(context, mapper, roleOptions, mediatr, configurationProvider)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw new NotFoundException();

            if (entity.Kind == ProfileKind.Consumer)
                return RedirectToAction("Edit", "Consumers", new { id });

            if (entity.Kind == ProfileKind.Producer)
                return RedirectToAction("Edit", "Producers", new { id });

            if (entity.Kind == ProfileKind.Store)
                return RedirectToAction("Edit", "Stores", new { id });

            return RedirectToAction("Index", "Dashboard");
        }
    }
}

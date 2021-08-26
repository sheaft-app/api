using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core.Exceptions;
using Sheaft.Core.Options;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Controllers
{
    public class BusinessController : ManageController
    {
        public BusinessController(
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
            var entity = await _context.Users
                .AsNoTracking()
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync(token);

            if (entity == null)
                throw SheaftException.NotFound();

            if (entity.Kind == ProfileKind.Consumer)
                return RedirectToAction("Edit", "Consumers", new {id});

            if (entity.Kind == ProfileKind.Producer)
                return RedirectToAction("Edit", "Producers", new {id});

            if (entity.Kind == ProfileKind.Store)
                return RedirectToAction("Edit", "Stores", new {id});

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Controllers
{
    public class ManageController : Controller
    {
        protected readonly IAppDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly ISheaftMediatr _mediatr;
        protected readonly RoleOptions _roleOptions;
        protected readonly IConfigurationProvider _configurationProvider;

        public ManageController(
            IAppDbContext context,
            IMapper mapper,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ISheaftMediatr mediatr,
            IConfigurationProvider configurationProvider)
        {
            _context = context;
            _mapper = mapper;
            _mediatr = mediatr;
            _roleOptions = roleOptions.Value;
            _configurationProvider = configurationProvider;
        }

        protected async Task<List<TagViewModel>> GetTags(CancellationToken token)
        {
            return await _context.Tags
                .AsNoTracking()
                .Where(t => !t.RemovedOn.HasValue)
                .ProjectTo<TagViewModel>(_configurationProvider)
                .ToListAsync(token);
        }

        protected async Task<RequestUser> GetRequestUser(CancellationToken token)
        {
            var requestUser = User.ToIdentityUser(HttpContext.TraceIdentifier);

            var cookie = HttpContext.Request.ImpersonificationId();
            if (cookie.HasValue)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == cookie.Value, token);
                if (user != null)
                {
                    var roles = user.Kind == ProfileKind.Consumer
                        ? new List<string>()
                        {
                            _roleOptions.Consumer.Value
                        }
                        : new List<string>()
                        {
                            _roleOptions.Owner.Value,
                            user.Kind == ProfileKind.Producer ? _roleOptions.Producer.Value : _roleOptions.Store.Value
                        };

                    var uid = User.TryGetUserId();
                    Impersonification impersonification = null;
                    if (uid.HasValue)
                        impersonification = new Impersonification(uid.Value, User.GetName());

                    requestUser = new RequestUser(user.Id, user.Name, user.Email, roles, HttpContext.TraceIdentifier,
                        impersonification);
                }
            }

            return requestUser;
        }

        protected async Task<IEnumerable<CountryViewModel>> GetCountries(CancellationToken token)
        {
            return await _context.Countries.AsNoTracking().ProjectTo<CountryViewModel>(_configurationProvider)
                .ToListAsync(token);
        }

        protected async Task<IEnumerable<NationalityViewModel>> GetNationalities(CancellationToken token)
        {
            return await _context.Nationalities.AsNoTracking().ProjectTo<NationalityViewModel>(_configurationProvider)
                .ToListAsync(token);
        }
    }
}
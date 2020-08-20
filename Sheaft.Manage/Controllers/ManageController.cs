using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Core.Extensions;
using Sheaft.Core.Security;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Models.ViewModels;
using Sheaft.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Manage.Controllers
{
    public class ManageController : Controller
    {
        protected readonly IAppDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IMediator _mediatr;
        protected readonly RoleOptions _roleOptions;
        protected readonly IConfigurationProvider _configurationProvider;

        public ManageController(
            IAppDbContext context,
            IMapper mapper,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IMediator mediatr,
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
                if(user != null)
                {
                    var name = user.Company != null ? user.Company.Name : $"{user.FirstName} {user.LastName}";
                    var email = user.Company != null ? user.Company.Email : user.Email;
                    var roles = user.Company != null ? 
                        new List<string>()
                        {
                            _roleOptions.Owner.Value,
                            user.Company.Kind == ProfileKind.Producer ? _roleOptions.Producer.Value : _roleOptions.Store.Value
                        } :
                        new List<string>()
                        {
                            _roleOptions.Consumer.Value
                        };

                    requestUser = new RequestUser(user.Id, name, email, roles, user.Company?.Id, HttpContext.TraceIdentifier, User.TryGetUserId(), User.GetName());
                }
            }

            return requestUser;
        }

    }
}

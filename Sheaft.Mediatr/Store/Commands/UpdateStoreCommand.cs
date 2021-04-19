using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Auth.Commands;
using Sheaft.Mediatr.BusinessClosing.Commands;
using Sheaft.Mediatr.User.Commands;
using Sheaft.Options;

namespace Sheaft.Mediatr.Store.Commands
{
    public class UpdateStoreCommand : Command
    {
        protected UpdateStoreCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateStoreCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid StoreId { get; set; }
        public ProfileKind? Kind { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public IEnumerable<TimeSlotGroupDto> OpeningHours { get; set; }
    }

    public class UpdateStoreCommandHandler : CommandsHandler,
        IRequestHandler<UpdateStoreCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public UpdateStoreCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<UpdateStoreCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(UpdateStoreCommand request, CancellationToken token)
        {
            var store = await _context.Stores.SingleAsync(e => e.Id == request.StoreId, token);
            if(store.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            store.SetName(request.Name);
            store.SetFirstname(request.FirstName);
            store.SetLastname(request.LastName);
            store.SetEmail(request.Email);
            store.SetProfileKind(request.Kind);
            store.SetPhone(request.Phone);
            store.SetOpenForNewBusiness(request.OpenForNewBusiness);
            
            store.SetSummary(request.Summary);
            store.SetDescription(request.Description);
            store.SetFacebook(request.Facebook);
            store.SetTwitter(request.Twitter);
            store.SetWebsite(request.Website);
            store.SetInstagram(request.Instagram);

            var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
            var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

            store.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                request.Address.City, request.Address.Country, department, request.Address.Longitude,
                request.Address.Latitude);

            if (request.Tags != null)
            {
                var tags = await _context.Tags.Where(t => request.Tags.Contains(t.Id)).ToListAsync(token);
                store.SetTags(tags);
            }

            if (request.OpeningHours != null)
            {
                var openingHours = new List<TimeSlotHour>();
                foreach (var oh in request.OpeningHours)
                {
                    openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                }

                store.SetOpeningHours(openingHours);
            }

            await _context.SaveChangesAsync(token);
            
            var resultImage = await _mediatr.Process(new UpdateUserPreviewCommand(request.RequestUser)
            {
                UserId = store.Id,
                Picture = request.Picture,
                SkipAuthUpdate = true
            }, token);

            if (!resultImage.Succeeded)
                return Failure(resultImage);

            var roles = new List<Guid> {_roleOptions.Owner.Id, _roleOptions.Store.Id};
            var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
            {
                Email = store.Email,
                FirstName = store.FirstName,
                LastName = store.LastName,
                Name = store.Name,
                Phone = store.Phone,
                Picture = store.Picture,
                Roles = roles,
                UserId = store.Id
            }, token);

            if (!authResult.Succeeded)
                return authResult;

            return Success();
        }
    }
}
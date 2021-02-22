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
using Sheaft.Application.Auth.Commands;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Options;
using Sheaft.Application.Picture.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Store.Commands
{
    public class UpdateStoreCommand : Command
    {
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
        public bool OpenForNewBusiness { get; set; }
        public FullAddressInput Address { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
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
            var store = await _context.GetByIdAsync<Domain.Store>(request.StoreId, token);
            if(store.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            store.SetName(request.Name);
            store.SetFirstname(request.FirstName);
            store.SetLastname(request.LastName);
            store.SetEmail(request.Email);
            store.SetProfileKind(request.Kind);
            store.SetPhone(request.Phone);
            store.SetOpenForNewBusiness(request.OpenForNewBusiness);

            var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
            var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

            store.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                request.Address.City, request.Address.Country, department, request.Address.Longitude,
                request.Address.Latitude);

            if (request.Tags != null)
            {
                var tags = await _context.FindAsync<Domain.Tag>(t => request.Tags.Contains(t.Id), token);
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

            var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
            {
                UserId = store.Id,
                Picture = request.Picture,
                SkipAuthUpdate = true
            }, token);

            if (!resultImage.Succeeded)
                return Failure(resultImage.Exception);

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
﻿using System;
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
using Sheaft.Application.Legal.Commands;
using Sheaft.Application.Sponsor.Commands;
using Sheaft.Application.User.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Store.Commands
{
    public class RegisterStoreCommand : Command<Guid>
    {
        [JsonConstructor]
        public RegisterStoreCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid StoreId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public string SponsoringCode { get; set; }
        public FullAddressInput Address { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public BusinessLegalInput Legals { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }

    public class RegisterStoreCommandHandler : CommandsHandler,
        IRequestHandler<RegisterStoreCommand, Result<Guid>>
    {
        private readonly RoleOptions _roleOptions;

        public RegisterStoreCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<RegisterStoreCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result<Guid>> Handle(RegisterStoreCommand request, CancellationToken token)
        {
            var store = await _context.FindByIdAsync<Domain.Store>(request.StoreId, token);
            if (store != null)
                return Failure<Guid>(MessageKind.Register_User_AlreadyExists);

            var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
            var department =
                await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

            var address = request.Address != null
                ? new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country,
                    department, request.Address.Longitude, request.Address.Latitude)
                : null;

            var openingHours = new List<TimeSlotHour>();
            if (request.OpeningHours != null)
            {
                foreach (var oh in request.OpeningHours)
                {
                    openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                }
            }

            store = new Domain.Store(request.StoreId, request.Name, request.FirstName, request.LastName,
                request.Email,
                address, openingHours, request.OpenForNewBusiness, request.Phone);

            if (request.Tags != null && request.Tags.Any())
            {
                var tags = await _context.FindAsync<Domain.Tag>(t => request.Tags.Contains(t.Id), token);
                store.SetTags(tags);
            }

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                await _context.AddAsync(store, token);
                await _context.SaveChangesAsync(token);

                var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
                {
                    UserId = store.Id,
                    Picture = request.Picture,
                    SkipAuthUpdate = true
                }, token);

                if (!resultImage.Succeeded)
                    return Failure<Guid>(resultImage.Exception);

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
                    return Failure<Guid>(authResult.Exception);

                var result = await _mediatr.Process(new CreateBusinessLegalCommand(request.RequestUser)
                {
                    Address = request.Legals.Address,
                    Name = request.Legals.Name,
                    Email = request.Legals.Email,
                    Siret = request.Legals.Siret,
                    Kind = request.Legals.Kind,
                    VatIdentifier = request.Legals.VatIdentifier,
                    UserId = store.Id,
                    Owner = request.Legals.Owner
                }, token);

                if (!result.Succeeded)
                    return result;

                await transaction.CommitAsync(token);

                if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                {
                    _mediatr.Post(new CreateSponsoringCommand(request.RequestUser)
                        {Code = request.SponsoringCode, UserId = store.Id});
                }

                return Success(store.Id);
            }
        }
    }
}
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
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Auth.Commands;
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
        public IEnumerable<PictureInputDto> Pictures { get; set; }
        public IEnumerable<TimeSlotGroupDto> OpeningHours { get; set; }
    }

    public class UpdateStoreCommandHandler : CommandsHandler,
        IRequestHandler<UpdateStoreCommand, Result>
    {
        private readonly IPictureService _imageService;
        private readonly RoleOptions _roleOptions;

        public UpdateStoreCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IPictureService imageService,
            ILogger<UpdateStoreCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(UpdateStoreCommand request, CancellationToken token)
        {
            var entity = await _context.Stores.SingleAsync(e => e.Id == request.StoreId, token);
            if(entity.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            entity.SetName(request.Name);
            entity.SetFirstname(request.FirstName);
            entity.SetLastname(request.LastName);
            entity.SetEmail(request.Email);
            entity.SetProfileKind(request.Kind);
            entity.SetPhone(request.Phone);
            entity.SetOpenForNewBusiness(request.OpenForNewBusiness);
            
            entity.SetSummary(request.Summary);
            entity.SetDescription(request.Description);
            entity.SetFacebook(request.Facebook);
            entity.SetTwitter(request.Twitter);
            entity.SetWebsite(request.Website);
            entity.SetInstagram(request.Instagram);

            var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
            var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

            entity.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                request.Address.City, request.Address.Country, department, request.Address.Longitude,
                request.Address.Latitude);

            if (request.Tags != null)
            {
                var tags = await _context.Tags.Where(t => request.Tags.Contains(t.Id)).ToListAsync(token);
                entity.SetTags(tags);
            }

            if (request.OpeningHours != null)
            {
                var openingHours = new List<OpeningHours>();
                foreach (var oh in request.OpeningHours)
                    openingHours.AddRange(oh.Days.Select(c => new OpeningHours(c, oh.From, oh.To)));

                entity.SetOpeningHours(openingHours);
            }

            if (request.Pictures != null && request.Pictures.Any())
            {
                entity.ClearPictures();
                    
                var result = Success<string>();
                foreach (var picture in request.Pictures)
                {
                    var id = Guid.NewGuid();
                    result = await _imageService.HandleUserPictureAsync(entity, id, picture.Data, token);
                    if (!result.Succeeded)
                        break;

                    if (!string.IsNullOrWhiteSpace(result.Data))
                        entity.AddPicture(new ProfilePicture(id, result.Data, picture.Position));
                }

                if (!result.Succeeded)
                    return Failure(result);
            }
            
            await _context.SaveChangesAsync(token);
            
            var resultImage = await _mediatr.Process(new UpdateUserPreviewCommand(request.RequestUser)
            {
                UserId = entity.Id,
                Picture = request.Picture,
                SkipAuthUpdate = true
            }, token);

            if (!resultImage.Succeeded)
                return Failure(resultImage);

            var roles = new List<Guid> {_roleOptions.Owner.Id, _roleOptions.Store.Id};
            var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
            {
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Name = entity.Name,
                Phone = entity.Phone,
                Picture = entity.Picture,
                Roles = roles,
                UserId = entity.Id
            }, token);

            if (!authResult.Succeeded)
                return authResult;

            return Success();
        }
    }
}
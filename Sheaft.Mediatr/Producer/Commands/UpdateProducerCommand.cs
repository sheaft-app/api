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

namespace Sheaft.Mediatr.Producer.Commands
{
    public class UpdateProducerCommand : Command
    {
        protected UpdateProducerCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateProducerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
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
        public bool? NotSubjectToVat { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        
    }

    public class UpdateProducerCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProducerCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public UpdateProducerCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<UpdateProducerCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(UpdateProducerCommand request, CancellationToken token)
        {
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);
            if(producer.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            producer.SetName(request.Name);
            producer.SetFirstname(request.FirstName);
            producer.SetLastname(request.LastName);
            producer.SetEmail(request.Email);
            producer.SetProfileKind(request.Kind);
            producer.SetPhone(request.Phone);
            producer.SetOpenForNewBusiness(request.OpenForNewBusiness);
            
            producer.SetSummary(request.Summary);
            producer.SetDescription(request.Description);
            producer.SetFacebook(request.Facebook);
            producer.SetTwitter(request.Twitter);
            producer.SetWebsite(request.Website);
            producer.SetInstagram(request.Instagram);

            if (request.NotSubjectToVat.HasValue)
            {
                producer.SetNotSubjectToVat(request.NotSubjectToVat.Value);
            }

            var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
            var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

            producer.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                request.Address.City, request.Address.Country, department, request.Address.Longitude,
                request.Address.Latitude);

            if (request.Tags != null)
            {
                var tags = await _context.Tags.Where(t => request.Tags.Contains(t.Id)).ToListAsync(token);
                producer.SetTags(tags);
            }

            await _context.SaveChangesAsync(token);
            
            var resultImage = await _mediatr.Process(new UpdateUserPreviewCommand(request.RequestUser)
            {
                UserId = producer.Id,
                Picture = request.Picture,
                SkipAuthUpdate = true
            }, token);

            if (!resultImage.Succeeded)
                return Failure(resultImage);

            var roles = new List<Guid> {_roleOptions.Owner.Id, _roleOptions.Producer.Id};
            var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
            {
                Email = producer.Email,
                FirstName = producer.FirstName,
                LastName = producer.LastName,
                Name = producer.Name,
                Phone = producer.Phone,
                Picture = producer.Picture,
                Roles = roles,
                UserId = producer.Id
            }, token);

            if (!authResult.Succeeded)
                return authResult;

            return Success();
        }
    }
}
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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Auth.Commands;
using Sheaft.Mediatr.BatchDefinition.Commands;
using Sheaft.Mediatr.Catalog.Commands;
using Sheaft.Mediatr.Legal.Commands;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.Mediatr.Producer.Commands
{
    public class RegisterProducerCommand : Command<Guid>
    {
        protected RegisterProducerCommand()
        {
        }

        [JsonConstructor]
        public RegisterProducerCommand(RequestUser requestUser) : base(requestUser)
        {
            ProducerId = RequestUser.Id;
        }

        public Guid ProducerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public string SponsoringCode { get; set; }
        public BusinessLegalInputDto Legals { get; set; }
        public AddressDto Address { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public bool NotSubjectToVat { get; set; }
        public IEnumerable<Guid> Tags { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            ProducerId = RequestUser.Id;
        }
    }

    public class RegisterProducerCommandHandler : CommandsHandler,
        IRequestHandler<RegisterProducerCommand, Result<Guid>>
    {
        private readonly RoleOptions _roleOptions;

        public RegisterProducerCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<RegisterProducerCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result<Guid>> Handle(RegisterProducerCommand request, CancellationToken token)
        {
            var producer =
                await _context.Producers.SingleOrDefaultAsync(
                    r => r.Id == request.ProducerId || r.Email == request.Email, token);

            if (producer is { RemovedOn: null })
                return Failure<Guid>("Un compte existe déjà avec ces informations.");

            var address = await GetAddressAsync(request.Address, token);
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                if (producer != null)
                {
                    _context.Restore(producer);
                }
                else
                {
                    producer = new Domain.Producer(request.ProducerId, request.Name, request.FirstName,
                        request.LastName,
                        request.Email,
                        address, request.OpenForNewBusiness, request.Phone);

                    await _context.AddAsync(producer, token);
                }

                producer.SetNotSubjectToVat(request.NotSubjectToVat);
                producer.SetAddress(address);
                await _context.SaveChangesAsync(token);

                if (request.Tags != null && request.Tags.Any())
                {
                    var tags = await _context.Tags.Where(t => request.Tags.Contains(t.Id)).ToListAsync(token);
                    producer.SetTags(tags);
                }

                var resultImage = await _mediatr.Process(new UpdateUserPreviewCommand(request.RequestUser)
                {
                    UserId = producer.Id,
                    Picture = request.Picture,
                    SkipAuthUpdate = true
                }, token);

                if (!resultImage.Succeeded)
                    return Failure<Guid>(resultImage);

                var roles = new List<Guid> { _roleOptions.Owner.Id, _roleOptions.Producer.Id };
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
                    return Failure<Guid>(authResult);

                var result = await _mediatr.Process(new CreateBusinessLegalCommand(request.RequestUser)
                {
                    Billing = request.Legals.Billing,
                    Address = request.Legals.Address,
                    Name = request.Legals.Name,
                    Email = request.Legals.Email,
                    Siret = request.Legals.Siret,
                    Kind = request.Legals.LegalKind,
                    VatIdentifier = request.NotSubjectToVat ? null : request.Legals.VatIdentifier,
                    UserId = producer.Id,
                    RegistrationKind = request.Legals.RegistrationKind,
                    RegistrationCity = request.Legals.RegistrationCity,
                    RegistrationCode = request.Legals.RegistrationCode,
                    Owner = request.Legals.Owner
                }, token);

                if (!result.Succeeded)
                    return result;

                var catalogConsumerResult = await _mediatr.Process(new CreateCatalogCommand(request.RequestUser)
                {
                    Kind = CatalogKind.Consumers,
                    Name = "Catalogue Consommateurs",
                    IsAvailable = true,
                    IsDefault = true,
                    Products = new List<ProductPriceInputDto>()
                }, token);

                if (!catalogConsumerResult.Succeeded)
                    return catalogConsumerResult;

                var catalogStoreResult = await _mediatr.Process(new CreateCatalogCommand(request.RequestUser)
                {
                    Kind = CatalogKind.Stores,
                    Name = "Catalogue Professionnels",
                    IsAvailable = true,
                    IsDefault = true,
                    Products = new List<ProductPriceInputDto>()
                }, token);

                if (!catalogStoreResult.Succeeded)
                    return catalogStoreResult;

                var batchDefinitionResult = await _mediatr.Process(new CreateBatchDefinitionCommand(request.RequestUser)
                {
                    Name = "Tracabilité par défaut",
                    IsDefault = true,
                    Description =
                        "Configuration générée par défaut, vous pouvez la modifier pour ajouter des champs spécifiques à votre gestion de la tracabilité pour votre production.",
                    FieldDefinitions = new List<BatchField>()
                }, token);

                if (!batchDefinitionResult.Succeeded)
                    return batchDefinitionResult;

                await transaction.CommitAsync(token);

                if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                {
                    _mediatr.Post(new CreateSponsoringCommand(request.RequestUser)
                        { Code = request.SponsoringCode, UserId = producer.Id });
                }

                return Success(producer.Id);
            }
        }

        private async Task<UserAddress> GetAddressAsync(AddressDto address, CancellationToken token)
        {
            if (address == null)
                return null;
            
            var departmentCode = UserAddress.GetDepartmentCode(address.Zipcode);
            var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

            return new UserAddress(address.Line1, address.Line2, address.Zipcode,
                    address.City,
                    address.Country, department, address.Longitude, address.Latitude);
        }
    }
}
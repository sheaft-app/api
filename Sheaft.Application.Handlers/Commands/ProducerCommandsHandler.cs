using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Options;
using Sheaft.Exceptions;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Events;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace Sheaft.Application.Handlers
{
    public class ProducerCommandsHandler : ResultsHandler,
        IRequestHandler<GenerateProducersFileCommand, Result<bool>>,
        IRequestHandler<RegisterProducerCommand, Result<Guid>>,
        IRequestHandler<UpdateProducerCommand, Result<bool>>,
        IRequestHandler<CheckProducerConfigurationCommand, Result<bool>>,
        IRequestHandler<UpdateProducerTagsCommand, Result<bool>>,
        IRequestHandler<SetProducerProductsWithNoVatCommand, Result<bool>>
    {
        private readonly RoleOptions _roleOptions;
        private readonly IBlobService _blobService;

        public ProducerCommandsHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IBlobService blobService,
            ILogger<ProducerCommandsHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _blobService = blobService;
        }

        public async Task<Result<bool>> Handle(GenerateProducersFileCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producers = await _context.GetAsync<Producer>(token);
                var prods = producers.Select(p => new ProducerListItem(p));

                var result = await _blobService.UploadProducersListAsync(
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(prods, 
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }})), token);
                
                if(!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<Guid>> Handle(RegisterProducerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var producer = await _context.FindByIdAsync<Producer>(request.RequestUser.Id, token);
                    if (producer != null)
                        return Conflict<Guid>(MessageKind.Register_User_AlreadyExists);

                    var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                    var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                    var address = request.Address != null ?
                        new UserAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City,
                        request.Address.Country, department, request.Address.Longitude, request.Address.Latitude)
                        : null;

                    producer = new Producer(request.RequestUser.Id, request.Name, request.FirstName, request.LastName, request.Email,
                        address, request.OpenForNewBusiness, request.Phone, request.Description);
                    producer.SetNotSubjectToVat(request.NotSubjectToVat);

                    if (request.Tags != null && request.Tags.Any())
                    {
                        var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                        producer.SetTags(tags);
                    }

                    await _context.AddAsync(producer, token);
                    await _context.SaveChangesAsync(token);

                    var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
                    {
                        UserId = producer.Id,
                        Picture = request.Picture,
                        SkipAuthUpdate = true
                    }, token);

                    if (!resultImage.Success)
                        return Failed<Guid>(resultImage.Exception);

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

                    if (!authResult.Success)
                        return Failed<Guid>(authResult.Exception);

                    var result = await _mediatr.Process(new CreateBusinessLegalCommand(request.RequestUser)
                    {
                        Address = request.Legals.Address,
                        Name = request.Legals.Name,
                        Email = request.Legals.Email,
                        Siret = request.Legals.Siret,
                        Kind = request.Legals.Kind,
                        VatIdentifier = request.NotSubjectToVat ? null : request.Legals.VatIdentifier,
                        UserId = producer.Id,
                        Owner = request.Legals.Owner
                    }, token);

                    if (!result.Success)
                        return result;

                    await transaction.CommitAsync(token);

                    if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                    {
                        _mediatr.Post(new CreateSponsoringCommand(request.RequestUser) { Code = request.SponsoringCode, UserId = producer.Id });
                    }

                    _mediatr.Post(new ProducerRegisteredEvent(request.RequestUser) { ProducerId = producer.Id });

                    return Created(producer.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(UpdateProducerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.Id, token);

                producer.SetName(request.Name);
                producer.SetFirstname(request.FirstName);
                producer.SetLastname(request.LastName);
                producer.SetEmail(request.Email);
                producer.SetProfileKind(request.Kind);
                producer.SetPhone(request.Phone);
                producer.SetDescription(request.Description);
                producer.SetOpenForNewBusiness(request.OpenForNewBusiness);

                if (request.NotSubjectToVat.HasValue)
                {
                    producer.SetNotSubjectToVat(request.NotSubjectToVat.Value);
                }

                var departmentCode = UserAddress.GetDepartmentCode(request.Address.Zipcode);
                var department = await _context.Departments.SingleOrDefaultAsync(d => d.Code == departmentCode, token);

                producer.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country, department, request.Address.Longitude, request.Address.Latitude);
                                
                if (request.Tags != null)
                {
                    var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                    producer.SetTags(tags);
                }

                await _context.SaveChangesAsync(token);

                var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
                {
                    UserId = producer.Id,
                    Picture = request.Picture,
                    SkipAuthUpdate = true
                }, token);

                if (!resultImage.Success)
                    return Failed<bool>(resultImage.Exception);

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

                if (!authResult.Success)
                    return authResult;

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckProducerConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var business = await _mediatr.Process(new CheckBusinessLegalConfigurationCommand(request.RequestUser) { UserId = request.ProducerId }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Process(new CheckWalletPaymentsConfigurationCommand(request.RequestUser) { UserId = request.ProducerId }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(UpdateProducerTagsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.ProducerId, token);

                var productTags = await _context.Products.Get(p => p.Producer.Id == producer.Id).SelectMany(p => p.Tags).Select(p => p.Tag).Distinct().ToListAsync(token);
                producer.SetTags(productTags);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(SetProducerProductsWithNoVatCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.ProducerId, token);
                if (!producer.NotSubjectToVat)
                    return Ok(false);

                var products = await _context.FindAsync<Product>(p => p.Producer.Id == producer.Id, token);
                foreach(var product in products)
                {
                    product.SetVat(0);
                    await _context.SaveChangesAsync(token);
                }

                return Ok(true);
            });
        }        

        internal class ProducerListItem
        {
            internal ProducerListItem(User user)
            {
                Address = new AddressItem(user.Address);
                Id = user.Id.ToString("N");
                Name = user.Name;
                Picture = user.Picture;
            }
            public string Id { get; set; }
            public string Name { get; set; }
            public string Picture { get; set; }
            public AddressItem Address { get; set; }
        }

        internal class AddressItem {
            internal AddressItem(UserAddress address)
            {
                Line1 = address.Line1;
                Line2 = address.Line2;
                Zipcode = address.Zipcode;
                City = address.City;
                Latitude = address.Latitude;
                Longitude = address.Longitude;
            }
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string Zipcode { get; set; }
            public string City { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
        }
    }
}
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using System.Linq;
using System.Collections.Generic;
using Sheaft.Interop.Enums;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using System.IO;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.Primitives;
using Sheaft.Services.Interop;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class CompanyCommandsHandler : CommandsHandler,
        IRequestHandler<RegisterCompanyCommand, CommandResult<Guid>>,
        IRequestHandler<UpdateCompanyCommand, CommandResult<bool>>,
        IRequestHandler<UpdateCompanyPictureCommand, CommandResult<bool>>,
        IRequestHandler<DeleteCompaniesCommand, CommandResult<bool>>,
        IRequestHandler<DeleteCompanyCommand, CommandResult<bool>>,
        IRequestHandler<RemoveCompanyDataCommand, CommandResult<string>>
    {
        private readonly IMediator _mediator;
        private readonly IAppDbContext _context;
        private readonly IQueueService _queuesService;
        private readonly IBlobService _blobsService;
        private readonly RoleOptions _roleOptions;

        public CompanyCommandsHandler(
            IMediator mediator,
            IAppDbContext context,
            IQueueService queuesService,
            IBlobService blobsService,
            ILogger<CompanyCommandsHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions) : base(logger)
        {
            _roleOptions = roleOptions.Value;
            _blobsService = blobsService;
            _queuesService = queuesService;
            _mediator = mediator;
            _context = context;
        }

        public async Task<CommandResult<Guid>> Handle(RegisterCompanyCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    if (!request.Owner.Roles.Contains(_roleOptions.Producer.Value) && !request.Owner.Roles.Contains(_roleOptions.Store.Value))
                        return ValidationResult<Guid>(MessageKind.RegisterCompany_CannotContains_ProducerAndStoreRoles);

                    var kind = ProfileKind.Producer;
                    if (request.Owner.Roles.Contains(_roleOptions.Store.Value))
                    {
                        kind = ProfileKind.Store;
                    }

                    var departmentCode = Address.GetDepartmentCode(request.Address.Zipcode);
                    var department = await _context.GetSingleAsync<Department>(d => d.Code == departmentCode, token);

                    var openingHours = new List<TimeSlotHour>();
                    if (request.OpeningHours != null)
                    {
                        foreach (var oh in request.OpeningHours)
                        {
                            openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                        }
                    }

                    var company = new Company(Guid.NewGuid(), request.Name, kind, request.Email, request.Siret, request.VatIdentifier, request.Address != null ? new Address(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, department, request.Address.Longitude, request.Address.Latitude) : null, openingHours, request.AppearInBusinessSearchResults, request.Phone, request.Description);
                    if (request.Tags != null && request.Tags.Any())
                    {
                        var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                        company.SetTags(tags);
                    }

                    await _context.AddAsync(company, token);
                    await _context.SaveChangesAsync(token);

                    var owner = await _mediator.Send(new RegisterOwnerCommand(request.RequestUser)
                    {
                        CompanyId = company.Id,
                        FirstName = request.Owner.FirstName,
                        LastName = request.Owner.LastName,
                        Email = request.Owner.Email,
                        Roles = request.Owner.Roles,
                        Phone = request.Owner.Phone,
                        Picture = request.Owner.Picture,
                        SponsoringCode = request.Owner.SponsoringCode,
                        Id = request.RequestUser.Id
                    }, token);

                    if (!owner.Success)
                        return CommandFailed<Guid>(owner.Exception);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return CreatedResult(company.Id);
                }
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateCompanyCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var company = await _context.GetByIdAsync<Company>(request.Id, token);

                var departmentCode = Address.GetDepartmentCode(request.Address.Zipcode);
                var department = await _context.GetSingleAsync<Department>(d => d.Code == departmentCode, token);

                var openingHours = new List<TimeSlotHour>();
                if (request.OpeningHours != null)
                {
                    foreach (var oh in request.OpeningHours)
                    {
                        openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                    }
                }

                company.SetOpeningHours(openingHours);
                company.SetName(request.Name);
                company.SetEmail(request.Email);
                company.SetPhone(request.Phone);
                company.SetDescription(request.Description);
                company.SetSiret(request.Siret);
                company.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, department, request.Address.Longitude, request.Address.Latitude);
                company.SetVatIdentifier(request.VatIdentifier);
                company.SetAppearInBusinessSearchResults(request.AppearInBusinessSearchResults);

                if (request.Tags != null)
                {
                    var tags = await _context.FindAsync<Tag>(t => request.Tags.Contains(t.Id), token);
                    company.SetTags(tags);
                }

                var entry = _context.Companies.Update(company);
                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteCompaniesCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                foreach (var id in request.Ids)
                {
                    var result = await _mediator.Send(new DeleteCompanyCommand(request.RequestUser) { Id = id, Reason = request.Reason }, token);
                    if (!result.Success)
                        return CommandFailed<bool>(result.Exception);
                }

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }


        public async Task<CommandResult<bool>> Handle(DeleteCompanyCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Company>(request.Id, token);
                var email = entity.Email;

                entity.CloseCompany(request.Reason);
                var success = await _context.SaveChangesAsync(token) > 0;

                await _queuesService.ProcessCommandAsync(RemoveCompanyDataCommand.QUEUE_NAME, new RemoveCompanyDataCommand(request.RequestUser) { Id = request.Id, Email = email }, token);
                return OkResult(success);
            });
        }

        public async Task<CommandResult<string>> Handle(RemoveCompanyDataCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Company>(request.Id, token);
                return OkResult(request.Email);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateCompanyPictureCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var company = await _context.GetByIdAsync<Company>(request.Id, token);

                var base64Data = Regex.Match(request.Picture, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                var bytes = Convert.FromBase64String(base64Data);

                using (Image image = Image.Load(bytes))
                {
                    using (var blobStream = new MemoryStream())
                    {
                        image.Clone(context => context.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(64, 64)
                        })).Save(blobStream, new JpegEncoder { Quality = 100 });

                        var compPicture = await _blobsService.UploadCompanyPictureAsync(company.Id, blobStream, token);
                        if (!compPicture.Success)
                            return CommandFailed<bool>(compPicture.Exception);

                        company.SetPicture(compPicture.Result);
                        var entry = _context.Update(company);
                    }
                }

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}
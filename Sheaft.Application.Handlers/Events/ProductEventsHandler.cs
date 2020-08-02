﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{
    public class ProductEventsHandler :
        INotificationHandler<ProductImportSucceededEvent>,
        INotificationHandler<ProductImportFailedEvent>,
        INotificationHandler<ProductImportProcessingEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ISignalrService _signalrService;
        private readonly IConfiguration _configuration;

        public ProductEventsHandler(IConfiguration configuration, IAppDbContext context, IEmailService emailService, ISignalrService signalrService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _signalrService = signalrService;
        }

        public async Task Handle(ProductImportSucceededEvent productEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(productEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Company.Id, nameof(ProductImportSucceededEvent), new { JobId = job.Id, UserId = job.User.Id });

            var userName = $"{job.User.FirstName} {job.User.LastName}";
            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/jobs/{job.Id}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                userName,
                ProductImportSucceededEvent.MAILING_TEMPLATE_ID,
                new { UserName = userName, Name = job.Name, job.CreatedOn, JobUrl = url, DownloadUrl = job.File },
                token);
        }

        public async Task Handle(ProductImportFailedEvent productEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(productEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Company.Id, nameof(ProductImportFailedEvent), new { JobId = job.Id, UserId = job.User.Id });

            var userName = $"{job.User.FirstName} {job.User.LastName}";
            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/jobs/{job.Id}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                userName,
                ProductImportFailedEvent.MAILING_TEMPLATE_ID,
                new { UserName = userName, Name = job.Name, job.CreatedOn, JobUrl = url },
                token);
        }

        public async Task Handle(ProductImportProcessingEvent productEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(productEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Company.Id, nameof(ProductImportProcessingEvent), new { JobId = job.Id, UserId = job.User.Id });
        }
    }
}
using Sheaft.Infrastructure.Interop;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Events;
using Sheaft.Application.Commands;
using System;
using Sheaft.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Services.Interop;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class AgreementCommandsHandler : CommandsHandler,
        IRequestHandler<CreateAgreementCommand, CommandResult<Guid>>,
        IRequestHandler<AcceptAgreementCommand, CommandResult<bool>>,
        IRequestHandler<CancelAgreementsCommand, CommandResult<bool>>,
        IRequestHandler<CancelAgreementCommand, CommandResult<bool>>,
        IRequestHandler<RefuseAgreementsCommand, CommandResult<bool>>,
        IRequestHandler<RefuseAgreementCommand, CommandResult<bool>>,
        IRequestHandler<DeleteAgreementCommand, CommandResult<bool>>,
        IRequestHandler<RestoreAgreementCommand, CommandResult<bool>>
    {
        private readonly IMediator _mediatr;
        private readonly IAppDbContext _context;
        private readonly IQueueService _queuesService;

        public AgreementCommandsHandler(
            IMediator mediatr,
            IAppDbContext context,
            IQueueService queuesService,
            ILogger<AgreementCommandsHandler> logger) : base(logger)
        {
            _queuesService = queuesService;
            _mediatr = mediatr;
            _context = context;
        }

        public async Task<CommandResult<Guid>> Handle(CreateAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var store = await _context.GetByIdAsync<Company>(request.StoreId, token);
                var delivery = await _context.GetByIdAsync<DeliveryMode>(request.DeliveryModeId, token);

                var selectedHours = new List<TimeSlotHour>();
                if (request.SelectedHours != null && request.SelectedHours.Any())
                {
                    foreach (var sh in request.SelectedHours)
                    {
                        selectedHours.AddRange(sh.Days.Select(d => new TimeSlotHour(d, sh.From, sh.To)));
                    }
                }

                var entity = new Agreement(Guid.NewGuid(), store, delivery, user, selectedHours);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                if (request.RequestUser.CompanyId == store.Id)
                    await _queuesService.ProcessEventAsync(AgreementCreatedByStoreEvent.QUEUE_NAME, new AgreementCreatedByStoreEvent(request.RequestUser) { Id = entity.Id }, token);

                if (request.RequestUser.CompanyId == delivery.Producer.Id)
                    await _queuesService.ProcessEventAsync(AgreementCreatedByProducerEvent.QUEUE_NAME, new AgreementCreatedByProducerEvent(request.RequestUser) { Id = entity.Id }, token);

                return OkResult(entity.Id);
            });
        }

        public async Task<CommandResult<bool>> Handle(AcceptAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Agreement>(request.Id, token);
                entity.AcceptAgreement();

                var selectedHours = new List<TimeSlotHour>();
                if (request.SelectedHours != null && request.SelectedHours.Any())
                {
                    foreach (var sh in request.SelectedHours)
                    {
                        selectedHours.AddRange(sh.Days.Select(d => new TimeSlotHour(d, sh.From, sh.To)));
                    }

                    entity.SetSelectedHours(selectedHours);
                }

                _context.Update(entity);
                await _context.SaveChangesAsync(token);

                if (request.RequestUser.CompanyId == entity.Store.Id)
                    await _queuesService.ProcessEventAsync(AgreementAcceptedByStoreEvent.QUEUE_NAME, new AgreementAcceptedByStoreEvent(request.RequestUser) { Id = entity.Id }, token);

                if (request.RequestUser.CompanyId == entity.Delivery.Producer.Id)
                    await _queuesService.ProcessEventAsync(AgreementAcceptedByProducerEvent.QUEUE_NAME, new AgreementAcceptedByProducerEvent(request.RequestUser) { Id = entity.Id }, token);

                return OkResult(true);
            });
        }

        public async Task<CommandResult<bool>> Handle(CancelAgreementsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var agreementId in request.Ids)
                    {
                        var result = await _mediatr.Send(new CancelAgreementCommand(request.RequestUser) { Id = agreementId, Reason = request.Reason }, token);
                        if (!result.Success)
                            return CommandFailed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return OkResult(true);
                }
            });
        }

        public async Task<CommandResult<bool>> Handle(CancelAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Agreement>(request.Id, token);
                entity.CancelAgreement(request.Reason);

                _context.Agreements.Update(entity);
                await _context.SaveChangesAsync(token);

                if (request.RequestUser.CompanyId == entity.Store.Id)
                    await _queuesService.ProcessEventAsync(AgreementCancelledByStoreEvent.QUEUE_NAME, new AgreementCancelledByStoreEvent(request.RequestUser) { Id = entity.Id }, token);

                if (request.RequestUser.CompanyId == entity.Delivery.Producer.Id)
                    await _queuesService.ProcessEventAsync(AgreementCancelledByProducerEvent.QUEUE_NAME, new AgreementCancelledByProducerEvent(request.RequestUser) { Id = entity.Id }, token);

                return OkResult(true);
            });
        }

        public async Task<CommandResult<bool>> Handle(RefuseAgreementsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var agreementId in request.Ids)
                    {
                        var result = await _mediatr.Send(new RefuseAgreementCommand(request.RequestUser) { Id = agreementId, Reason = request.Reason }, token);
                        if (!result.Success)
                            return CommandFailed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return OkResult(true);
                }
            });
        }

        public async Task<CommandResult<bool>> Handle(RefuseAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Agreement>(request.Id, token);
                entity.RefuseAgreement(request.Reason);

                _context.Agreements.Update(entity);
                await _context.SaveChangesAsync(token);

                if (request.RequestUser.CompanyId == entity.Store.Id)
                    await _queuesService.ProcessEventAsync(AgreementRefusedByStoreEvent.QUEUE_NAME, new AgreementRefusedByStoreEvent(request.RequestUser) { Id = entity.Id }, token);

                if (request.RequestUser.CompanyId == entity.Delivery.Producer.Id)
                    await _queuesService.ProcessEventAsync(AgreementRefusedByProducerEvent.QUEUE_NAME, new AgreementRefusedByProducerEvent(request.RequestUser) { Id = entity.Id }, token);

                return OkResult(true);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Agreement>(request.Id, token);

                _context.Agreements.Remove(entity);
                await _context.SaveChangesAsync(token);

                return OkResult(true);
            });
        }

        public async Task<CommandResult<bool>> Handle(RestoreAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Agreements.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                entity.Restore();

                _context.Update(entity);
                await _context.SaveChangesAsync(token);

                return OkResult(true);
            });
        }
    }
}
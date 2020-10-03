using Sheaft.Application.Interop;
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
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class AgreementCommandsHandler : ResultsHandler,
        IRequestHandler<CreateAgreementCommand, Result<Guid>>,
        IRequestHandler<AcceptAgreementCommand, Result<bool>>,
        IRequestHandler<CancelAgreementsCommand, Result<bool>>,
        IRequestHandler<CancelAgreementCommand, Result<bool>>,
        IRequestHandler<RefuseAgreementsCommand, Result<bool>>,
        IRequestHandler<RefuseAgreementCommand, Result<bool>>,
        IRequestHandler<DeleteAgreementCommand, Result<bool>>,
        IRequestHandler<ResetAgreementStatusToCommand, Result<bool>>,
        IRequestHandler<RestoreAgreementCommand, Result<bool>>
    {
        public AgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<AgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var store = await _context.GetByIdAsync<Store>(request.StoreId, token);
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

                _mediatr.Post(new AgreementCreatedEvent(request.RequestUser) { AgreementId = entity.Id });

                return Ok(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(AcceptAgreementCommand request, CancellationToken token)
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

                await _context.SaveChangesAsync(token);

                _mediatr.Post(new AgreementAcceptedEvent(request.RequestUser) { AgreementId = entity.Id });
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CancelAgreementsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var agreementId in request.Ids)
                    {
                        var result = await _mediatr.Process(new CancelAgreementCommand(request.RequestUser) { Id = agreementId, Reason = request.Reason }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(CancelAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Agreement>(request.Id, token);
                entity.CancelAgreement(request.Reason);

                await _context.SaveChangesAsync(token);

                _mediatr.Post(new AgreementCancelledEvent(request.RequestUser) { AgreementId = entity.Id });
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RefuseAgreementsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var agreementId in request.Ids)
                    {
                        var result = await _mediatr.Process(new RefuseAgreementCommand(request.RequestUser) { Id = agreementId, Reason = request.Reason }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(RefuseAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Agreement>(request.Id, token);
                entity.RefuseAgreement(request.Reason);

                await _context.SaveChangesAsync(token);

                _mediatr.Post(new AgreementRefusedEvent(request.RequestUser) { AgreementId = entity.Id });
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(DeleteAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Agreement>(request.Id, token);

                _context.Remove(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ResetAgreementStatusToCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Agreements.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                entity.Reset();

                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RestoreAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Agreements.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);

                _context.Restore(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}
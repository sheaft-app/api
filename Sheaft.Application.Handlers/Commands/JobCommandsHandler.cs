using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using System;
using Sheaft.Domain.Enums;
using RestSharp.Serialization.Json;
using Newtonsoft.Json;

namespace Sheaft.Application.Handlers
{
    public class JobCommandsHandler : ResultsHandler,
        IRequestHandler<CancelJobsCommand, Result<bool>>,
        IRequestHandler<RetryJobsCommand, Result<bool>>,
        IRequestHandler<PauseJobsCommand, Result<bool>>,
        IRequestHandler<ResumeJobsCommand, Result<bool>>,
        IRequestHandler<ArchiveJobsCommand, Result<bool>>,
        IRequestHandler<StartJobsCommand, Result<bool>>,
        IRequestHandler<CompleteJobsCommand, Result<bool>>,
        IRequestHandler<FailJobsCommand, Result<bool>>,
        IRequestHandler<CancelJobCommand, Result<bool>>,
        IRequestHandler<RetryJobCommand, Result<bool>>,
        IRequestHandler<PauseJobCommand, Result<bool>>,
        IRequestHandler<ResumeJobCommand, Result<bool>>,
        IRequestHandler<ArchiveJobCommand, Result<bool>>,
        IRequestHandler<UnarchiveJobCommand, Result<bool>>,
        IRequestHandler<StartJobCommand, Result<bool>>,
        IRequestHandler<CompleteJobCommand, Result<bool>>,
        IRequestHandler<FailJobCommand, Result<bool>>,
        IRequestHandler<ResetJobCommand, Result<bool>>,
        IRequestHandler<DeleteJobCommand, Result<bool>>,
        IRequestHandler<RestoreJobCommand, Result<bool>>,
        IRequestHandler<UpdateJobCommand, Result<bool>>
    {
        public JobCommandsHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<JobCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(CancelJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
           {
               using (var transaction = await _context.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Process(new CancelJobCommand(request.RequestUser) { Id = jobId, Reason = request.Reason }, token);
                       if (!result.Success)
                           return Failed<bool>(result.Exception);
                   }

                   await transaction.CommitAsync(token);
                   return Ok(true);
               }
           });
        }

        public async Task<Result<bool>> Handle(RetryJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
           {
               using (var transaction = await _context.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Process(new RetryJobCommand(request.RequestUser) { Id = jobId }, token);
                       if (!result.Success)
                           return Failed<bool>(result.Exception);
                   }

                   await transaction.CommitAsync(token);
                   return Ok(true);
               }
           });
        }

        public async Task<Result<bool>> Handle(PauseJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
           {
               using (var transaction = await _context.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Process(new PauseJobCommand(request.RequestUser) { Id = jobId }, token);
                       if (!result.Success)
                           return Failed<bool>(result.Exception);
                   }

                   await transaction.CommitAsync(token);
                   return Ok(true);
               }
           });
        }

        public async Task<Result<bool>> Handle(ArchiveJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
           {
               using (var transaction = await _context.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Process(new ArchiveJobCommand(request.RequestUser) { Id = jobId }, token);
                       if (!result.Success)
                           return Failed<bool>(result.Exception);
                   }

                   await transaction.CommitAsync(token);
                   return Ok(true);
               }
           });
        }

        public async Task<Result<bool>> Handle(ResumeJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
           {
               using (var transaction = await _context.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Process(new ResumeJobCommand(request.RequestUser) { Id = jobId }, token);
                       if (!result.Success)
                           return Failed<bool>(result.Exception);
                   }

                   await transaction.CommitAsync(token);
                   return Ok(true);
               }
           });
        }

        public async Task<Result<bool>> Handle(StartJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var jobId in request.Ids)
                    {
                        var result = await _mediatr.Process(new StartJobCommand(request.RequestUser) { Id = jobId }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(CompleteJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var jobId in request.Ids)
                    {
                        var result = await _mediatr.Process(new CompleteJobCommand(request.RequestUser) { Id = jobId }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(FailJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var jobId in request.Ids)
                    {
                        var result = await _mediatr.Process(new FailJobCommand(request.RequestUser) { Id = jobId, Reason = request.Reason }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(CancelJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.CancelJob(request.Reason);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RetryJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.RetryJob();
                await _context.SaveChangesAsync(token);

                EnqueueJobCommand(entity, request.RequestUser);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(PauseJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.PauseJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ArchiveJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.ArchiveJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(UnarchiveJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.UnarchiveJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ResumeJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.ResumeJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(StartJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.StartJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CompleteJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.SetDownloadUrl(request.FileUrl);
                entity.CompleteJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(FailJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.FailJob(request.Reason);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ResetJobCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Job>(request.Id, token);

            entity.ResetJob();
            await _context.SaveChangesAsync(token);
            
            EnqueueJobCommand(entity, request.RequestUser);
            return Ok(true);
        }

        public async Task<Result<bool>> Handle(DeleteJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                _context.Remove(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RestoreJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Jobs.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                _context.Restore(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(UpdateJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.SetName(request.Name);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        private void EnqueueJobCommand(Job entity, RequestUser requestUser)
        {
            switch (entity.Kind)
            {
                case JobKind.ExportPickingOrders:
                    var exportPickingOrderCommand = JsonConvert.DeserializeObject<ExportPickingOrderCommand>(entity.Command);
                    _mediatr.Post(new ExportPickingOrderCommand(requestUser) { JobId = exportPickingOrderCommand.JobId, PurchaseOrderIds = exportPickingOrderCommand.PurchaseOrderIds });
                    break;
                case JobKind.ExportUserData:
                    var exportUserDataCommand = JsonConvert.DeserializeObject<ExportUserDataCommand>(entity.Command);
                    _mediatr.Post(new ExportUserDataCommand(requestUser) { Id = exportUserDataCommand.Id });
                    break;
                case JobKind.ImportProducts:
                    var importProductsCommand = JsonConvert.DeserializeObject<ImportProductsCommand>(entity.Command);
                    _mediatr.Post(new ImportProductsCommand(requestUser) { Id = importProductsCommand.Id, Uri = importProductsCommand.Uri });
                    break;
            }
        }
    }
}
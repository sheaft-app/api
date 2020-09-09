using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{
    public class JobCommandsHandler : CommandsHandler,
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
        private readonly IAppDbContext _context;
        private readonly IMediator _mediatr;
        private readonly IQueueService _queuesService;

        public JobCommandsHandler(
            IMediator mediatr, 
            IAppDbContext context,
            IQueueService queueService,
            ILogger<JobCommandsHandler> logger) : base(logger)
        {
            _queuesService = queueService;
            _context = context;
            _mediatr = mediatr;
        }

        public async Task<Result<bool>> Handle(CancelJobsCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
           {
               using (var transaction = await _context.Database.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Send(new CancelJobCommand(request.RequestUser) { Id = jobId, Reason = request.Reason }, token);
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
               using (var transaction = await _context.Database.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Send(new RetryJobCommand(request.RequestUser) { Id = jobId }, token);
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
               using (var transaction = await _context.Database.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Send(new PauseJobCommand(request.RequestUser) { Id = jobId }, token);
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
               using (var transaction = await _context.Database.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Send(new ArchiveJobCommand(request.RequestUser) { Id = jobId }, token);
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
               using (var transaction = await _context.Database.BeginTransactionAsync(token))
               {
                   foreach (var jobId in request.Ids)
                   {
                       var result = await _mediatr.Send(new ResumeJobCommand(request.RequestUser) { Id = jobId }, token);
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
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var jobId in request.Ids)
                    {
                        var result = await _mediatr.Send(new StartJobCommand(request.RequestUser) { Id = jobId }, token);
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
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var jobId in request.Ids)
                    {
                        var result = await _mediatr.Send(new CompleteJobCommand(request.RequestUser) { Id = jobId }, token);
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
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var jobId in request.Ids)
                    {
                        var result = await _mediatr.Send(new FailJobCommand(request.RequestUser) { Id = jobId, Reason = request.Reason }, token);
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
                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(RetryJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.RetryJob();
                _context.Update(entity);

                var result = await _context.SaveChangesAsync(token);
                await _queuesService.InsertJobToProcessAsync(entity, token);

                return Ok(result > 0);
            });
        }

        public async Task<Result<bool>> Handle(PauseJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.PauseJob();
                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(ArchiveJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.ArchiveJob();
                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(UnarchiveJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.UnarchiveJob();
                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(ResumeJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.ResumeJob();
                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(StartJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.StartJob();
                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
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

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(FailJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.FailJob(request.Reason);
                _context.Update(entity);
                
                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(ResetJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.ResetJob();
                _context.Update(entity);

                var result = await _context.SaveChangesAsync(token);
                await _queuesService.InsertJobToProcessAsync(entity, token);

                return Ok(result > 0);
            });
        }

        public async Task<Result<bool>> Handle(DeleteJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                _context.Remove(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(RestoreJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Jobs.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);

                entity.Restore();
                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(UpdateJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.SetName(request.Name);

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}
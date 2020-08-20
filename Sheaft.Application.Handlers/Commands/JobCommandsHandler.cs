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
        IRequestHandler<CancelJobsCommand, CommandResult<bool>>,
        IRequestHandler<RetryJobsCommand, CommandResult<bool>>,
        IRequestHandler<PauseJobsCommand, CommandResult<bool>>,
        IRequestHandler<ResumeJobsCommand, CommandResult<bool>>,
        IRequestHandler<ArchiveJobsCommand, CommandResult<bool>>,
        IRequestHandler<StartJobsCommand, CommandResult<bool>>,
        IRequestHandler<CompleteJobsCommand, CommandResult<bool>>,
        IRequestHandler<FailJobsCommand, CommandResult<bool>>,
        IRequestHandler<CancelJobCommand, CommandResult<bool>>,
        IRequestHandler<RetryJobCommand, CommandResult<bool>>,
        IRequestHandler<PauseJobCommand, CommandResult<bool>>,
        IRequestHandler<ResumeJobCommand, CommandResult<bool>>,
        IRequestHandler<ArchiveJobCommand, CommandResult<bool>>,
        IRequestHandler<UnarchiveJobCommand, CommandResult<bool>>,
        IRequestHandler<StartJobCommand, CommandResult<bool>>,
        IRequestHandler<CompleteJobCommand, CommandResult<bool>>,
        IRequestHandler<FailJobCommand, CommandResult<bool>>,
        IRequestHandler<ResetJobCommand, CommandResult<bool>>,
        IRequestHandler<DeleteJobCommand, CommandResult<bool>>,
        IRequestHandler<RestoreJobCommand, CommandResult<bool>>,
        IRequestHandler<UpdateJobCommand, CommandResult<bool>>
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

        public async Task<CommandResult<bool>> Handle(CancelJobsCommand request,
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
                           return CommandFailed<bool>(result.Exception);
                   }

                   await transaction.CommitAsync(token);
                   return OkResult(true);
               }
           });
        }

        public async Task<CommandResult<bool>> Handle(RetryJobsCommand request,
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
                           return CommandFailed<bool>(result.Exception);
                   }

                   await transaction.CommitAsync(token);
                   return OkResult(true);
               }
           });
        }

        public async Task<CommandResult<bool>> Handle(PauseJobsCommand request,
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
                           return CommandFailed<bool>(result.Exception);
                   }

                   await transaction.CommitAsync(token);
                   return OkResult(true);
               }
           });
        }

        public async Task<CommandResult<bool>> Handle(ArchiveJobsCommand request,
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
                           return CommandFailed<bool>(result.Exception);
                   }

                   await transaction.CommitAsync(token);
                   return OkResult(true);
               }
           });
        }

        public async Task<CommandResult<bool>> Handle(ResumeJobsCommand request,
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
                           return CommandFailed<bool>(result.Exception);
                   }

                   await transaction.CommitAsync(token);
                   return OkResult(true);
               }
           });
        }

        public async Task<CommandResult<bool>> Handle(StartJobsCommand request,
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
                            return CommandFailed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return OkResult(true);
                }
            });
        }

        public async Task<CommandResult<bool>> Handle(CompleteJobsCommand request,
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
                            return CommandFailed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return OkResult(true);
                }
            });
        }

        public async Task<CommandResult<bool>> Handle(FailJobsCommand request,
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
                            return CommandFailed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return OkResult(true);
                }
            });
        }

        public async Task<CommandResult<bool>> Handle(CancelJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.CancelJob(request.Reason);
                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(RetryJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.RetryJob();
                _context.Update(entity);

                var result = await _context.SaveChangesAsync(token);
                await _queuesService.InsertJobToProcessAsync(entity, token);

                return OkResult(result > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(PauseJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.PauseJob();
                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(ArchiveJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.ArchiveJob();
                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(UnarchiveJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.UnarchiveJob();
                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(ResumeJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.ResumeJob();
                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(StartJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.StartJob();
                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(CompleteJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.SetDownloadUrl(request.FileUrl);
                entity.CompleteJob();

                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(FailJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.FailJob(request.Reason);
                _context.Update(entity);
                
                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(ResetJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                entity.ResetJob();
                _context.Update(entity);

                var result = await _context.SaveChangesAsync(token);
                await _queuesService.InsertJobToProcessAsync(entity, token);

                return OkResult(result > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);

                _context.Remove(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(RestoreJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Jobs.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);

                entity.Restore();
                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.SetName(request.Name);

                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}
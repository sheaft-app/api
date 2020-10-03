using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Exceptions;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{
    public class ConsumerCommandsHandler : ResultsHandler,
        IRequestHandler<RegisterConsumerCommand, Result<Guid>>,
        IRequestHandler<UpdateConsumerCommand, Result<bool>>,
        IRequestHandler<CheckConsumerConfigurationCommand, Result<bool>>
    {
        private readonly RoleOptions _roleOptions;

        public ConsumerCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<ConsumerCommandsHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result<Guid>> Handle(RegisterConsumerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var consumer = await _context.FindSingleAsync<Consumer>(r => r.Id == request.RequestUser.Id || r.Email == request.Email, token);
                    if (consumer != null)
                        return Conflict<Guid>(MessageKind.Register_User_AlreadyExists);

                    consumer = new Consumer(request.RequestUser.Id, request.Email, request.FirstName, request.LastName, request.Phone);
                    await _context.AddAsync(consumer, token);
                    await _context.SaveChangesAsync(token);

                    var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser) { 
                            UserId = consumer.Id, 
                            Picture = request.Picture, 
                            SkipAuthUpdate = true 
                        }, token);

                    if (!resultImage.Success)
                        return Failed<Guid>(resultImage.Exception);

                    var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
                    {
                        Email = consumer.Email,
                        FirstName = consumer.FirstName,
                        LastName = consumer.LastName,
                        Name = consumer.Name,
                        Phone = consumer.Phone,
                        Picture = resultImage.Data,
                        Roles = new List<Guid> { _roleOptions.Consumer.Id },
                        UserId = consumer.Id
                    }, token);

                    if (!authResult.Success)
                        return Failed<Guid>(authResult.Exception);

                    await transaction.CommitAsync(token);

                    if (!string.IsNullOrWhiteSpace(request.SponsoringCode))
                    {
                        _mediatr.Post(new CreateSponsoringCommand(request.RequestUser) { Code = request.SponsoringCode, UserId = consumer.Id });
                    }

                    return Created(consumer.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(UpdateConsumerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var consumer = await _context.GetByIdAsync<Consumer>(request.Id, token);

                consumer.SetEmail(request.Email);
                consumer.SetPhone(request.Phone);
                consumer.SetFirstname(request.FirstName);
                consumer.SetLastname(request.LastName);

                await _context.SaveChangesAsync(token);

                var resultImage = await _mediatr.Process(new UpdateUserPictureCommand(request.RequestUser)
                {
                    UserId = consumer.Id,
                    Picture = request.Picture,
                    SkipAuthUpdate = true
                }, token);

                if (!resultImage.Success)
                    return Failed<bool>(resultImage.Exception);

                var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
                {
                    Email = consumer.Email,
                    FirstName = consumer.FirstName,
                    LastName = consumer.LastName,
                    Name = consumer.Name,
                    Phone = consumer.Phone,
                    Picture = consumer.Picture,
                    Roles = new List<Guid> { _roleOptions.Consumer.Id },
                    UserId = consumer.Id
                }, token);

                if (!authResult.Success)
                    return authResult;

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckConsumerConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var business = await _mediatr.Process(new CheckConsumerLegalConfigurationCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Process(new CheckWalletPaymentsConfigurationCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                return Ok(true);
            });
        }
    }
}
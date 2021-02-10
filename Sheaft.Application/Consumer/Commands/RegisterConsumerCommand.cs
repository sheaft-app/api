using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class RegisterConsumerCommand : Command<Guid>
    {
        [JsonConstructor]
        public RegisterConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string SponsoringCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
    }
    
    public class RegisterConsumerCommandHandler : CommandsHandler,
        IRequestHandler<RegisterConsumerCommand, Result<Guid>>
    {
        private readonly RoleOptions _roleOptions;

        public RegisterConsumerCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RegisterConsumerCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result<Guid>> Handle(RegisterConsumerCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
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
            return await ExecuteAsync(request, async () =>
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
            return await ExecuteAsync(request, async () =>
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

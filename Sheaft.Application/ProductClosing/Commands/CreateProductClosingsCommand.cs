using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.ProductClosing.Commands
{
    public class CreateProductClosingsCommand : Command<List<Guid>>
    {
        [JsonConstructor]
        public CreateProductClosingsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public List<ClosingInput> Closings { get; set; }
    }

    public class CreateProductClosingsCommandHandler : CommandsHandler,
        IRequestHandler<CreateProductClosingsCommand, Result<List<Guid>>>
    {
        public CreateProductClosingsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateProductClosingsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<List<Guid>>> Handle(CreateProductClosingsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var ids = new List<Guid>();
                foreach (var closing in request.Closings)
                {
                    var result = await _mediatr.Process(
                        new CreateProductClosingCommand(request.RequestUser)
                            {Closing = closing, ProductId = request.ProductId}, token);
                    
                    if (!result.Succeeded)
                        return Failure<List<Guid>>(result.Exception);

                    ids.Add(result.Data);
                }

                await transaction.CommitAsync(token);
                return Success(ids);
            }
        }
    }
}
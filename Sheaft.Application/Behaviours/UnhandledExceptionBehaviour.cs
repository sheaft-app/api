using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Common;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Behaviours
{
    internal class UnhandledExceptionBehaviour<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
        where TRequest : ITrackedUser
        where TResponse : Result
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken token,
            MediatR.RequestHandlerDelegate<TResponse> next)
        {
            var type = typeof(TRequest).Name;

            try
            {
                return await next();
            }
            catch (SheaftException sheaftException)
            {
                _logger.LogError(sheaftException, $"Sheaft error on executing {type} : {sheaftException.Message}");
                throw;
            }
            catch (NotSupportedException notSupported)
            {
                _logger.LogError(notSupported, $"Not supported error on executing {type} : {notSupported.Message}");
                throw new UnexpectedException(notSupported);
            }
            catch (InvalidOperationException invalidOperation)
            {
                if (invalidOperation.Source == "Microsoft.EntityFrameworkCore" &&
                    invalidOperation.Message.StartsWith("Enumerator failed to MoveNextAsync"))
                {
                    _logger.LogWarning(invalidOperation, $"Entity not found while processing {type}");
                    throw new NotFoundException(invalidOperation);
                }

                _logger.LogError(invalidOperation,
                    $"Invalid operation error on executing {type} : {invalidOperation.Message}");

                throw new UnexpectedException(invalidOperation);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unexpected error on executing {type} : {e.Message}");
                throw new UnexpectedException(e);
            }
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Mediator;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Behaviours
{
    internal class UnhandledExceptionBehaviour<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
        where TResponse: IResult
    {
        private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger;

        public UnhandledExceptionBehaviour(ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger)
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
            catch (NotSupportedException notSupported)
            {
                _logger.LogError(notSupported, $"Not supported error on executing {type} : {notSupported.Message}");
                throw;
            }
            catch (InvalidOperationException invalidOperation)
            {
                _logger.LogError(invalidOperation,
                    $"Invalid operation error on executing {type} : {invalidOperation.Message}");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unexpected error on executing {type} : {e.Message}");
                throw;
            }
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Application.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ITrackedUser
        where TResponse : Result
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
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
            catch (DbUpdateConcurrencyException dbUpdateConcurrency)
            {
                _logger.LogError(dbUpdateConcurrency,
                    $"DbConcurrency error on executing {type} : {dbUpdateConcurrency.Message}");

                throw SheaftException.Conflict(dbUpdateConcurrency);
            }
            catch (DbUpdateException dbUpdate)
            {
                _logger.LogError(dbUpdate, $"DbUpdate error on executing {type} : {dbUpdate.Message}");

                if (dbUpdate.InnerException != null &&
                    dbUpdate.InnerException.Message.Contains("duplicate key row"))
                    throw SheaftException.AlreadyExists(dbUpdate);

                throw SheaftException.BadRequest(dbUpdate);
            }
            catch (NotSupportedException notSupported)
            {
                _logger.LogError(notSupported, $"Not supported error on executing {type} : {notSupported.Message}");
                throw SheaftException.Unexpected(notSupported);
            }
            catch (InvalidOperationException invalidOperation)
            {
                if (invalidOperation.Source == "Microsoft.EntityFrameworkCore" &&
                    invalidOperation.Message.StartsWith("Enumerator failed to MoveNextAsync"))
                {
                    _logger.LogWarning(invalidOperation, $"Entity not found while processing {type}");
                    throw SheaftException.NotFound(invalidOperation);
                }

                _logger.LogError(invalidOperation,
                    $"Invalid operation error on executing {type} : {invalidOperation.Message}");

                throw SheaftException.Unexpected(invalidOperation);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unexpected error on executing {type} : {e.Message}");
                throw SheaftException.Unexpected(e);
            }
        }
    }
}
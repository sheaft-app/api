using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Sheaft.Exceptions;

namespace Sheaft.Application.Interop
{
    public class ResultsHandler
    {
        protected readonly ILogger _logger;
        protected readonly IMediator _mediatr;
        protected readonly IAppDbContext _context;
        protected readonly IQueueService _queueService;

        public ResultsHandler(
            IMediator mediatr, 
            IAppDbContext context, 
            IQueueService queueService, 
            ILogger logger)
        {
            _mediatr = mediatr;
            _context = context;
            _queueService = queueService;
            _logger = logger;
        }

        protected Result<T> Ok<T>(T result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Ok), result, message, objs);
            return new SuccessResult<T>(result, message, objs);
        }
        protected Result<T> ValidationError<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.ValidationError), message, objs);
            return new FailedResult<T>(new ValidationException(message, objs));
        }
        protected Result<T> BadRequest<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.BadRequest), message, objs);
            return new FailedResult<T>(new BadRequestException(message, objs));
        }
        protected Result<T> Conflict<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Conflict), message, objs);
            return new FailedResult<T>(new ConflictException(message, objs));
        }

        protected Result<T> Unauthorized<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Unauthorized), message, objs);
            return new FailedResult<T>(new UnauthorizedException(message, objs));
        }

        protected Result<T> Forbidden<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Forbidden), message, objs);
            return new FailedResult<T>(new ForbiddenException(message, objs));
        }

        protected Result<T> NotFound<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.NotFound), message, objs);
            return new FailedResult<T>(new NotFoundException(message, objs));
        }

        protected Result<T> Created<T>(T result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Created), result, message, objs);
            return new SuccessResult<T>(result, message, objs);
        }

        protected Result<T> Accepted<T>(T result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Accepted), result, message, objs);
            return new SuccessResult<T>(result, message);
        }

        protected Result<T> Locked<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Locked), message, objs);
            return new FailedResult<T>(new LockedException(message, objs));
        }

        protected Result<T> TooManyRetries<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Locked), message, objs);
            return new FailedResult<T>(new TooManyRetriesException(message, objs));
        }

        protected Result<T> InternalError<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.InternalError), message, objs);
            return new FailedResult<T>(new UnexpectedException(message, objs));
        }

        protected Result<T> Failed<T>(Exception exception, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Failed), exception, message, objs);
            return new FailedResult<T>(exception, message, objs);
        }

        protected Result<T> Failed<T>(SheaftException exception, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Failed), exception, message, objs);
            return new FailedResult<T>(exception, message, objs);
        }

        // IEnumerable

        protected Result<IEnumerable<T>> Ok<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Ok), result, message, objs);
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected Result<IEnumerable<T>> Created<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Created), result, message, objs);
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected Result<IEnumerable<T>> Accepted<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(ResultsHandler.Accepted), result, message, objs);
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected async Task<Result<T>> ExecuteAsync<T>(Func<Task<Result<T>>> method)
        {
            try
            {
                _logger.LogTrace($"{nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}");
                return await method();
            }
            catch (SheaftException e)
            {
                _logger.LogError($"SheaftException: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return Failed<T>(e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError($"DbUpdateConcurrencyException: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}, message: {{1}}", e, e.Message);
                return Failed<T>(e);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"DbUpdateException: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}, message: {{1}}", e, e.Message);
                return Failed<T>(e);
            }
            catch (NotSupportedException e)
            {
                _logger.LogError($"NotSupportedException: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}, message: {{1}}", e, e.Message);
                return Failed<T>(e);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"InvalidOperationException: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}, message: {{1}}", e, e.Message);
                return Failed<T>(e);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}, message: {{1}}", e, e.Message);
                return InternalError<T>();
            }
        }

        protected async Task<Result<IEnumerable<T>>> ExecuteAsync<T>(Func<Task<Result<IEnumerable<T>>>> method)
        {
            try
            {
                _logger.LogTrace($"{nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}");
                return await method();
            }
            catch (SheaftException e)
            {
                _logger.LogError($"SheaftException: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return Failed<IEnumerable<T>>(e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError($"DbUpdateConcurrencyException: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}, message: {{1}}", e, e.Message);
                return Failed<IEnumerable<T>>(e);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"DbUpdateException: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}, message: {{1}}", e, e.Message);
                return Failed<IEnumerable<T>>(e);
            }
            catch (NotSupportedException e)
            {
                _logger.LogError($"NotSupportedException: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}, message: {{1}}", e, e.Message);
                return Failed<IEnumerable<T>>(e);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"InvalidOperationException: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}, message: {{1}}", e, e.Message);
                return Failed<IEnumerable<T>>(e);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}, message: {{1}}", e, e.Message);
                return InternalError<IEnumerable<T>>();
            }
        }
    }
}
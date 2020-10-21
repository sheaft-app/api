using HotChocolate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using Sheaft.Exceptions;
using Sheaft.Localization;
using System;
using System.Collections.Generic;

namespace Sheaft.GraphQL.Services
{
    public class SheaftErrorFilter : IErrorFilter
    {
        private readonly IStringLocalizer<MessageResources> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SheaftErrorFilter> _logger;
        private RequestUser CurrentUser
        {
            get
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    return _httpContextAccessor.HttpContext.User.ToIdentityUser(_httpContextAccessor.HttpContext.TraceIdentifier);
                else
                    return new RequestUser(_httpContextAccessor.HttpContext.TraceIdentifier);
            }
        }

        public SheaftErrorFilter(ILogger<SheaftErrorFilter> logger, IStringLocalizer<MessageResources> localizer, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _localizer = localizer;
        }

        public IError OnError(IError error)
        {
            error = error.AddExtension("RequestIdentifier", _httpContextAccessor.HttpContext.TraceIdentifier);
            var message = "Une erreur inattendue est survenue, veuillez renouveler votre demande. Si l'erreur persiste, contactez notre support.";

            var kind = ExceptionKind.Unexpected;
            var exception = error.Exception;

            var statusCode = 400;
            if (error.Exception != null && error.Exception is SheaftException exc)
            {
                exception = exc;

                if (exc.Error.HasValue)
                    message = _localizer[exc.Error.Value.ToString("G"), exc.Params ?? new object[] { }];

                error = error.WithCode(exc.Kind.ToString("G"));
                error = error.WithMessage(message);

                error = error.AddExtension(exc.Kind.ToString("G"), message);
                kind = exc.Kind;

                switch (exc.Kind)
                {
                    case ExceptionKind.Conflict:
                        statusCode = 409;
                        break;
                    case ExceptionKind.Forbidden:
                        statusCode = 403;
                        break;
                    case ExceptionKind.Gone:
                        statusCode = 410;
                        break;
                    case ExceptionKind.Locked:
                        statusCode = 423;
                        break;
                    case ExceptionKind.NotFound:
                        statusCode = 404;
                        break;
                    case ExceptionKind.Unauthorized:
                        statusCode = 401;
                        break;
                    case ExceptionKind.Unexpected:
                        statusCode = 500;
                        break;
                    case ExceptionKind.TooManyRetries:
                    case ExceptionKind.BadRequest:
                    case ExceptionKind.Validation:
                    case ExceptionKind.AlreadyExists:
                    default:
                        statusCode = 400;
                        break;
                }
            }
            else
            {
                statusCode = 400;
                error = error.WithMessage(message);
            }

            if (error.Code == "AUTH_NOT_AUTHORIZED")
            {
                statusCode = 401;
                kind = ExceptionKind.Unauthorized;
                message = _localizer[ExceptionKind.Unauthorized.ToString("G")];
                error = error.WithCode(ExceptionKind.Unauthorized.ToString("G"));
                error = error.WithMessage(message);

                error = error.AddExtension(ExceptionKind.Unauthorized.ToString("G"), message);
            }

            NewRelic.Api.Agent.NewRelic.SetTransactionName("StatusCode", statusCode.ToString());

            var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
            currentTransaction.AddCustomAttribute("ExceptionKind", kind);
            currentTransaction.AddCustomAttribute("ExceptionMessage", message);

            var parameters = new Dictionary<string, object>
            {
                { "RequestId", _httpContextAccessor.HttpContext.TraceIdentifier },
                { "UserIdentifier", CurrentUser.Id.ToString("N") },
                { "IsAuthenticated", CurrentUser.IsAuthenticated.ToString() },
                { "Roles", string.Join(";", CurrentUser.Roles) },
                { "StatusCode", statusCode },
                { "ExceptionKind", kind },
                { "ExceptionMessage", message },
            };

            NewRelic.Api.Agent.NewRelic.NoticeError(exception, parameters);
            return error;
        }
    }
}
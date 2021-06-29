using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Localization;

namespace Sheaft.GraphQL
{
    public class SheaftErrorFilter : IErrorFilter
    {
        private readonly IStringLocalizer<MessageResources> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SheaftErrorFilter(
            IStringLocalizer<MessageResources> localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public IError OnError(IError error)
        {
            var message = "Une erreur inattendue est survenue.";
            var extensions = error.Extensions?.ToDictionary(pair => pair.Key, pair => pair.Value) ?? new Dictionary<string, object?>();
            
            extensions.Add("RequestIdentifier", _httpContextAccessor.HttpContext.TraceIdentifier);

            var kind = ExceptionKind.Unexpected;
            var exception = error.Exception;
            string code = null;

            var statusCode = 400;
            if (error.Exception != null && error.Exception is SheaftException exc)
            {
                exception = exc;
                code = exc.Kind.ToString("G");

                if (exc.Error.HasValue)
                    message = _localizer[exc.Error.Value.ToString("G"), exc.Params ?? new object[] { }];

                kind = exc.Kind;

                message = message != "Une erreur inattendue est survenue."
                    ? message
                    : _localizer[exc.Kind.ToString("G")];
                
                extensions.Add(exc.Kind.ToString("G"), message);

                switch (exc.Kind)
                {
                    case ExceptionKind.Conflict:
                        statusCode = 409;
                        break;
                    case ExceptionKind.Forbidden:
                    case ExceptionKind.Unauthorized:
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
                    case ExceptionKind.Unauthenticated:
                        statusCode = 401;
                        break;
                    case ExceptionKind.Unexpected:
                        statusCode = 500;
                        break;
                    default:
                        statusCode = 400;
                        break;
                }
            }
            else if (error.Exception != null && error.Exception is GraphQLException e)
            {
                exception = e;
                message = e.Message;
            }
            else
            {
                statusCode = 400;
                code = MessageKind.BadRequest.ToString("G");
                message = error.Message;
            }

            if (error.Code == "AUTH_NOT_AUTHENTICATED")
            {
                statusCode = 401;
                kind = ExceptionKind.Unauthenticated;
                message = _localizer[ExceptionKind.Unauthenticated.ToString("G")];
                code = ExceptionKind.Unauthenticated.ToString("G");
                extensions.Add(ExceptionKind.Unauthenticated.ToString("G"), message);
            }

            if (error.Code == "AUTH_NOT_AUTHORIZED")
            {
                statusCode = 403;
                kind = ExceptionKind.Forbidden;
                message = _localizer[ExceptionKind.Forbidden.ToString("G")];
                code = ExceptionKind.Forbidden.ToString("G");
                extensions.Add(ExceptionKind.Forbidden.ToString("G"), message);
            }

            if (exception != null)
                extensions.Add("StackTrace", exception.StackTrace);

            extensions.Add("StatusCode", statusCode);
            error = error.WithExtensions(extensions);
            error = error.WithCode(code);
            error = error.WithMessage(message);

            NewRelic.Api.Agent.NewRelic.SetTransactionName("StatusCode", statusCode.ToString());

            var currentTransaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
            currentTransaction.AddCustomAttribute("ExceptionKind", kind);
            currentTransaction.AddCustomAttribute("ExceptionMessage", message);

            var parameters = new Dictionary<string, object>
            {
                {"RequestId", _httpContextAccessor.HttpContext.TraceIdentifier},
                // {"UserIdentifier", CurrentUser.Id.ToString("N")},
                // {"IsAuthenticated", CurrentUser.IsAuthenticated.ToString()},
                // {"Roles", string.Join(";", CurrentUser.Roles)},
                {"StatusCode", statusCode},
                {"ExceptionKind", kind},
                {"ExceptionMessage", message},
            };

            if(exception != null)
                NewRelic.Api.Agent.NewRelic.NoticeError(exception, parameters);
            
            return error;
        }
    }
}
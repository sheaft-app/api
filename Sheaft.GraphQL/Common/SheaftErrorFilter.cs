using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Localization;

namespace Sheaft.GraphQL.Common
{
    public class SheaftErrorFilter : IErrorFilter
    {
        private readonly IStringLocalizer<MessageResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private RequestUser CurrentUser => _currentUserService.GetCurrentUserInfo().Data;

        public SheaftErrorFilter(
            ICurrentUserService currentUserService,
            IStringLocalizer<MessageResources> localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public IError OnError(IError error)
        {
            var message = "Une erreur inattendue est survenue.";
            var extensions = error.Extensions.ToDictionary(pair => pair.Key, pair => pair.Value);
            
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
                    default:
                        statusCode = 400;
                        break;
                }
            }
            else
            {
                statusCode = 400;
                code = MessageKind.BadRequest.ToString("G");
            }

            if (error.Code == "AUTH_NOT_AUTHORIZED")
            {
                statusCode = 401;
                kind = ExceptionKind.Unauthorized;
                message = _localizer[ExceptionKind.Unauthorized.ToString("G")];
                code = ExceptionKind.Unauthorized.ToString("G");
                extensions.Add(ExceptionKind.Unauthorized.ToString("G"), message);
            }

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
                {"UserIdentifier", CurrentUser.Id.ToString("N")},
                {"IsAuthenticated", CurrentUser.IsAuthenticated.ToString()},
                {"Roles", string.Join(";", CurrentUser.Roles)},
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
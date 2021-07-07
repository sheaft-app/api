using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;

namespace Sheaft.GraphQL
{
    public class SheaftErrorFilter : IErrorFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SheaftErrorFilter(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
            switch (error.Exception)
            {
                case SheaftException exc:
                    exception = exc;
                    code = exc.Kind.ToString("G");
                    message = !string.IsNullOrWhiteSpace(exc.Error) ? exc.Error : "Une erreur inattendue est survenue.";
                    kind = exc.Kind;
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

                    break;
                case GraphQLException e:
                    exception = e;
                    message = e.Message;
                    break;
                default:
                    statusCode = 400;
                    code = "BadRequest";
                    message = error.Message;
                    break;
            }

            switch (error.Code)
            {
                case "AUTH_NOT_AUTHENTICATED":
                    statusCode = 401;
                    kind = ExceptionKind.Unauthenticated;
                    message = "Vous devez être connecté pour accéder à la ressource.";
                    code = ExceptionKind.Unauthenticated.ToString("G");
                    extensions.Add(ExceptionKind.Unauthenticated.ToString("G"), message);
                    break;
                case "AUTH_NOT_AUTHORIZED":
                    statusCode = 403;
                    kind = ExceptionKind.Forbidden;
                    message = "Vous n'êtes pas autorisé à accéder à la ressource.";
                    code = ExceptionKind.Forbidden.ToString("G");
                    extensions.Add(ExceptionKind.Forbidden.ToString("G"), message);
                    break;
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
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Sheaft.Exceptions;
using Sheaft.Localization;

namespace Sheaft.GraphQL.Services
{
    public class SheaftErrorFilter : IErrorFilter
    {
        private readonly IStringLocalizer<MessageResources> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SheaftErrorFilter> _logger;

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
            if (error.Exception != null && error.Exception is SheaftException)
            {
                var exc = error.Exception as SheaftException;

                if (exc.Error.HasValue)
                    message = _localizer[exc.Error.Value.ToString("G"), exc.Params ?? new object[] { }];

                error = error.WithCode(exc.Kind.ToString("G"));
                error = error.WithMessage(message);

                error = error.AddExtension(exc.Kind.ToString("G"), message);

                _logger.LogError(exc, $"{error.Code} -  {error.Message}", error);
            }
            else
            {
                error = error.WithMessage(message);
                _logger.LogError(error.Exception, $"{error.Code} -  {error.Message}", error);
            }

            if (error.Code == "AUTH_NOT_AUTHORIZED")
            {
                error = error.WithCode(ExceptionKind.Unauthorized.ToString("G"));
                error = error.WithMessage(_localizer[ExceptionKind.Unauthorized.ToString("G")]);

                error = error.AddExtension(ExceptionKind.Unauthorized.ToString("G"), _localizer[ExceptionKind.Unauthorized.ToString("G")]);
            }

            return error;
        }
    }
}
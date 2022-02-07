using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace Sheaft.Application.Behaviours
{
    internal class ValidationBehaviour<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
        where TRequest : MediatR.IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken token,
            MediatR.RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults =
                await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, token)));
            
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
            if (failures.Count != 0)
                throw new ValidationException("An error occured while validating data.", failures.Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage)));

            return await next();
        }
    }
}
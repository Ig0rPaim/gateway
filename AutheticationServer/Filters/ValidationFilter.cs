using AutheticationServer.Extensions;
using FluentValidation;

namespace AutheticationServer.Filters
{
    public class ValidationFilter<T> : IEndpointFilter where T : class
    {
        private readonly IValidator<T> _validator;
        public ValidationFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context,
                      EndpointFilterDelegate next)
        {
            var parameter = context.Arguments.SingleOrDefault(p => p.GetType() == typeof(T));

            if (parameter is null) return Results.BadRequest("O parametro é inválido.");

            var result = await _validator.ValidateAsync((T)parameter);

            if (!result.IsValid)
            {
                var errors = result.Errors.GetErrors();
                return Results.Problem(errors);
            }
            // now the actual endpoint execution
            return await next(context);
        }
    }

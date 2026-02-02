using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetUserSales;

public static class GetUserSalesValidator
{
    public static IValidator<GetUserSalesCommand> Rules { get; } =
        new InlineValidator<GetUserSalesCommand>
        {
            r => r.RuleFor(c => c.CustomerId).NotEmpty(),
            r => r.RuleFor(c => c.PageNumber).GreaterThan(0),
            r => r.RuleFor(c => c.PageSize).GreaterThan(0),
        };

    public static void ValidateAndThrow(this GetUserSalesCommand command) => Rules.ValidateAndThrow(command);
}

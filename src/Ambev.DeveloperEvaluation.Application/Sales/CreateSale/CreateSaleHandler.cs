using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Sales.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler(
    ILogger<CreateSaleHandler> logger,
    ISaleDiscountService discountService,
    ISaleRepository saleRepository
) : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var now = DateTimeOffset.UtcNow;

        var sale = request.ToSaleEntity(now);
        await discountService.ApplyDiscounts(sale, request.Discounts, cancellationToken);

        await saleRepository.Create(sale, cancellationToken);

        logger.LogInformation("New sale created: {SaleId}", sale.Id);
        return new CreateSaleResult { Id = sale.Id };
    }
}

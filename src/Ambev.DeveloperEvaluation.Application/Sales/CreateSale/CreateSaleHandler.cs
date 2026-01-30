using Ambev.DeveloperEvaluation.Domain.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Sales.Services;
using Ambev.DeveloperEvaluation.Domain.Sales.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler(
    ILogger<CreateSaleHandler> logger,
    ISaleDiscountService discountService,
    ISaleRepository saleRepository,
    IPublisher publisher
) : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        request.ValidateAndThrow();

        var now = DateTimeOffset.UtcNow;

        var sale = request.ToSaleEntity(now);
        await discountService.ApplyDiscounts(sale, request.Discounts, cancellationToken);

        sale.ValidateAndThrow();

        await saleRepository.Create(sale, cancellationToken);

        logger.LogInformation("New sale created: {SaleId}", sale.Id);
        await publisher.Publish(new SaleCreated { SaleId = sale.Id, Timestamp = now }, cancellationToken);
        return new CreateSaleResult { Id = sale.Id };
    }
}

using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler(IMapper mapper, IDiscountRepository discountRepository, ISaleRepository saleRepository)
    : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var now = DateTimeOffset.UtcNow;
        var discounts = await discountRepository
            .GetDiscounts(request.Discounts, cancellationToken)
            .ToDictionaryAsync(x => x.Id, cancellationToken: cancellationToken);

        var sale = request.ToSaleEntity(discounts, now);

        await saleRepository.Create(sale, cancellationToken);

        return new CreateSaleResult { Id = sale.Id };
    }
}

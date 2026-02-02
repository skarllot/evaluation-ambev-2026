using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleProfile : Profile
{
    protected CreateSaleProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
    }
}
using Ambev.DeveloperEvaluation.Application.Sales.GetUserSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetUserSales;

public class GetUserSalesProfile : Profile
{
    public GetUserSalesProfile()
    {
        CreateMap<GetUserSalesItem, GetUserSalesResponseItem>();
    }
}

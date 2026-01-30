using Ambev.DeveloperEvaluation.Domain.Sales.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class DomainModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAutomaticDiscount, FourItemsAutomaticDiscount>();
        builder.Services.AddScoped<IAutomaticDiscount, TenItemsAutomaticDiscount>();
        builder.Services.AddScoped<ISaleDiscountService, SaleDiscountService>();
    }
}

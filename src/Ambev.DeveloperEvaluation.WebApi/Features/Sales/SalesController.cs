using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public class SalesController(IMapper mapper, IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            BadRequest(new ApiResponse(validationResult));
        }

        var command = mapper.Map<CreateSaleCommand>(request) with { CustomerId = GetCurrentUserId() };
        var result = await mediator.Send(command);

        return Created(
            string.Empty,
            new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = mapper.Map<CreateSaleResponse>(result),
            }
        );
    }
}

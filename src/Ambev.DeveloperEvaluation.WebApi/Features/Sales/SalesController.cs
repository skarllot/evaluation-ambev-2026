using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetUserSales;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetUserSales;
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
        var result = await mediator.Send(command, HttpContext.RequestAborted);

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

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedResponse<GetUserSalesResponseItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserSales(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] DateTimeOffset? startDate = null,
        [FromQuery] DateTimeOffset? endDate = null
    )
    {
        var command = new GetUserSalesCommand
        {
            CustomerId = GetCurrentUserId(),
            StartDate = startDate,
            EndDate = endDate,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
        var result = await mediator.Send(command, HttpContext.RequestAborted);

        return Ok(
            new PaginatedResponse<GetUserSalesResponseItem>
            {
                Success = true,
                Message = "Sales retrieved successfully",
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize),
                TotalCount = result.TotalCount,
                Data = mapper.Map<List<GetUserSalesResponseItem>>(result.Items),
            }
        );
    }
}

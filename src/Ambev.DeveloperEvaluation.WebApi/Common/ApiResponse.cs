using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class ApiResponse
{
    public ApiResponse() { }

    public ApiResponse(ValidationResult validationResult)
    {
        Success = validationResult.IsValid;
        Errors = validationResult.Errors.Select(e => (ValidationErrorDetail)e).ToList();
    }

    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<ValidationErrorDetail> Errors { get; set; } = [];
}

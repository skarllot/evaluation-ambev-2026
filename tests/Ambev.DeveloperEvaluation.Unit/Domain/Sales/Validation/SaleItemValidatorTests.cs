using Ambev.DeveloperEvaluation.Domain.Sales.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Sales.Validation.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales.Validation;

public class SaleItemValidatorTests
{
    [Fact(DisplayName = "Valid sale item should pass validation")]
    public void Given_ValidSaleItem_When_Validated_Then_ShouldNotHaveErrors()
    {
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        var result = SaleItemValidator.Rules.TestValidate(saleItem);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Empty product ID should fail validation")]
    public void Given_EmptyProductId_When_Validated_Then_ShouldHaveError()
    {
        var saleItem = SaleItemTestData.GenerateSaleItemWithEmptyProductId();
        var result = SaleItemValidator.Rules.TestValidate(saleItem);
        result.ShouldHaveValidationErrorFor(si => si.ProductId);
    }

    [Theory(DisplayName = "Invalid quantity values should fail validation")]
    [InlineData(0)] // Below minimum
    [InlineData(-1)] // Negative
    [InlineData(21)] // Above maximum
    [InlineData(100)] // Well over maximum
    public void Given_InvalidQuantity_When_Validated_Then_ShouldHaveError(int quantity)
    {
        var saleItem = SaleItemTestData.GenerateSaleItemWithInvalidQuantity(quantity);
        var result = SaleItemValidator.Rules.TestValidate(saleItem);
        result.ShouldHaveValidationErrorFor(si => si.Quantity.Value);
    }

    [Theory(DisplayName = "Valid quantity edge cases should pass validation")]
    [InlineData(1)] // Minimum valid
    [InlineData(20)] // Maximum valid
    [InlineData(10)] // Mid-range
    public void Given_ValidQuantityEdgeCases_When_Validated_Then_ShouldNotHaveErrors(int quantity)
    {
        var saleItem = SaleItemTestData.GenerateSaleItemWithInvalidQuantity(quantity);
        var result = SaleItemValidator.Rules.TestValidate(saleItem);
        result.ShouldNotHaveValidationErrorFor(si => si.Quantity.Value);
    }

    [Theory(DisplayName = "Invalid price values should fail validation")]
    [InlineData(0)] // Zero
    [InlineData(-10)] // Negative
    [InlineData(1_000_000)] // Exceeds maximum
    [InlineData(2_000_000)] // Well over maximum
    public void Given_InvalidPrice_When_Validated_Then_ShouldHaveError(decimal price)
    {
        var saleItem = SaleItemTestData.GenerateSaleItemWithInvalidPrice(price);
        var result = SaleItemValidator.Rules.TestValidate(saleItem);
        result.ShouldHaveValidationErrorFor(si => si.UnitPrice.Amount);
    }

    [Fact(DisplayName = "Price with more than 2 decimal places should fail validation")]
    public void Given_PriceWithHighPrecision_When_Validated_Then_ShouldHaveError()
    {
        var saleItem = SaleItemTestData.GenerateSaleItemWithHighPrecisionPrice();
        var result = SaleItemValidator.Rules.TestValidate(saleItem);
        result.ShouldHaveValidationErrorFor(si => si.UnitPrice.Amount);
    }

    [Theory(DisplayName = "Valid price edge cases should pass validation")]
    [InlineData(0.01)] // Minimum valid
    [InlineData(999999.99)] // Maximum valid
    [InlineData(500)] // Mid-range
    [InlineData(19.99)] // Common price
    public void Given_ValidPriceEdgeCases_When_Validated_Then_ShouldNotHaveErrors(decimal price)
    {
        var saleItem = SaleItemTestData.GenerateSaleItemWithInvalidPrice(price);
        var result = SaleItemValidator.Rules.TestValidate(saleItem);
        result.ShouldNotHaveValidationErrorFor(si => si.UnitPrice.Amount);
    }
}

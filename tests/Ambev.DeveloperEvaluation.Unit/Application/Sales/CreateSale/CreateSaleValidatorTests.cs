using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

public class CreateSaleValidatorTests
{
    [Fact(DisplayName = "Valid command should pass validation")]
    public void Given_ValidCommand_When_Validated_Then_ShouldNotHaveErrors()
    {
        var command = CreateSaleCommandTestData.GenerateValidCommand();
        var result = CreateSaleValidator.Rules.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Empty customer ID should fail validation")]
    public void Given_EmptyCustomerId_When_Validated_Then_ShouldHaveError()
    {
        var command = CreateSaleCommandTestData.GenerateCommandWithEmptyCustomerId();
        var result = CreateSaleValidator.Rules.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.CustomerId);
    }

    [Fact(DisplayName = "Empty branch ID should fail validation")]
    public void Given_EmptyBranchId_When_Validated_Then_ShouldHaveError()
    {
        var command = CreateSaleCommandTestData.GenerateCommandWithEmptyBranchId();
        var result = CreateSaleValidator.Rules.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.BranchId);
    }

    [Fact(DisplayName = "Item with empty product ID should fail validation")]
    public void Given_ItemWithEmptyProductId_When_Validated_Then_ShouldHaveError()
    {
        var command = CreateSaleCommandTestData.GenerateCommandWithInvalidItem(productId: Guid.Empty);
        var result = CreateSaleValidator.Rules.TestValidate(command);
        result.ShouldHaveValidationErrorFor("Items[0].ProductId");
    }

    [Theory(DisplayName = "Item with invalid quantity should fail validation")]
    [InlineData(0)] // Below minimum
    [InlineData(-1)] // Negative
    [InlineData(21)] // Above maximum
    [InlineData(100)] // Well over maximum
    public void Given_ItemWithInvalidQuantity_When_Validated_Then_ShouldHaveError(int quantity)
    {
        var command = CreateSaleCommandTestData.GenerateCommandWithInvalidItem(quantity: quantity);
        var result = CreateSaleValidator.Rules.TestValidate(command);
        result.ShouldHaveValidationErrorFor("Items[0].Quantity");
    }

    [Theory(DisplayName = "Item with valid quantity edge cases should pass validation")]
    [InlineData(1)] // Minimum valid
    [InlineData(20)] // Maximum valid
    [InlineData(10)] // Mid-range
    public void Given_ItemWithValidQuantityEdgeCases_When_Validated_Then_ShouldNotHaveErrors(int quantity)
    {
        var command = CreateSaleCommandTestData.GenerateCommandWithInvalidItem(quantity: quantity);
        var result = CreateSaleValidator.Rules.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor("Items[0].Quantity");
    }

    [Theory(DisplayName = "Item with invalid unit price should fail validation")]
    [InlineData(0)] // Zero
    [InlineData(-10)] // Negative
    public void Given_ItemWithInvalidUnitPrice_When_Validated_Then_ShouldHaveError(decimal unitPrice)
    {
        var command = CreateSaleCommandTestData.GenerateCommandWithInvalidItem(unitPrice: unitPrice);
        var result = CreateSaleValidator.Rules.TestValidate(command);
        result.ShouldHaveValidationErrorFor("Items[0].UnitPrice");
    }

    [Fact(DisplayName = "Item with unit price having more than 2 decimal places should fail validation")]
    public void Given_ItemWithHighPrecisionUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        var command = CreateSaleCommandTestData.GenerateCommandWithInvalidItem(unitPrice: 10.123m);
        var result = CreateSaleValidator.Rules.TestValidate(command);
        result.ShouldHaveValidationErrorFor("Items[0].UnitPrice");
    }

    [Theory(DisplayName = "Item with valid unit price edge cases should pass validation")]
    [InlineData(0.01)] // Very small
    [InlineData(10.50)] // Common price
    [InlineData(999.99)] // Large price
    public void Given_ItemWithValidUnitPriceEdgeCases_When_Validated_Then_ShouldNotHaveErrors(decimal unitPrice)
    {
        var command = CreateSaleCommandTestData.GenerateCommandWithInvalidItem(unitPrice: unitPrice);
        var result = CreateSaleValidator.Rules.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor("Items[0].UnitPrice");
    }

    [Fact(DisplayName = "Empty discount ID should fail validation")]
    public void Given_EmptyDiscountId_When_Validated_Then_ShouldHaveError()
    {
        var command = CreateSaleCommandTestData.GenerateCommandWithEmptyDiscount();
        var result = CreateSaleValidator.Rules.TestValidate(command);
        result.ShouldHaveValidationErrorFor("Discounts[0]");
    }
}

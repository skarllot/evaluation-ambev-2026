using Ambev.DeveloperEvaluation.Domain.Sales.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Sales.Validation.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales.Validation;

public class DiscountValidatorTests
{
    [Fact(DisplayName = "Valid discount with multiplier should pass validation")]
    public void Given_ValidDiscountWithMultiplier_When_Validated_Then_ShouldNotHaveErrors()
    {
        var discount = DiscountTestData.GenerateValidDiscountWithMultiplier();
        var result = DiscountValidator.Rules.TestValidate(discount);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Valid discount with amount should pass validation")]
    public void Given_ValidDiscountWithAmount_When_Validated_Then_ShouldNotHaveErrors()
    {
        var discount = DiscountTestData.GenerateValidDiscountWithAmount();
        var result = DiscountValidator.Rules.TestValidate(discount);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Discount with neither multiplier nor amount should fail validation")]
    public void Given_DiscountWithNeitherValue_When_Validated_Then_ShouldHaveError()
    {
        var discount = DiscountTestData.GenerateInvalidDiscountWithNeitherValue();
        var result = DiscountValidator.Rules.TestValidate(discount);
        result.ShouldHaveValidationErrorFor(d => d);
    }

    [Fact(DisplayName = "Discount with both multiplier and amount should fail validation")]
    public void Given_DiscountWithBothValues_When_Validated_Then_ShouldHaveError()
    {
        var discount = DiscountTestData.GenerateInvalidDiscountWithBothValues();
        var result = DiscountValidator.Rules.TestValidate(discount);
        result.ShouldHaveValidationErrorFor(d => d);
    }

    [Fact(DisplayName = "Discount with empty code should fail validation")]
    public void Given_EmptyCode_When_Validated_Then_ShouldHaveError()
    {
        var discount = DiscountTestData.GenerateDiscountWithEmptyCode();
        var result = DiscountValidator.Rules.TestValidate(discount);
        result.ShouldHaveValidationErrorFor(d => d.Code.Value);
    }

    [Theory(DisplayName = "Invalid multiplier values should fail validation")]
    [InlineData(0)] // Zero
    [InlineData(-0.1)] // Negative
    [InlineData(1)] // 100%
    [InlineData(1.5)] // Over 100%
    public void Given_InvalidMultiplier_When_Validated_Then_ShouldHaveError(decimal multiplier)
    {
        var discount = DiscountTestData.GenerateDiscountWithInvalidMultiplier(multiplier);
        var result = DiscountValidator.Rules.TestValidate(discount);
        result.ShouldHaveValidationErrorFor(d => d.Multiplier);
    }

    [Fact(DisplayName = "Multiplier with more than 4 decimal places should fail validation")]
    public void Given_MultiplierWithHighPrecision_When_Validated_Then_ShouldHaveError()
    {
        var discount = DiscountTestData.GenerateDiscountWithHighPrecisionMultiplier();
        var result = DiscountValidator.Rules.TestValidate(discount);
        result.ShouldHaveValidationErrorFor(d => d.Multiplier);
    }

    [Theory(DisplayName = "Invalid amount values should fail validation")]
    [InlineData(0)] // Zero
    [InlineData(-10)] // Negative
    [InlineData(100_000)] // Exceeds maximum
    [InlineData(150_000)] // Well over maximum
    public void Given_InvalidAmount_When_Validated_Then_ShouldHaveError(decimal amount)
    {
        var discount = DiscountTestData.GenerateDiscountWithInvalidAmount(amount);
        var result = DiscountValidator.Rules.TestValidate(discount);
        result.ShouldHaveValidationErrorFor(d => d.Amount);
    }

    [Fact(DisplayName = "Amount with more than 2 decimal places should fail validation")]
    public void Given_AmountWithHighPrecision_When_Validated_Then_ShouldHaveError()
    {
        var discount = DiscountTestData.GenerateDiscountWithHighPrecisionAmount();
        var result = DiscountValidator.Rules.TestValidate(discount);
        result.ShouldHaveValidationErrorFor(d => d.Amount);
    }

    [Theory(DisplayName = "Valid multiplier edge cases should pass validation")]
    [InlineData(0.0001)] // Minimum valid
    [InlineData(0.9999)] // Maximum valid
    [InlineData(0.5)] // Mid-range
    public void Given_ValidMultiplierEdgeCases_When_Validated_Then_ShouldNotHaveErrors(decimal multiplier)
    {
        var discount = DiscountTestData.GenerateDiscountWithInvalidMultiplier(multiplier);
        var result = DiscountValidator.Rules.TestValidate(discount);
        result.ShouldNotHaveValidationErrorFor(d => d.Multiplier);
    }

    [Theory(DisplayName = "Valid amount edge cases should pass validation")]
    [InlineData(0.01)] // Minimum valid
    [InlineData(99999.99)] // Maximum valid
    [InlineData(500)] // Mid-range
    public void Given_ValidAmountEdgeCases_When_Validated_Then_ShouldNotHaveErrors(decimal amount)
    {
        var discount = DiscountTestData.GenerateDiscountWithInvalidAmount(amount);
        var result = DiscountValidator.Rules.TestValidate(discount);
        result.ShouldNotHaveValidationErrorFor(d => d.Amount);
    }
}

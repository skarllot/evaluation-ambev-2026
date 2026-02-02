using Ambev.DeveloperEvaluation.Domain.Sales.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Sales.Validation.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales.Validation;

public class SaleValidatorTests
{
    [Fact(DisplayName = "Valid sale should pass validation")]
    public void Given_ValidSale_When_Validated_Then_ShouldNotHaveErrors()
    {
        var sale = SaleTestData.GenerateValidSale();
        var result = SaleValidator.Rules.TestValidate(sale);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "Invalid sale number should fail validation")]
    [InlineData(-1)] // Negative
    [InlineData(-100)] // More negative
    public void Given_InvalidSaleNumber_When_Validated_Then_ShouldHaveError(long number)
    {
        var sale = SaleTestData.GenerateSaleWithInvalidNumber(number);
        var result = SaleValidator.Rules.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.Number);
    }

    [Theory(DisplayName = "Valid sale number edge cases should pass validation")]
    [InlineData(0)] // Zero (before database-generated value)
    [InlineData(1)] // Minimum valid
    [InlineData(100)] // Common value
    [InlineData(999999)] // Large value
    public void Given_ValidSaleNumberEdgeCases_When_Validated_Then_ShouldNotHaveErrors(long number)
    {
        var sale = SaleTestData.GenerateSaleWithInvalidNumber(number);
        var result = SaleValidator.Rules.TestValidate(sale);
        result.ShouldNotHaveValidationErrorFor(s => s.Number);
    }

    [Fact(DisplayName = "Empty customer ID should fail validation")]
    public void Given_EmptyCustomerId_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateSaleWithEmptyCustomerId();
        var result = SaleValidator.Rules.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.CustomerId);
    }

    [Fact(DisplayName = "Empty branch ID should fail validation")]
    public void Given_EmptyBranchId_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateSaleWithEmptyBranchId();
        var result = SaleValidator.Rules.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.BranchId);
    }

    [Fact(DisplayName = "Empty created date should fail validation")]
    public void Given_EmptyCreatedAt_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateSaleWithEmptyCreatedAt();
        var result = SaleValidator.Rules.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.CreatedAt);
    }

    [Fact(DisplayName = "Duplicate products in items should fail validation")]
    public void Given_DuplicateProducts_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateSaleWithDuplicateProducts();
        var result = SaleValidator.Rules.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.Items);
    }

    [Fact(DisplayName = "Duplicate discounts should fail validation")]
    public void Given_DuplicateDiscounts_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateSaleWithDuplicateDiscounts();
        var result = SaleValidator.Rules.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.Discounts);
    }
}

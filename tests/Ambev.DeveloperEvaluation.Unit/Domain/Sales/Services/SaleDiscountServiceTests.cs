using System.Collections.Immutable;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Sales.Services;
using Ambev.DeveloperEvaluation.Unit.Domain.Sales.Services.TestData;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales.Services;

/// <summary>
/// Contains unit tests for the <see cref="SaleDiscountService"/> class.
/// </summary>
public class SaleDiscountServiceTests
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IAutomaticDiscount _automaticDiscount1;
    private readonly IAutomaticDiscount _automaticDiscount2;
    private readonly SaleDiscountService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleDiscountServiceTests"/> class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public SaleDiscountServiceTests()
    {
        _discountRepository = Substitute.For<IDiscountRepository>();
        _automaticDiscount1 = Substitute.For<IAutomaticDiscount>();
        _automaticDiscount2 = Substitute.For<IAutomaticDiscount>();

        var automaticDiscounts = new List<IAutomaticDiscount> { _automaticDiscount1, _automaticDiscount2 };

        _service = new SaleDiscountService(_discountRepository, automaticDiscounts);
    }

    /// <summary>
    /// Tests that when a sale has discounts and requested discounts is empty,
    /// the existing discounts are cleared and automatic discounts are applied.
    /// </summary>
    [Fact(
        DisplayName = "Given sale has discounts and requested discounts is empty When applying discounts Then clears existing discounts and applies automatic"
    )]
    public async Task ApplyDiscounts_SaleHasDiscountsAndRequestedIsEmpty_ClearsDiscountsAndAppliesAutomatic()
    {
        // Given
        var existingDiscount = SaleDiscountServiceTestData.GenerateDiscount(code: "EXISTING10");
        var sale = SaleDiscountServiceTestData.GenerateSale(4);
        sale.Discounts.Add(existingDiscount);

        var requestedDiscounts = ImmutableList<Guid>.Empty;

        SetupDiscountRepository(new Dictionary<Guid, Discount>());

        // When
        await _service.ApplyDiscounts(sale, requestedDiscounts, CancellationToken.None);

        // Then
        sale.Discounts.Should().BeEmpty();
        await _automaticDiscount1.Received(1).TryApply(sale, Arg.Any<CancellationToken>());
        await _automaticDiscount2.Received(1).TryApply(sale, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that when a sale has discounts and requested discounts are not equal,
    /// the existing discounts are replaced with the requested ones.
    /// </summary>
    [Fact(
        DisplayName = "Given sale has discounts and requested discounts are not equal When applying discounts Then replaces discounts"
    )]
    public async Task ApplyDiscounts_SaleHasDiscountsAndRequestedAreNotEqual_ReplacesDiscounts()
    {
        // Given
        var existingDiscount = SaleDiscountServiceTestData.GenerateDiscount(code: "EXISTING10");
        var requestedDiscount = SaleDiscountServiceTestData.GenerateDiscount(code: "NEW20");

        var sale = SaleDiscountServiceTestData.GenerateSale(4);
        sale.Discounts.Add(existingDiscount);

        var requestedDiscounts = ImmutableList.Create(requestedDiscount.Id);

        SetupDiscountRepository(new Dictionary<Guid, Discount> { { requestedDiscount.Id, requestedDiscount } });

        // When
        await _service.ApplyDiscounts(sale, requestedDiscounts, CancellationToken.None);

        // Then
        sale.Discounts.Should().ContainSingle();
        sale.Discounts[0].Should().Be(requestedDiscount);
        sale.Discounts.Should().NotContain(existingDiscount);
    }

    /// <summary>
    /// Tests that when a sale has no discounts and requested discounts is not empty,
    /// the requested discounts are added to the sale.
    /// </summary>
    [Fact(
        DisplayName = "Given sale has no discounts and requested discounts is not empty When applying discounts Then adds requested discounts"
    )]
    public async Task ApplyDiscounts_SaleHasNoDiscountsAndRequestedIsNotEmpty_AddsRequestedDiscounts()
    {
        // Given
        var requestedDiscount1 = SaleDiscountServiceTestData.GenerateDiscount(code: "DISC10");
        var requestedDiscount2 = SaleDiscountServiceTestData.GenerateDiscount(code: "DISC20");

        var sale = SaleDiscountServiceTestData.GenerateSale(4);

        var requestedDiscounts = ImmutableList.Create(requestedDiscount1.Id, requestedDiscount2.Id);

        SetupDiscountRepository(
            new Dictionary<Guid, Discount>
            {
                { requestedDiscount1.Id, requestedDiscount1 },
                { requestedDiscount2.Id, requestedDiscount2 },
            }
        );

        // When
        await _service.ApplyDiscounts(sale, requestedDiscounts, CancellationToken.None);

        // Then
        sale.Discounts.Should().HaveCount(2);
        sale.Discounts.Should().Contain(requestedDiscount1);
        sale.Discounts.Should().Contain(requestedDiscount2);
    }

    /// <summary>
    /// Tests that all automatic discounts are called when requested discounts is empty.
    /// </summary>
    [Fact(
        DisplayName = "Given requested discounts is empty When applying discounts Then calls all automatic discounts"
    )]
    public async Task ApplyDiscounts_RequestedDiscountsEmpty_CallsAllAutomaticDiscounts()
    {
        // Given
        var sale = SaleDiscountServiceTestData.GenerateSale(4);
        var requestedDiscounts = ImmutableList<Guid>.Empty;

        SetupDiscountRepository(new Dictionary<Guid, Discount>());

        // When
        await _service.ApplyDiscounts(sale, requestedDiscounts, CancellationToken.None);

        // Then
        await _automaticDiscount1.Received(1).TryApply(sale, Arg.Any<CancellationToken>());
        await _automaticDiscount2.Received(1).TryApply(sale, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that all automatic discounts are called when requested discounts is not empty.
    /// </summary>
    [Fact(
        DisplayName = "Given requested discounts is not empty When applying discounts Then calls all automatic discounts"
    )]
    public async Task ApplyDiscounts_RequestedDiscountsNotEmpty_CallsAllAutomaticDiscounts()
    {
        // Given
        var requestedDiscount = SaleDiscountServiceTestData.GenerateDiscount(code: "MANUAL10");
        var sale = SaleDiscountServiceTestData.GenerateSale(4);
        var requestedDiscounts = ImmutableList.Create(requestedDiscount.Id);

        SetupDiscountRepository(new Dictionary<Guid, Discount> { { requestedDiscount.Id, requestedDiscount } });

        // When
        await _service.ApplyDiscounts(sale, requestedDiscounts, CancellationToken.None);

        // Then
        await _automaticDiscount1.Received(1).TryApply(sale, Arg.Any<CancellationToken>());
        await _automaticDiscount2.Received(1).TryApply(sale, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that all automatic discounts are called when sale has existing discounts cleared.
    /// </summary>
    [Fact(DisplayName = "Given sale has existing discounts When clearing discounts Then calls all automatic discounts")]
    public async Task ApplyDiscounts_SaleHasExistingDiscounts_CallsAllAutomaticDiscounts()
    {
        // Given
        var existingDiscount = SaleDiscountServiceTestData.GenerateDiscount(code: "EXISTING10");
        var sale = SaleDiscountServiceTestData.GenerateSale(4);
        sale.Discounts.Add(existingDiscount);

        var requestedDiscounts = ImmutableList<Guid>.Empty;

        SetupDiscountRepository(new Dictionary<Guid, Discount>());

        // When
        await _service.ApplyDiscounts(sale, requestedDiscounts, CancellationToken.None);

        // Then
        await _automaticDiscount1.Received(1).TryApply(sale, Arg.Any<CancellationToken>());
        await _automaticDiscount2.Received(1).TryApply(sale, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a validation exception is thrown when a requested discount is not found.
    /// </summary>
    [Fact(DisplayName = "Given requested discount not found When applying discounts Then throws validation exception")]
    public async Task ApplyDiscounts_RequestedDiscountNotFound_ThrowsValidationException()
    {
        // Given
        var validDiscount = SaleDiscountServiceTestData.GenerateDiscount(code: "VALID10");
        var invalidDiscountId = Guid.NewGuid();
        var sale = SaleDiscountServiceTestData.GenerateSale(4);

        var requestedDiscounts = ImmutableList.Create(validDiscount.Id, invalidDiscountId);

        SetupDiscountRepository(new Dictionary<Guid, Discount> { { validDiscount.Id, validDiscount } });

        // When
        var act = () => _service.ApplyDiscounts(sale, requestedDiscounts, CancellationToken.None);

        // Then
        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.WithMessage($"*Invalid discount requested*{invalidDiscountId}*");
        await _automaticDiscount1.DidNotReceive().TryApply(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        await _automaticDiscount2.DidNotReceive().TryApply(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a validation exception is thrown when multiple requested discounts are not found.
    /// </summary>
    [Fact(
        DisplayName = "Given multiple requested discounts not found When applying discounts Then throws validation exception with all invalid IDs"
    )]
    public async Task ApplyDiscounts_MultipleRequestedDiscountsNotFound_ThrowsValidationExceptionWithAllInvalidIds()
    {
        // Given
        var invalidDiscountId1 = Guid.NewGuid();
        var invalidDiscountId2 = Guid.NewGuid();
        var sale = SaleDiscountServiceTestData.GenerateSale(4);

        var requestedDiscounts = ImmutableList.Create(invalidDiscountId1, invalidDiscountId2);

        SetupDiscountRepository(new Dictionary<Guid, Discount>());

        // When
        var act = () => _service.ApplyDiscounts(sale, requestedDiscounts, CancellationToken.None);

        // Then
        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.WithMessage($"*Invalid discount requested*{invalidDiscountId1}*{invalidDiscountId2}*");
        await _automaticDiscount1.DidNotReceive().TryApply(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        await _automaticDiscount2.DidNotReceive().TryApply(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a validation exception is thrown when requesting a discount with less than 4 items.
    /// </summary>
    [Theory(DisplayName = "Given sale has less than 4 items When requesting discount Then throws validation exception")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task ApplyDiscounts_SaleHasLessThan4ItemsAndDiscountRequested_ThrowsValidationException(
        int itemQuantity
    )
    {
        // Given
        var discount = SaleDiscountServiceTestData.GenerateDiscount(code: "DISC10");
        var sale = SaleDiscountServiceTestData.GenerateSale(itemQuantity);

        var requestedDiscounts = ImmutableList.Create(discount.Id);

        SetupDiscountRepository(new Dictionary<Guid, Discount> { { discount.Id, discount } });

        // When
        var act = () => _service.ApplyDiscounts(sale, requestedDiscounts, CancellationToken.None);

        // Then
        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.WithMessage("*Purchases below 4 items cannot have a discount*");
        await _automaticDiscount1.DidNotReceive().TryApply(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        await _automaticDiscount2.DidNotReceive().TryApply(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that discount is applied successfully when sale has 4 or more items.
    /// </summary>
    [Theory(DisplayName = "Given sale has 4 or more items When requesting discount Then applies discount successfully")]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task ApplyDiscounts_SaleHas4OrMoreItemsAndDiscountRequested_AppliesDiscountSuccessfully(
        int itemQuantity
    )
    {
        // Given
        var discount = SaleDiscountServiceTestData.GenerateDiscount(code: "DISC10");
        var sale = SaleDiscountServiceTestData.GenerateSale(itemQuantity);

        var requestedDiscounts = ImmutableList.Create(discount.Id);

        SetupDiscountRepository(new Dictionary<Guid, Discount> { { discount.Id, discount } });

        // When
        await _service.ApplyDiscounts(sale, requestedDiscounts, CancellationToken.None);

        // Then
        sale.Discounts.Should().ContainSingle();
        sale.Discounts[0].Should().Be(discount);
        await _automaticDiscount1.Received(1).TryApply(sale, Arg.Any<CancellationToken>());
        await _automaticDiscount2.Received(1).TryApply(sale, Arg.Any<CancellationToken>());
    }

    private void SetupDiscountRepository(Dictionary<Guid, Discount> discounts)
    {
        _discountRepository
            .GetDiscounts(Arg.Any<IEnumerable<Guid>>(), false)
            .Returns(callInfo =>
            {
                var requestedIds = callInfo.ArgAt<IEnumerable<Guid>>(0);
                return discounts
                    .Where(kvp => requestedIds.Contains(kvp.Key))
                    .Select(kvp => kvp.Value)
                    .ToAsyncEnumerable();
            });
    }
}

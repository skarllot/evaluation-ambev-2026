using System.Collections.Immutable;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Sales.Services;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly ISaleDiscountService _discountService;
    private readonly ISaleRepository _saleRepository;
    private readonly IPublisher _publisher;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates the handler instance.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _timeProvider = Substitute.For<TimeProvider>();
        _discountService = Substitute.For<ISaleDiscountService>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _publisher = Substitute.For<IPublisher>();
        _handler = new CreateSaleHandler(_logger, _timeProvider, _discountService, _saleRepository, _publisher);
    }

    /// <summary>
    /// Tests that an invalid request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid command When handling Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = CreateSaleCommandTestData.GenerateCommandWithEmptyCustomerId();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that a valid request successfully creates a sale and calls all dependencies correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then creates sale and calls all dependencies")]
    public async Task Handle_ValidRequest_CreatesSaleAndCallsAllDependencies()
    {
        // Given
        var command = CreateSaleCommandTestData.GenerateValidCommand();
        var now = DateTimeOffset.UtcNow;
        var expectedSaleId = Guid.NewGuid();

        _timeProvider.GetUtcNow().Returns(now);

        _saleRepository
            .Create(Arg.Do<Sale>(s => SetSaleId(s, expectedSaleId)), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedSaleId);

        // Verify discount service was called
        await _discountService
            .Received(1)
            .ApplyDiscounts(
                Arg.Any<Sale>(),
                Arg.Is<IImmutableList<Guid>>(d => d.SequenceEqual(command.Discounts)),
                Arg.Any<CancellationToken>()
            );

        // Verify repository was called
        await _saleRepository
            .Received(1)
            .Create(
                Arg.Is<Sale>(s =>
                    s.CustomerId == command.CustomerId
                    && s.BranchId == command.BranchId
                    && s.Items.Count == 2
                    && // Two distinct products
                    !s.IsCancelled
                    && s.CreatedAt == now.UtcDateTime
                ),
                Arg.Any<CancellationToken>()
            );

        // Verify publisher was called
        await _publisher
            .Received(1)
            .Publish(
                Arg.Is<SaleCreated>(e => e.SaleId == expectedSaleId && e.Timestamp == now),
                Arg.Any<CancellationToken>()
            );
    }

    /// <summary>
    /// Tests that the discount service is called with the correct sale and discounts.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then calls discount service with correct parameters")]
    public async Task Handle_ValidRequest_CallsDiscountServiceWithCorrectParameters()
    {
        // Given
        var command = CreateSaleCommandTestData.GenerateValidCommand();
        var now = DateTimeOffset.UtcNow;

        _timeProvider.GetUtcNow().Returns(now);
        _saleRepository.Create(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _discountService
            .Received(1)
            .ApplyDiscounts(
                Arg.Is<Sale>(s => s.CustomerId == command.CustomerId && s.BranchId == command.BranchId),
                Arg.Is<IImmutableList<Guid>>(d => d.SequenceEqual(command.Discounts)),
                Arg.Any<CancellationToken>()
            );
    }

    /// <summary>
    /// Tests that the repository is called with the correct sale entity.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then calls repository with mapped sale entity")]
    public async Task Handle_ValidRequest_CallsRepositoryWithMappedSaleEntity()
    {
        // Given
        var command = CreateSaleCommandTestData.GenerateValidCommand();
        var now = DateTimeOffset.UtcNow;

        _timeProvider.GetUtcNow().Returns(now);
        _saleRepository.Create(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository
            .Received(1)
            .Create(
                Arg.Is<Sale>(s =>
                    s.CustomerId == command.CustomerId
                    && s.BranchId == command.BranchId
                    && s.Items.Count == 2
                    && !s.IsCancelled
                    && s.CreatedAt == now.UtcDateTime
                ),
                Arg.Any<CancellationToken>()
            );
    }

    /// <summary>
    /// Tests that the publisher is called with the correct event after successful creation.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then publishes SaleCreated event")]
    public async Task Handle_ValidRequest_PublishesSaleCreatedEvent()
    {
        // Given
        var command = CreateSaleCommandTestData.GenerateValidCommand();
        var now = DateTimeOffset.UtcNow;
        var expectedSaleId = Guid.NewGuid();

        _timeProvider.GetUtcNow().Returns(now);
        _saleRepository
            .Create(Arg.Do<Sale>(s => SetSaleId(s, expectedSaleId)), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _publisher
            .Received(1)
            .Publish(
                Arg.Is<SaleCreated>(e => e.SaleId == expectedSaleId && e.Timestamp == now),
                Arg.Any<CancellationToken>()
            );
    }

    /// <summary>
    /// Tests that sale entity validation is performed after discount application.
    /// This test verifies that if the created sale entity is invalid, a validation exception is thrown.
    /// </summary>
    [Fact(
        DisplayName = "Given command that creates invalid sale entity When handling Then throws validation exception"
    )]
    public async Task Handle_InvalidSaleEntity_ThrowsValidationException()
    {
        // Given
        var command = CreateSaleCommandTestData.GenerateValidCommand();
        var now = DateTimeOffset.UtcNow;

        _timeProvider.GetUtcNow().Returns(now);

        // Configure discount service to add duplicate products to make the sale invalid
        _discountService
            .ApplyDiscounts(Arg.Any<Sale>(), Arg.Any<IImmutableList<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var sale = callInfo.Arg<Sale>();
                // Add a duplicate product to make sale validation fail
                if (sale.Items.Count > 0)
                {
                    sale.Items.Add(sale.Items[0]);
                }
                return Task.CompletedTask;
            });

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage("*Each product can only appear once per sale*");
    }

    /// <summary>
    /// Tests that the sale items are correctly mapped from command items.
    /// </summary>
    [Fact(DisplayName = "Given command with multiple items When handling Then maps items correctly")]
    public async Task Handle_CommandWithMultipleItems_MapsItemsCorrectly()
    {
        // Given
        var command = CreateSaleCommandTestData.GenerateValidCommand();
        var now = DateTimeOffset.UtcNow;

        _timeProvider.GetUtcNow().Returns(now);
        _saleRepository.Create(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository
            .Received(1)
            .Create(
                Arg.Is<Sale>(s =>
                    s.Items.Count == 2
                    && // Two distinct products from the test data
                    s.Items.All(item => command.Items.Any(cmdItem => cmdItem.ProductId == item.ProductId))
                ),
                Arg.Any<CancellationToken>()
            );
    }

    /// <summary>
    /// Tests that the logger is called after successful sale creation.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then logs sale creation")]
    public async Task Handle_ValidRequest_LogsSaleCreation()
    {
        // Given
        var command = CreateSaleCommandTestData.GenerateValidCommand();
        var now = DateTimeOffset.UtcNow;
        var expectedSaleId = Guid.NewGuid();

        _timeProvider.GetUtcNow().Returns(now);
        _saleRepository
            .Create(Arg.Do<Sale>(s => SetSaleId(s, expectedSaleId)), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _logger
            .Received(1)
            .Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains(expectedSaleId.ToString())),
                null,
                Arg.Any<Func<object, Exception?, string>>()
            );
    }

    /// <summary>
    /// Helper method to set the Id property on a Sale entity using reflection.
    /// This is needed because it is an init-only property.
    /// </summary>
    private static void SetSaleId(Sale sale, Guid id)
    {
        var idProperty = typeof(Sale).BaseType!.GetProperty(nameof(Sale.Id));
        idProperty!.SetValue(sale, id);
    }
}

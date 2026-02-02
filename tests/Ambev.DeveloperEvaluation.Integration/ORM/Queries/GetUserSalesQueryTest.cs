using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Queries;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.ORM.Queries;

public class GetUserSalesQueryTest
{
    [Fact]
    public void CountQuery_ReturnsValidSqlStatement()
    {
        using var context = new DefaultContext(
            new DbContextOptionsBuilder<DefaultContext>().UseNpgsql("Host=localhost").Options
        );

        var customerId = Guid.Parse("12345678-1234-1234-1234-123456789012");
        var startDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var endDate = new DateTimeOffset(2024, 12, 31, 23, 59, 59, TimeSpan.Zero);

        var queryString = GetUserSalesQuery.CountQuery(context, customerId, startDate, endDate).ToQueryString();

        Assert.True(
            queryString == ExpectedCountQueryString,
            $"Expected: {ExpectedCountQueryString}\n\nActual: {queryString}"
        );
    }

    [Fact]
    public void CountQuery_WithNullDates_ReturnsValidSqlStatement()
    {
        using var context = new DefaultContext(
            new DbContextOptionsBuilder<DefaultContext>().UseNpgsql("Host=localhost").Options
        );

        var customerId = Guid.Parse("12345678-1234-1234-1234-123456789012");

        var queryString = GetUserSalesQuery.CountQuery(context, customerId, null, null).ToQueryString();

        Assert.True(
            queryString == ExpectedCountQueryStringWithNullDates,
            $"Expected: {ExpectedCountQueryStringWithNullDates}\n\nActual: {queryString}"
        );
    }

    [Fact]
    public void GetQuery_ReturnsValidSqlStatement()
    {
        using var context = new DefaultContext(
            new DbContextOptionsBuilder<DefaultContext>().UseNpgsql("Host=localhost").Options
        );

        var customerId = Guid.Parse("12345678-1234-1234-1234-123456789012");
        var startDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var endDate = new DateTimeOffset(2024, 12, 31, 23, 59, 59, TimeSpan.Zero);

        var queryString = GetUserSalesQuery
            .GetQuery(context, customerId, pageNumber: 1, pageSize: 10, startDate, endDate)
            .ToQueryString();

        Assert.True(
            queryString == ExpectedGetQueryString,
            $"Expected: {ExpectedGetQueryString}\n\nActual: {queryString}"
        );
    }

    [Fact]
    public void GetQuery_WithNullDates_ReturnsValidSqlStatement()
    {
        using var context = new DefaultContext(
            new DbContextOptionsBuilder<DefaultContext>().UseNpgsql("Host=localhost").Options
        );

        var customerId = Guid.Parse("12345678-1234-1234-1234-123456789012");

        var queryString = GetUserSalesQuery
            .GetQuery(context, customerId, pageNumber: 2, pageSize: 20, null, null)
            .ToQueryString();

        Assert.True(
            queryString == ExpectedGetQueryStringWithNullDates,
            $"Expected: {ExpectedGetQueryStringWithNullDates}\n\nActual: {queryString}"
        );
    }

    private const string ExpectedCountQueryString = """
        -- @__customerId_0='12345678-1234-1234-1234-123456789012'
        -- @__startDate_1='2024-01-01T00:00:00.0000000+00:00' (Nullable = true) (DbType = DateTime)
        -- @__endDate_2='2024-12-31T23:59:59.0000000+00:00' (Nullable = true) (DbType = DateTime)
        SELECT s."Id", s."BranchId", s."CreatedAt", s."CustomerId", s."IsCancelled", s."Number", s."UpdatedAt"
        FROM "Sales" AS s
        WHERE s."CustomerId" = @__customerId_0 AND COALESCE(s."UpdatedAt", s."CreatedAt")::timestamptz >= @__startDate_1 AND COALESCE(s."UpdatedAt", s."CreatedAt")::timestamptz <= @__endDate_2
        """;

    private const string ExpectedCountQueryStringWithNullDates = """
        -- @__customerId_0='12345678-1234-1234-1234-123456789012'
        SELECT s."Id", s."BranchId", s."CreatedAt", s."CustomerId", s."IsCancelled", s."Number", s."UpdatedAt"
        FROM "Sales" AS s
        WHERE s."CustomerId" = @__customerId_0
        """;

    private const string ExpectedGetQueryString = """
        -- @__customerId_0='12345678-1234-1234-1234-123456789012'
        -- @__startDate_1='2024-01-01T00:00:00.0000000+00:00' (Nullable = true) (DbType = DateTime)
        -- @__endDate_2='2024-12-31T23:59:59.0000000+00:00' (Nullable = true) (DbType = DateTime)
        -- @__p_4='10'
        -- @__p_3='0'
        SELECT t."Number", t.c, t.c0, t."Id", t."Name", t."Id0", t0."ProductId", t0."ProductName", t0."Quantity", t0."UnitPrice", t0."TotalAmount", t0."Id", t1."DiscountCode", t1."Multiplier", t1."Amount", t1."DiscountsId", t1."SaleId", t1."Id", t."IsCancelled"
        FROM (
            SELECT s."Number", COALESCE(s."UpdatedAt", s."CreatedAt")::timestamptz AS c, (
                SELECT COALESCE(sum(s0."Quantity"::numeric(8,2) * s0."UnitPrice"), 0.0)
                FROM "SaleItems" AS s0
                WHERE s."Id" = s0."SaleId") * (1.0 - COALESCE((
                SELECT COALESCE(sum(d0."Multiplier"), 0.0)
                FROM "DiscountSale" AS d
                INNER JOIN "Discounts" AS d0 ON d."DiscountsId" = d0."Id"
                WHERE s."Id" = d."SaleId"), 0.0)) - COALESCE((
                SELECT COALESCE(sum(d2."Amount"), 0.0)
                FROM "DiscountSale" AS d1
                INNER JOIN "Discounts" AS d2 ON d1."DiscountsId" = d2."Id"
                WHERE s."Id" = d1."SaleId"), 0.0) AS c0, b."Id", b."Name", s."IsCancelled", s."Id" AS "Id0"
            FROM "Sales" AS s
            INNER JOIN "Branches" AS b ON s."BranchId" = b."Id"
            WHERE s."CustomerId" = @__customerId_0 AND COALESCE(s."UpdatedAt", s."CreatedAt")::timestamptz >= @__startDate_1 AND COALESCE(s."UpdatedAt", s."CreatedAt")::timestamptz <= @__endDate_2
            ORDER BY COALESCE(s."UpdatedAt", s."CreatedAt")::timestamptz DESC
            LIMIT @__p_4 OFFSET @__p_3
        ) AS t
        LEFT JOIN (
            SELECT p."Id" AS "ProductId", p."Name" AS "ProductName", s1."Quantity", s1."UnitPrice", s1."Quantity"::numeric(8,2) * s1."UnitPrice" AS "TotalAmount", s1."Id", s1."SaleId"
            FROM "SaleItems" AS s1
            INNER JOIN "Products" AS p ON s1."ProductId" = p."Id"
        ) AS t0 ON t."Id0" = t0."SaleId"
        LEFT JOIN (
            SELECT d4."Code" AS "DiscountCode", d4."Multiplier", d4."Amount", d3."DiscountsId", d3."SaleId", d4."Id"
            FROM "DiscountSale" AS d3
            INNER JOIN "Discounts" AS d4 ON d3."DiscountsId" = d4."Id"
        ) AS t1 ON t."Id0" = t1."SaleId"
        ORDER BY t.c DESC, t."Id0", t."Id", t0."Id", t0."ProductId", t1."DiscountsId", t1."SaleId"
        """;

    private const string ExpectedGetQueryStringWithNullDates = """
        -- @__customerId_0='12345678-1234-1234-1234-123456789012'
        -- @__p_1='20'
        SELECT t."Number", t.c, t.c0, t."Id", t."Name", t."Id0", t0."ProductId", t0."ProductName", t0."Quantity", t0."UnitPrice", t0."TotalAmount", t0."Id", t1."DiscountCode", t1."Multiplier", t1."Amount", t1."DiscountsId", t1."SaleId", t1."Id", t."IsCancelled"
        FROM (
            SELECT s."Number", COALESCE(s."UpdatedAt", s."CreatedAt")::timestamptz AS c, (
                SELECT COALESCE(sum(s0."Quantity"::numeric(8,2) * s0."UnitPrice"), 0.0)
                FROM "SaleItems" AS s0
                WHERE s."Id" = s0."SaleId") * (1.0 - COALESCE((
                SELECT COALESCE(sum(d0."Multiplier"), 0.0)
                FROM "DiscountSale" AS d
                INNER JOIN "Discounts" AS d0 ON d."DiscountsId" = d0."Id"
                WHERE s."Id" = d."SaleId"), 0.0)) - COALESCE((
                SELECT COALESCE(sum(d2."Amount"), 0.0)
                FROM "DiscountSale" AS d1
                INNER JOIN "Discounts" AS d2 ON d1."DiscountsId" = d2."Id"
                WHERE s."Id" = d1."SaleId"), 0.0) AS c0, b."Id", b."Name", s."IsCancelled", s."Id" AS "Id0"
            FROM "Sales" AS s
            INNER JOIN "Branches" AS b ON s."BranchId" = b."Id"
            WHERE s."CustomerId" = @__customerId_0
            ORDER BY COALESCE(s."UpdatedAt", s."CreatedAt")::timestamptz DESC
            LIMIT @__p_1 OFFSET @__p_1
        ) AS t
        LEFT JOIN (
            SELECT p."Id" AS "ProductId", p."Name" AS "ProductName", s1."Quantity", s1."UnitPrice", s1."Quantity"::numeric(8,2) * s1."UnitPrice" AS "TotalAmount", s1."Id", s1."SaleId"
            FROM "SaleItems" AS s1
            INNER JOIN "Products" AS p ON s1."ProductId" = p."Id"
        ) AS t0 ON t."Id0" = t0."SaleId"
        LEFT JOIN (
            SELECT d4."Code" AS "DiscountCode", d4."Multiplier", d4."Amount", d3."DiscountsId", d3."SaleId", d4."Id"
            FROM "DiscountSale" AS d3
            INNER JOIN "Discounts" AS d4 ON d3."DiscountsId" = d4."Id"
        ) AS t1 ON t."Id0" = t1."SaleId"
        ORDER BY t.c DESC, t."Id0", t."Id", t0."Id", t0."ProductId", t1."DiscountsId", t1."SaleId"
        """;
}

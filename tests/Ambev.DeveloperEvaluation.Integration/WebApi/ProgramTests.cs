using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.WebApi;

public class ProgramTests
{
    [Fact]
    public void ShouldNotHasMissingDependencies()
    {
        var hostBuilder = Program.CreateHostBuilder([]);
        var serviceProvider = hostBuilder.Services.BuildServiceProvider(
            new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true }
        );
        Assert.NotNull(serviceProvider);
    }
}

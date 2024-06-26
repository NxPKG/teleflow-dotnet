using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Teleflow.Models.Integrations;
using Xunit;
using Xunit.Abstractions;

namespace Teleflow.Tests.IntegrationTests;

public class IntegrationTests : BaseIntegrationTest
{
    public IntegrationTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Should_Get_Integrations()
    {
        var integrations = await Integration.Get();
        integrations.Should().NotBeNull();
        integrations.Data.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_Serialize_Credentials_WithNulls_IsEmpty()
    {
        JsonConvert.SerializeObject(new Credentials(), TeleflowClient.DefaultSerializerSettings)
            .Should()
            .Be("{}");
    }
}
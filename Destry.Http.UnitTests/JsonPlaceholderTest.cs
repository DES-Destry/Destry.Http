using System.Collections.Immutable;
using Destry.Http.UnitTests.Controllers.JsonPlaceholder;
using Xunit.Abstractions;

namespace Destry.Http.UnitTests;

public class JsonPlaceholderTest(ITestOutputHelper output)
{
    private readonly IJsonPlaceholderController _jsonPlaceholderController =
        new ControllerBuilder().From<IJsonPlaceholderController>();

    [Fact]
    public async Task ItShouldWork()
    {
        var response = (await _jsonPlaceholderController.GetAllPosts())?.ToImmutableArray() ?? [];
        Assert.NotEmpty(response);

        output.WriteLine(response.Length.ToString());
    }
}

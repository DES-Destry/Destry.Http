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
        Assert.Equal(100, response.Length);
    }

    [Fact]
    public async Task ItShouldSendRequestWithPathParam()
    {
        var post = await _jsonPlaceholderController.GetPostById(1);
        Assert.Equal(1, post.Id);
    }
}

using System.Collections.Immutable;
using Destry.Http.UnitTests.Controllers.JsonPlaceholder;
using Xunit.Abstractions;

namespace Destry.Http.UnitTests;

public class JsonPlaceholderTest(ITestOutputHelper output)
{
    private readonly IJsonPlaceholderController _jsonPlaceholderController =
        new ControllerBuilder().From<IJsonPlaceholderController>();

    [Fact(DisplayName = "It should work")]
    public async Task ItShouldWork()
    {
        var response = (await _jsonPlaceholderController.GetAllPosts())?.ToImmutableArray() ?? [];

        Assert.NotEmpty(response);
        Assert.Equal(100, response.Length);
    }

    [Fact(DisplayName = "Send a request with path parameter")]
    public async Task ItShouldSendRequestWithPathParam()
    {
        const int postId = 1;

        var post = await _jsonPlaceholderController.GetPostById(postId);

        Assert.Equal(postId, post.Id);
    }

    [Fact(DisplayName = "Send a request with query")]
    public async Task ItShouldSendRequestWithQuery()
    {
        const int postId = 1;

        var comments =
            (await _jsonPlaceholderController.GetCommentsForPost(postId))?.ToImmutableArray() ?? [];
        var isAllCommentsRelatedWithCorrectPost = comments.All(comment => comment.PostId == postId);

        Assert.NotEmpty(comments);
        Assert.True(isAllCommentsRelatedWithCorrectPost);
    }
}

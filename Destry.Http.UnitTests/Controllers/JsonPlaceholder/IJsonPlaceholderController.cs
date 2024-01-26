using Destry.Http.Controller;
using Destry.Http.Data;
using Destry.Http.Methods;
using Destry.Http.UnitTests.Controllers.JsonPlaceholder.Types;

namespace Destry.Http.UnitTests.Controllers.JsonPlaceholder;

// Controller isn't required. Used for baseUrl specifying
// [Controller("https://jsonplaceholder.typicode.com")] - for users without Docker
[Controller("http://localhost:6789")]
public interface IJsonPlaceholderController
{
    [SendGet("posts")]
    Task<IEnumerable<Post>> GetAllPosts();

    [SendGet("posts/{id}")]
    Task<Post> GetPostById([Param] int id);

    [SendGet("comments")]
    Task<IEnumerable<Comment>> GetCommentsForPost([Query] int postId);
}

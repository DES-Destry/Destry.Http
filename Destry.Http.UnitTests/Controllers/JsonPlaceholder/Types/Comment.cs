namespace Destry.Http.UnitTests.Controllers.JsonPlaceholder.Types;

public record Comment(
    int Id,
    int PostId,
    string Name,
    string Email,
    string Body
);

namespace Destry.Http.UnitTests.Controllers.JsonPlaceholder.Types;

public record User(
    int Id,
    string Name,
    string Username,
    string Email,
    Address Address,
    string Phone,
    string Website,
    Company Company
);

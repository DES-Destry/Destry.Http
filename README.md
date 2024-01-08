# Destry.Http

A NuGet package that allow you quickly build HTTP clients. Just describe an interface with attributes and you will get ready http client with less code compared to other HTTP clients.

## Installation (coming soon...)
Find in your NuGet explorer `Destry.Http` package or install it with CLI:

```bash
$ dotnet add package Destry.Http
```

## Get started

To get started you need do describe your API with Controller interface. For example:
```csharp
using Destry.Http.Controller;
using Destry.Http.Data;
using Destry.Http.Methods;

namespace Destry.AwesomeProject;

// Controller isn't required. Used for baseUrl specifying
[Controller("https://jsonplaceholder.typicode.com")]
public interface IJsonPlaceholderController
{
    // Highly reccomend to use Task<*>, http requests are async as rule
    // Sync methods aren't tested yet
    [SendGet("posts")]
    Task<IEnumerable<Post>> GetAllPosts();
    
    [SendPost("posts")]
    Task<IEnumerable<Post>> CreatePosts([Body] IEnumerable<Post> posts);

    [SendGet("posts/{id}")]
    Task<Post> GetPostById([Param] int id);

    [SendGet("comments")]
    Task<IEnumerable<Comment>> GetCommentsForPost([Query] int postId);
}
```

To get ready controller to use, get it with `ControllerBuilder`:
```csharp
using Destry.Http;

var controller = new ControllerBuilder().FromInterface<IJsonPlaceholderController>();
var posts = await controller.GetAllPosts();
```

### Specify base url with `ControllerBuilder`

Well, you can specify your base url with `ControllerBuilder.WithBaseUrl("url")` and not pass url in `[Controller]` attribute or even not pass `[Controller]` attribute.
```csharp
using Destry.Http.Controller;
using Destry.Http.Data;
using Destry.Http.Methods;

namespace Destry.AwesomeProject;

public interface IJsonPlaceholderController
{
    [SendGet("posts")]
    Task<IEnumerable<Post>> GetAllPosts();

    // Other methods
}
```

```csharp
using Destry.Http;

var controller = new ControllerBuilder()
    .WithBaseUrl("https://jsonplaceholder.typicode.com")
    .FromInterface<IJsonPlaceholderController>();
```

## Customization

By default `Destry.Http` using `System.Net.Http.HttpClient`. It's highly recommend to use it, but still you can change HTTP request sending behaviour in edge cases. Extend `Sender` class to make you own implementation.

```csharp
using SomeAwesomeHttpClient;

namespace Destry.AwesomeProject;

internal sealed class YourAwesomeSender : Sender
{
    public override void SetBaseUrl(string url) => // Set base url value
    public override void AddQuery(string name, string value) => // Add query value
    public override void AddHeader(string name, string value) => // Add header value
    public override void AddParam(string name, string value) => // Add param value
    public override void SetBody(object body) => // Set your body

    public override void Reset()
    {
        // Delete all data what you set with SetBaseUrl, SetBody, AddQuery, etc.
    }

    public override async Task<T?> SendHttpRequestAsync<T>(string httpMethod, string resource) where T : default
    {
        // Sending request behaviour
    }
}

```

And then pass it with `ControllerBuilder.WithSender(sender)` method.

```csharp
using Destry.Http;

var sender = new YourAwesomeSender();
var controller = new ControllerBuilder()
    .WithBaseUrl("https://jsonplaceholder.typicode.com")
    .WithSender(sender)
    .FromInterface<IJsonPlaceholderController>();
```

## All attributes

Use attributes to describe HTTP method:
- [SendGet]
- [SendHead]
- [SendPost]
- [SendPut]
- [SendPatch]
- [SendDelete]



Use attributes to add data to your request:
- [Body]
- [Header]
- [Param]
- [Query]
- [ExcludeFromRequest] (coming soon...)

*all data attributes can be used with classes and primitives (except body, that better to use only with classes, but using it with primitives is allowed to)
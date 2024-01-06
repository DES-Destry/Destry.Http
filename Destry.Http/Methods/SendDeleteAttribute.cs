namespace Destry.Http.Methods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class SendDeleteAttribute(string path) : SendAttribute(path);

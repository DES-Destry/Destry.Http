namespace Destry.Http.Methods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class SendPostAttribute(string path) : SendAttribute(path);

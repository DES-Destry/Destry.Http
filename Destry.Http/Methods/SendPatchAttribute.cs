namespace Destry.Http.Methods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class SendPatchAttribute(string path) : SendAttribute(path);

namespace Destry.Http;

public abstract class Sender
{
    internal Sender NewInstance()
    {
        var type = GetType();
        return (Sender) Activator.CreateInstance(type)!;
    }

    public abstract void SetBaseUrl(string url);
    public abstract void AddQuery(string name, string value);
    public abstract void AddHeader(string name, string value);
    public abstract void AddParam(string name, string value);
    public abstract void SetBody(object body);

    public abstract Task<T?> SendHttpRequestAsync<T>(string httpMethod, string path);
}

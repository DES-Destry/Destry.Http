namespace Destry.Http;

public abstract class Sender
{
    public abstract void AddQuery(string name, string value);
    public abstract void AddHeader(string name, string value);
    public abstract void AddParam(string name, string value);
    public abstract void SetBody(object body);

    public abstract Task<T> ExecuteAsync<T>(string httpMethod);
}

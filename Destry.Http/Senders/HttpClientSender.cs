namespace Destry.Http.Senders;

public sealed class HttpClientSender : Sender
{
    public override void AddQuery(string name, string value)
    {
        throw new NotImplementedException();
    }

    public override void AddHeader(string name, string value)
    {
        throw new NotImplementedException();
    }

    public override void AddParam(string name, string value)
    {
        throw new NotImplementedException();
    }

    public override async Task<T> ExecuteAsync<T>() { throw new NotImplementedException(); }
}

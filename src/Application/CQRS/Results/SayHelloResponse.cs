using Application.CQRS;

namespace Application.CQRS.Results;

public class SayHelloResponse : IResponse
{
    public string Message
    {
        get;
        set;
    }
}
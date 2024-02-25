using Application.CQRS;

namespace Application.CQRS.Results;

public class EventResponse : IResponse
{
    public object Message
    {
        get;
        set;
    }
}
using Application.CQRS;

using ICQRSRequest = MediatR.IRequest<Application.CQRS.Results.SayHelloResponse>;

namespace Application.Queries;

public class SayHelloRequest : IRequest, ICQRSRequest
{
    public string Name
    {
        get;
        set;
    }
}
using Application.CQRS;
using Application.CQRS.Results;
using Domain.Interfaces;

namespace Application.Results;

public class Response<T> where T : IResponse
{
    public ServiceExceptionResponse Error
    {
        get; set;
    }
    public T Data
    {
        get; set;
    }
}
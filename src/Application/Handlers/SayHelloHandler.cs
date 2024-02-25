using Application.CQRS.Results;
using Application.Queries;
using Application.CQRS.Results;
using Domain;
using Domain.Models;
using MediatR;

namespace Application.Handlers;

public class SayHelloHandler : IRequestHandler<SayHelloRequest, SayHelloResponse>
{
    private DataSetOptions _dataSetConfig
    {
        get; set;
    }
    public SayHelloHandler(DataSetOptions dataSetConfig)
    {
        _dataSetConfig = dataSetConfig;
    }

    public async Task<SayHelloResponse> Handle(SayHelloRequest request, CancellationToken cancellationToken)
    {
        if (request.Name == "Abel")
            throw new ArgumentException("The name Abel is not allowed");
        return await Task.FromResult(new SayHelloResponse() { Message = $"Hello there {request.Name}: _dataSetConfig = {_dataSetConfig.BucketName}" });
    }
}
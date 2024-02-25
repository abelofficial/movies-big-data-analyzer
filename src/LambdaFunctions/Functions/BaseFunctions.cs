using System;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Application.CQRS;
using Application.CQRS.Results;
using Application.Results;
using LambdaFunctions.Settings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using IRequest = Application.CQRS.IRequest;

[assembly: LambdaSerializer(typeof(CustomLambdaSerializer))]
namespace LambdaFunctions.Functions;
public abstract class BaseFunctions
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IMediator _mediator;
    private readonly JsonSerializerOptions _serializeOptions;

    protected BaseFunctions()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        _serviceProvider = Startup.ConfigureServices().BuildServiceProvider();
        _mediator = _serviceProvider.GetService<IMediator>();

        var serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(new TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All))
        };
    }

    protected async Task<object> HandleResponse<TRequest, TResponse>(string eventRequest, ILambdaContext context, Func<TRequest, Task<TResponse>> lambdaFunction)
    where TResponse : IResponse
    where TRequest : IRequest
    {
        try
        {
            Log.Information("Parsing event request: {@EventRequest}", eventRequest);
            var request = JsonSerializer.Deserialize<TRequest>(eventRequest);
            Log.Information("Processing request: {@Request}", request);
            var result = await lambdaFunction(request);
            Log.Debug("Generated response successfully: {@Result}", result);
            return new Response<TResponse> { Data = result, Error = null };
        }
        catch (Exception error)
        {
            return new Response<EventResponse> { Data = default, Error = HandleExceptionResponse<EventResponse>(error) };
        }
    }

    protected async Task<Response<TResponse>> HandleResponse<TRequest, TResponse>(TRequest request, ILambdaContext context, Func<TRequest, Task<TResponse>> lambdaFunction)
    where TResponse : IResponse
    where TRequest : IRequest
    {
        try
        {
            Log.Information("Processing request: {@Request}", request);
            var result = await lambdaFunction(request);
            Log.Debug("Generated response successfully: {@Result}", result);
            return new Response<TResponse> { Data = result, Error = null };
        }
        catch (Exception error)
        {
            var exceptionResponse = HandleExceptionResponse<TResponse>(error);
            return new Response<TResponse> { Data = default, Error = exceptionResponse };
        }
    }

    private ServiceExceptionResponse HandleExceptionResponse<TResponse>(Exception error)
    where TResponse : IResponse
    {

        Log.Error("UnknownException: {@Error}", error);
        return new ServiceExceptionResponse
        {
            Status = HttpStatusCode.GetName(HttpStatusCode.InternalServerError),
            Message = "An error occurred while processing your request."
        };
    }
}
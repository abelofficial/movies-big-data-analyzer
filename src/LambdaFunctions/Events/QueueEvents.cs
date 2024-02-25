using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Application.Queries;
using LambdaFunctions.Functions;
using System.Collections.Generic;
using Application.CQRS.Results;
using Serilog;

namespace LambdaFunctions.Events;

public class QueueEvents : BaseFunctions
{

    public QueueEvents() : base()
    {

    }

    public async Task<Dictionary<string, object>> ProcessDataSet(SQSEvent eventRequest, ILambdaContext context)
    {
        var response = new Dictionary<string, object>();

        foreach (var record in eventRequest.Records)
        {
            var processResult = await HandleResponse<SayHelloRequest, SayHelloResponse>(record.Body, context, async (req) =>
            {
                return await _mediator.Send(req);
            });

            response.Add(record.MessageId, processResult);
        }

        Log.Debug("Finished processing all events: {@Result}", response);
        return response;
    }
}


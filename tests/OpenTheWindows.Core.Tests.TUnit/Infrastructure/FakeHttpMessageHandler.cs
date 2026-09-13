using System.Net;
using System.Net.Http;

namespace OpenTheWindows.Core.Tests.TUnit.Infrastructure;

internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<Func<HttpRequestMessage, HttpResponseMessage>> _responses = new();

    public List<HttpRequestMessage> Requests { get; } = [];

    public FakeHttpMessageHandler(params Func<HttpRequestMessage, HttpResponseMessage>[] responses)
    {
        foreach (var response in responses)
        {
            _responses.Enqueue(response);
        }
    }

    public void Enqueue(Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
    {
        _responses.Enqueue(responseFactory);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(request);

        if (_responses.Count == 0)
        {
            throw new InvalidOperationException($"No fake response registered for {request.Method} {request.RequestUri}");
        }

        var responseFactory = _responses.Dequeue();
        return Task.FromResult(responseFactory(request));
    }

    public static HttpResponseMessage JsonResponse(string payload, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json"),
        };
    }
}

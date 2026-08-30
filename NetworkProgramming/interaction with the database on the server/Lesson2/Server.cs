using Core.ModelRequest;
using ShopApp.Data;
using Lesson2.Services;
using Lesson2.Services.Interfaces;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Lesson2;

public class Server : IDisposable
{
    private readonly Socket _serverSocket;
    private Socket? _clientSocket;

    private const int BufferSize = 1024;

    private readonly Dictionary<TypeRequest, IServices> _serviceByTypeRequest = [];

    public Server(EndPoint endPoint, int backlog)
    {
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        _serverSocket.Bind(endPoint);
        _serverSocket.Listen(backlog);

        _serviceByTypeRequest.Add(TypeRequest.Read,   new ReadServices  (new ApplicationContext()));
        _serviceByTypeRequest.Add(TypeRequest.Create, new CreateServices(new ApplicationContext()));
        _serviceByTypeRequest.Add(TypeRequest.Update, new UpdateServices(new ApplicationContext()));
        _serviceByTypeRequest.Add(TypeRequest.Delete, new DeleteServices(new ApplicationContext()));
    }

    public async Task<string> ReceiveAsync()
    {
        _clientSocket = await _serverSocket.AcceptAsync();
        var reques = await ReadResponceAsync(_clientSocket);

        return reques;
    }


    public async Task<string> HandleRequestAsync(string request)
    {
        var requestObject = JsonSerializer.Deserialize<Request>(request);

        var responce = await _serviceByTypeRequest[requestObject.TypeRequest].ExecuteAsync(requestObject.Body);

        var responceJson = JsonSerializer.Serialize(responce);

        return responceJson;
    }

    private async Task<string> ReadResponceAsync(Socket clientSocket)
    {
        int readBytes;
        var buffer = new byte[BufferSize];
        var requestBuilder = new StringBuilder();
        do
        {
            readBytes = await clientSocket.ReceiveAsync(buffer);
            var request = Encoding.UTF8.GetString(buffer, 0, readBytes);
            requestBuilder.Append(request);
        }
        while (readBytes > 0);

        return requestBuilder.ToString();
    }

    public async Task SendAsync(string responce)
    {
        if (_clientSocket != null)
        {
            var responceBytes = Encoding.UTF8.GetBytes(responce);
            await _clientSocket.SendAsync(responceBytes);

            _clientSocket.Dispose();
        }
    }

    public void Dispose()
    {
        _serverSocket.Dispose();
    }
}
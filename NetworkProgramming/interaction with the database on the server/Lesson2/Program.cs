using Lesson2;
using ShopApp.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

await using (var db = new ApplicationContext())
{
    db.Database.EnsureCreated();
}

var ipEndPoint = new IPEndPoint(IPAddress.Loopback, 8888);
var server = new Server(ipEndPoint, 1000);

Console.WriteLine("Сервер запущен");

while (true)
{
    try
    {
        var request = await server.ReceiveAsync();
        var responce = await server.HandleRequestAsync(request);
        await server.SendAsync(responce);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка обработки запроса: {ex.Message}");
    }
}

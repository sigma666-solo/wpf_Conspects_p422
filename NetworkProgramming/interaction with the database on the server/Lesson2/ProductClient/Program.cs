using Core.Model;
using Core.ModelRequest;
using Core.ModelResponce;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ProductClient;

class Program
{
    private const string Host = "127.0.0.1";
    private const int Port = 8888;

    static async Task Main()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Управление продуктами ===");
            Console.WriteLine("1. Показать все (Read)");
            Console.WriteLine("2. Создать (Create)");
            Console.WriteLine("3. Обновить (Update)");
            Console.WriteLine("4. Удалить по Id (Delete)");
            Console.WriteLine("0. Выход");
            Console.Write("Выбор: ");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1": await ReadAllAsync(); break;
                case "2": await CreateAsync(); break;
                case "3": await UpdateAsync(); break;
                case "4": await DeleteAsync(); break;
                case "0": return;
                default: Console.WriteLine("Неверный ввод"); break;
            }
        }
    }

    private static async Task ReadAllAsync()
    {
        var responce = await SendAsync(TypeRequest.Read, string.Empty);

        if (!string.IsNullOrEmpty(responce.Body))
        {
            var products = JsonSerializer.Deserialize<List<Product>>(responce.Body) ?? [];
            Console.WriteLine("Список продуктов:");
            foreach (var p in products)
            {
                Console.WriteLine($"  Id: {p.Id}, Name: {p.Name}, Description: {p.Description}");
            }
        }
        else
        {
            Console.WriteLine("Продуктов нет.");
        }
    }

    private static async Task CreateAsync()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine() ?? string.Empty;
        Console.Write("Description: ");
        var description = Console.ReadLine();

        var product = new Product { Name = name, Description = description };
        var responce = await SendAsync(TypeRequest.Create, JsonSerializer.Serialize(product));

        Console.WriteLine($"[{responce.TypeResponse}] {responce.Body}");
    }

    private static async Task UpdateAsync()
    {
        Console.Write("Id: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;

        Console.Write("Name: ");
        var name = Console.ReadLine() ?? string.Empty;
        Console.Write("Description: ");
        var description = Console.ReadLine();

        var product = new Product { Id = id, Name = name, Description = description };
        var responce = await SendAsync(TypeRequest.Update, JsonSerializer.Serialize(product));

        Console.WriteLine($"[{responce.TypeResponse}] {responce.Body}");
    }

    private static async Task DeleteAsync()
    {
        Console.Write("Id: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;

        var responce = await SendAsync(TypeRequest.Delete, JsonSerializer.Serialize(id));

        Console.WriteLine($"[{responce.TypeResponse}] {responce.Body}");
    }

    private static async Task<Responce> SendAsync(TypeRequest typeRequest, string body)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(IPAddress.Loopback, Port);
        var stream = client.GetStream();

        var request = new Request { TypeRequest = typeRequest, Body = body };
        var requestBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));

        await stream.WriteAsync(requestBytes);
        await stream.FlushAsync();

        client.Client.Shutdown(SocketShutdown.Send);

        var responseBuilder = new StringBuilder();
        var buffer = new byte[1024];
        int readBytes;
        do
        {
            readBytes = await stream.ReadAsync(buffer);
            responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, readBytes));
        }
        while (readBytes > 0);

        return JsonSerializer.Deserialize<Responce>(responseBuilder.ToString()) ?? new Responce();
    }
}

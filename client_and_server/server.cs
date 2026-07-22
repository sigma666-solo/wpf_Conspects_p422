using System;
using System.Numerics;
using System.Net;
using System.Net.Sockets;
using System.Text;

class FactorialServer
{
    static void Main()
    {
        // Прием входящих подключений по  tcp
        const int port = 5000;
        var listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"Server started on port {port}...");

        while (true)
        {
             //ожидание нового подключения клиента
            var client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            // Обрабатываем клиента в отдельном потоке
            _ = System.Threading.Tasks.Task.Run(() => HandleClient(client));
        }
    }

    static void HandleClient(TcpClient client)
    {
        using (client)// гарантия закрытия соединения и освобождения ресурсов
        using (var stream = client.GetStream())
        {
            // Читаем 4 байта Int32
            byte[] buf = new byte[4];
            ReadExact(stream, buf, 0, 4);
            int n = BitConverter.ToInt32(buf, 0);
            //валидация
            if (n < 0)
            {
                SendString(stream, "Error: n must be >= 0");
                return;
            }
            //подсчет факториала
            BigInteger fact = Factorial(n);
            //отправка результата клиенту
            SendString(stream, fact.ToString());
        }
    }

    static BigInteger Factorial(int n)
    {
         // Изначально факториал 0! и 1! равен 1
        BigInteger result = BigInteger.One;

        // Перемножаем числа от 2 до n
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

    static void SendString(NetworkStream stream, string s)
    {
        // Отправим: [4 байта длина в байтах][данные UTF-8]
        byte[] data = Encoding.UTF8.GetBytes(s);
        // Длина в байтах (Int32)
        byte[] len = BitConverter.GetBytes(data.Length);
        // Отправляем длину и затем сам текст
        stream.Write(len, 0, len.Length);
        stream.Write(data, 0, data.Length);
    }

    static void ReadExact(NetworkStream stream, byte[] buffer, int offset, int count)
    {
        // Читаем строго count байт: TCP может вернуть меньше за один Read
        int readTotal = 0;

        while (readTotal < count)
        {
            int read = stream.Read(buffer, offset + readTotal, count - readTotal);
            // Если прочитали 0 — соединение закрыто
            if (read == 0) throw new Exception("Connection closed.");
            readTotal += read;
        }
    }
}


using System;
using System.Net.Sockets;
using System.Text;

class FactorialClient
{
    static void Main()
    {
        const string host = "127.0.0.1";
        const int port = 5000;

        Console.Write("Enter n: ");
        int n = int.Parse(Console.ReadLine()!);

        using var client = new TcpClient(host, port);
        using var stream = client.GetStream();

        // Отправляем Int32 (4 байта)
        byte[] buf = BitConverter.GetBytes(n);
        stream.Write(buf, 0, buf.Length);

        // Читаем ответ: [4 байта длина][UTF-8 строки]
        byte[] lenBuf = new byte[4];
        ReadExact(stream, lenBuf, 0, 4);
        int len = BitConverter.ToInt32(lenBuf, 0);

        byte[] data = new byte[len];
        ReadExact(stream, data, 0, len);

        string result = Encoding.UTF8.GetString(data);
        Console.WriteLine($"Factorial: {result}");
    }

    static void ReadExact(NetworkStream stream, byte[] buffer, int offset, int count)
    {
        int readTotal = 0;
        while (readTotal < count)
        {
            int read = stream.Read(buffer, offset + readTotal, count - readTotal);
            if (read == 0) throw new Exception("Connection closed.");
            readTotal += read;
        }
    }
}


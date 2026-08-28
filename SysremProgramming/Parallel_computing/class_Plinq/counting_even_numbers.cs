using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main()
    {
        int[] numbers = new int[1000000];  
        Random rand = new Random(42);
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = rand.Next(5000, 10000); 
        }

        var parallelNumbers = numbers.AsParallel();
        var conditionNumbers = parallelNumbers.Where(x => x % 2 == 0);
        
        Console.WriteLine("Запуск ПАРАЛЛЕЛЬНОГО вычисления");
        Stopwatch stopwatch = Stopwatch.StartNew();

        Console.WriteLine(conditionNumbers.Count());

        stopwatch.Stop();
        Console.WriteLine($"Параллельное вычисление завершено за: {stopwatch.ElapsedMilliseconds} мс");
    }
}
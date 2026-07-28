using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        // Генерируем массив из 1000 чисел, чтобы хорошо загрузить все ядра процессора
        int[] numbers = new int[1000];
        Random rand = new Random(42);
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = rand.Next(5000, 10000); 
        }

        BigInteger[] results = new BigInteger[numbers.Length];

        Console.WriteLine("Запуск ПАРАЛЛЕЛЬНОГО вычисления (1000 задач)...");
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Параллельный цикл распределяет вычисления по всем ядрам i3-10105F
        Parallel.For(0, numbers.Length, i =>
        {
            results[i] = ComputeFactorial(numbers[i]);
        });

        stopwatch.Stop();
        Console.WriteLine($"Параллельное вычисление завершено за: {stopwatch.ElapsedMilliseconds} мс");
        
        // Заставляем процессор переводить гигантские числа в строки
        Console.WriteLine("Конвертация результатов в текст...");
        long totalLength = 0;
        for (int i = 0; i < results.Length; i++)
        {
            totalLength += results[i].ToString().Length;
        }
        
        Console.WriteLine($"Суммарная длина всех результатов: {totalLength} знаков");
        Console.ReadLine();
    }

    static BigInteger ComputeFactorial(int number)
    {
        BigInteger result = 1;
        for (int i = 2; i <= number; i++)
        {
            result *= i;
        }
        return result;
    }
}

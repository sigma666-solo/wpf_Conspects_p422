using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        int[] numbers = new int [3000];
        Random rand = new Random(42);   
        for(int i =0;i<numbers.Length;i++)
        {
            numbers[i]=rand.Next(40000, 50000);
        }
        BigInteger[] result =new BigInteger[numbers.Length];

        Stopwatch stopwatch = Stopwatch.StartNew();
        Parallel.For(0, numbers.Length, i =>
        {
            result[i] = ComputeFibonacci(numbers[i]);    
        });

        Console.WriteLine($"Параллельное вычисление завершено за: {stopwatch.ElapsedMilliseconds} мс");
        Console.WriteLine($"Вычислено элементов: {result.Length}. Первая цифра первого числа: {result[0].ToString()[0]}");
        Console.ReadLine();
    }
    static BigInteger ComputeFibonacci(int n)
    {
        if(n<=0) return 0;
        if (n==1) return 1;

        BigInteger prev =0 ;
        BigInteger current =1;

        for (int i = 2; i <= n; i++)
        {
            BigInteger next = prev + current;
            prev = current;
            current = next;
        }
        return current;
    }
    
}


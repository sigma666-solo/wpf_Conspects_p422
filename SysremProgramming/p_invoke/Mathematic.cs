using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("MySumLibrary.dll", CallingConvention = CallingConvention.StdCall)]
    public static extern int Summa(int a, int b);

    [DllImport("MySumLibrary.dll",CallingConvention=CallingConvention.StdCall)]
    public static extern int Sub(int a, int b);

    [DllImport("MySumLibrary.dll",CallingConvention=CallingConvention.StdCall)]
    public static extern int Mult(int a, int b);

    [DllImport("MySumLibrary.dll",CallingConvention=CallingConvention.StdCall)]
    public static extern int Division(int a, int b);
    static void Main()
    {
        int sum_result = Summa (2,2);
        Console.WriteLine($"Результат сложения в C++: {sum_result}"); 
        int sub_result = Sub (2,2);
        Console.WriteLine($"Результат вычитания в C++: {sub_result}");

        int mult_result = Mult (2,2);
        Console.WriteLine($"Результат умножения в C++: {mult_result}");

        int division_result = Division (2,2);
        Console.WriteLine($"Результат деления в C++: {division_result}");
    }
}
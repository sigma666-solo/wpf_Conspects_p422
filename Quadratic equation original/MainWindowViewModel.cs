using System;
namespace Quadratic_equation_original;

//наследование от ViewModelBase
public class MainWindowViewModel : ViewModelBase
{
    private string _inputA = "";

     public string InputA
    {
        get => _inputA;
        set { _inputA = value; OnPropertyChanged(); }
    }

    private string _inputB = "";

    public string InputB
    {
        get => _inputB;
        set { _inputB = value; OnPropertyChanged(); }
    }

    private string _inputC = "";

    public string InputC
    {
        get => _inputC;
        set { _inputC = value; OnPropertyChanged(); }
    }

    private string _resultText = "Введите данные";

    public string ResultText
    {
        get => _resultText;
        set { _resultText = value; OnPropertyChanged(); }
    }

    // Метод для расчета (вызывается из View)
    
    public void ExecuteCalculate()
{
    // Печатаем в консоль для отладки
    Console.WriteLine($"DEBUG: A='{InputA}', B='{InputB}', C='{InputC}'");

    if (double.TryParse(InputA, out var a) && 
        double.TryParse(InputB, out var b) && 
        double.TryParse(InputC, out var c))
    {
        var (count, x1, x2) = EquationSolver.QuadraticEquationSolver(a, b, c);
        
        Console.WriteLine($"DEBUG: Solver returned count={count}");

        ResultText = count switch {
            2 => $"x1 = {x1:0.##}, x2 = {x2:0.##}",
            1 => $"x = {x1:0.##}",
            _ => "Корней нет"
        };
    }
    else
    {
        ResultText = "Ошибка ввода! Проверьте числа.";
    }
}

}
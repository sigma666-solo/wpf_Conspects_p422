using System.IO.Pipelines;
using System.Reflection.Metadata.Ecma335;
using Avalonia.Controls;
using Avalonia.Controls.Documents;


namespace Quadratic_equation_original;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    //обработка нажатия кнопки "Решить"
    private void Decide_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            //Извлечение текста из TextBox
            string strA = AInput.Text ?? "";
            string strB = BInput.Text ?? "";
            string strC = CInput.Text ?? "";
            
            //Преобразование строк в числа
            bool isSuccesA = double.TryParse(strA, out double numA);
            bool isSuccesB = double.TryParse(strB, out double numB);
            bool isSuccesC = double.TryParse(strC, out double numC);
            
            //проверка на числа и подсчёт результата
            if (isSuccesA && isSuccesB && isSuccesC)
            {
                var (count,x1,x2) = EquationSolver.QuadraticEquationSolver(numA,numB,numC);

                switch (count)
                {
                    case 2:
                        Result.Text=$"x1 = {x1:0.##}    x2 = {x2:0.##}";
                        break;
                    case 1:
                        Result.Text=$"x = {x1:0.##}";
                        break;
                    case 0:
                        Result.Text="Корней нет";
                        break;
                }
            }
            else
            {
                Result.Text = "Ошибка: одно из введённый значений не число!";
            }
        }
    }
}
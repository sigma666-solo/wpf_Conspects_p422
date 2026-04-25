using Avalonia.Controls;
using Avalonia.Interactivity;
namespace Quadratic_equation;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

     private void CheckBox_Toggled(object? sender, RoutedEventArgs e)
    {
        // Проверяем, что метод вызван именно чекбоксом
        if (sender is CheckBox checkBox)
        {
            // Находим TextBox по имени и меняем его доступность
            // IsChecked может быть null (три состояния), поэтому приводим к bool через ?? false
            MyInput.IsEnabled = checkBox.IsChecked ?? false;
        }
    }

}
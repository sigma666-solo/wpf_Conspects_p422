using System.IO.Pipelines;
using System.Reflection.Metadata.Ecma335;
using Avalonia.Controls;
using Avalonia.Controls.Documents;


namespace Quadratic_equation_original;

public partial class MainWindow : Window
{
    //экземпляр нашей логики _viewModel
    private readonly MainWindowViewModel _viewModel;
    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowViewModel();
        DataContext = _viewModel; // Привязываем данные
    }
    //обработка нажатия кнопки "Решить"
    private void Decide_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _viewModel.ExecuteCalculate();
    }
}
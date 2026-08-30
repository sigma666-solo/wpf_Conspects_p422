using Avalonia.Controls;
using ShopApp.ViewModels;

namespace ShopApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}

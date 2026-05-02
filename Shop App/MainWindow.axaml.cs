using Avalonia.Controls;
using ShopApp.ViewModels; // Чтобы видеть MainViewModel
using ShopApp.Services;   // На всякий случай

namespace ShopApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // Устанавливаем "мозги" окна. 
        // Теперь все {Binding} в XAML будут искать свойства в MainViewModel
        DataContext = new MainViewModel();
    }
}
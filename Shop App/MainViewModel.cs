using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShopApp.Data;

namespace ShopApp.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public string Status { get; set; } = "Shop App владеет базой данных (ProductsP422). Клиент и сервер находятся в Lesson2.";

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

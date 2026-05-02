using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShopApp.Models;   // Твой namespace из варианта Б
using ShopApp.Services; // Твой namespace из варианта Б

namespace ShopApp.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _dbService;

    // Список пользователей, который будет привязан к DataGrid или ListBox
    public ObservableCollection<User> Users { get; set; } = new();

    public MainViewModel()
    {
        _dbService = new DatabaseService();
        
        // Загружаем данные сразу при создании
        LoadUsers();
    }

    public void LoadUsers()
    {
        var data = _dbService.GetAllUsers();
        
        Users.Clear();
        foreach (var user in data)
        {
            Users.Add(user);
        }
    }

    // Стандартная реализация для обновления интерфейса
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
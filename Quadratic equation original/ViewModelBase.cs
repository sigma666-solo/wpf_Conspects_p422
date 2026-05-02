using System.ComponentModel;
using System.Runtime.CompilerServices;//Это пространство имен содержит сам интерфейс INotifyPropertyChanged.

namespace Quadratic_equation_original;

public class ViewModelBase : INotifyPropertyChanged
{
    //Это и есть «канал связи» с интерфейсом.
    public event PropertyChangedEventHandler? PropertyChanged;

    // Метод, который уведомляет интерфейс об изменениях
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
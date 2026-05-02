using Avalonia;
using System;

namespace ShopApp; // Проверь, чтобы тут было БЕЗ пробела и подчеркивания

class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>() // Если App подчеркнуто, проверь namespace в App.axaml.cs
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
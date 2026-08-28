using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Diagnostics;//для 12 строки
using System.IO;

namespace Father;

public partial class MainWindow : Window
{

    private Process? _secondAppProcess;
    public MainWindow()
    {
        InitializeComponent();
        BtnStop.IsEnabled = false; 

    }

    private void BtnStart_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_secondAppProcess != null && !_secondAppProcess.HasExited) return;//для предотврпащения открытия кучи окон
        var startInfo = new ProcessStartInfo();

        #if DEBUG //для отладчика 
        string secondProjectPath = @"D:\Git_Wpf_Conspects\wpf_Conspects_p422\Avalonia_Daughter\AnotherWpfApp\AnotherWpfApp.csproj";
        startInfo.FileName = "dotnet";//утилита командной строки для запуска
        startInfo.Arguments = $"run --project \"{secondProjectPath}\"";//аргументы запуска

        #else //для релиза(собранной программы)
        string exeName = OperatingSystem.IsWindows() ? "AnotherWpfApp.exe" : "AnotherAvaloniaApp";
        startInfo.FileName = Path.Combine(AppContext.BaseDirectory, exeName);
        #endif

        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = false; 

        try
        {
            _secondAppProcess = Process.Start(startInfo);

             if (_secondAppProcess != null)
            {
                // ПОЛУЧАЕМ ID дочернего процесса и выводим его на экран родительского окна
                int childPid = _secondAppProcess.Id;
                TxtChildPid.Text = $"Запущен дочерний PID: {childPid}";
            }

            BtnStart.IsEnabled = false;
            BtnStop.IsEnabled = true;
        }
         catch (Exception ex) 
        {
            Console.WriteLine($"Ошибка запуска процесса: {ex.Message}");
            TxtChildPid.Text = "Ошибка запуска процесса!";
        }
    }

      private void BtnStop_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_secondAppProcess != null && !_secondAppProcess.HasExited)
        {
            _secondAppProcess.Kill(entireProcessTree: true);
            _secondAppProcess.Dispose();
            _secondAppProcess = null;
        }

        BtnStart.IsEnabled = true;
        BtnStop.IsEnabled = false;

        // Сбрасываем текст состояния
        TxtChildPid.Text = "Дочерний процесс завершен";
    }
}

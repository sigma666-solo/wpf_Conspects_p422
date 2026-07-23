using Avalonia.Controls;
using System;
using System.Windows;

namespace AnotherWpfApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

// Получаем уникальный ID этого процесса в ОС и выводим на экран
        int currentPid = Environment.ProcessId; 
        TxtPid.Text = currentPid.ToString();
    }
}
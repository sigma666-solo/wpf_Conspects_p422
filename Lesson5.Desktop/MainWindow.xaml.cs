using System;
using System.Threading;
using System.Windows;

namespace Lesson5.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CancellationTokenSource? _cancellationTokenSource;
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private void SumButton_Click(object sender, RoutedEventArgs e)
    {
        ulong number = ulong.Parse(Number.Text);
        StartBackgroundTask((token) => CalculateSum(number, token));
    }

    // Обработчик для новой кнопки разности
    private void DifferenceButton_Click(object sender, RoutedEventArgs e)
    {
        ulong number = ulong.Parse(Number.Text);
        StartBackgroundTask((token) => CalculateDifference(number, token));
    }

    // Вынесенный общий метод для инициализации токена и потока
    private void StartBackgroundTask(Action<CancellationToken> action)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        ChangeStatusControlElements();
        ThreadPool.QueueUserWorkItem(_ => action(cancellationToken));
    }

    private void CalculateSum(ulong number, CancellationToken cancellationToken)
    {
        RunTaskWithCancellation(cancellationToken, () =>
        {
            ulong sum = 0;
            for (ulong i = 1; i <= number; i++)
            {
                sum += i;
                cancellationToken.ThrowIfCancellationRequested();
            }
            return sum.ToString();
        });
    }

    // Новый метод для подсчёта разности чисел от 1 до N
    private void CalculateDifference(ulong number, CancellationToken cancellationToken)
    {
        RunTaskWithCancellation(cancellationToken, () =>
        {
            // Пример логики разности: 0 - 1 - 2 - 3...
            long difference = 0; 
            for (ulong i = 1; i <= number; i++)
            {
                // Приведение к long во избежание переполнения ulong при отрицательных значениях
                difference -= (long)i; 
                cancellationToken.ThrowIfCancellationRequested();
            }
            return difference.ToString();
        });
    }

    // Общая обёртка для обработки отмены и обновления UI
    private void RunTaskWithCancellation(CancellationToken cancellationToken, Func<string> calculation)
    {
        try
        {
            string resultStr = calculation();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Result.Text = resultStr;
            });
        }
        catch (OperationCanceledException)
        {
            MessageBox.Show("Операция успешно отменена");
        }
        finally
        {
            Application.Current.Dispatcher.Invoke(ChangeStatusControlElements);
        }

        SomeAction(new SomeClass1());
        SomeAction(new SomeClass2());
    }

    private void SomeAction(AbstractSomeClass abstractSomeClass)
    {
        abstractSomeClass.Foo();
    }

    abstract class AbstractSomeClass
    {
        public abstract void Foo();
    }

    class SomeClass1 : AbstractSomeClass
    {
        public override void Foo() { }
    }

    class SomeClass2 : AbstractSomeClass
    {
        public override void Foo() { }
    }

    private void ChangeStatusControlElements()
    {
        Number.IsEnabled = !Number.IsEnabled;
        SumButton.IsEnabled = !SumButton.IsEnabled;
        DifferenceButton.IsEnabled = !DifferenceButton.IsEnabled; // Блокируем новую кнопку
        CancellationSumButton.IsEnabled = !CancellationSumButton.IsEnabled;
    }

    private void CancellationSumButton_Click(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource?.Cancel();
    }
}

using System;
using System.Threading;

namespace SafeAccount;

//логика банковского счета
public class BankAccount
{
    //заглушка
    private readonly object _locker = new object();
    private int _balance;

    //конструктор для начального баланса
    public BankAccount(int initialBalance)
    {
        _balance = initialBalance;
    }

    //свойство для безопасного получения текущего баланса
    public int Balance
    {
        get
        {
            //блокировка для избежания чтения частично измененного значения
            lock(_locker)
            {
                return _balance;
            }
        }
    }

    //метод для безопасного пополнения баланса
    public void Credit(int amount)
    {
        lock(_locker)
        {
            _balance += amount;
            Console.WriteLine($"[Поток {Thread.CurrentThread.ManagedThreadId}]: Положено {amount}. Баланс: {_balance}");
        }
    }

    //метод для снятия баланса (с проверкой)
    public void Debit(int amount)
    {
        lock(_locker)
        {
            if(_balance>=amount)
            {
                _balance -= amount;
                Console.WriteLine($"[Поток {Thread.CurrentThread.ManagedThreadId}]: Снято {amount}. Баланс: {_balance}");
            }
            else
            {
                Console.WriteLine($"[Поток {Thread.CurrentThread.ManagedThreadId}]: Отказ! Недостаточно средств для снятия {amount}. Баланс: {_balance}");
            }
        }
    }
}

public class Program
{
    static void Main()
    {
        Console.WriteLine("Запуск многопоточной симуляции...");

        //объект класса общего счета
        BankAccount account = new BankAccount (1000);

        // Создаем потоки и передаем им методы объекта account через лямбда-выражения
        Thread t1 = new Thread(() => RunWithdrawals(account));
        Thread t2 = new Thread(() => RunDeposits(account));

        // Запуск потоков
        t1.Start();
        t2.Start();

        // Ожидание завершения работы обоих потоков
        t1.Join();
        t2.Join();

        // Проверяем финальный баланс через свойство Balance
        Console.WriteLine($"\nПроверка завершена. Финальный баланс: {account.Balance}");

         // Логика работы потока на снятие
    static void RunWithdrawals(BankAccount account)
    {
        for (int i = 0; i < 5; i++)
        {
            account.Debit(300);
            Thread.Sleep(10); // Имитация задержки для переключения контекста ОС
        }
    }
    }
    // Логика работы потока на пополнение
    static void RunDeposits(BankAccount account)
    {
        for (int i = 0; i < 5; i++)
        {
            account.Credit(200);
            Thread.Sleep(10);
        }
    }
}

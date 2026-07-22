using System;

namespace Lesson7.ViewModels
{
    /// <summary>
    /// Потокобезопасный класс банковского счета.
    /// Поддерживает пополнение (Credit), списание (Debit) с проверкой
    /// достаточности средств и получение баланса (Balance).
    /// </summary>
    public class Account
    {
        private decimal _balance;
        private readonly object _lock = new();

        /// <summary>
        /// Создает счет с указанным начальным балансом.
        /// </summary>
        /// <param name="initialBalance">Начальный баланс (не может быть отрицательным).</param>
        /// <exception cref="ArgumentException">Если initialBalance &lt; 0.</exception>
        public Account(decimal initialBalance)
        {
            if (initialBalance < 0)
                throw new ArgumentException("Начальный баланс не может быть отрицательным.", nameof(initialBalance));

            _balance = initialBalance;
        }

        /// <summary>
        /// Текущий баланс счета. Потокобезопасен.
        /// </summary>
        public decimal Balance
        {
            get
            {
                lock (_lock)
                {
                    return _balance;
                }
            }
        }

        /// <summary>
        /// Пополняет счет на указанную сумму.
        /// </summary>
        /// <param name="amount">Сумма пополнения (должна быть положительной).</param>
        /// <exception cref="ArgumentException">Если amount &lt;= 0.</exception>
        public void Credit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма пополнения должна быть положительной.", nameof(amount));

            lock (_lock)
            {
                _balance += amount;
            }
        }

        /// <summary>
        /// Списывает указанную сумму со счета.
        /// </summary>
        /// <param name="amount">Сумма списания (должна быть положительной).</param>
        /// <returns>true, если списание выполнено успешно; false, если недостаточно средств.</returns>
        /// <exception cref="ArgumentException">Если amount &lt;= 0.</exception>
        public bool Debit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма списания должна быть положительной.", nameof(amount));

            lock (_lock)
            {
                if (_balance >= amount)
                {
                    _balance -= amount;
                    return true;
                }
                return false;
            }
        }
    }
}

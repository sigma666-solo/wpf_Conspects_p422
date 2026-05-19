using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient; // NEW: Главный пакет для работы с ADO.NET

namespace Lesson7.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // Строка подключения теперь хранится прямо здесь в виде константы
        private const string ConnectionString = "Server=localhost;Database=CompanyDB;User Id=SA;Password=YourStrong%Passw0rd;TrustServerCertificate=True;";

        private ObservableCollection<Employee> _employees = new();
        private Employee? _selectedEmployee;
        private string _fullNameInput = string.Empty;
        private string _positionInput = string.Empty;
        private string _departmentInput = string.Empty;
        private string _salaryInput = string.Empty;
        private string _errorMessage = string.Empty;

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set { _employees = value; OnPropertyChanged(); }
        }

        public Employee? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
                FillInputsFromSelected();
            }
        }

        public string FullNameInput { get => _fullNameInput; set { _fullNameInput = value; OnPropertyChanged(); } }
        public string PositionInput { get => _positionInput; set { _positionInput = value; OnPropertyChanged(); } }
        public string DepartmentInput { get => _departmentInput; set { _departmentInput = value; OnPropertyChanged(); } }
        public string SalaryInput { get => _salaryInput; set { _salaryInput = value; OnPropertyChanged(); } }
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }

        public MainWindowViewModel()
        {
            LoadEmployees();
        }

        // ========================================================
        // ADO.NET: Чтение данных (SELECT)
        // ========================================================
        private void LoadEmployees()
        {
            var list = new ObservableCollection<Employee>();
            string query = "SELECT id, full_name, position, department, salary FROM Employees";

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var emp = new Employee
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Position = reader.GetString(2),
                                Department = reader.GetString(3),
                                Salary = reader.GetDecimal(4)
                            };
                            list.Add(emp);
                        }
                    }
                }
                Employees = list;
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка загрузки данных: {ex.Message}";
            }
        }

        private void FillInputsFromSelected()
        {
            if (SelectedEmployee != null)
            {
                FullNameInput = SelectedEmployee.FullName;
                PositionInput = SelectedEmployee.Position;
                DepartmentInput = SelectedEmployee.Department;
                SalaryInput = SelectedEmployee.Salary.ToString("F2");
                ErrorMessage = string.Empty;
            }
        }

        private bool ValidateData(out decimal parsedSalary)
        {
            parsedSalary = 0;
            if (string.IsNullOrWhiteSpace(FullNameInput)) { ErrorMessage = "Ошибка: ФИО не может быть пустым!"; return false; }
            if (string.IsNullOrWhiteSpace(PositionInput)) { ErrorMessage = "Ошибка: Должность не может быть пустой!"; return false; }
            if (string.IsNullOrWhiteSpace(DepartmentInput)) { ErrorMessage = "Ошибка: Отдел не может быть пустым!"; return false; }
            if (!decimal.TryParse(SalaryInput, out parsedSalary) || parsedSalary < 0) { ErrorMessage = "Ошибка: Некорректная зарплата!"; return false; }
            ErrorMessage = string.Empty;
            return true;
        }

        // ========================================================
        // ADO.NET: Сохранение (INSERT или UPDATE)
        // ========================================================
        public void SaveEmployee()
        {
            if (!ValidateData(out decimal salary)) return;

            string query;
            if (SelectedEmployee == null)
            {
                // Запрос на добавление
                query = "INSERT INTO Employees (full_name, position, department, salary) VALUES (@FullName, @Position, @Department, @Salary)";
            }
            else
            {
                // Запрос на обновление по id
                query = "UPDATE Employees SET full_name = @FullName, position = @Position, department = @Department, salary = @Salary WHERE id = @Id";
            }

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        // Безопасная передача параметров против SQL-инъекций
                        command.Parameters.AddWithValue("@FullName", FullNameInput);
                        command.Parameters.AddWithValue("@Position", PositionInput);
                        command.Parameters.AddWithValue("@Department", DepartmentInput);
                        command.Parameters.AddWithValue("@Salary", salary);
                        
                        if (SelectedEmployee != null)
                        {
                            command.Parameters.AddWithValue("@Id", SelectedEmployee.Id);
                        }

                        command.ExecuteNonQuery(); // Выполняем команду в базе
                    }
                }
                LoadEmployees(); // Обновляем таблицу
                ClearInputs();   // Сбрасываем форму
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка сохранения: {ex.Message}";
            }
        }

        public void ClearInputs()
        {
            SelectedEmployee = null;
            FullNameInput = string.Empty;
            PositionInput = string.Empty;
            DepartmentInput = string.Empty;
            SalaryInput = string.Empty;
            ErrorMessage = string.Empty;
        }

        // ========================================================
        // ADO.NET: Удаление (DELETE)
        // ========================================================
        public void DeleteEmployee()
        {
            if (SelectedEmployee == null)
            {
                ErrorMessage = "Ошибка: Выберите сотрудника для удаления!";
                return;
            }

            string query = "DELETE FROM Employees WHERE id = @Id";

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", SelectedEmployee.Id);
                        command.ExecuteNonQuery();
                    }
                }
                LoadEmployees();
                ClearInputs();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при удалении: {ex.Message}";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

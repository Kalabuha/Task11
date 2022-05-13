using Bank_StashYourCrap.Bank.DataContext.Interfaces;
using Bank_StashYourCrap.Bank.DataContext.RepositoriesDataBase.Entities;
using Bank_StashYourCrap.Bank.DataContext.RepositoriesDataBase.Scripts;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Bank_StashYourCrap.Bank.PeopleModels.Employees.Base;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank_StashYourCrap.Bank.DataContext.RepositoriesDataBase
{
    internal class RepositoryEmployeesDataBase : IRepositoryEmployees
    {
        private readonly SqlConnectionStringBuilder _builder;

        public RepositoryEmployeesDataBase()
        {
            _builder = new SqlConnectionStringBuilder
            {
                ApplicationName = "Bank_Stash_Your_Crap_Application",
                DataSource = "(LocalDB)\\mssqllocaldb",
                InitialCatalog = "Bank_Stash_Your_Crap_DateBase",
                UserID = "",
                Password = "",
                TrustServerCertificate = true,
                Pooling = true
            };

            CheckDatabase(_builder);
        }

        private void CheckDatabase(SqlConnectionStringBuilder builder)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                for (int attemptsConnect = 2; attemptsConnect > 0; attemptsConnect--)
                {
                    try
                    {
                        connection.Open();
                        return;
                    }
                    catch (Exception) when (attemptsConnect > 1)
                    {
                        DataBaseScripts.BuildDataBase(builder);
                    }
                }

                throw new Exception("Не удалось создать и подключиться к БД");
            }
        }

        public async Task<Employee?> GetOneManAsync(int passSeries, int passNumber)
        {
            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = $"SELECT " +
                         $"C.Name, C.Surname, C.Patronymic, C.Pass_Series, C.Pass_Number, C.Access_Level, Phone_Numbers_id " +
                         $"FROM [Employees] AS C WHERE C.Pass_Series = {passSeries} AND C.Pass_Number = {passNumber}";
                    var employeeEntities = await GetEmployeeEntities(command);
                    if (employeeEntities.Count == 0)
                        return null;
                    if (employeeEntities.Count != 1)
                        throw new ApplicationException($"Должен был быть один работник с паспортом {passSeries} {passNumber}");

                    command.CommandText = $"SELECT " +
                        $"P.Phone_Number_id, P.Phone_Number " +
                        $"FROM [Phone_Numbers] AS P WHERE P.Phone_Number_id = {employeeEntities[0].PhoneNumbersId}";
                    var phoneNumberEntities = await GetPhoneNumberEntities(command);

                    return AssembleClient(employeeEntities, phoneNumberEntities).FirstOrDefault();
                }
                catch (Exception)
                {
                    throw new ApplicationException("Не удалось создать и подключиться к БД");
                }
            }
        }

        public async Task<List<Employee>?> GetCollectionPeopleAsync()
        {
            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = "SELECT " +
                        "C.Name, C.Surname, C.Patronymic, C.Pass_Series, C.Pass_Number, C.Access_Level, Phone_Numbers_id " +
                        "FROM [Employees] AS C";
                    var employeeEntities = await GetEmployeeEntities(command);

                    command.CommandText = "SELECT " +
                        "P.Phone_Number_id, P.Phone_Number " +
                        "FROM [Phone_Numbers] AS P";
                    var phoneNumberEntities = await GetPhoneNumberEntities(command);

                    return AssembleClient(employeeEntities, phoneNumberEntities);
                }
                catch (Exception)
                {
                    throw new ApplicationException("Не удалось создать и подключиться к БД");
                }
            }
        }

        public async Task AddManAsync(Employee? newMan)
        {
            throw new NotImplementedException("Добавить работника метод отстуствует");
        }

        public async Task EditManAsync(Employee? changedMan)
        {
            throw new NotImplementedException("Изменить работника метод отстуствует");
        }

        public async Task DeleteManAsync(Employee? removedMan)
        {
            throw new NotImplementedException("Удалить работника метод отстуствует");
        }

        private async Task<List<EmployeeEntity>> GetEmployeeEntities(SqlCommand command)
        {
            var clientEntities = new List<EmployeeEntity>();

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    clientEntities.Add(new EmployeeEntity
                    {
                        Name = reader.GetString(0),
                        Surname = reader.GetString(1),
                        Patronymic = reader.GetString(2),
                        PassSeries = reader.GetInt32(3),
                        PassNumber = reader.GetInt32(4),
                        AccessLevel = reader.GetInt32(5),
                        PhoneNumbersId = reader.GetInt32(6)
                    });
                }
            }
            return clientEntities;
        }

        private async Task<List<PhoneNumberEntity>> GetPhoneNumberEntities(SqlCommand command)
        {
            var phoneNumberEntities = new List<PhoneNumberEntity>();

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    phoneNumberEntities.Add(new PhoneNumberEntity
                    {
                        Id = reader.GetInt32(0),
                        PhoneNumber = reader.GetInt64(1)
                    });
                }
            }
            return phoneNumberEntities;
        }

        private List<Employee> AssembleClient(
            ICollection<EmployeeEntity> employeeEntities,
            ICollection<PhoneNumberEntity> phoneEntities)
        {
            var employees = new List<Employee>();
            foreach (var employeeEntity in employeeEntities)
            {
                var employee = new Employee
                {
                    Name = employeeEntity.Name,
                    Surname = employeeEntity.Surname,
                    Patronymic = employeeEntity.Patronymic,
                    PassSeries = employeeEntity.PassSeries,
                    PassNumber = employeeEntity.PassNumber,
                    AccessLevel = (EmployeeAccessLevel)employeeEntity.AccessLevel
                };

                var employee_phones = phoneEntities
                    .Where(p => p.Id == employeeEntity.PhoneNumbersId)
                    .ToArray();
                foreach (var phoneNumber in employee_phones)
                {
                    employee.PhoneNumbers.Add(phoneNumber.PhoneNumber.ToString()!);
                }

                employees.Add(employee);
            }
            return employees;
        }
    }
}
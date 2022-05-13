using Bank_StashYourCrap.Bank.PeopleModels.Clients;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Bank_StashYourCrap.Bank.DataContext.RepositoriesDataBase.Scripts
{
    internal static class DataBaseScripts
    {
        internal static void BuildDataBase(SqlConnectionStringBuilder connectionStringBuilder)
        {
            CreateDataBase(connectionStringBuilder);
            Thread.Sleep(3000);
            CreateTables(connectionStringBuilder);
            Thread.Sleep(3000);

            FillDataBase(connectionStringBuilder);
        }

        private static void CreateDataBase(SqlConnectionStringBuilder connectionStringBuilder)
        {
            string createDataBase = $"CREATE DATABASE {connectionStringBuilder.InitialCatalog}";
            var connectionStringBuilderInitial = new SqlConnectionStringBuilder
            {
                ApplicationName = connectionStringBuilder.ApplicationName,
                DataSource = connectionStringBuilder.DataSource,
                InitialCatalog = "master", // Для создания новой БД нужно подключиться к БД master
                UserID = connectionStringBuilder.UserID,
                Password = connectionStringBuilder.Password,
                TrustServerCertificate = connectionStringBuilder.TrustServerCertificate,
                Pooling = connectionStringBuilder.Pooling
            };

            ExecuteNonQueryCommands(connectionStringBuilderInitial, createDataBase);
        }

        private static void CreateTables(SqlConnectionStringBuilder connectionStringBuilder)
        {
            // Создание таблиц
            string createClientsTable =
                "CREATE TABLE [Clients](" +
                "[Name] nvarchar(20) NOT NULL, " +
                "[Surname] nvarchar(20) NOT NULL, " +
                "[Patronymic] nvarchar(20) NOT NULL, " +
                "[Pass_Series] INT NOT NULL, " +
                "[Pass_Number] INT NOT NULL, " +
                "[Bank_Accounts_id] INT NOT NULL, " +
                "[Phone_Numbers_id] INT NOT NULL)";

            string createEmployeesTable =
                "CREATE TABLE [Employees](" +
                "[Name] nvarchar(20) NOT NULL, " +
                "[Surname] nvarchar(20) NOT NULL, " +
                "[Patronymic] nvarchar(20) NOT NULL, " +
                "[Pass_Series] INT NOT NULL, " +
                "[Pass_Number] INT NOT NULL, " +
                "[Access_Level] INT NOT NULL, " +
                "[Phone_Numbers_id] INT NOT NULL)";

            string createPhoneNumbersTable =
                "CREATE TABLE [Phone_Numbers](" +
                "[Phone_Number_id] INT NOT NULL, " +
                "[Phone_Number] BIGINT NOT NULL)";

            string createBankAccountsTable =
                "CREATE TABLE [Bank_Accounts](" +
                "[Bank_Account_id] INT NOT NULL, " +
                "[Accounts_Number] BIGINT NOT NULL, " +
                "[Accounts_Type] INT NOT NULL)";

            // Первичные ключи
            string setPkClientsTable =
                "ALTER TABLE [Clients] " +
                "ADD CONSTRAINT [PK_Clients_Passport] PRIMARY KEY ([Pass_Series], [Pass_Number])";

            string setPkEmploeesTable =
                "ALTER TABLE [Employees] " +
                "ADD CONSTRAINT [PK_Employees_Passport] PRIMARY KEY ([Pass_Series], [Pass_Number])";

            // Вторичные ключи и связи таблиц
            //string setFkPhonesClientsTable =
            //    "ALTER TABLE [Clients] " +
            //    "ADD CONSTRAINT [FK_Clients_Phones_id] FOREIGN KEY ([Phone_Numbers_id]) " +
            //    "REFERENCES [Phone_Numbers] ([Phone_Number_id]) " +
            //    "ON DELETE CASCADE " +
            //    "ON UPDATE CASCADE";

            //string setFkAccountsClientsTable =
            //    "ALTER TABLE [Clients] " +
            //    "ADD CONSTRAINT [FK_Clients_Accounts_id] FOREIGN KEY ([Bank_Accounts_id]) " +
            //    "REFERENCES [Bank_Accounts] ([Bank_Account_id]) " +
            //    "ON DELETE CASCADE " +
            //    "ON UPDATE CASCADE";

            //string setFkPhonesEmployeesTable =
            //    "ALTER TABLE [Employees] " +
            //    "ADD CONSTRAINT [FK_Employees_Phones_id] FOREIGN KEY ([Phone_Numbers_id]) " +
            //    "REFERENCES [Phone_Numbers] ([Phone_Number_id]) " +
            //    "ON DELETE CASCADE " +
            //    "ON UPDATE CASCADE";

            ExecuteNonQueryCommands(
                connectionStringBuilder,
                createClientsTable,
                createEmployeesTable,
                createPhoneNumbersTable,
                createBankAccountsTable,
                setPkClientsTable,
                setPkEmploeesTable
                //setFkPhonesClientsTable,
                //setFkAccountsClientsTable,
                //setFkPhonesEmployeesTable
                );
        }

        private static void FillDataBase(SqlConnectionStringBuilder connectionStringBuilder)
        {
            string insertPhoneNumbers = "INSERT INTO [Phone_Numbers] (" +
                "[Phone_Number_id], " +
                "[Phone_Number]) VALUES ";

            string insertBankAccounts = "INSERT INTO [Bank_Accounts] (" +
                "[Bank_Account_id], " +
                "[Accounts_Number], " +
                "[Accounts_Type]) VALUES ";

            string insertClients = "INSERT INTO [Clients] (" +
                "[Name], " +
                "[Surname], " +
                "[Patronymic], " +
                "[Pass_Series], " +
                "[Pass_Number], " +
                "[Bank_Accounts_id], " +
                "[Phone_Numbers_id]) VALUES ";

            string insertEmployees = "INSERT INTO [Employees] (" +
                "[Name], " +
                "[Surname], " +
                "[Patronymic], " +
                "[Pass_Series], " +
                "[Pass_Number], " +
                "[Access_Level], " +
                "[Phone_Numbers_id]) VALUES ";

            // Получение строк со значениями
            int phoneNumberId = 1;
            (string phoneNumbersClientsValues, string bankAccountsClientsValues, string clientsValues, phoneNumberId) =
                GetValuesStringClients(phoneNumberId);

            (string phoneNumbersEmployeesValues, string employeesValues) =
                GetValuesStringEmployees(phoneNumberId);

            string[] insertRequests = null!;
            if (!string.IsNullOrEmpty(clientsValues))
            {
                insertRequests = new string[]
                {
                    insertPhoneNumbers + phoneNumbersClientsValues,
                    insertBankAccounts + bankAccountsClientsValues,
                    insertClients + clientsValues
                };
            }

            if (!string.IsNullOrEmpty(employeesValues))
            {
                if (insertRequests != null)
                {
                    insertRequests = new string[]
                    {
                        insertRequests[0],
                        insertRequests[1],
                        insertRequests[2],
                        insertPhoneNumbers + phoneNumbersEmployeesValues,
                        insertEmployees + employeesValues
                    };
                }
                else
                {
                    insertRequests = new string[]
                    {
                        insertPhoneNumbers + phoneNumbersEmployeesValues,
                        insertEmployees + employeesValues
                    };
                }
            }

            if (insertRequests != null && insertRequests.Length != 0)
            {
                ExecuteNonQueryCommands(connectionStringBuilder, insertRequests);
            }
        }

        private static (string, string, string, int) GetValuesStringClients(int phoneNumberId)
        {
            string clientsValuesString = string.Empty;
            string phoneNumbersValuesString = string.Empty;
            string bankAccountsValuesString = string.Empty;

            var allClients = ReadFileClients();

            int bankAccountId = 1;

            if (allClients != null)
            {
                for (int i = 0; i < allClients.Count; i++)
                {
                    clientsValuesString +=
                        $"('{allClients[i].Name}', " +
                        $"'{allClients[i].Surname}', " +
                        $"'{allClients[i].Patronymic}', " +
                        $"{allClients[i].PassSeries}, " +
                        $"{allClients[i].PassNumber}, " +
                        $"{phoneNumberId}, " +
                        $"{bankAccountId})";

                    for (int p = 0; p < allClients[i].PhoneNumbers.Count; p++)
                    {
                        phoneNumbersValuesString +=
                            $"({phoneNumberId}, " +
                            $"{allClients[i].PhoneNumbers[p]})";

                        if (p != allClients[i].PhoneNumbers.Count - 1 || i != allClients.Count - 1)
                        {
                            phoneNumbersValuesString += ", ";
                        }
                    }

                    for (int a = 0; a < allClients[i].Accounts.Count; a++)
                    {
                        bankAccountsValuesString +=
                            $"({bankAccountId}, " +
                            $"{allClients[i].Accounts[a].NumberAccount}, " +
                            $"{(int)allClients[i].Accounts[a].TypeAccount})";

                        if (a != allClients[i].Accounts.Count - 1 || i != allClients.Count - 1)
                        {
                            bankAccountsValuesString += ", ";
                        }
                    }

                    phoneNumberId++;
                    bankAccountId++;

                    if (i != allClients.Count - 1)
                    {
                        clientsValuesString += ", ";
                    }
                }
            }
            return (phoneNumbersValuesString, bankAccountsValuesString, clientsValuesString, phoneNumberId);
        }

        private static (string, string) GetValuesStringEmployees(int phoneNumberId)
        {
            string employeesValuesString = string.Empty;
            string phoneNumbersValuesString = string.Empty;

            var allEmployees = ReadFileEmployees();

            if (allEmployees != null)
            {
                for (int i = 0; i < allEmployees.Count; i++)
                {
                    employeesValuesString +=
                        $"('{allEmployees[i].Name}', " +
                        $"'{allEmployees[i].Surname}', " +
                        $"'{allEmployees[i].Patronymic}', " +
                        $"{allEmployees[i].PassSeries}, " +
                        $"{allEmployees[i].PassNumber}, " +
                        $"{(int)allEmployees[i].AccessLevel}, " +
                        $"{phoneNumberId})";

                    for (int p = 0; p < allEmployees[i].PhoneNumbers.Count; p++)
                    {
                        phoneNumbersValuesString +=
                            $"({phoneNumberId}, " +
                            $"{allEmployees[i].PhoneNumbers[p]})";

                        if (p != allEmployees[i].PhoneNumbers.Count - 1 || i != allEmployees.Count - 1)
                        {
                            phoneNumbersValuesString += ", ";
                        }
                    }

                    phoneNumberId++;

                    if (i != allEmployees.Count - 1)
                    {
                        employeesValuesString += ", ";
                    }
                }
            }
            return (phoneNumbersValuesString, employeesValuesString);
        }

        private static void ExecuteNonQueryCommands(SqlConnectionStringBuilder connectionStringBuilder, params string[] sqlRequests)
        {
            if (sqlRequests == null && sqlRequests!.Length != 0)
            {
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                try
                {
                    for (int i = 0; i < sqlRequests.Length; i++)
                    {
                        command.CommandText = sqlRequests[i];
                        command.ExecuteNonQuery();
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private static List<Client>? ReadFileClients()
        {
            string directoryPath = @"..\..\..\Bank\DataContext\RepositoriesDataFiles\Data";
            string fileClientsName = @"ClientsData.json";
            string fullfileClientsName = Path.Combine(directoryPath, fileClientsName);

            string AllLine;
            using (StreamReader sr = new StreamReader(fullfileClientsName, Encoding.UTF8))
            {
                AllLine = sr.ReadToEnd();
            }
            var clients = JsonConvert.DeserializeObject<List<Client>>(AllLine);

            return clients;
        }

        private static List<Employee>? ReadFileEmployees()
        {
            string directoryPath = @"..\..\..\Bank\DataContext\RepositoriesDataFiles\Data";
            string fileEmployeesName = @"EmployeesData.json";
            string fullfileEmployeesName = Path.Combine(directoryPath, fileEmployeesName);

            string AllLine;
            using (StreamReader sr = new StreamReader(fullfileEmployeesName, Encoding.UTF8))
            {
                AllLine = sr.ReadToEnd();
            }
            var employees = JsonConvert.DeserializeObject<List<Employee>>(AllLine);

            return employees;
        }
    }
}

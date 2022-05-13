using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;
using Bank_StashYourCrap.Bank.DataContext.Interfaces;
using Bank_StashYourCrap.Bank.BankModels;
using Bank_StashYourCrap.Bank.DataContext.RepositoriesDataBase.Scripts;
using Bank_StashYourCrap.Bank.DataContext.RepositoriesDataBase.Entities;

namespace Bank_StashYourCrap.Bank.DataContext.RepositoriesDataBase
{
    internal class RepositoryClientsDataBase : IRepositoryClients
    {
        private readonly SqlConnectionStringBuilder _builder;

        public RepositoryClientsDataBase()
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
        }

        // Чтобы найти человека, нужно знать его серию и номер паспорта.
        public async Task<Client?> GetOneManAsync(int passSeries, int passNumber)
        {
            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = $"SELECT " +
                        $"C.Name, C.Surname, C.Patronymic, C.Pass_Series, C.Pass_Number, C.Bank_Accounts_id, Phone_Numbers_id " +
                        $"FROM [Clients] AS C WHERE C.Pass_Series = {passSeries} AND C.Pass_Number = {passNumber}";
                    var clientEntities = await GetClientEntities(command);
                    if (clientEntities.Count == 0)
                        return null;
                    if (clientEntities.Count != 1)
                        throw new ApplicationException($"Должен был быть один клиент с паспортом {passSeries} {passNumber}");

                    command.CommandText = $"SELECT " +
                        $"P.Phone_Number_id, P.Phone_Number " +
                        $"FROM [Phone_Numbers] AS P WHERE P.Phone_Number_id = {clientEntities[0].PhoneNumbersId}";
                    var phoneNumberEntities = await GetPhoneNumberEntities(command);

                    command.CommandText = $"SELECT " +
                        $"A.Bank_Account_id, A.Accounts_Number, A.Accounts_Type " +
                        $"FROM [Bank_Accounts] AS A WHERE A.Bank_Account_id = {clientEntities[0].AccountId}";
                    var bankAccountsEntities = await GetBankAccountEntities(command);

                    return AssembleClient(clientEntities, phoneNumberEntities, bankAccountsEntities).FirstOrDefault();
                }
                catch (Exception)
                {
                    throw new ApplicationException("Не удалось создать и подключиться к БД");
                }
            }
        }

        public async Task<List<Client>?> GetCollectionPeopleAsync()
        {
            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = "SELECT " +
                        "C.Name, C.Surname, C.Patronymic, C.Pass_Series, C.Pass_Number, C.Bank_Accounts_id, Phone_Numbers_id " +
                        "FROM [Clients] AS C";
                    var clientEntities = await GetClientEntities(command);
                    if (clientEntities.Count == 0)
                        return null;

                    command.CommandText = "SELECT " + 
                        "P.Phone_Number_id, P.Phone_Number " + 
                        "FROM [Phone_Numbers] AS P";
                    var phoneNumberEntities = await GetPhoneNumberEntities(command);

                    command.CommandText = "SELECT " +
                        "A.Bank_Account_id, A.Accounts_Number, A.Accounts_Type " +
                        "FROM [Bank_Accounts] AS A";
                    var bankAccountsEntities = await GetBankAccountEntities(command);
                    
                    return AssembleClient(clientEntities, phoneNumberEntities, bankAccountsEntities);
                }
                catch (Exception)
                {
                    throw new ApplicationException("Не удалось создать и подключиться к БД");
                }
            }
        }

        public async Task AddManAsync(Client? newMan)
        {
            if (newMan == null)
                return;

            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = connection.CreateCommand();
                    (int phoneNumbersNewId, int bankAccountsNewId) = await GetNewId(command);

                    command.CommandText = $"INSERT INTO [Clients] " +
                        $"([Name], [Surname], [Patronymic], [Pass_Series], [Pass_Number], [Bank_Accounts_id], [Phone_Numbers_id]) " +
                        $"VALUES ('{newMan.Name}', '{newMan.Surname}', '{newMan.Patronymic}', {newMan.PassSeries}, {newMan.PassNumber}, {bankAccountsNewId}, {phoneNumbersNewId})";
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO [Bank_Accounts] " +
                        "([Bank_Account_id], [Accounts_Number], [Accounts_Type]) VALUES ";
                    for (int i = 0; i < newMan.Accounts.Count; i++)
                    {
                        command.CommandText +=
                            $"({bankAccountsNewId}, {newMan.Accounts[i].NumberAccount}, {(int)newMan.Accounts[i].TypeAccount})";

                        if (i != newMan.Accounts.Count - 1)
                        {
                            command.CommandText += ", ";
                        }
                    }
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO [Phone_Numbers] " +
                        "([Phone_Number_id], [Phone_Number]) VALUES ";
                    for (int i = 0; i < newMan.PhoneNumbers.Count; i++)
                    {
                        command.CommandText += $"({phoneNumbersNewId}, {newMan.PhoneNumbers[i]})";

                        if (i != newMan.PhoneNumbers.Count - 1)
                        {
                            command.CommandText += ", ";
                        }
                    }
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public async Task EditManAsync(Client? changedMan)
        {
            if (changedMan == null)
                return;

            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = $"UPDATE [Clients] SET " +
                        $"[Name] = '{changedMan.Name.Trim()}', " +
                        $"[Surname] = '{changedMan.Surname.Trim()}', " +
                        $"[Patronymic] = '{changedMan.Patronymic.Trim()}' " +
                        $"WHERE [Pass_Series] = {changedMan.PassSeries} AND [Pass_Number] = {changedMan.PassNumber}";
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = $"SELECT C.Bank_Accounts_id, C.Phone_Numbers_id FROM [Clients] AS C " +
                        $"WHERE C.Pass_Series = {changedMan.PassSeries} AND C.Pass_Number = {changedMan.PassNumber}";
                    (int phoneNumberId, int bankAccountId) = await GetOldId(command);
                    if (phoneNumberId == -1 || bankAccountId == -1)
                        return;

                    command.CommandText = $"DELETE B FROM [Bank_Accounts] AS B WHERE B.Bank_Account_id = {bankAccountId}";
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = $"DELETE P FROM [Phone_Numbers] AS P WHERE P.Phone_Number_id = {phoneNumberId}";
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO [Bank_Accounts] " +
                        "([Bank_Account_id], [Accounts_Number], [Accounts_Type]) VALUES ";
                    for (int i = 0; i < changedMan.Accounts.Count; i++)
                    {
                        command.CommandText +=
                            $"({bankAccountId}, {changedMan.Accounts[i].NumberAccount}, {(int)changedMan.Accounts[i].TypeAccount})";

                        if (i != changedMan.Accounts.Count - 1)
                        {
                            command.CommandText += ", ";
                        }
                    }
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO [Phone_Numbers] " +
                        "([Phone_Number_id], [Phone_Number]) VALUES ";
                    for (int i = 0; i < changedMan.PhoneNumbers.Count; i++)
                    {
                        command.CommandText += $"({phoneNumberId}, {changedMan.PhoneNumbers[i]})";

                        if (i != changedMan.PhoneNumbers.Count - 1)
                        {
                            command.CommandText += ", ";
                        }
                    }
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public async Task DeleteManAsync(Client? removedMan)
        {
            if (removedMan == null)
                return;

            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText = $"SELECT C.Bank_Accounts_id, C.Phone_Numbers_id FROM [Clients] AS C " +
                        $"WHERE C.Pass_Series = {removedMan.PassSeries} AND C.Pass_Number = {removedMan.PassNumber}";
                    (int phoneNumberId, int bankAccountId) = await GetOldId(command);
                    if (phoneNumberId == -1 || bankAccountId == -1)
                        return;

                    command.CommandText = $"DELETE B FROM [Bank_Accounts] AS B WHERE B.Bank_Account_id = {bankAccountId}";
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = $"DELETE P FROM [Phone_Numbers] AS P WHERE P.Phone_Number_id = {phoneNumberId}";
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = $"DELETE C FROM [Clients] AS C " +
                        $"WHERE C.Pass_Series = {removedMan.PassSeries} AND C.Pass_Number = {removedMan.PassNumber}";
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        private async Task<List<ClientEntity>> GetClientEntities(SqlCommand command)
        {
            var clientEntities = new List<ClientEntity>();

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    clientEntities.Add(new ClientEntity
                    {
                        Name = reader.GetString(0),
                        Surname = reader.GetString(1),
                        Patronymic = reader.GetString(2),
                        PassSeries = reader.GetInt32(3),
                        PassNumber = reader.GetInt32(4),
                        AccountId = reader.GetInt32(5),
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

        private async Task<List<BankAccountEntity>> GetBankAccountEntities(SqlCommand command)
        {
            var bankAccountEntities = new List<BankAccountEntity>();

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    bankAccountEntities.Add(new BankAccountEntity
                    {
                        Id = reader.GetInt32(0),
                        NumberAccount = reader.GetInt64(1),
                        TypeAccount = reader.GetInt32(2)
                    });
                }
            }
            return bankAccountEntities;
        }

        private List<Client> AssembleClient(
            ICollection<ClientEntity> clientEntities,
            ICollection<PhoneNumberEntity> phoneEntities,
            ICollection<BankAccountEntity> accountEntities)
        {
            var clients = new List<Client>();
            foreach (var clientEntity in clientEntities)
            {
                var client = new Client
                {
                    Name = clientEntity.Name,
                    Surname = clientEntity.Surname,
                    Patronymic = clientEntity.Patronymic,
                    PassSeries = clientEntity.PassSeries,
                    PassNumber = clientEntity.PassNumber,
                };

                var client_phones = phoneEntities
                    .Where(p => p.Id == clientEntity.PhoneNumbersId)
                    .ToArray();
                foreach (var phoneNumber in client_phones)
                {
                    client.PhoneNumbers.Add(phoneNumber.PhoneNumber.ToString()!);
                }

                var client_accounts = accountEntities
                    .Where(a => a.Id == clientEntity.AccountId)
                    .ToArray();
                foreach (var bankAccount in client_accounts)
                {
                    client.Accounts.Add(new BankAccount
                    {
                        NumberAccount = bankAccount.NumberAccount,
                        TypeAccount = (TA)bankAccount.TypeAccount
                    });
                }
                clients.Add(client);
            }
            return clients;
        }

        private async Task<(int phoneId, int accountId)> GetNewId(SqlCommand command)
        {
            command.CommandText = "SELECT P.Phone_Number_id, P.Phone_Number FROM [Phone_Numbers] AS P";
            var phoneNumberEntities = await GetPhoneNumberEntities(command);
            var allPhoneNumberId = new List<int>();
            foreach (var phoneNumberEntity in phoneNumberEntities)
            {
                allPhoneNumberId.Add(phoneNumberEntity.Id);
            }

            command.CommandText = "SELECT A.Bank_Account_id, A.Accounts_Number, A.Accounts_Type FROM [Bank_Accounts] AS A";
            var bankAccountsEntities = await GetBankAccountEntities(command);
            var allBankAccountId = new List<int>();
            foreach (var bankAccountsEntity in bankAccountsEntities)
            {
                allBankAccountId.Add(bankAccountsEntity.Id);
            }

            int freePhoneNumberId = -1, freeBankAccountId = -1;
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (!allPhoneNumberId.Contains(i) && freePhoneNumberId == -1)
                {
                    freePhoneNumberId = i;
                }
                if (!allBankAccountId.Contains(i) && freeBankAccountId == -1)
                {
                    freeBankAccountId = i;
                }
                if (freePhoneNumberId != -1 && freeBankAccountId != -1)
                {
                    break;
                }
            }

            if (freePhoneNumberId == -1 || freeBankAccountId == -1)
            {
                throw new ApplicationException("Закончились свободные id");
            }

            return (freePhoneNumberId, freeBankAccountId);
        }

        private async Task<(int phoneId, int accountId)> GetOldId(SqlCommand command)
        {
            int phoneNumberId = -1, bankAccountId = -1;
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    bankAccountId = reader.GetInt32(0);
                    phoneNumberId = reader.GetInt32(1);
                }
            }

            return (phoneNumberId, bankAccountId);
        }
    }
}

using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Bank_StashYourCrap.Bank.PeopleModels.Base;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using System.Threading.Tasks;
using System.Threading;

namespace Bank_StashYourCrap.Bank.DataContext
{
    internal class RepositoryPeopleData
    {
        private readonly string _pathDirectoryData;

        private readonly string _pathClientsFile;
        private readonly string _pathEmployeesFile;

        private string _pathGenericFile = default!;

        // Этот класс ничего не знает о логике приложения. Тут только доступ к данным: чтение, запись.
        public RepositoryPeopleData()
        {
            _pathDirectoryData = @"..\..\..\Bank\DataContext\Data";

            _pathClientsFile = "ClientsData.json";
            _pathEmployeesFile = "EmployeesData.json";
        }

        private void CheckingAndCreatingDirectories(string pathDirectory)
        {
            var isDirectoryExists = Directory.Exists(pathDirectory);
            if (!isDirectoryExists)
            {
                Directory.CreateDirectory(pathDirectory);
            }
        }

        private void CheckingAndCreatingFile(string pathFile)
        {
            var isFileExists = File.Exists(pathFile);
            if (!isFileExists)
            {
                File.Create(pathFile);
            }
        }

        private void DefineFileForWritingAndReading<TMan>() where TMan : Human
        {
            if (typeof(TMan) == typeof(Client))
            {
                _pathGenericFile = _pathClientsFile;
            }
            else if (typeof(TMan) == typeof(Employee))
            {
                _pathGenericFile = _pathEmployeesFile;
            }
            else
            {
                throw new Exception($"Неизвестный тип данных. Репозиторий не знает такой тип данных: {typeof(TMan).Name}");
            }
        }

        internal async Task<List<TMan>?> GetCollectionPeopleAsync<TMan>() where TMan : Human
        {
            DefineFileForWritingAndReading<TMan>();

            // _pathGenericFile меняется в зависимости от того, когой тип будет использоваться в TMan.
            // _pathDirectoryData не меняется, устанавливается в конструкторе
            var fileFullPath = Path.Combine(_pathDirectoryData, _pathGenericFile);

            CheckingAndCreatingDirectories(_pathDirectoryData);
            CheckingAndCreatingFile(fileFullPath);

            string AllLine;
            using (StreamReader sr = new StreamReader(fileFullPath, Encoding.UTF8))
            {
                AllLine = await sr.ReadToEndAsync();
            }

            var people = JsonConvert.DeserializeObject<List<TMan>>(AllLine);
            return people;
        }

        // Чтобы найти человека, нужно знать его серию и номер паспорта.
        internal async Task<TMan?> GetOneManAsync<TMan>(int passSeries, int passNumber) where TMan : Human
        {
             var people = await GetCollectionPeopleAsync<TMan>();

            var oneMan = people?
                .FirstOrDefault(m => m.PassSeries == passSeries && m.PassNumber == passNumber);

            return oneMan;
        }

        internal async Task AddManAsync<TMan>(TMan? newMan) where TMan : Human
        {
            var people = await GetCollectionPeopleAsync<TMan>();
            if (newMan == null || people == null)
            {
                return;
            }

            people.Add(newMan);
            await SaveDataAsync(people);
        }

        internal async Task EditManAsync<TMan>(TMan? changedMan) where TMan : Human
        {
            var people = await GetCollectionPeopleAsync<TMan>();
            if (changedMan == null || people == null)
            {
                return;
            }

            var personYouAreLookingFor = people
                .FirstOrDefault(m => m.PassSeries == changedMan.PassSeries && m.PassNumber == changedMan.PassNumber);
            if (personYouAreLookingFor == null)
            {
                return;
            }

            var indexPerson = people.IndexOf(personYouAreLookingFor);
            if (indexPerson == -1)
            {
                return;
            }
            people[indexPerson] = changedMan;
            await SaveDataAsync(people);
        }

        internal async Task DeleteManAsync<TMan>(TMan? removedMan) where TMan : Human
        {
            var people = await GetCollectionPeopleAsync<TMan>();
            if (removedMan == null || people == null)
            {
                return;
            }

            var personYouWantToRemove = people
                .FirstOrDefault(m => m.PassSeries == removedMan.PassSeries && m.PassNumber == removedMan.PassNumber);
            if (personYouWantToRemove == null)
            {
                return;
            }

            var isRemove = people.Remove(personYouWantToRemove);
            if (isRemove == true)
            {
                await SaveDataAsync(people);
            }
        }

        private async Task SaveDataAsync<TMan>(List<TMan> people) where TMan : Human
        {
            // _pathGenericFile меняется в зависимости от того, когой тип будет использоваться в TMan.
            // _pathDirectoryData не меняется, устанавливается в конструкторе
            var fileFullPath = Path.Combine(_pathDirectoryData, _pathGenericFile);
            var json = JsonConvert.SerializeObject(people, Formatting.Indented);

            await using (StreamWriter sw = new StreamWriter(fileFullPath, false))
            {
                await sw.WriteLineAsync(json);
            }
        }
    }
}

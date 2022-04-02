using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Bank_StashYourCrap.Bank.PeopleModels.Base;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;

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

        internal List<TMan>? GetCollectionPeople<TMan>() where TMan : Human
        {
            DefineFileForWritingAndReading<TMan>();

            // _pathGenericFile меняется в зависимости от того, когой тип будет использоваться в TMan.
            // _pathDirectoryData не меняется, устанавливается в конструкторе
            var fileFullPath = Path.Combine(_pathDirectoryData, _pathGenericFile);

            CheckingAndCreatingDirectories(_pathDirectoryData);
            CheckingAndCreatingFile(fileFullPath);

            List<TMan>? people = null;
            using (StreamReader sr = new StreamReader(fileFullPath, Encoding.UTF8))
            {
                var AllLine = sr.ReadToEnd();

                people = JsonConvert.DeserializeObject<List<TMan>>(AllLine);
            }

            return people;
        }

        // Чтобы найти человека, нужно знать его серию и номер паспорта.
        internal TMan? GetOneMan<TMan>(int passSeries, int passNumber) where TMan : Human
        {
            var people = GetCollectionPeople<TMan>();

            var oneMan = people?
                .FirstOrDefault(m => m.PassSeries == passSeries && m.PassNumber == passNumber);

            return oneMan;
        }

        internal void AddMan<TMan>(TMan? newMan) where TMan : Human
        {
            var people = GetCollectionPeople<TMan>();
            if (newMan == null || people == null)
            {
                return;
            }

            people.Add(newMan);
            SaveData(people);
        }

        internal void EditMan<TMan>(TMan? changedMan) where TMan : Human
        {
            var people = GetCollectionPeople<TMan>();
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
            SaveData(people);
        }

        internal void DeleteMan<TMan>(TMan? removedMan) where TMan : Human
        {
            var people = GetCollectionPeople<TMan>();
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
                SaveData(people);
            }
        }

        private void SaveData<TMan>(List<TMan> people) where TMan : Human
        {
            // _pathGenericFile меняется в зависимости от того, когой тип будет использоваться в TMan.
            // _pathDirectoryData не меняется, устанавливается в конструкторе
            var fileFullPath = Path.Combine(_pathDirectoryData, _pathGenericFile);

            using (StreamWriter sw = new StreamWriter(fileFullPath, false))
            {
                var json = JsonConvert.SerializeObject(people, Formatting.Indented);
                sw.WriteLine(json);
            }
        }
    }
}

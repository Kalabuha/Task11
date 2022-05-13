using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_StashYourCrap.Bank.DataContext.Interfaces;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;

namespace Bank_StashYourCrap.Bank.DataContext.RepositoriesDataFiles
{
    internal class RepositoryEmployeesDataFile : IRepositoryEmployees
    {
        private readonly string _DirectoryPath;
        private readonly string _FileName;

        private readonly string _FullPathEmployeeFile;

        public RepositoryEmployeesDataFile()
        {
            _DirectoryPath = @"..\..\..\Bank\DataContext\RepositoriesDataFiles\Data";
            _FileName = @"EmployeesData.json";

            _FullPathEmployeeFile = Path.Combine(_DirectoryPath, _FileName);
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

        public async Task<List<Employee>?> GetCollectionPeopleAsync()
        {
            CheckingAndCreatingDirectories(_DirectoryPath);
            CheckingAndCreatingFile(_FullPathEmployeeFile);

            string AllLine;
            using (StreamReader sr = new StreamReader(_FullPathEmployeeFile, Encoding.UTF8))
            {
                AllLine = await sr.ReadToEndAsync();
            }

            var people = JsonConvert.DeserializeObject<List<Employee>>(AllLine);
            return people;
        }

        // Чтобы найти человека, нужно знать его серию и номер паспорта.
        public async Task<Employee?> GetOneManAsync(int passSeries, int passNumber)
        {
            var people = await GetCollectionPeopleAsync();

            var oneMan = people?
                .FirstOrDefault(m => m.PassSeries == passSeries && m.PassNumber == passNumber);

            return oneMan;
        }

        public async Task AddManAsync(Employee? newMan)
        {
            var people = await GetCollectionPeopleAsync();
            if (newMan == null || people == null)
            {
                return;
            }

            people.Add(newMan);
            await SaveDataAsync(people);
        }

        public async Task EditManAsync(Employee? changedMan)
        {
            var people = await GetCollectionPeopleAsync();
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

        public async Task DeleteManAsync(Employee? removedMan)
        {
            var people = await GetCollectionPeopleAsync();
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

        private async Task SaveDataAsync(List<Employee> people)
        {
            var json = JsonConvert.SerializeObject(people, Formatting.Indented);

            await using (StreamWriter sw = new StreamWriter(_FullPathEmployeeFile, false))
            {
                await sw.WriteLineAsync(json);
            }
        }
    }
}
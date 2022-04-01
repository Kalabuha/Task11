using Bank_StashYourCrap.Bank.PeopleModels.Base;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Bank_StashYourCrap.Bank.Data
{
    internal class RepositoryPeopleData
    {
        private readonly string _pathDirectoryData;

        public RepositoryPeopleData(string pathDirectoryData)
        {
            _pathDirectoryData = pathDirectoryData;
        }

        internal ObservableCollection<TMan>? GetCollectionPeople<TMan>(string fileName) where TMan : Human
        {
            var fileFullPath = Path.Combine(_pathDirectoryData, fileName);

            ObservableCollection<TMan>? people;

            using (StreamReader sr = new StreamReader(fileFullPath, Encoding.UTF8))
            {
                var AllLine = sr.ReadToEnd();

                people = JsonConvert.DeserializeObject<ObservableCollection<TMan>>(AllLine);
            }

            return people;
        }

        // Если о человеке неизвестно ничего, серии и номера паспорта будет достаточно, чтобы найти его.
        internal TMan? GetOneMan<TMan>(string fileName, int passSeries, int passNumber) where TMan : Human
        {
            var people = GetCollectionPeople<TMan>(fileName);

            var oneMan = people?
                .FirstOrDefault(m => m.PassSeries == passSeries && m.PassNumber == passNumber);

            return oneMan;
        }

        internal void AddOneMan<TMan>(string fileName, TMan newMan) where TMan : Human
        {
            var people = GetCollectionPeople<TMan>(fileName);
            if (newMan == null || people == null)
            {
                return;
            }
            people.Add(newMan);
            SaveData(fileName, people);
        }

        internal void EditMan<TMan>(string fileName, TMan changedMan) where TMan : Human
        {
            var people = GetCollectionPeople<TMan>(fileName);
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
            SaveData(fileName, people);
        }

        internal void DeleteMan<TMan>(string fileName, TMan removedMan) where TMan : Human
        {
            var people = GetCollectionPeople<TMan>(fileName);
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
                SaveData(fileName, people);
            }
        }

        private void SaveData<TMan>(string fileName, ObservableCollection<TMan> people) where TMan : Human
        {
            var fileFullPath = Path.Combine(_pathDirectoryData, fileName);

            using (StreamWriter sw = new StreamWriter(fileFullPath, false))
            {
                var json = JsonConvert.SerializeObject(people, Formatting.Indented);
                sw.WriteLine(json);
            }
        }
    }
}

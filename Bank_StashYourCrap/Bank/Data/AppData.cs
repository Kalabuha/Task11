using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Bank_StashYourCrap.Bank.PeopleModels.Employees.Base;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;
using Bank_StashYourCrap.Bank.BankModels;


namespace Bank_StashYourCrap.Bank.Data
{
    internal class AppData
    {
        private readonly string _pathDirectoryData;
        private readonly string _pahtClientsData;
        private readonly string _pahtEmployeesData;

        public AppData()
        {
            var fileClients = "ClientsData.json";
            var fileEmployees = "EmployeesData.json";

            _pathDirectoryData = @"..\..\..\Bank\Data";

            _pahtClientsData = Path.Combine(_pathDirectoryData, fileClients);
            _pahtEmployeesData = Path.Combine(_pathDirectoryData, fileEmployees);
        }


        #region Работники
        internal ObservableCollection<Employee> GetAllEmployees()
        {
            return new ObservableCollection<Employee>();
        }
        #endregion

        #region Клиенты
        internal ObservableCollection<Client>? GetAllClients()
        {
            ObservableCollection<Client>? clients;

            using (StreamReader sr = new StreamReader(_pahtClientsData, Encoding.UTF8))
            {
                var AllLine = sr.ReadToEnd();

                clients = JsonConvert.DeserializeObject<ObservableCollection<Client>>(AllLine);
            }
            
            return clients;
        }

        internal ObservableCollection<Client> GetOneClient(int passport)
        {
            return new ObservableCollection<Client>();
        }

        internal ObservableCollection<Client> AddClient()
        {
            return new ObservableCollection<Client>();
        }

        internal ObservableCollection<Client> EditClient()
        {
            return new ObservableCollection<Client>();
        }

        internal ObservableCollection<Client> DeleteClient()
        {
            return new ObservableCollection<Client>();
        }

        public void SaveData(ObservableCollection<Client> list)
        {
            //var json = JsonSerializer.Serialize(list, new JsonSerializerOptions() { WriteIndented = true });

            //using (StreamWriter sw = new StreamWriter(_pahtEmployeesData, false))
            //{
            //    sw.WriteLine(json);
            //}
        }
        #endregion

    }
}

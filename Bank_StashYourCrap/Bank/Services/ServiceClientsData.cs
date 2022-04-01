using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_StashYourCrap.Bank.DataContext;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;

namespace Bank_StashYourCrap.Bank.Services
{
    internal class ServiceClientsData
    {
        private readonly RepositoryPeopleData _repository;

        public ServiceClientsData(RepositoryPeopleData repository)
        {
            _repository = repository;
        }

        // Проверка коллекции данных на совподения
        public ObservableCollection<Client> FindMatchingPassportsInDataCollection()
        {
            var allClients = _repository.GetCollectionPeople<Client>() ?? new ObservableCollection<Client>();

            var uniqueClients = new HashSet<(int series, int number)>();
            var duplicateClients = new ObservableCollection<Client>();

            foreach (var client in allClients)
            {
                var isAdded = uniqueClients.Add((client.PassSeries, client.PassNumber));
                if (!isAdded)
                {
                    duplicateClients.Add(client);
                }
            }
            return duplicateClients;
        }

        public ObservableCollection<Client> GetAllClients()
        {
            var allClients = _repository.GetCollectionPeople<Client>();

            return allClients ?? new ObservableCollection<Client>();
        }

        public bool AddClient(Client newClient)
        {
            var client = _repository.GetOneMan<Client>(newClient.PassSeries, newClient.PassNumber);

            // Если будет получен результат отличный от null, добовлять нельзя.
            if (client != null)
            {
                return false;
            }

            _repository.AddOneMan(newClient);
            return true;
        }

        public void EditClient(Client newClient)
        {
            // Если разрешить изменение паспорта, то здесь необходимо сделать проверку на совподения.
            _repository.EditMan(newClient);
        }

        public void DeleteClient(Client newClient)
        {
            // Это же банковское приложение. Тут можно сделать запрет удаления должников банка :)
            _repository.DeleteMan(newClient);
        }

    }
}

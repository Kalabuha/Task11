using Bank_StashYourCrap.Bank.DataContext;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;
using Bank_StashYourCrap.Mappers;
using Bank_StashYourCrap.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
        public ObservableCollection<ClientModel> FindMatchingPassportsInDataCollection()
        {
            var allClientsEntities = _repository.GetCollectionPeople<Client>() ?? new List<Client>();

            // HashSet нужен только для проверки совподений паспортов
            var uniqueClients = new HashSet<(int series, int number)>();

            var duplicateClientsModels = new ObservableCollection<ClientModel>();
            foreach (var clientEntity in allClientsEntities)
            {
                var isAdded = uniqueClients.Add((clientEntity.PassSeries, clientEntity.PassNumber));
                if (!isAdded)
                {
                    duplicateClientsModels.Add(clientEntity.ConvertEntityToModel());
                }
            }
            return duplicateClientsModels;
        }


        public ObservableCollection<ClientModel> GetAllClients()
        {
            var allClientsEntities = _repository.GetCollectionPeople<Client>() ?? new List<Client>();

            var allClientsModels = allClientsEntities.Select(c => c.ConvertEntityToModel());

            return new ObservableCollection<ClientModel>(allClientsModels);
        }

        public bool AddClient(ClientModel newClientModel)
        {
            var clientEntity = _repository.GetOneMan<Client>(newClientModel.PassSeries, newClientModel.PassNumber);

            // Если будет получен результат отличный от null, добовлять нельзя.
            if (clientEntity != null)
            {
                return false;
            }

            _repository.AddMan(newClientModel.ConvertModelToEntity());
            return true;
        }

        public void EditClient(ClientModel editClientModel)
        {
            // Если разрешить изменение паспорта, то здесь необходимо сделать проверку на совподения.
            _repository.EditMan(editClientModel.ConvertModelToEntity());
        }

        public void DeleteClient(ClientModel deleteClientModel)
        {
            // Это же банковское приложение. Тут можно сделать запрет удаления должников банка :)
            _repository.DeleteMan(deleteClientModel.ConvertModelToEntity());
        }

    }
}

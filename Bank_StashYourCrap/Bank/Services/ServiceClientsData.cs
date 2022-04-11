using Bank_StashYourCrap.Bank.BankModels;
using Bank_StashYourCrap.Bank.DataContext;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Bank_StashYourCrap.Mappers;
using Bank_StashYourCrap.Models;
using System;
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

        public ObservableCollection<string> GetAllTypesAccount()
        {
            var allTypesAccountAsString = new ObservableCollection<string>();

            var allTypesAccountAsEnum = Enum.GetValues(typeof(TA));

            foreach (var oneType in allTypesAccountAsEnum)
            {
                if (oneType != null)
                {
                    allTypesAccountAsString.Add(ClientEntityModelConverter.ConvertTypeAccountToString((TA)oneType));
                }
            }

            return allTypesAccountAsString;
        }

        public ObservableCollection<ClientModel> GetAllClients()
        {
            var allClientsEntities = _repository.GetCollectionPeople<Client>() ?? new List<Client>();

            var allClientsModels = allClientsEntities.Select(c => c.ConvertEntityToModel());

            return new ObservableCollection<ClientModel>(allClientsModels);
        }

        public bool AddClient(ClientModel newClientModel)
        {
            var newClientEntity = newClientModel.ConvertModelToEntity();

            var clientEntity = _repository.GetOneMan<Client>(newClientEntity.PassSeries, newClientEntity.PassNumber);

            // Если будет получен результат отличный от null, добавлять нельзя.
            if (clientEntity != null)
            {
                return false;
            }

            _repository.AddMan(newClientEntity);
            return true;
        }

        public void EditClient(ClientModel editClientModel)
        {
            _repository.EditMan(editClientModel.ConvertModelToEntity());
        }

        public void DeleteClient(ClientModel deleteClientModel)
        {
            // Это же банковское приложение. Тут нужно сделать запрет удаления должников банка :)
            _repository.DeleteMan(deleteClientModel.ConvertModelToEntity());
        }

    }
}

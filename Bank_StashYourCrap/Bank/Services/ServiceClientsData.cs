using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bank_StashYourCrap.Bank.DataContext;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Bank_StashYourCrap.Bank.PeopleModels.Employees.Base;
using Bank_StashYourCrap.Mappers;
using Bank_StashYourCrap.Models;
using Bank_StashYourCrap.Bank.Services.Modifiers;
using Bank_StashYourCrap.Bank.BankModels;

namespace Bank_StashYourCrap.Bank.Services
{
    internal class ServiceClientsData
    {
        private readonly RepositoryPeopleData _repository;
        private readonly ConfidentialDataHider _dataHider;

        public ServiceClientsData(RepositoryPeopleData repository)
        {
            _repository = repository;
            _dataHider = new ConfidentialDataHider();
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

        public ObservableCollection<ClientModel> GetAllClients(Employee userSystem)
        {
            if (userSystem.AccessLevel == EmployeeAccessLevel.Consultant)
            {
                return GetAllClientsHiddenData();
            }
            else if (userSystem.AccessLevel == EmployeeAccessLevel.Manager)
            {
                return GetAllClientsOpenData();
            }
            else
            {
                throw new Exception("Не известный уровень доступа");
            }
        }

        private ObservableCollection<ClientModel> GetAllClientsOpenData()
        {
            var allClientsEntities = _repository.GetCollectionPeople<Client>() ?? new List<Client>();

            var allClientsModels = allClientsEntities.Select(c => c.ConvertEntityToModel());

            return new ObservableCollection<ClientModel>(allClientsModels);
        }

        private ObservableCollection<ClientModel> GetAllClientsHiddenData()
        {
            var allClientsEntities = _repository.GetCollectionPeople<Client>() ?? new List<Client>();

            var allClientsModels = allClientsEntities.Select(c => _dataHider.HideData(c.ConvertEntityToModel()));

            return new ObservableCollection<ClientModel>(allClientsModels);
        }

        public bool AddClient(ClientModel newClientModel)
        {
            var passSeries = int.Parse(newClientModel.PassSeries);
            var passNumber = int.Parse(newClientModel.PassNumber);
            var clientEntity = _repository.GetOneMan<Client>(passSeries, passNumber);

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

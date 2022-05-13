using Bank_StashYourCrap.Bank.BankModels;
using Bank_StashYourCrap.Bank.DataContext.Interfaces;
using Bank_StashYourCrap.Mappers;
using Bank_StashYourCrap.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Bank_StashYourCrap.Bank.Services
{
    internal class ServiceClientsData
    {
        private readonly IRepositoryClients _repository;

        public ServiceClientsData(IRepositoryClients repository)
        {
            _repository = repository;
        }

        public ObservableCollection<string> GetAllTypesAccount()
        {
            var allTypesAccountAsEnum = Enum.GetValues(typeof(TA));

            var allTypesAccountAsString = new ObservableCollection<string>();
            foreach (var oneTypeAsString in allTypesAccountAsEnum)
            {
                allTypesAccountAsString.Add(ClientEntityModelConverter.ConvertTypeAccountToString((TA)oneTypeAsString));
            }

            return allTypesAccountAsString;
        }

        public async Task<ObservableCollection<ClientModel>> GetAllClientsAsync()
        {
            var allClientsEntities = await _repository.GetCollectionPeopleAsync();
            if (allClientsEntities == null)
            {
                return new ObservableCollection<ClientModel>();
            }

            var allClientsModels = allClientsEntities.Select(c => c.ConvertEntityToModel());
            return new ObservableCollection<ClientModel>(allClientsModels);
        }

        public async Task<bool> AddClientAsync(ClientModel newClientModel)
        {
            var newClientEntity = newClientModel.ConvertModelToEntity();
            var clientEntityisAlreadyInDB =
                await _repository.GetOneManAsync(newClientEntity.PassSeries, newClientEntity.PassNumber);
            // Если будет получен результат отличный от null, добавлять нельзя.
            if (clientEntityisAlreadyInDB != null)
            {
                return false;
            }

            await _repository.AddManAsync(newClientEntity);
            return true;
        }

        public async Task EditClientAsync(ClientModel editClientModel)
        {
            await _repository.EditManAsync(editClientModel.ConvertModelToEntity());
        }

        public async Task DeleteClientAsync(ClientModel deleteClientModel)
        {
            // Это же банковское приложение. Тут нужно сделать запрет удаления должников банка :)
            await _repository.DeleteManAsync(deleteClientModel.ConvertModelToEntity());
        }

    }
}

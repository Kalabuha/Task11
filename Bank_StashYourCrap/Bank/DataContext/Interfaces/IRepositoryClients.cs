using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;

namespace Bank_StashYourCrap.Bank.DataContext.Interfaces
{
    internal interface IRepositoryClients
    {
        Task<List<Client>?> GetCollectionPeopleAsync();
        Task<Client?> GetOneManAsync(int passSeries, int passNumber);
        Task AddManAsync(Client? newMan);
        Task EditManAsync(Client? changedMan);
        Task DeleteManAsync(Client? removedMan);

    }
}

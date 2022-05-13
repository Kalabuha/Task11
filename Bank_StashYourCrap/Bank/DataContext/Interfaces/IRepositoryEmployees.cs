using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;

namespace Bank_StashYourCrap.Bank.DataContext.Interfaces
{
    internal interface IRepositoryEmployees
    {
        Task<List<Employee>?> GetCollectionPeopleAsync();
        Task<Employee?> GetOneManAsync(int passSeries, int passNumber);
        Task AddManAsync(Employee? newMan);
        Task EditManAsync(Employee? changedMan);
        Task DeleteManAsync(Employee? removedMan);
    }
}

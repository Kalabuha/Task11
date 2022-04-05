using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_StashYourCrap.Bank.DataContext;
using Bank_StashYourCrap.Models;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Bank_StashYourCrap.Mappers;

namespace Bank_StashYourCrap.Bank.Services
{
    internal class ServiceEmployeesData
    {
        private readonly RepositoryPeopleData _repository;

        public ServiceEmployeesData(RepositoryPeopleData repository)
        {
            _repository = repository;
        }

        public ObservableCollection<EmployeeModel> GetAllEmployees()
        {
            var allClientsEntities = _repository.GetCollectionPeople<Employee>() ?? new List<Employee>();

            var allClientsModels = allClientsEntities.Select(c => c.ConvertEntityToModel());

            return new ObservableCollection<EmployeeModel>(allClientsModels);
        }
    }
}

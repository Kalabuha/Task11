using Bank_StashYourCrap.Bank.DataContext.Interfaces;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Bank_StashYourCrap.Mappers;
using Bank_StashYourCrap.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Bank_StashYourCrap.Bank.Services
{
    internal class ServiceEmployeesData
    {
        private readonly IRepositoryEmployees _repository;

        public ServiceEmployeesData(IRepositoryEmployees repository)
        {
            _repository = repository;
        }

        public async Task<ObservableCollection<EmployeeModel>> GetAllEmployeesAsync()
        {
            var allClientsEntities = await _repository.GetCollectionPeopleAsync();
            if (allClientsEntities == null)
            {
                return new ObservableCollection<EmployeeModel>();
            }

            var allClientsModels = allClientsEntities.Select(c => c.ConvertEntityToModel());
            return new ObservableCollection<EmployeeModel>(allClientsModels);
        }

        public Task<Employee?> GetEmployee(EmployeeModel employeeModel)
        {
            var passSeries = int.Parse(employeeModel.PassSeries);
            var passNumber = int.Parse(employeeModel.PassNumber);
            return _repository.GetOneManAsync(passSeries, passNumber);
        }
    }
}

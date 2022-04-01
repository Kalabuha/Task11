using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_StashYourCrap.Bank.Data;

namespace Bank_StashYourCrap.Bank.Services
{
    internal class ServiceEmployeesData
    {
        private readonly string _fileNameEmployeesData;
        private readonly RepositoryPeopleData _repository;

        public ServiceEmployeesData(RepositoryPeopleData repository)
        {
            _fileNameEmployeesData = "EmployeesData.json";
            _repository = repository;
        }
    }
}

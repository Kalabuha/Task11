using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_StashYourCrap.Bank.DataContext;

namespace Bank_StashYourCrap.Bank.Services
{
    internal class ServiceEmployeesData
    {
        private readonly RepositoryPeopleData _repository;

        public ServiceEmployeesData(RepositoryPeopleData repository)
        {
            _repository = repository;
        }
    }
}

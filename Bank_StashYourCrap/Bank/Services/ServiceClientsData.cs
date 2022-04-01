using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_StashYourCrap.Bank.Data;

namespace Bank_StashYourCrap.Bank.Services
{
    internal class ServiceClientsData
    {
        private readonly string _fileNameClientsData;
        private readonly RepositoryPeopleData _repository;

        public ServiceClientsData(RepositoryPeopleData repository)
        {
            _fileNameClientsData = "ClientsData.json";
            _repository = repository;
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_StashYourCrap.Bank.DataContext.RepositoriesDataBase.Entities.Base;

namespace Bank_StashYourCrap.Bank.DataContext.RepositoriesDataBase.Entities
{
    internal class EmployeeEntity : HumanEntity
    {
        public int AccessLevel { get; set; }
    }
}

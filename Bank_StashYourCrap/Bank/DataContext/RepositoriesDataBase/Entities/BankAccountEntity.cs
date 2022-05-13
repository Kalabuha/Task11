using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_StashYourCrap.Bank.DataContext.RepositoriesDataBase.Entities
{
    internal class BankAccountEntity
    {
        public int Id { get; set; }
        public int TypeAccount { get; set; }
        public long NumberAccount { get; set; }
    }
}

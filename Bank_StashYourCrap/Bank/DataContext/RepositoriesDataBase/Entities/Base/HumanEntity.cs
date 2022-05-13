using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_StashYourCrap.Bank.DataContext.RepositoriesDataBase.Entities.Base
{
    internal abstract class HumanEntity
    {
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string Patronymic { get; set; } = default!;
        public int PhoneNumbersId { get; set; } = new();
        public int PassSeries { get; set; }
        public int PassNumber { get; set; }
    }
}

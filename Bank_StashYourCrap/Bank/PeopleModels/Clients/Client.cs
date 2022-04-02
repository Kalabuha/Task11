using System.Collections.Generic;
using Bank_StashYourCrap.Bank.PeopleModels.Base;
using Bank_StashYourCrap.Bank.BankModels;

namespace Bank_StashYourCrap.Bank.PeopleModels.Clients
{
    internal class Client : Human
    {
        public List<BankAccount> Accounts { get; set; } = new();
    }
}

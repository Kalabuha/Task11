using Bank_StashYourCrap.Bank.PeopleModels.Employees.Base;
using Bank_StashYourCrap.Bank.PeopleModels.Base;

namespace Bank_StashYourCrap.Bank.PeopleModels.Employees
{
    internal class Employee : Human
    {
        public EmployeeAccessLevel AccessLevel { get; set; }
    }
}

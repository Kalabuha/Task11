using Bank_StashYourCrap.Models.Base;

namespace Bank_StashYourCrap.Models
{
    internal class EmployeeModel : HumanModel
    {
        public string AccessLevel { get; set; } = default!;
    }
}

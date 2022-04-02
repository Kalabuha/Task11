using Bank_StashYourCrap.Models.Base;
using System.Collections.ObjectModel;

namespace Bank_StashYourCrap.Models
{
    internal class ClientModel : HumanModel
    {
        public ObservableCollection<BankAccountModel> Accounts { get; set; } = new();
    }
}

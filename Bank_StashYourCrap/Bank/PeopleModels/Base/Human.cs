using System.Collections.ObjectModel;

namespace Bank_StashYourCrap.Bank.PeopleModels.Base
{
    internal abstract class Human
    {
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string Patronymic { get; set; } = default!;
        public ObservableCollection<string> PhoneNumbers { get; set; } = new();
        public int PassSeries { get; set; }
        public int PassNumber { get; set; }
    }
}

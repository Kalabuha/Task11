﻿using System.Collections.ObjectModel;

namespace Bank_StashYourCrap.Models.Base
{
    internal class HumanModel
    {
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string Patronymic { get; set; } = default!;
        public ObservableCollection<string> PhoneNumbers { get; set; } = new();
        public int PassSeries { get; set; }
        public int PassNumber { get; set; }
    }
}

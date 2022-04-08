namespace Bank_StashYourCrap.Bank.Services
{
    internal class ServiceDataVerification
    {
        public bool IsValidName(string name)
        {
            if (IsNullOrWhiteSpaceOrEmpty(name))
            {
                return false;
            }

            name = name.Trim();
            if (name.Length < 3)
            {
                return false;
            }

            for (int i = 0; i < name.Length; i++)
            {
                if (!char.IsLetter(name[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsValidPassSeries(string passSeries)
        {
            if (IsNullOrWhiteSpaceOrEmpty(passSeries))
            {
                return false;
            }

            passSeries = passSeries.Trim();
            if (passSeries.Length != 4)
            {
                return false;
            }

            return IsValidNumber(passSeries);
        }

        public bool IsValidPassNumber(string passNumber)
        {
            if (IsNullOrWhiteSpaceOrEmpty(passNumber))
            {
                return false;
            }

            passNumber = passNumber.Trim();
            if (passNumber.Length != 6)
            {
                return false;
            }

            return IsValidNumber(passNumber);
        }

        public bool IsValidPhoneNumber(string phoneNumber)
        {
            if (IsNullOrWhiteSpaceOrEmpty(phoneNumber))
            {
                return false;
            }

            phoneNumber = phoneNumber.Trim();
            if (phoneNumber.Length < 6 || phoneNumber.Length > 12)
            {
                return false;
            }

            return IsValidNumber(phoneNumber);
        }

        public bool IsValidNumberAccount(string numberAccount)
        {
            if (IsNullOrWhiteSpaceOrEmpty(numberAccount))
            {
                return false;
            }

            numberAccount = numberAccount.Trim();
            if (numberAccount.Length != 14)
            {
                return false;
            }

            return true;
        }

        private bool IsValidNumber(string number)
        {
            for (int i = 0; i < number.Length; i++)
            {
                if (!char.IsDigit(number[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsNullOrWhiteSpaceOrEmpty(string word)
        {
            if (string.IsNullOrEmpty(word) || string.IsNullOrWhiteSpace(word))
            {
                return true;
            }

            return false;
        }
    }
}

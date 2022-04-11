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

        public bool IsValidNumber(string number, byte numberOfDigits)
        {
            if (IsNullOrWhiteSpaceOrEmpty(number))
            {
                return false;
            }

            number = number.Trim();
            if (number.Length != numberOfDigits)
            {
                return false;
            }

            return IsValidNumber(number);
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

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

        public bool IsValidNumber(string number, byte numberOfDigitsMin, byte numberOfDigitsMax)
        {
            if (IsNullOrWhiteSpaceOrEmpty(number))
            {
                return false;
            }

            number = number.Trim();
            if (number.Length <= numberOfDigitsMin)
            {
                return false;
            }
            if (number.Length >= numberOfDigitsMax)
            {
                return false;
            }

            return IsIntegerNumber(number);
        }

        private bool IsIntegerNumber(string number)
        {
            return long.TryParse(number, out long value);
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

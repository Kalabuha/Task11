using Bank_StashYourCrap.Localizations.Base;
using System.Collections.Generic;

namespace Bank_StashYourCrap.Localizations
{
    internal class EngLang : AppLocalization
    {
        public override Dictionary<int, string> StringLibrary { get; }

        public EngLang()
        {
            StringLibrary = new Dictionary<int, string>()
            {
                { 0, "Client program"},
                { 1, "Menu" },
                { 2, "About the program" },
                { 3, "Change the language" },
                { 4, "System" },
                { 5, "Login" },
                { 6, "Log out" },
                { 7, "Seria" },
                { 8, "Number" },
                { 9, "Search" },
                { 10, "Add" },
                { 11, "Edit" },
                { 12, "Delete" },
                { 13, "Log in to continue working. To do this, select System from the menu." },
                { 14, "Login completed. User:" },
                { 15, "List of clients" },
                { 16, "Selected client: " },
                { 17, "Name" },
                { 18, "Surname" },
                { 19, "Patronymic" },
                { 20, "Passport series" },
                { 21, "Passport number" },
                { 22, "Phone numbers" },
                { 23, "Client Accounts" },
                { 24, "Consultant" },
                { 25, "Manager" },
                { 26, "User registration window" },
                { 27, "" },
                { 28, "Русский язык" },
                { 29, "English language" },
                { 30, "Add new client" },
                { 31, "Change customer information" },
                { 32, "Delete selected client" },
                { 33, "Budget account" },
                { 34, "Foreign currency account" },
                { 35, "Account frozen" },
                { 36, "Savings account" },
                { 37, "Correspondent account" },
                { 38, "Insured account" },
                { 39, "If you can't take all your crap with you, you know where to go..." },
                { 40, "Client manager window" },
                { 41, "Confirm" },
                { 42, "Reset" },
                { 43, "Number of digits from 6 to 12. Must be only digits" },
                { 44, "The entered phone number is already in the list" },
                { 45, "Invoice night must contain 14 digits" },
                { 46, "The entered account number is already in the list" },
                { 47, "Name entered incorrectly. Only letters are allowed, at least 3" },
                { 48, "The last name was entered incorrectly. Only letters are allowed, at least 3" },
                { 49, "Middle name entered incorrectly. Only letters are allowed, at least 3" },
                { 50, "Passport series entered incorrectly. Must be 4 digits" },
                { 51, "Passport number entered incorrectly. Must be 6 digits" },
                { 52, "The list must contain at least one phone number" },
                { 53, "The list must contain at least one open bank account" },
                { 54, "Failed to add new client. There is already a client with such a passport" },
                { 55, "Edit selected client" },
                { 56, "Delete selected client" },
            };
        }
    }
}

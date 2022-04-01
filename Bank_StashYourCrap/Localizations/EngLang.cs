using Bank_StashYourCrap.Localizations.Base;
using System.Collections.Generic;

namespace Bank_StashYourCrap.Localizations
{
    internal class EngLang : Localization
    {
        public override Dictionary<int, string> StringLibrary { get; }

        public EngLang()
        {
            StringLibrary = new Dictionary<int, string>()
            {
                { 0, "Программа для работы с клиентами"},
                { 1, "Меню" },
                { 2, "О программе" },
                { 3, "Закрыть программу" },
                { 4, "Система" },
                { 5, "Вход в систему" },
                { 6, "Выйти из системы" },
                { 7, "Серия" },
                { 8, "Номер" },
                { 9, "Найти" },
                { 10, "Добавить" },
                { 11, "Изменить" },
                { 12, "Удалить" },
                { 13, "Для продолжнеия работы войдите в систему. Для этого выберете в меню пункт Система." },
                { 14, "Вы вошли в систему под именем:" },
                { 15, "" },
                { 16, "" },
                { 17, "" },
                { 18, "" },
                { 19, "" },
            };
        }
    }
}

using Bank_StashYourCrap.Localizations.Base;
using System.Collections.Generic;

namespace Bank_StashYourCrap.Localizations
{
    internal class RusLang : Localization
    {
        public override Dictionary<int, string> StringLibrary { get; }

        public RusLang()
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
                { 7, "Поиск" },
                { 8, "Найти" },
                { 9, "Добавить" },
                { 10, "Изменить" },
                { 11, "Удалить" },
                { 12, "Для продолжнеия работы войдите в систему. Для этого выберете в меню пункт Система." },
                { 13, "Вы вошли в систему под именем:" },
                { 14, "" },
                { 15, "" },
                { 16, "" },
                { 17, "" },
                { 18, "" },
            };
        }
    }
}

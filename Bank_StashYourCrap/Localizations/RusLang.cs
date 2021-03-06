using Bank_StashYourCrap.Localizations.Base;
using System.Collections.Generic;

namespace Bank_StashYourCrap.Localizations
{
    internal class RusLang : AppLocalization
    {
        public override Dictionary<int, string> StringLibrary { get; }

        public RusLang()
        {
            StringLibrary = new Dictionary<int, string>()
            {
                { 0, "Программа для работы с клиентами"},
                { 1, "Меню" },
                { 2, "О программе" },
                { 3, "Изменить язык" },
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
                { 14, "Вход в систему выполнен. Пользователь:" },
                { 15, "Список клиентов" },
                { 16, "Выбранный клиент: " },
                { 17, "Имя" },
                { 18, "Фамилия" },
                { 19, "Отчество" },
                { 20, "Серия паспорта" },
                { 21, "Номер паспорта" },
                { 22, "Номера телефонов" },
                { 23, "Счета клиента" },
                { 24, "Консультант" },
                { 25, "Менеджер" },
                { 26, "Окно регистрации пользователя" },
                { 27, "" },
                { 28, "Русский язык" },
                { 29, "English language" },
                { 30, "Добавить нового клиента" },
                { 31, "Изменить информацию о клиенте" },
                { 32, "Удалить выбранного клиента" },
                { 33, "Бюджетный счёт" },
                { 34, "Счет в иностранной валюте" },
                { 35, "Счёт заморожен" },
                { 36, "Сберегательный счёт" },
                { 37, "Корреспондентский счет" },
                { 38, "Застрахованный счет" },
                { 39, "Если ты не можешь взять весь свой крэп с собой, ты знаешь куда можно обратиться..." },
                { 40, "Окно менеджера клиента" },
                { 41, "Подтвердить" },
                { 42, "Сбросить" },
                { 43, "Количество цифр от 6 до 12. Должны быть только цифры" },
                { 44, "Введённый номер телефона уже есть в списке" },
                { 45, "Ночер счёта должен содержать 14 цифр" },
                { 46, "Введённый номер счёта уже есть в списке" },
                { 47, "Имя введено некоректно. Допускаются только буквы, не меньше 3" },
                { 48, "Фамилия введена некоректно. Допускаются только буквы, не меньше 3" },
                { 49, "Отчество введено некоректно. Допускаются только буквы, не меньше 3" },
                { 50, "Серия паспорта введена некоректно. Должно быть 4 цифры" },
                { 51, "Номер паспорта введён некоректно. Должно быть 6 цифр" },
                { 52, "В списке должен быть хотя бы один телефонный номер" },
                { 53, "В списке должен быть хотя бы один открытый банковский счёт" },
                { 54, "Не удалось добавить нового клиента. Клиент с таким паспортом уже есть" },
                { 55, "Изменить выбранного клиента" },
                { 56, "Удалить выбранного клиента" },
            };
        }
    }
}

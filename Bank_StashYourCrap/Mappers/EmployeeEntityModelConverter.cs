using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Bank_StashYourCrap.Bank.PeopleModels.Employees.Base;
using Bank_StashYourCrap.Localizations.Base;
using Bank_StashYourCrap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bank_StashYourCrap.Mappers
{
    internal static class EmployeeEntityModelConverter
    {
        static public AppLocalization _localization = default!;

        public static void SetLocalization(AppLocalization localization)
        {
            if (localization == null)
            {
                throw new Exception("Указано недопустимое значение для локализации конвертора.");
            }

            _localization = localization;
        }

        // Сущность это то, что приходит из источника (файлов).
        #region Преобрвазование из сущности в модель
        public static EmployeeModel ConvertEntityToModel(this Employee employeeEntity)
        {
            var employeeModel = new EmployeeModel()
            {
                Name = employeeEntity.Name,
                Surname = employeeEntity.Surname,
                Patronymic = employeeEntity.Patronymic,
                PassSeries = employeeEntity.PassSeries.ToString(),
                PassNumber = employeeEntity.PassNumber.ToString(),
                PhoneNumbers = employeeEntity.PhoneNumbers.ConvertListToObservableCollection<string>(),
                AccessLevel = employeeEntity.AccessLevel.ConvertAccessLevelEntityToModel()
            };

            return employeeModel;
        }

        private static ObservableCollection<Type> ConvertListToObservableCollection<Type>(this ICollection<Type> list)
        {
            return new ObservableCollection<Type>(list);
        }

        private static string ConvertAccessLevelEntityToModel(this EmployeeAccessLevel employeeAccessLevel)
        {
            var dictionary = _localization.StringLibrary;
            switch (employeeAccessLevel)
            {
                case EmployeeAccessLevel.Consultant:
                    return dictionary[24];

                case EmployeeAccessLevel.Manager:
                    return dictionary[25];

                default: throw new Exception("Не известный уровень доступа.");
            }
        }
        #endregion
    }
}

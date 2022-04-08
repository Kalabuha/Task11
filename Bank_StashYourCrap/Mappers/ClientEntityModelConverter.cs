using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bank_StashYourCrap.Bank.BankModels;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;
using Bank_StashYourCrap.Localizations.Base;
using Bank_StashYourCrap.Models;

namespace Bank_StashYourCrap.Mappers
{
    internal static class ClientEntityModelConverter
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
        public static ClientModel ConvertEntityToModel(this Client clientEntity)
        {
            var clientModel = new ClientModel()
            {
                Name = clientEntity.Name,
                Surname = clientEntity.Surname,
                Patronymic = clientEntity.Patronymic,
                PassSeries = clientEntity.PassSeries.ToString(),
                PassNumber = clientEntity.PassNumber.ToString(),
                PhoneNumbers = clientEntity.PhoneNumbers.ConvertListToObservableCollection<string>(),
                Accounts = clientEntity.Accounts.ConvertAccountEntityToModel(),
            };

            return clientModel;
        }

        private static ObservableCollection<Type> ConvertListToObservableCollection<Type>(this ICollection<Type> list)
        {
            return new ObservableCollection<Type>(list);
        }

        private static ObservableCollection<BankAccountModel> ConvertAccountEntityToModel(this ICollection<BankAccount> accountsEntities)
        {
            var accountsModels = new ObservableCollection<BankAccountModel>();

            foreach (var accountEntity in accountsEntities)
            {
                accountsModels.Add(accountEntity.ConvertBankAccountEntityToModel());
            }

            return accountsModels;
        }

        private static BankAccountModel ConvertBankAccountEntityToModel(this BankAccount entity)
        {
            return new BankAccountModel()
            {
                NumberAccount = entity.NumberAccount,
                TypeAccount = ConvertTypeAccountToString(entity.TypeAccount)
            };
        }

        public static string ConvertTypeAccountToString(TA typeAccount)
        {
            if (_localization == null)
            {
                throw new Exception("Не указана локализация для конвертора.");
            }

            var dictionary = _localization.StringLibrary;
            switch (typeAccount)
            {
                case TA.Budget: return dictionary[33];

                case TA.ForeignCurrency: return dictionary[34];

                case TA.Frozen: return dictionary[35];

                case TA.Savings: return dictionary[36];

                case TA.Correspondent: return dictionary[37];

                case TA.Insured: return dictionary[38];

                default:
                    throw new NotImplementedException("Тип счёта не известен.");
            }
        }
        #endregion

        #region Преобразование из модели в сущность
        public static Client ConvertModelToEntity(this ClientModel clientModel)
        {
            var clientEntity = new Client()
            {
                Name = clientModel.Name,
                Surname = clientModel.Surname,
                Patronymic = clientModel.Patronymic,
                PassSeries = int.Parse(clientModel.PassSeries),
                PassNumber = int.Parse(clientModel.PassNumber),
                PhoneNumbers = clientModel.PhoneNumbers.ConvertObservableCollectionToList<string>(),
                Accounts = clientModel.Accounts.ConvertAccountModelToEntity()
            };

            return clientEntity;
        }

        private static List<Type> ConvertObservableCollectionToList<Type>(this ICollection<Type> obsCollection)
        {
            return new List<Type>(obsCollection);
        }

        private static List<BankAccount> ConvertAccountModelToEntity(this ICollection<BankAccountModel> accountsModels)
        {
            var accountsEntities = new List<BankAccount>();

            foreach (var accountModel in accountsModels)
            {
                accountsEntities.Add(accountModel.ConvertBankAccountModelToEntity());
            }

            return accountsEntities;
        }

        private static BankAccount ConvertBankAccountModelToEntity(this BankAccountModel model)
        {
            return new BankAccount()
            {
                NumberAccount = model.NumberAccount,
                TypeAccount = ConvertStringToTypeAccount(model.TypeAccount)
            };
        }

        public static TA ConvertStringToTypeAccount(string typeAccount)
        {
            if (_localization == null)
            {
                throw new Exception("Не указана локализация для конвертора.");
            }

            var dictionary = _localization.StringLibrary;
            if (typeAccount == dictionary[33])
                return TA.Budget;

            else if (typeAccount == dictionary[34])
                return TA.ForeignCurrency;

            else if (typeAccount == dictionary[35])
                return TA.Frozen;

            else if (typeAccount == dictionary[36])
                return TA.Savings;

            else if (typeAccount == dictionary[37])
                return TA.Correspondent;

            else if (typeAccount == dictionary[38])
                return TA.Insured;

            else
                throw new NotImplementedException("Тип счёта не известен.");
        }
        #endregion
    }
}

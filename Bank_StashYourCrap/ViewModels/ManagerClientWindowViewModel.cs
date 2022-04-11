using Bank_StashYourCrap.Bank.Services;
using Bank_StashYourCrap.Commands;
using Bank_StashYourCrap.Localizations.Base;
using Bank_StashYourCrap.Models;
using Bank_StashYourCrap.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bank_StashYourCrap.ViewModels
{
    internal class ManagerClientWindowViewModel : BaseViewModel
    {
        private readonly ServiceClientsData _serviceClientsData;
        private readonly ServiceDataVerification _serviceDataVerification;

        // Основные события для данного окна. Два из них должны быть null. Булевые поля для простоты проверки.
        public event Func<ClientModel, bool>? AddAction;
        public bool isAddActionHandlerAttached = false;

        public event Action<ClientModel>? UpdateAction;
        public bool isUpdateActionHandlerAttached = false;

        public event Action<ClientModel>? DeleteAction;
        public bool isDeleteActionHandlerAttached = false;

        public AppLocalization Localization { get; }

#pragma warning disable CS8618
        public ManagerClientWindowViewModel(ServiceClientsData serviceClientsData, AppLocalization localization, ClientModel client)
#pragma warning restore CS8618
        {
            SelectedClient = client;
            ReadClientModel();

            CanselAddClientLabel = "";

            Localization = localization;
            _serviceClientsData = serviceClientsData;
            _serviceDataVerification = new ServiceDataVerification();

            AllTypesAccount = _serviceClientsData.GetAllTypesAccount();
            CreateAllCommands();
        }

        private void CreateAllCommands()
        {
            AddPhoneNumberCommand = new ActionCommand(
                OnExecuteAddPhoneNumberCommand, CanExecuteAddPhoneNumberCommand);

            RemovePhoneNumberCommand = new ActionCommand(
                OnExecuteRemovePhoneNumberCommand, CanExecuteRemovePhoneNumberCommand);

            AddBankAccountCommand = new ActionCommand(
                OnExecuteAddBankAccountCommand, CanExecuteAddBankAccountCommand);

            RemoveAccountCommand = new ActionCommand(
                OnExecuteRemoveAccountCommand, CanExecuteRemoveAccountCommand);

            CUDActionCommand = new ActionCommand(
                OnExecuteCUDActionCommand, CanExecuteCUDActionCommand);

            CUDActionCancelCommand = new ActionCommand(
                OnExecuteCUDActionCancelCommand, CanExecuteCUDActionCancelCommand);
        }
        #region Свойство статус окна
        private string _status = default!;
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
        #endregion

        // Свойства для редактирования
        #region Свойство имя клиента
        private string _nameTextBox = default!;
        public string NameTextBox
        {
            get => _nameTextBox;
            set => Set(ref _nameTextBox, value);
        }
        public bool IsReadOnlyNameTextBox { get; set; } = true;
        #endregion

        #region Свойство фамилия клиента
        private string _surnameTextBox = default!;
        public string SurnameTextBox
        {
            get => _surnameTextBox;
            set => Set(ref _surnameTextBox, value);
        }
        public bool IsReadOnlySurnameTextBox { get; set; } = true;
        #endregion

        #region Свойство отчество клиента
        private string _patronymicTextBox = default!;
        public string PatronymicTextBox
        {
            get => _patronymicTextBox;
            set => Set(ref _patronymicTextBox, value);
        }
        public bool IsReadOnlyPatronymicTextBox { get; set; } = true;
        #endregion

        #region Свойство серия паспорта клиента
        private string _passSeriesTextBox = default!;
        public string PassSeriesTextBox
        {
            get => _passSeriesTextBox;
            set => Set(ref _passSeriesTextBox, value);
        }
        public bool IsReadOnlyPassSeriesTextBox { get; set; } = true;
        #endregion

        #region Свойство номер паспорта клиента
        private string _passNumberTextBox = default!;
        public string PassNumberTextBox
        {
            get => _passNumberTextBox;
            set => Set(ref _passNumberTextBox, value);
        }
        public bool IsReadOnlyPassNumberTextBox { get; set; } = true;
        #endregion

        #region Свойство номер телефона клиента
        private string _phoneNumberTextBox = default!;
        public string PhoneNumberTextBox
        {
            get => _phoneNumberTextBox;
            set => Set(ref _phoneNumberTextBox, value);
        }
        public bool IsReadOnlyPhoneNumberTextBox { get; set; } = true;
        #endregion

        #region Свойство номер счёта клиента
        private string _accountNumberTextBox = default!;
        public string AccountNumberTextBox
        {
            get => _accountNumberTextBox;
            set => Set(ref _accountNumberTextBox, value);
        }
        public bool IsReadOnlyAccountNumberTextBox { get; set; } = true;
        #endregion

        #region Свойство коллекция телефонных номеров клиента
        private ObservableCollection<string> _phoneNumbers;

        public ObservableCollection<string> PhoneNumbersListBox
        {
            get => _phoneNumbers;
            set => Set(ref _phoneNumbers, value);
        }
        #endregion

        #region Свойство коллекция счетов клиента
        private ObservableCollection<BankAccountModel> _bankAccounts;

        public ObservableCollection<BankAccountModel> BankAccountsListBox
        {
            get => _bankAccounts;
            set => Set(ref _bankAccounts, value);
        }
        #endregion

        // Выбранные элементы
        #region Свойство выбранный тип счёта клиента
        private string _accountTypeComboBoxSelected = default!;
        public string AccountTypeComboBoxSelected
        {
            get => _accountTypeComboBoxSelected;
            set => Set(ref _accountTypeComboBoxSelected, value);
        }
        public bool IsEnableAccountTypeComboBoxSelected { get; set; } = false;
        #endregion

        #region Свойство выбранный номер телефона клиента из списка
        private string? _phoneNumberListBoxSelected;
        public string? PhoneNumberListBoxSelected
        {
            get => _phoneNumberListBoxSelected;
            set => Set(ref _phoneNumberListBoxSelected, value);
        }
        #endregion

        #region Свойство выбранный счёт клиента из списка
        private BankAccountModel? _accountListBoxSelected;
        public BankAccountModel? AccountListBoxSelected
        {
            get => _accountListBoxSelected;
            set => Set(ref _accountListBoxSelected, value);
        }
        #endregion

        // Свойства подсказки
        #region Свойство подсказка к имени
        private string _nameLabel;
        public string NameLabel
        {
            get => _nameLabel;
            set => Set(ref _nameLabel, value);
        }
        #endregion

        #region Свойство подсказка к фамилии
        private string _surnameLabel;
        public string SurnameLabel
        {
            get => _surnameLabel;
            set => Set(ref _surnameLabel, value);
        }
        #endregion

        #region Свойство подсказка к отчеству
        private string _patronymicLabel;
        public string PatronymicLabel
        {
            get => _patronymicLabel;
            set => Set(ref _patronymicLabel, value);
        }
        #endregion

        #region Свойство подсказка к серии паспорта
        private string _passSeriesLabel;
        public string PassSeriesLabel
        {
            get => _passSeriesLabel;
            set => Set(ref _passSeriesLabel, value);
        }
        #endregion

        #region Свойство подсказка к номеру паспорта
        private string _passNumberLabel;
        public string PassNumberLabel
        {
            get => _passNumberLabel;
            set => Set(ref _passNumberLabel, value);
        }
        #endregion

        #region Свойство подсказка к номеру телефона
        private string _phoneNumberLabel;
        public string PhoneNumberLabel
        {
            get => _phoneNumberLabel;
            set => Set(ref _phoneNumberLabel, value);
        }
        #endregion

        #region Свойство подсказка к номеру счёта
        private string _accountNumberLabel;
        public string AccountNumberLabel
        {
            get => _accountNumberLabel;
            set => Set(ref _accountNumberLabel, value);
        }
        #endregion

        #region Свойство подсказка отмена добавления нового клиента, потому что такой паспорт уже есть в списке клиентов
        private string _canselAddClientLabel;
        public string CanselAddClientLabel
        {
            get => _canselAddClientLabel;
            set => Set(ref _canselAddClientLabel, value);
        }
        #endregion

        // Доп. свойства
        #region Свойство коллекция всех возможных типов счетов
        public ObservableCollection<string> AllTypesAccount { get; }
        #endregion

        #region Свойство клиент для которого выполняется выбранное действие
        private ClientModel? _selectedClient;
        public ClientModel? SelectedClient
        {
            get => _selectedClient;
            set => Set(ref _selectedClient, value);
        }
        #endregion

        #region Свойство видимость текстблоков. Используется, если нужно скрыть какие-нибудь текстблоки
        public Visibility VisibilityTextBoxes { get; set; } = Visibility.Visible;
        #endregion

        // Команды
        #region Команда добавить номер телефона в коллекцию номеров клиента
        public ICommand AddPhoneNumberCommand { get; private set; }
        private void OnExecuteAddPhoneNumberCommand(object parameter)
        {
            if (!_serviceDataVerification.IsValidPhoneNumber(PhoneNumberTextBox))
            {
                PhoneNumberLabel = Localization.StringLibrary[43];
                return;
            }
            else if (PhoneNumbersListBox.Contains(PhoneNumberTextBox))
            {
                PhoneNumberLabel = Localization.StringLibrary[44];
                return;
            }

            PhoneNumberLabel = "";
            PhoneNumbersListBox.Add(PhoneNumberTextBox);
        }

        private bool CanExecuteAddPhoneNumberCommand(object parameter)
        {
            if (PhoneNumberTextBox == null && IsReadOnlyPhoneNumberTextBox)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Команда удалить выбранный номер телефона из коллекции номеров клиента
        public ICommand RemovePhoneNumberCommand { get; private set; }
        private void OnExecuteRemovePhoneNumberCommand(object parameter)
        {
            PhoneNumbersListBox.Remove(PhoneNumberListBoxSelected!);
            PhoneNumberListBoxSelected = null;
        }

        private bool CanExecuteRemovePhoneNumberCommand(object parameter)
        {
            if (PhoneNumberListBoxSelected == null || PhoneNumbersListBox.Count == 0 || IsReadOnlyPhoneNumberTextBox)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Команда добавить банковский счёт в коллекцию счетов клиента
        public ICommand AddBankAccountCommand { get; private set; } = default!;
        private void OnExecuteAddBankAccountCommand(object parameter)
        {
            if (!_serviceDataVerification.IsValidNumber(AccountNumberTextBox, numberOfDigits: 14))
            {
                AccountNumberLabel = Localization.StringLibrary[45];
                return;
            }

            if (BankAccountsListBox.Select(a => a.NumberAccount.ToString()).Contains(AccountNumberTextBox.Trim()))
            {
                AccountNumberLabel = Localization.StringLibrary[46];
                return;
            }

            AccountNumberLabel = "";
            BankAccountsListBox.Add(new BankAccountModel()
            {
                TypeAccount = AccountTypeComboBoxSelected,
                NumberAccount = long.Parse(AccountNumberTextBox)
            });
        }

        private bool CanExecuteAddBankAccountCommand(object parameter)
        {
            if ((AccountTypeComboBoxSelected == null || AccountNumberTextBox == null) && IsReadOnlyAccountNumberTextBox)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Команда удалить выбранный банковский счёт из коллекции счетов клиента
        public ICommand RemoveAccountCommand { get; private set; }
        private void OnExecuteRemoveAccountCommand(object parameter)
        {
            BankAccountsListBox.Remove(AccountListBoxSelected!);
            AccountListBoxSelected = null;
        }

        private bool CanExecuteRemoveAccountCommand(object parameter)
        {
            if (AccountListBoxSelected == null || BankAccountsListBox.Count == 0 || IsReadOnlyAccountNumberTextBox)
            {
                return false;
            }
            return true;
        }
        #endregion

        // Основные кнопки
        #region Команда подтвердить выбранное действие (добавить, изменить или удалить клиента)
        // Create, Update, Delete - CUD
        public ICommand CUDActionCommand { get; private set; } = default!;
        private void OnExecuteCUDActionCommand(object parameter)
        {
            bool IsValidAllData = CheckValidityAllData();

            if (IsValidAllData)
            {
                var newClient = CreateClientModel();
                // Здесь выполняется основное действие для окна.
                bool? result = AddAction?.Invoke(newClient);
                UpdateAction?.Invoke(newClient);
                DeleteAction?.Invoke(newClient);
                if (result != null && result == false)
                {
                    CanselAddClientLabel = Localization.StringLibrary[54];
                    return;
                }
                var window = (Window)parameter;
                window.Close();
            }
        }

        private bool CanExecuteCUDActionCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region Команда отменить выбранное действие (сбрасывает изменения данных). Не закрывает окно.
        // Create, Update, Delete - CUD
        public ICommand CUDActionCancelCommand { get; private set; } = default!;
        private void OnExecuteCUDActionCancelCommand(object parameter)
        {
            ReadClientModel();
        }

        private bool CanExecuteCUDActionCancelCommand(object parameter)
        {
            if (isDeleteActionHandlerAttached)
            {
                return false;
            }
            return true;
        }
        #endregion

        private bool CheckValidityAllData()
        {
            bool IsValidAllData = true;
            if (!_serviceDataVerification.IsValidName(NameTextBox))
            {
                IsValidAllData = false;
                NameLabel = Localization.StringLibrary[47];
            }
            else NameLabel = "";
            if (!_serviceDataVerification.IsValidName(SurnameTextBox))
            {
                IsValidAllData = false;
                SurnameLabel = Localization.StringLibrary[48];
            }
            else SurnameLabel = "";
            if (!_serviceDataVerification.IsValidName(PatronymicTextBox))
            {
                IsValidAllData = false;
                PatronymicLabel = Localization.StringLibrary[49];
            }
            else PatronymicLabel = "";
            if (!_serviceDataVerification.IsValidNumber(PassSeriesTextBox, numberOfDigits: 4))
            {
                IsValidAllData = false;
                PassSeriesLabel = Localization.StringLibrary[50];
            }
            else PassSeriesLabel = "";
            if (!_serviceDataVerification.IsValidNumber(_passNumberTextBox, numberOfDigits: 6))
            {
                IsValidAllData = false;
                PassNumberLabel = Localization.StringLibrary[51];
            }
            else PassNumberLabel = "";
            if (!PhoneNumbersListBox.Any())
            {
                IsValidAllData = false;
                PhoneNumberLabel = Localization.StringLibrary[52];
            }
            else PhoneNumberLabel = "";
            if (!BankAccountsListBox.Any())
            {
                AccountNumberLabel = Localization.StringLibrary[53];
                IsValidAllData = false;
            }
            else AccountNumberLabel = "";

            return IsValidAllData;
        }

        private void ReadClientModel()
        {
            if (SelectedClient == null)
            {
                throw new Exception("Ввыбранный клиент потерялся по дороге. Выполнять действия над ним невозможно.");
            }

            NameTextBox = new string(SelectedClient.Name);
            SurnameTextBox = new string(SelectedClient.Surname);
            PatronymicTextBox = new string(SelectedClient.Patronymic);
            PassSeriesTextBox = new string(SelectedClient.PassSeries);
            PassNumberTextBox = new string(SelectedClient.PassNumber);
            PhoneNumbersListBox = new ObservableCollection<string>(SelectedClient.PhoneNumbers);
            BankAccountsListBox = new ObservableCollection<BankAccountModel>(SelectedClient.Accounts);
        }

        private ClientModel CreateClientModel()
        {
            var newClient = new ClientModel()
            {
                Name = NameTextBox,
                Surname = SurnameTextBox,
                Patronymic = PatronymicTextBox,
                PassSeries = PassSeriesTextBox,
                PassNumber = PassNumberTextBox,
                PhoneNumbers = PhoneNumbersListBox,
                Accounts = BankAccountsListBox,
            };

            return newClient;
        }
    }
}

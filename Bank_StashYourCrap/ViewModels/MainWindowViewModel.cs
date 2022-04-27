using Bank_StashYourCrap.Bank.DataContext;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Bank_StashYourCrap.Bank.PeopleModels.Employees.Base;
using Bank_StashYourCrap.Bank.Services;
using Bank_StashYourCrap.Commands;
using Bank_StashYourCrap.Localizations;
using Bank_StashYourCrap.Localizations.Base;
using Bank_StashYourCrap.Mappers;
using Bank_StashYourCrap.Models;
using Bank_StashYourCrap.ViewModels.Base;
using Bank_StashYourCrap.Views.Windows;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Bank_StashYourCrap.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private readonly ServiceClientsData _clientsService;
        private readonly ServiceEmployeesData _employeesService;

        
        public MainWindowViewModel()
        {
            var repository = new RepositoryPeopleData();

            _clientsService = new ServiceClientsData(repository);
            _employeesService = new ServiceEmployeesData(repository);

            SetupApplicationConfiguration();
            UserRegistrationChecks();
            CreateAllCommands();
        }

        private void SetupApplicationConfiguration()
        {
            SetupRussianLocalization();
            SetupLocalization();
            Title = Localization!.StringLibrary[0];
        }

        private void SetupLocalization()
        {
            EmployeeEntityModelConverter.SetLocalization(Localization);
            ClientEntityModelConverter.SetLocalization(Localization);
        }

        private void SetupRussianLocalization()
        {
            Localization = new RusLang();
        }

        private void SetupEnglishLocalization()
        {
            Localization = new EngLang();
        }

        private void CreateAllCommands()
        {
            CrateNewClientCommand = new ActionCommand(
                OnExecuteCallCreateClientManagerWindowCommand, CanExecuteCallCreateClientManagerWindowCommand);

            EditClientCommand = new ActionCommand(
                OnExecuteEditClientCommand, CanExecuteEditClientCommand);

            DeleteClientCommand = new ActionCommand(
                OnExecuteDeleteClientCommand, CanExecuteDeleteClientCommand);

            CallRegistrationWindowCommand = new ActionCommand(
                OnExecuteCallRegistrationWindowCommand, CanExecuteCallRegistrationWindowCommand);

            ShowBillboardWindowCommand = new ActionCommand(
                OnExecuteShowBillboardWindowCommand, CanExecuteShowBillboardWindowCommand);

            UnRegistrationCommand = new ActionCommand(
                OnExecuteUnRegistrationCommand, CanExecuteUnRegistrationCommand);

            SetupRussianLanguageCommand = new ActionCommand(
                OnExecuteSetupRussianLanguageCommand, CanExecuteSetupRussianLanguageCommand);

            SetupEnglishLanguageCommand = new ActionCommand(
                OnExecuteSetupEnglishLanguageCommand, CanExecuteSetupEnglishLanguageCommand);
        }

        private void UserRegistrationChecks()
        {
            if (RegisteredUser == null)
            {
                Status = Localization.StringLibrary[13];
            }
            else
            {
                Status = Localization.StringLibrary[14] + " " + $"{RegisteredUser.Name}";
            }
        }

        // Свойства
        #region Свойство локализация языка
        private AppLocalization _localization = default!;
        public AppLocalization Localization
        {
            get => _localization;
            set => Set(ref _localization, value);
        }
        #endregion

        #region Свойство заглавие окна
        private string _title = default!;
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        #endregion

        #region Свойство статус программы
        private string _status = default!;
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
        #endregion

        #region Свойство пользователь, который вошёл в систему
        private Employee? _registeredUser;
        public Employee? RegisteredUser
        {
            get => _registeredUser;
            set => Set(ref _registeredUser, value);
        }
        #endregion

        #region Свойство выбранный клиент из списка всех клиентов
        private ClientModel? _selectedClient;
        public ClientModel? SelectedClient
        {
            get => _selectedClient;
            set => Set(ref _selectedClient, value);
        }
        #endregion

        #region Свойство коллекция всех клиентов
        private ObservableCollection<ClientModel>? _clients;

        public ObservableCollection<ClientModel>? Clients
        {
            get => _clients;
            set => Set(ref _clients, value);
        }
        #endregion

        #region Свойства окно регистрации работника
        private RegistrationEmployeeWindow? RegistrationEmployeeWindow { get; set; }
        private RegistrationEmployeeWindowViewModel? RegistrationEmployeeWindowViewModel { get; set; }
        #endregion

        #region Свойства окна менеджера клиента
        private ManagerClientWindow? ManagerClientWindow { get; set; }
        private ManagerClientWindowViewModel? ManagerClientWindowViewModel { get; set; }
        #endregion

        #region Свойство видимость текстблоков. Используется, если нужно скрыть какие-нибудь текстблоки
        private Visibility _visibilityTextBoxes = Visibility.Visible;
        public Visibility VisibilityTextBoxes
        {
            get => _visibilityTextBoxes;
            set => Set(ref _visibilityTextBoxes, value);
        }
        #endregion

        // Команды CUD
        #region Команда открыть окно для создания нового клиента
        public ICommand CrateNewClientCommand { get; private set; } = default!;

        private void OnExecuteCallCreateClientManagerWindowCommand(object parameter)
        {
            var managmentClientWindow = new ManagerClientWindow()
            {
                Owner = Application.Current.MainWindow,
            };
            ManagerClientWindow = managmentClientWindow;
            managmentClientWindow.Closed += OnDialogWindowClosed!;

            var newClient = new ClientModel
            {
                Name = string.Empty,
                Surname = string.Empty,
                Patronymic = string.Empty,
                PassSeries = string.Empty,
                PassNumber = string.Empty,
                PhoneNumbers = new ObservableCollection<string>(),
                Accounts = new ObservableCollection<BankAccountModel>()
            };

            var managerClientWindowViewModel = new ManagerClientWindowViewModel(_clientsService, Localization, newClient);
            ManagerClientWindowViewModel = managerClientWindowViewModel;

            managerClientWindowViewModel.AddAction += _clientsService.AddClientAsync;
            managerClientWindowViewModel.isAddActionHandlerAttached = true;

            managerClientWindowViewModel.Status = Localization.StringLibrary[30];
            SetDataEditingRights(managerClientWindowViewModel);
            managmentClientWindow.DataContext = ManagerClientWindowViewModel;
            managmentClientWindow.ShowDialog();
        }

        private bool CanExecuteCallCreateClientManagerWindowCommand(object parameter)
        {
            // Только менеджер может создавать нового клиента
            if (RegisteredUser != null && RegisteredUser.AccessLevel == EmployeeAccessLevel.Manager)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Команда открыть окно для изменения выбранного клиента
        public ICommand EditClientCommand { get; private set; } = default!;

        private void OnExecuteEditClientCommand(object parameter)
        {
            var managmentClientWindow = new ManagerClientWindow()
            {
                Owner = Application.Current.MainWindow,
            };
            ManagerClientWindow = managmentClientWindow;
            managmentClientWindow.Closed += OnDialogWindowClosed!;

            var managerClientWindowViewModel = new ManagerClientWindowViewModel(_clientsService, Localization, SelectedClient!);
            ManagerClientWindowViewModel = managerClientWindowViewModel;

            managerClientWindowViewModel.UpdateAction += _clientsService.EditClientAsync;
            managerClientWindowViewModel.isUpdateActionHandlerAttached = true;

            managerClientWindowViewModel.Status = Localization.StringLibrary[55];
            SetDataEditingRights(managerClientWindowViewModel);
            managmentClientWindow.DataContext = ManagerClientWindowViewModel;
            managmentClientWindow.ShowDialog();
        }

        private bool CanExecuteEditClientCommand(object parameter)
        {
            // Изменять может и консультант и менеджер. Но у консультанта урезаны возможности по изменению
            if (RegisteredUser != null && SelectedClient != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Команда открыть окно для удаления выбранного клиента
        public ICommand DeleteClientCommand { get; private set; } = default!;

        private void OnExecuteDeleteClientCommand(object parameter)
        {
            var managmentClientWindow = new ManagerClientWindow()
            {
                Owner = Application.Current.MainWindow,
            };
            ManagerClientWindow = managmentClientWindow;
            managmentClientWindow.Closed += OnDialogWindowClosed!;

            var managerClientWindowViewModel = new ManagerClientWindowViewModel(_clientsService, Localization, SelectedClient!);
            ManagerClientWindowViewModel = managerClientWindowViewModel;

            managerClientWindowViewModel.DeleteAction += _clientsService.DeleteClientAsync;
            managerClientWindowViewModel.isDeleteActionHandlerAttached = true;

            managerClientWindowViewModel.Status = Localization.StringLibrary[56];
            SetDataEditingRights(managerClientWindowViewModel);
            managmentClientWindow.DataContext = ManagerClientWindowViewModel;
            managmentClientWindow.ShowDialog();
        }

        private bool CanExecuteDeleteClientCommand(object parameter)
        {
            // Удалять может только менеджер
            if (RegisteredUser != null &&
                RegisteredUser.AccessLevel == EmployeeAccessLevel.Manager &&
                SelectedClient != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        // Дополнительные меторды для создания окна менеджера клиента
        #region Установки события после закрытия окна менеджера и установка открытости свойств - только чтение или нет

        // Разный уровень доступа даёт разные права по возможности редактирования всяких свойств клиента
        // Права редактирования также зависят от того, какое действие выбрано, удаление, изменение, добавление.
        // Этот метод может работать только с объектом ManagerClientWindowViewModel
        private void SetDataEditingRights(ManagerClientWindowViewModel managerClientWindowViewModel)
        {
            if (RegisteredUser == null)
                throw new NotImplementedException("Пользователь не зарегестрирован");

            // По умолчанию во всех свойствах стоит true (т.е. Только для чтения)
            // Тут надо только разрешать свойства для изменения - ставить false
            if (managerClientWindowViewModel.isAddActionHandlerAttached)
            {
                if (RegisteredUser.AccessLevel == EmployeeAccessLevel.Manager)
                {
                    managerClientWindowViewModel.IsReadOnlyNameTextBox = false;
                    managerClientWindowViewModel.IsReadOnlySurnameTextBox = false;
                    managerClientWindowViewModel.IsReadOnlyPatronymicTextBox = false;
                    managerClientWindowViewModel.IsReadOnlyPassSeriesTextBox = false;
                    managerClientWindowViewModel.IsReadOnlyPassNumberTextBox = false;
                    managerClientWindowViewModel.IsReadOnlyPhoneNumberTextBox = false;
                    managerClientWindowViewModel.IsReadOnlyAccountNumberTextBox = false;
                    managerClientWindowViewModel.IsEnableAccountTypeComboBoxSelected = true;
                }
            }
            else if (managerClientWindowViewModel.isUpdateActionHandlerAttached)
            {
                if (RegisteredUser.AccessLevel == EmployeeAccessLevel.Consultant)
                {
                    managerClientWindowViewModel.IsReadOnlyPhoneNumberTextBox = false;
                    managerClientWindowViewModel.VisibilityTextBoxes = Visibility.Hidden;
                }
                else if (RegisteredUser.AccessLevel == EmployeeAccessLevel.Manager)
                {
                    managerClientWindowViewModel.IsReadOnlyNameTextBox = false;
                    managerClientWindowViewModel.IsReadOnlySurnameTextBox = false;
                    managerClientWindowViewModel.IsReadOnlyPatronymicTextBox = false;
                    managerClientWindowViewModel.IsReadOnlyPhoneNumberTextBox = false;
                    managerClientWindowViewModel.IsReadOnlyAccountNumberTextBox = false;
                    managerClientWindowViewModel.IsEnableAccountTypeComboBoxSelected = true;
                }
            }
            else if (managerClientWindowViewModel.isDeleteActionHandlerAttached)
            {
                return;
            }
            else
            {
                throw new NotImplementedException("Не выбранно действие для окна менеждера.");
            }
        }

        private async void OnDialogWindowClosed(object sender, EventArgs e)
        {
            ((Window)sender).Closed -= OnDialogWindowClosed!;
            RegisteredUser ??= RegistrationEmployeeWindowViewModel?.ConfirmUser;
            if (RegisteredUser != null)
            {
                Clients = await _clientsService.GetAllClientsAsync();

                if (RegisteredUser.AccessLevel == EmployeeAccessLevel.Consultant)
                {
                    VisibilityTextBoxes = Visibility.Hidden;
                }
                else
                {
                    VisibilityTextBoxes = Visibility.Visible;
                }
            }
            else
            {
                Clients = null;
            }
            UserRegistrationChecks();
            ManagerClientWindowViewModel = null;
            RegistrationEmployeeWindow = null;
        }
        #endregion

        // Прочие команды
        #region Команда найти клиента по номеру и серии паспорта
        public ICommand SearchClientByPassport { get; private set; } = default!;

        private void OnExecuteSearchClientByPassportCommand(object parameter)
        {

        }

        private bool CanExecuteSearchClientByPassportCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region Команда вызвать окно информации о программе
        public ICommand ShowBillboardWindowCommand { get; private set; } = default!;

        private void OnExecuteShowBillboardWindowCommand(object parameter)
        {
            var billboardWindow = new ShowBillboardWindow()
            {
                Owner = Application.Current.MainWindow,
            };

            billboardWindow.Title = Localization.StringLibrary[39];
            billboardWindow.ShowDialog();
        }

        private bool CanExecuteShowBillboardWindowCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region Команда вызвать окно регистрации пользователя системы
        public ICommand CallRegistrationWindowCommand { get; private set; } = default!;

        private async void OnExecuteCallRegistrationWindowCommand(object parameter)
        {
            var registrationWindow = new RegistrationEmployeeWindow()
            {
                Owner = Application.Current.MainWindow,
            };
            RegistrationEmployeeWindow = registrationWindow;

            registrationWindow.Title = Localization.StringLibrary[26];
            registrationWindow.Closed += OnDialogWindowClosed!;

            var registrationWindowViewModel = await RegistrationEmployeeWindowViewModel.CreateAsync(_employeesService);
            registrationWindowViewModel.Localization = Localization;
            RegistrationEmployeeWindowViewModel = registrationWindowViewModel;
            registrationWindow.DataContext = registrationWindowViewModel;
            registrationWindow.ShowDialog();
        }

        private bool CanExecuteCallRegistrationWindowCommand(object parameter)
        {
            return RegisteredUser == null;
        }

        
        #endregion

        #region Команда выйти из системы
        public ICommand UnRegistrationCommand { get; private set; } = default!;

        private void OnExecuteUnRegistrationCommand(object parameter)
        {
            RegisteredUser = null;
            Clients = null;
            UserRegistrationChecks();
        }

        private bool CanExecuteUnRegistrationCommand(object parameter)
        {
            return RegisteredUser != null;
        }
        #endregion

        #region Команда установить русский язык
        public ICommand SetupRussianLanguageCommand { get; private set; } = default!;

        private void OnExecuteSetupRussianLanguageCommand(object parameter)
        {
            SetupRussianLocalization();
            UserRegistrationChecks();
            Title = Localization.StringLibrary[0];
        }

        private bool CanExecuteSetupRussianLanguageCommand(object parameter)
        {
            if (Localization is RusLang)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Команда установить английский язык
        public ICommand SetupEnglishLanguageCommand { get; private set; } = default!;

        private void OnExecuteSetupEnglishLanguageCommand(object parameter)
        {
            SetupEnglishLocalization();
            UserRegistrationChecks();
            Title = Localization.StringLibrary[0];
        }

        private bool CanExecuteSetupEnglishLanguageCommand(object parameter)
        {
            if (Localization is EngLang)
            {
                return false;
            }
            return true;
        }
        #endregion
    }


}

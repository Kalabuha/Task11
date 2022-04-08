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

        public AppLocalization Localization { get; private set; } = default!;

        public MainWindowViewModel()
        {
            var repository = new RepositoryPeopleData();

            _clientsService = new ServiceClientsData(repository);
            _employeesService = new ServiceEmployeesData(repository);

            SetupLocalization();
            StartApplicationConfiguration();
            UserRegistrationChecks();
            CreateAllCommands();
        }

        private void StartApplicationConfiguration()
        {
            Title = Localization!.StringLibrary[0];
        }

        private void SetupLocalization()
        {
            Localization = new RusLang();
            EmployeeEntityModelConverter.SetLocalization(Localization);
            ClientEntityModelConverter.SetLocalization(Localization);
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
        }

        private void UserRegistrationChecks()
        {
            if (RegisteredUser == null)
            {
                Status = Localization.StringLibrary[13];
            }
            else
            {
                Status = Localization.StringLibrary[14] + " " + $"{RegisteredUser?.Name}";
            }
        }


        // Свойства
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


        // Команды
        #region Команда открыть окно для создания нового клиента
        public ICommand CrateNewClientCommand { get; private set; } = default!;

        private void OnExecuteCallCreateClientManagerWindowCommand(object parameter)
        {
            var managmentClientWindow = new ManagerClientWindow()
            {
                Owner = Application.Current.MainWindow,
            };
            ManagerClientWindow = managmentClientWindow;
            managmentClientWindow.Closed += OnManagerClientWindowClosed!;

            var newClient = new ClientModel
            {
                Name = "",
                Surname = "",
                Patronymic = "",
                PassSeries = "",
                PassNumber = "",
                PhoneNumbers = new ObservableCollection<string>(),
                Accounts = new ObservableCollection<BankAccountModel>()
            };

            var managerClientWindowViewModel = new ManagerClientWindowViewModel(_clientsService, Localization, newClient);
            ManagerClientWindowViewModel = managerClientWindowViewModel;
            managerClientWindowViewModel.AddAction += _clientsService.AddClient;
            managerClientWindowViewModel.Status = Localization.StringLibrary[30];
            managmentClientWindow.DataContext = ManagerClientWindowViewModel;
            managmentClientWindow.ShowDialog();
        }

        private bool CanExecuteCallCreateClientManagerWindowCommand(object parameter)
        {
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
            managmentClientWindow.Closed += OnManagerClientWindowClosed!;

            var managerClientWindowViewModel = new ManagerClientWindowViewModel(_clientsService, Localization, SelectedClient!);
            ManagerClientWindowViewModel = managerClientWindowViewModel;
            managerClientWindowViewModel.UpdateAction += _clientsService.EditClient;
            managerClientWindowViewModel.Status = Localization.StringLibrary[55];
            managmentClientWindow.DataContext = ManagerClientWindowViewModel;
            managmentClientWindow.ShowDialog();
        }

        private bool CanExecuteEditClientCommand(object parameter)
        {
            if (RegisteredUser != null &&
                RegisteredUser.AccessLevel == EmployeeAccessLevel.Manager &&
                SelectedClient != null)
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
            managmentClientWindow.Closed += OnManagerClientWindowClosed!;

            var managerClientWindowViewModel = new ManagerClientWindowViewModel(_clientsService, Localization, SelectedClient!);
            ManagerClientWindowViewModel = managerClientWindowViewModel;
            managerClientWindowViewModel.DeleteAction += _clientsService.DeleteClient;
            managerClientWindowViewModel.Status = Localization.StringLibrary[56];
            managmentClientWindow.DataContext = ManagerClientWindowViewModel;
            managmentClientWindow.ShowDialog();
        }

        private bool CanExecuteDeleteClientCommand(object parameter)
        {
            if (RegisteredUser != null &&
                RegisteredUser.AccessLevel == EmployeeAccessLevel.Manager &&
                SelectedClient != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        private void OnManagerClientWindowClosed(object sender, EventArgs e)
        {
            ((Window)sender).Closed -= OnManagerClientWindowClosed!;
            RegisteredUser = RegistrationEmployeeWindowViewModel?.ConfirmUser;
            if (RegisteredUser != null)
            {
                Clients = _clientsService.GetAllClients(RegisteredUser);
                UserRegistrationChecks();
            }
            RegistrationEmployeeWindow = null;
        }

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

        private void OnExecuteCallRegistrationWindowCommand(object parameter)
        {
            var registrationWindow = new RegistrationEmployeeWindow()
            {
                Owner = Application.Current.MainWindow,
            };
            RegistrationEmployeeWindow = registrationWindow;

            registrationWindow.Title = Localization.StringLibrary[26];
            registrationWindow.Closed += OnRegistrationWindowClosed!;

            var registrationWindowViewModel = new RegistrationEmployeeWindowViewModel(_employeesService);
            registrationWindowViewModel.Localization = Localization;
            RegistrationEmployeeWindowViewModel = registrationWindowViewModel;
            registrationWindow.DataContext = registrationWindowViewModel;
            registrationWindow.ShowDialog();
        }

        private bool CanExecuteCallRegistrationWindowCommand(object parameter)
        {
            return RegisteredUser == null;
        }

        private void OnRegistrationWindowClosed(object sender, EventArgs e)
        {
            ((Window)sender).Closed -= OnRegistrationWindowClosed!;
            RegisteredUser = RegistrationEmployeeWindowViewModel?.ConfirmUser;
            if (RegisteredUser != null)
            {
                Clients = _clientsService.GetAllClients(RegisteredUser);
                UserRegistrationChecks();
            }
            RegistrationEmployeeWindow = null;
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
    }


}

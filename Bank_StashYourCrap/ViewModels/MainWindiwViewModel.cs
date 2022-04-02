using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Bank_StashYourCrap.Localizations;
using Bank_StashYourCrap.Localizations.Base;
using Bank_StashYourCrap.ViewModels.Base;
using Bank_StashYourCrap.Bank.Services;
using Bank_StashYourCrap.Bank.DataContext;
using Bank_StashYourCrap.Models;
using Bank_StashYourCrap.Mappers;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;
using Bank_StashYourCrap.Commands;

namespace Bank_StashYourCrap.ViewModels
{
    internal class MainWindiwViewModel : BaseViewModel
    {
        private readonly ServiceClientsData _clientsService;
        private readonly ServiceEmployeesData _employeesService;

        public Localization Localization { get; private set; }

        public MainWindiwViewModel()
        {
            var repository = new RepositoryPeopleData();
            _clientsService = new ServiceClientsData(repository);
            _employeesService = new ServiceEmployeesData(repository);

            SetupLocalization();

            Clients = _clientsService.GetAllClients();

            Title = Localization!.StringLibrary[0];
            UserRegistrationChecks();

            СonstructAllCommands();
        }

        private void SetupLocalization()
        {
            Localization = new RusLang();
            ClientEntityModelConverter.SetLocalization(Localization);
        }

        private void СonstructAllCommands()
        {
            CrateNewClientCommand = new ActionCommand(
                execute: OnExecuteCrateNewClientCommand, can: CanExecuteCrateNewClientCommand);

            EditClientCommand = new ActionCommand(
                execute: OnExecuteEditClientCommand, can: CanExecuteEditClientCommand);

            DeleteClientCommand = new ActionCommand(
                execute: OnExecuteDeleteClientCommand, can: CanExecuteDeleteClientCommand);
        }

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
        private EmployeeModel? _registeredUser = null!;
        public EmployeeModel? RegisteredUser
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
        public ObservableCollection<ClientModel>? Clients { get; set; }
        #endregion


        #region Команда создать нового клиента
        public ICommand CrateNewClientCommand { get; private set; } = default!;

        private void OnExecuteCrateNewClientCommand(object parameter)
        {

        }

        private bool CanExecuteCrateNewClientCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region Команда изменить выбранного клиента
        public ICommand EditClientCommand { get; private set; } = default!;

        private void OnExecuteEditClientCommand(object parameter)
        {

        }

        private bool CanExecuteEditClientCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region Команда удалить выбранного клиента
        public ICommand DeleteClientCommand { get; private set; } = default!;

        private void OnExecuteDeleteClientCommand(object parameter)
        {

        }

        private bool CanExecuteDeleteClientCommand(object parameter)
        {
            if (true)
            {

            }
            return true;
        }
        #endregion

        private void UserRegistrationChecks()
        {
            var user = RegisteredUser;

            if (user == null)
            {
                Status = Localization.StringLibrary[13];
            }
            else
            {
                Status = Localization.StringLibrary[13] + $"{RegisteredUser?.Name}";
            }
        }


    }


}

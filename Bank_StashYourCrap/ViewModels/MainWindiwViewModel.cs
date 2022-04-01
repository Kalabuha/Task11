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
using Bank_StashYourCrap.Bank.Data;
using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Bank_StashYourCrap.Bank.PeopleModels.Clients;
using Bank_StashYourCrap.Commands;

namespace Bank_StashYourCrap.ViewModels
{
    internal class MainWindiwViewModel : BaseViewModel
    {
        private readonly AppData _appData;

        public Localization Localization { get; }

        public MainWindiwViewModel()
        {
            _appData = new AppData();
            Localization = new RusLang();

            Clients = _appData.GetAllClients();

            Title = Localization.StringLibrary[0];
            UserRegistrationChecks();

            СonstructAllCommands();
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
        private string _status;
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
        #endregion

        #region Свойство пользователь, который вошёл в систему
        private Employee _registeredUser = null!;
        public Employee RegisteredUser
        {
            get => _registeredUser;
            set => Set(ref _registeredUser, value);
        }
        #endregion

        #region Свойство выбранный клиент из списка всех клиентов
        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set => Set(ref _selectedClient, value);
        }
        #endregion

        #region Свойство коллекция всех клиентов
        public ObservableCollection<Client>? Clients { get; set; }
        #endregion


        #region Команда создать нового клиента
        public ICommand CrateNewClientCommand { get; private set; }

        private void OnExecuteCrateNewClientCommand(object parameter)
        {

        }

        private bool CanExecuteCrateNewClientCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region Команда изменить выбранного клиента
        public ICommand EditClientCommand { get; private set; }

        private void OnExecuteEditClientCommand(object parameter)
        {

        }

        private bool CanExecuteEditClientCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region Команда удалить выбранного клиента
        public ICommand DeleteClientCommand { get; private set; }

        private void OnExecuteDeleteClientCommand(object parameter)
        {

        }

        private bool CanExecuteDeleteClientCommand(object parameter)
        {
            return true;
        }
        #endregion

        private void UserRegistrationChecks()
        {
            var user = RegisteredUser;

            if (user == null)
            {
                Status = Localization.StringLibrary[12];
            }
            else
            {
                Status = Localization.StringLibrary[13] + $"{RegisteredUser.Name}";
            }
        }


    }


}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Bank_StashYourCrap.Commands;
using Bank_StashYourCrap.ViewModels.Base;
using Bank_StashYourCrap.Bank.Services;
using Bank_StashYourCrap.Bank.DataContext;
using Bank_StashYourCrap.Models;
using Bank_StashYourCrap.Localizations;
using Bank_StashYourCrap.Localizations.Base;
using Bank_StashYourCrap.Mappers;

namespace Bank_StashYourCrap.ViewModels
{
    internal class RegistrationEmployeeWindowViewModel : BaseViewModel
    {
        private readonly ServiceEmployeesData _employeesService;

        public AppLocalization Localization { get; private set; } = default!;

        public RegistrationEmployeeWindowViewModel()
        {
            var repository = new RepositoryPeopleData();
            _employeesService = new ServiceEmployeesData(repository);

            Localization = new RusLang();
            Employees = _employeesService.GetAllEmployees();

            ConfirmEmployeeCommand = new ActionCommand(
                execute: OnExecuteConfirmEmployeeCommand, can: CanExecuteConfirmEmployeeCommand);
        }

        #region Свойство коллекция всех работников
        public ObservableCollection<EmployeeModel>? Employees { get; private set; } = default!;
        #endregion

        #region Свойство пользователь, который хочет войти в систему
        private EmployeeModel? _selectedUser;
        public EmployeeModel? SelectedUser
        {
            get => _selectedUser;
            set => Set(ref _selectedUser, value);
        }
        #endregion

        #region Команда подтвердить выбор пользователя
        public ICommand ConfirmEmployeeCommand { get; private set; } = default!;

        private void OnExecuteConfirmEmployeeCommand(object parameter)
        {

        }

        private bool CanExecuteConfirmEmployeeCommand(object parameter)
        {
            if (SelectedUser == null)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}

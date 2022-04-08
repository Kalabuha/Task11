﻿using Bank_StashYourCrap.Bank.PeopleModels.Employees;
using Bank_StashYourCrap.Bank.Services;
using Bank_StashYourCrap.Commands;
using Bank_StashYourCrap.Localizations;
using Bank_StashYourCrap.Localizations.Base;
using Bank_StashYourCrap.Models;
using Bank_StashYourCrap.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Bank_StashYourCrap.ViewModels
{
    internal class RegistrationEmployeeWindowViewModel : BaseViewModel
    {
        private readonly ServiceEmployeesData _serviceEmployees;

        public RegistrationEmployeeWindowViewModel(ServiceEmployeesData serviceEmployees)
        {
            _serviceEmployees = serviceEmployees;

            Employees = _serviceEmployees.GetAllEmployees();

            ConfirmEmployeeCommand = new ActionCommand(
                execute: OnExecuteConfirmEmployeeCommand, can: CanExecuteConfirmEmployeeCommand);
        }

        #region Свойство локализация
        private AppLocalization _localization = default!;

        public AppLocalization Localization
        {
            get => _localization;
            set => Set(ref _localization, value);
        }
        #endregion

        #region Свойство коллекция всех работников
        public ObservableCollection<EmployeeModel>? Employees { get; private set; } = default!;
        #endregion

        #region Свойство выбранный из списка пользователь
        private EmployeeModel? _selectedUser;
        public EmployeeModel? SelectedUser
        {
            get => _selectedUser;
            set => Set(ref _selectedUser, value);
        }
        #endregion

        #region Свойство подтверждённый пользователь, который хочет войти в систему
        public Employee? ConfirmUser { get; set; }
        #endregion


        #region Команда подтвердить выбор пользователя
        public ICommand ConfirmEmployeeCommand { get; private set; } = default!;

        private void OnExecuteConfirmEmployeeCommand(object parameter)
        {
            ConfirmUser = _serviceEmployees.GetEmployee(SelectedUser!);
            var window = (Window)parameter;
            window.Close();
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

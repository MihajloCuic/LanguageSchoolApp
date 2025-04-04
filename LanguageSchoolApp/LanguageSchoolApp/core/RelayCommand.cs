﻿using System.Windows.Input;

namespace LanguageSchoolApp.core
{
    public class RelayCommand<T> : ICommand
    {
        private Action<T> _execute;
        private Func<T, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        { 
            add { CommandManager.RequerySuggested += value; }
            remove {  CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) 
        { 
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter) 
        { 
            _execute((T)parameter);
        }
    }
}

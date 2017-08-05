using System;
using System.Windows.Input;

namespace WPF.Utils
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        public void Execute(object parameter) => _execute(parameter);

        private readonly Func<object, bool> _canExecute;
        public bool CanExecute(object parameter) =>
            _canExecute == null || _canExecute(parameter);

        //Для команды без параметра
        public RelayCommand(Action execute, Func<object, bool> canExecute = null)
        {
            _execute = obj => execute();
            _canExecute = canExecute;
        }

        //Для команды с параметром
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}

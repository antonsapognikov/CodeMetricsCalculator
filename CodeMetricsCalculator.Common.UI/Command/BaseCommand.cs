using System;
using System.Windows.Input;

namespace CodeMetricsCalculator.Common.UI.Command
{
    public delegate bool BaseCommandOnCanExecute<in T>(T parameter);
    public delegate void BaseCommandOnExecute<in T>(T parameter);
    
    public class BaseCommand<T> : ICommand
    {
        private readonly BaseCommandOnCanExecute<T> _canExecute;
        private readonly BaseCommandOnExecute<T> _execute;

        public BaseCommand(BaseCommandOnExecute<T> execute) : this(parameter => true, execute)
        {          
        }

        public BaseCommand(BaseCommandOnCanExecute<T> canExecute, BaseCommandOnExecute<T> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(T parameter)
        {
            var handler = _canExecute;
            return handler != null && _canExecute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute((T) parameter);
        }

        public void Execute(T parameter)
        {
            var handler = _execute;
            if (handler != null)
            {
                handler(parameter);
            }
        }

        public void Execute(object parameter)
        {
            Execute((T) parameter);
        }

        public event EventHandler CanExecuteChanged = (sender, args) => { };
    }
}

using System;
using System.Windows;
using System.Windows.Input;
using CodeMetricsCalculator.Common.UI.Command;

namespace CodeMetricsCalculator.Common.UI.ViewModel.Base
{
    public abstract class UIViewModel : ObservableObject
    {
        protected Window View { get; private set; }
        
        public ICommand CloseCommand { get; set; }

        public event EventHandler Initialized = (sender, args) => { };

        internal void Initialize(Window view)
        {
            if (view == null)
                throw new ArgumentNullException("view");
            View = view;
            CloseCommand = CreateCommand<object>(parameter => View.Close());
            OnInitialize();
        }

        private void OnInitialize()
        {
            var handler = Initialized;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected ICommand CreateCommand<T>(BaseCommandOnExecute<T> defaultAction)
        {
            return new BaseCommand<T>(defaultAction);
        }
    }
}

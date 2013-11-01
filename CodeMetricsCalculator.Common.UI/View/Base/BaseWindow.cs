using System.Windows;
using CodeMetricsCalculator.Common.UI.ViewModel;
using CodeMetricsCalculator.Common.UI.ViewModel.Base;

namespace CodeMetricsCalculator.Common.UI.View.Base
{
    public abstract class BaseWindow<T> : Window where T : UIViewModel, new ()
    {
        public T ViewModel
        {
            get { return DataContext as T; }
            set { DataContext = value; }
        }

        protected BaseWindow()
        {
            Initialized += (sender, args) =>
            {
                var viewModel = new T();
                viewModel.Initialize(this);
                DataContext = viewModel;
            };
        }
    }
}

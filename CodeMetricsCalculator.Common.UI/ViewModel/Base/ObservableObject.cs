using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace CodeMetricsCalculator.Common.UI.ViewModel.Base
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

        protected void OnPropertyChanged(Expression<Func<object>> propertyExpression)
        {
            var body = propertyExpression.Body as MemberExpression ?? ((UnaryExpression) propertyExpression.Body).Operand as MemberExpression;
            if (body != null)
                OnPropertyChanged(body.Member.Name);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

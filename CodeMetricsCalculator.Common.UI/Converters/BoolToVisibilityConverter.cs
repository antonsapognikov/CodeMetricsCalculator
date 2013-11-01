using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CodeMetricsCalculator.Common.UI.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        private static BoolToVisibilityConverter _instance;
        private static BoolToVisibilityConverter _oppositeInstance;
        private static BoolToVisibilityConverter _instanceCollapsed;
        private static BoolToVisibilityConverter _oppositeInstanceCollapsed;

        public static BoolToVisibilityConverter Instance
        {
            get { return _instance ?? (_instance = new BoolToVisibilityConverter()); }
        }

        public static BoolToVisibilityConverter OppositeInstance
        {
            get { return _oppositeInstance ?? (_oppositeInstance = new BoolToVisibilityConverter(Visibility.Hidden, Visibility.Visible));}
        }

        public static BoolToVisibilityConverter InstanceCollapsed
        {
            get { return _instanceCollapsed ?? (_instanceCollapsed = new BoolToVisibilityConverter(Visibility.Visible, Visibility.Collapsed)); }
        }

        public static BoolToVisibilityConverter OppositeInstanceCollapsed
        {
            get { return _oppositeInstanceCollapsed ?? (_oppositeInstanceCollapsed = new BoolToVisibilityConverter(Visibility.Hidden, Visibility.Collapsed)); }
        }

        public Visibility OnTrue { get; set; }
        public Visibility OnFalse { get; set; }
        public Visibility OnNull { get; set; }

        public BoolToVisibilityConverter() : this(Visibility.Visible, Visibility.Hidden)
        {           
        }

        public BoolToVisibilityConverter(Visibility onTrue, Visibility onFalse, Visibility onNull = Visibility.Collapsed)
        {
            OnTrue = onTrue;
            OnFalse = onFalse;
            OnNull = onNull;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = value as bool?;
            return booleanValue == null ? OnNull : (booleanValue.Value ? OnTrue : OnFalse); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

using System.Collections.Generic;
using System.Windows.Input;
using CodeMetricsCalculator.Common.UI.ViewModel;
using CodeMetricsCalculator.Common.UI.ViewModel.Base;
using Microsoft.Win32;

namespace CodeMetricsCalculator.WpfApplication.ViewModel
{
    public class MainWindowViewModel : UIViewModel
    {
        private bool _isLogVisible;
        private bool _isResultVisible;
        private string _fileName;
        private string _log;
        private string _result; 
        private ICommand _openFileCommand;

        private const string JavaSourceFilter = "Java source files (*.java)|*.java|All files (*.*)|*.*";

        public bool IsLogVisible
        {
            get { return _isLogVisible; }
            set
            {
                _isLogVisible = value;
                OnPropertyChanged(() => IsLogVisible);
            }
        }

        public bool IsResultVisible
        {
            get { return _isResultVisible; }
            set
            {
                _isResultVisible = value;
                OnPropertyChanged(() => IsResultVisible);
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged(() => FileName);
            }
        }

        public string Log
        {
            get { return _log; }
            set
            {
                _log = value;
                OnPropertyChanged(() => Log);
            }
        }

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged(() => Result);
            }
        }

        public ICommand OpenFileCommand
        {
            get { return _openFileCommand ?? (_openFileCommand = CreateCommand<object>(parameter => OnCreateFile())); }
        }

        public MainWindowViewModel()
        {
            //Test values
            Log = "Exception: File not found!";
            IsLogVisible = true;
        }

        private void OnCreateFile()
        {
            var dialog = new OpenFileDialog {Filter = JavaSourceFilter, FileName = FileName};
            var result = dialog.ShowDialog(View);
            if (result.Value)
            {
                FileName = dialog.FileName;
            }
        }
    }
}

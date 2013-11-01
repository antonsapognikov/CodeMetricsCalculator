﻿using System.Windows.Input;
using CodeMetricsCalculator.Common.UI.ViewModel.Base;
using Microsoft.Win32;

namespace CodeMetricsCalculator.WpfApplication.ViewModel
{
    public class MainWindowViewModel : UIViewModel
    {
        private string _fileName;
        private string _log;
        private string _result; 
        private ICommand _openFileCommand;

        private const string JavaSourceFilter = "Java source files (*.java)|*.java|All files (*.*)|*.*";

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

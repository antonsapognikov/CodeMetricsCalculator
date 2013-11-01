using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CodeMetricsCalculator.Common.UI.ViewModel;
using CodeMetricsCalculator.Common.UI.ViewModel.Base;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;
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
        private IReadOnlyCollection<IClassInfo> _classes;
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
            Log = "Waiting for file...";
            IsLogVisible = true;
        }

        private void OnCreateFile()
        {
            var dialog = new OpenFileDialog {Filter = JavaSourceFilter, FileName = FileName};
            var result = dialog.ShowDialog(View);
            if (result.Value)
            {
                FileName = dialog.FileName;
                LoadAndParseFile();
            }
        }

        private async void LoadAndParseFile()
        {
            try
            {
                string source;
                Log = "Reading file...";
                using (var sr = new StreamReader(File.OpenRead(FileName)))
                {
                    source = await sr.ReadToEndAsync();
                }
                var code = new JavaCode(source);
                IsResultVisible = true;
                Log = "Parsing classes...";
                var parseClassesTask = new Task<IReadOnlyCollection<IClassInfo>>(() => new JavaClassParser().Parse(code));
                parseClassesTask.Start();
                _classes = await parseClassesTask;
                //Result = _classes.Select(@class => @class.Name).Aggregate((s, s1) => s + Environment.NewLine + s1);
                Log = string.Format("Parsed {0} classes.", _classes.Count);

                Log = "Parsing expressions...";
                var parseExpressionsTask = new Task<List<IExpressionInfo>>(() => _classes.Select(@class => @class.GetMethods())
                 .Aggregate(AggregateMethods)
                 .Select(info => info.GetBody())
                 .Select(info => info.GetExpressions())
                 .Aggregate(
                     AggregateExpressions).ToList());
                parseExpressionsTask.Start();
                var expressions = await parseExpressionsTask;
                Log = string.Format("Parsed {0} expressions.", expressions.Count);
                var sb = new StringBuilder();
                sb.AppendLine("All expressions: ");
                foreach (var expressionInfo in expressions)
                {
                    sb.AppendLine(expressionInfo.NormalizedSource);
                }
                Result = sb.ToString();
            }
            catch (Exception e)
            {
                Log = e.Message;
            }
        }

        private static IReadOnlyCollection<IExpressionInfo> AggregateExpressions(IReadOnlyCollection<IExpressionInfo> readOnlyCollection, IReadOnlyCollection<IExpressionInfo> expressionInfos)
        {
            var list = new List<IExpressionInfo>();
            list.AddRange(readOnlyCollection);
            list.AddRange(expressionInfos);
            return new ReadOnlyCollection<IExpressionInfo>(list);
        }

        private static IReadOnlyCollection<IMethodInfo> AggregateMethods(IReadOnlyCollection<IMethodInfo> readOnlyCollection,
            IReadOnlyCollection<IMethodInfo> onlyCollection)
        {
            var list = new List<IMethodInfo>();
            list.AddRange(readOnlyCollection);
            list.AddRange(onlyCollection);
            return new ReadOnlyCollection<IMethodInfo>(list);
        }
    }
}

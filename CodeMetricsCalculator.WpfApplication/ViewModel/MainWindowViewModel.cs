
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CodeMetricsCalculator.Common.UI.ViewModel.Base;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;
using Microsoft.Win32;

namespace CodeMetricsCalculator.WpfApplication.ViewModel
{
    public class MainWindowViewModel : UIViewModel
    {
        private string _fileName;
        private string _result;
        private StringBuilder _log;
        private IReadOnlyCollection<IClassInfo> _classes;
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

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged(() => Result);
            }
        }

        public StringBuilder Log
        {
            get { return _log; }
            set
            {
                _log = value;
                OnPropertyChanged(() => Log);
            }
        }

        public ICommand OpenFileCommand
        {
            get { return _openFileCommand ?? (_openFileCommand = CreateCommand<object>(parameter => OnCreateFile())); }
        }

        public MainWindowViewModel()
        {
            //Test values
            AppendLineToLog("Waiting for file...");
        }

        private void AppendLineToLog(string value)
        {
            if (_log == null)
                _log = new StringBuilder();
            _log.AppendLine(value ?? string.Empty);
            OnPropertyChanged(() => Log);
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
                AppendLineToLog("Reading file...");
                using (var sr = new StreamReader(File.OpenRead(FileName)))
                {
                    source = await sr.ReadToEndAsync();
                }
                var code = new JavaCode(source);
                AppendLineToLog("Parsing classes...");
                var parseClassesTask = new Task<IReadOnlyCollection<IClassInfo>>(() => new JavaClassParser().Parse(code));
                parseClassesTask.Start();
                _classes = await parseClassesTask;
                AppendLineToLog(string.Format("Parsed {0} classes.", _classes.Count));
            
                AppendLineToLog("Parsing methods...");
                var parseMethodsTask =
                    new Task<IReadOnlyCollection<IMethodInfo>>(() => _classes.Select(@class => @class.GetMethods())
                        .Aggregate(AggregateMethods));
                parseMethodsTask.Start();
                var methods = await parseMethodsTask;
                AppendLineToLog(string.Format("Parsed {0} methods.", methods.Count));

                AppendLineToLog("Parsing expressions...");
                var parseExpressionsTask = new Task<List<IExpressionInfo>>(() => methods
                    .Select(info => info.GetBody())
                    .Select(info => info.GetExpressions())
                    .Aggregate(AggregateExpressions).ToList());
                parseExpressionsTask.Start();
                var expressions = await parseExpressionsTask;
                AppendLineToLog(string.Format("Parsed {0} expressions.", expressions.Count));
                var sb = new StringBuilder();
                sb.AppendLine("All expressions: ");
                AppendLineToLog("Parsing operators...");
                foreach (var expressionInfo in expressions)
                {
                    sb.AppendLine(expressionInfo.NormalizedSource);
                    var parseOperatorsTask =
                        new Task<IReadOnlyDictionary<IOperatorInfo, int>>(() => expressionInfo.GetOperators());
                    parseOperatorsTask.Start();
                    var operators = await parseOperatorsTask;
                    sb.AppendLine("Operators:");
                    foreach (var op in operators)
                    {
                        sb.AppendLine(string.Format("{0} - {1} - {2}", op.Key.GetType().Name, op.Key.Name, op.Value));
                    }
                    sb.AppendLine("------------");
                }
                AppendLineToLog("Operators parsed.");
                Result = sb.ToString();
            }
            catch (Exception e)
            {
                AppendLineToLog(e.Message);
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

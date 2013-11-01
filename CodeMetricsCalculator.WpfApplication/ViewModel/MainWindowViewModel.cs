
﻿using System;
using System.Collections.Generic;
﻿using System.IO;
﻿using System.Text;
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
        private StringBuilder _result = new StringBuilder();
        private StringBuilder _log = new StringBuilder();
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

        public StringBuilder Result
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
            _log.AppendLine(string.Format("[{0}] - {1}", DateTime.Now, value ?? string.Empty));
            OnPropertyChanged(() => Log);
        }

        private void AppendLineToResult(string value)
        {
            _result.AppendLine(value ?? string.Empty);
            OnPropertyChanged(() => Result);
        }

        private void ClearResults()
        {
            _result.Clear();
        }

        private void OnCreateFile()
        {
            var dialog = new OpenFileDialog {Filter = JavaSourceFilter, FileName = FileName};
            var result = dialog.ShowDialog(View);
            if (result.Value)
            {
                FileName = dialog.FileName;
                Task.Run(() => LoadAndParseFile());
                //LoadAndParseFileAsync();
            }
        }

        private void LoadAndParseFile()
        {
            try
            {
                ClearResults();
                string source;
                AppendLineToLog("Reading file...");
                using (var sr = new StreamReader(File.OpenRead(FileName)))
                {
                    source =  sr.ReadToEnd();
                }
                var code = new JavaCode(source);
                AppendLineToLog("Parsing classes...");
                //var parseClassesTask = new Task<IReadOnlyCollection<IClassInfo>>(() => new JavaClassParser().Parse(code));
                //parseClassesTask.Start();
                _classes = new JavaClassParser().Parse(code);
                AppendLineToLog(string.Format("Parsed {0} classes.", _classes.Count));

                AppendLineToLog("Parsing...");
                foreach (var classInfo in _classes)
                {
                    AppendLineToResult(string.Format("####### class {0} #######", classInfo.Name));
                    //var parseMethodsTask = new Task<IReadOnlyCollection<IMethodInfo>>(classInfo.GetMethods);
                    //parseMethodsTask.Start();
                    var methods = classInfo.GetMethods();
                    AppendLineToResult("********* methods  *********");
                    foreach (var methodInfo in methods)
                    {
                        AppendLineToResult(string.Format("$$$$$$$$$ method {0} $$$$$$$$$", methodInfo.Name));
                        IMethodInfo mInfo = methodInfo;
                        //var parseExpressionsTask =
                        //    new Task<IReadOnlyCollection<IExpressionInfo>>(() => mInfo.GetBody().GetExpressions());
                        //parseExpressionsTask.Start();
                        var expressions = mInfo.GetBody().GetExpressions();
                        AppendLineToResult("Expressions: ");
                        foreach (var expressionInfo in expressions)
                        {
                            AppendLineToResult(expressionInfo.NormalizedSource);
                            //var parseOperatorsTask =
                            //    new Task<IReadOnlyDictionary<IOperatorInfo, int>>(expressionInfo.GetOperators);
                            //parseOperatorsTask.Start();
                            var operators = expressionInfo.GetOperators();
                            AppendLineToResult("Operators:");
                            foreach (var op in operators)
                            {
                                AppendLineToResult(string.Format("{0} - {1} - {2}", op.Key.GetType().Name, op.Key.Name,
                                    op.Value));
                            }
                            AppendLineToResult("------------");
                        }
                    }
                }
                AppendLineToLog("Parsing finished.");
            }
            catch (Exception e)
            {
                AppendLineToLog(e.Message);
            }
        }
    }
}

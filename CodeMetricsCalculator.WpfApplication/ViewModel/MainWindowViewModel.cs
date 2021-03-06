using System;
using System.Collections.Generic;
﻿using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CodeMetricsCalculator.Common;
using CodeMetricsCalculator.Common.UI.ViewModel.Base;
﻿using CodeMetricsCalculator.Metrics;
﻿using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;
using CodeMetricsCalculator.Parsers.Pascal;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;
using Microsoft.Win32;

namespace CodeMetricsCalculator.WpfApplication.ViewModel
{
    public class MainWindowViewModel : UIViewModel
    {
        private MetricType _metricType = MetricType.Holstead;
        private Language _language = Language.Pascal;
        private string _fileName;
        private StringBuilder _result = new StringBuilder();
        private StringBuilder _log = new StringBuilder();
        private IReadOnlyCollection<IClassInfo> _classes;
        private ICommand _openFileCommand;
        private ICommand _evaluateChepinCommand;
        private ICommand _evaluateHolsteadCommand;
        private ICommand _evaluateSpenCommand;
        private ICommand _evaluatePascalCommand;
        private ICommand _evaluateJavaCommand;

        private const string JavaSourceFilter = "All files (*.*)|*.*";

        public MetricType CurrentMetricType
        {
            get { return _metricType; }
            set
            {
                _metricType = value;
                OnPropertyChanged(() => CurrentMetricType);
            }
        }

        public Language CurrentLanguage
        {
            get { return _language; }
            set
            {
                _language = value;
                OnPropertyChanged(() => CurrentLanguage);
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

        public ICommand EvaluateChepinCommand
        {
            get { return _evaluateChepinCommand ?? (_evaluateChepinCommand = CreateCommand<object>(parameter => OnEvaluateMetric(MetricType.Chepin))); }
        }

        public ICommand EvaluateHolsteadCommand
        {
            get { return _evaluateHolsteadCommand ?? (_evaluateHolsteadCommand = CreateCommand<object>(parameter => OnEvaluateMetric(MetricType.Holstead))); }
        }

        public ICommand EvaluateSpenCommand
        {
            get { return _evaluateSpenCommand ?? (_evaluateSpenCommand = CreateCommand<object>(parameter => OnEvaluateMetric(MetricType.Spen))); }
        }

        public ICommand EvaluatePascalCommand
        {
            get { return _evaluatePascalCommand ?? (_evaluatePascalCommand = CreateCommand<object>(parameter => OnEvaluateLanguage(Language.Pascal))); }
        }

        public ICommand EvaluateJavaCommand
        {
            get { return _evaluateJavaCommand ?? (_evaluateJavaCommand = CreateCommand<object>(parameter => OnEvaluateLanguage(Language.Java))); }
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
            OnPropertyChanged(() => Result);
        }

        private void OnCreateFile()
        {
            var dialog = new OpenFileDialog {Filter = JavaSourceFilter, FileName = FileName};
            var result = dialog.ShowDialog(View);
            if (result.Value)
            {
                FileName = dialog.FileName;
                LoadAndParseFileAsync();
            }
        }

        private void OnEvaluateMetric(MetricType type)
        {
            CurrentMetricType = type;
            LoadAndParseFileAsync();
        }

        private void OnEvaluateLanguage(Language language)
        {
            CurrentLanguage = language;
            LoadAndParseFileAsync();
        }

        private async void LoadAndParseFileAsync()
        {
            try
            {
                ClearResults();
                string source;
                AppendLineToLog("Reading file...");
                if (!File.Exists(FileName))
                {
                    AppendLineToResult("Файл не найден!");
                    return;
                }
                using (var sr = new StreamReader(File.OpenRead(FileName)))
                {
                    source =  sr.ReadToEnd();
                }
                switch (CurrentLanguage)
                {
                    case Language.Pascal :
                        var pascalCode = new PascalCode(source);
                        AppendLineToLog("Parsing classes...");
                        _classes = new PascalClassParser().Parse(pascalCode);
                        break;
                    case Language.Java :
                        var javaCode = new JavaCode(source);
                        AppendLineToLog("Parsing classes...");
                        _classes = new JavaClassParser().Parse(javaCode);
                        break;
                }             
                AppendLineToLog(string.Format("Parsed {0} classes.", _classes.Count));
                AppendLineToLog("Parsing...");
                foreach (var classInfo in _classes)
                {
                    IClassInfo info = classInfo;
                    var parseClassTask = new Task(() => ParseClass(info));
                    parseClassTask.Start();
                    await parseClassTask;
                }
                AppendLineToLog("Parsing finished.");
            }
            catch (Exception e)
            {
                ClearResults();
                AppendLineToLog(e.Message);
            }
        }

        private void ParseClass(IClassInfo classInfo)
        {
            AppendLineToResult(string.Format("####### класс {0} #######", classInfo.Name));
            var fields = classInfo.GetFields();
            AppendLineToResult("-------------------------------------------------");
           /* AppendLineToResult("********* поля  *********");
            foreach (var field in fields)
            {
                AppendLineToResult(string.Format("€€€€€€€€€ поле {0} €€€€€€€€€", field.Name));
            }*/
            var methods = classInfo.GetMethods();
            AppendLineToResult("-------------------------------------------------");
            AppendLineToResult("********* методы  *********");
            foreach (var methodInfo in methods)
            {
                AppendLineToResult(string.Format("€€€€€€€€€ метод {0} €€€€€€€€€", methodInfo.Name));
            }
            AppendLineToResult("-------------------------------------------------");
            AppendLineToResult("********* информация *********");
            var dictionary = classInfo.GetClassDictionary();
            AppendLineToResult(string.Format("Количество идентификаторов - {0}", classInfo.GetIdentifiers().Count));
            AppendLineToResult(string.Format("Количество операторов - {0}", dictionary.Operators.Count));
            AppendLineToResult(string.Format("Количество операндов - {0}", dictionary.Operands.Count));
            AppendLineToResult(string.Format("Количество неуникальных операторов - {0}", dictionary.Operators.Values.Sum()));
            AppendLineToResult(string.Format("Количество неуникальных операндов - {0}", dictionary.Operands.Values.Sum()));
            AppendLineToResult("-------------------------------------------------");
            switch (CurrentMetricType)
            {
                case MetricType.Chepin :
                    AppendLineToResult("€€€€€€€€€ Метрика Чепина €€€€€€€€€");
                    AppendLineToResult(string.Format("Информационная прочность - {0}", ChepinMetricCalculator.Calculate(classInfo)));
                    break;
                case MetricType.Holstead :
                    AppendLineToResult("€€€€€€€€€ Метрика Холстеда €€€€€€€€€");
                    var holstead = new HolsteadMetricCalculator(classInfo);
                    AppendLineToResult(string.Format("Словарь программы - {0}", holstead.CalculateProgramDictionary()));
                    AppendLineToResult(string.Format("Объем программы - {0}", holstead.CalculateProgramVolume()));
                    AppendLineToResult(string.Format("Длина программы - {0}", holstead.CalculateProgramLength()));                   
                    AppendLineToResult(string.Format("Потенциальный объем программы - {0}", holstead.CalculateTheoreticalProgramVolume()));
                    AppendLineToResult(string.Format("Теоретическая длина программы - {0}", holstead.CalculateTheoreticalProgramLength()));
                    AppendLineToResult(string.Format("Уровень качества программы - {0}", holstead.CalculateProgramLevel()));
                    AppendLineToResult(string.Format("Параметры реальной программы - {0}", holstead.CalculateRealProgramParameters()));
                    AppendLineToResult(string.Format("Требуемый элементарные решения - {0}", holstead.CalculateRequiredElementarySolutions()));
                    AppendLineToResult(string.Format("Интеллектуальное содержание алгоритма - {0}", holstead.CalculateIntelligenceContent()));
                    break;
                case MetricType.Spen :
                    AppendLineToResult("€€€€€€€€€ Подсчет спена €€€€€€€€€");
                    foreach (var identifier in SpenMetricCalculator.Calculate(classInfo))
                    {
                        AppendLineToResult(string.Format("Идентификатор - {0}. Спен - {1}", identifier.Key.Name, identifier.Value));
                    }
                    break;
            }
        }
    }
}

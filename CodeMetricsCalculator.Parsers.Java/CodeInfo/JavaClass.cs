﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    public class JavaClass : JavaCode, IClassInfo
    {
        private readonly string _name;
        private IReadOnlyCollection<IMethodInfo> _methods;

        public JavaClass(string name, string originalSource) : base(originalSource)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public IReadOnlyCollection<IFieldInfo> GetFields()
        {
            var parsingResults = new JavaFieldParser().Parse(this);
            return parsingResults;
        }

        public IReadOnlyCollection<IMethodInfo> GetMethods()
        {
            return _methods ?? (_methods = new JavaMethodParser().Parse(this));
        }

        public CodeDictionary GetClassDictionary()
        {
            var methodDictionaries = GetMethods().Select(info => info.GetMethodDictionary()).ToList();
            var methodOperators = methodDictionaries.SelectMany(dictionary => dictionary.Operators);
            var methodOperands = methodDictionaries.SelectMany(dictionary => dictionary.Operands);
            var operators = new Dictionary<string, int>();
            foreach (var methodOperator in methodOperators)
            {
                if (operators.ContainsKey(methodOperator.Key))
                    operators[methodOperator.Key] += methodOperator.Value;
                else
                    operators.Add(methodOperator.Key, methodOperator.Value);
            }
            var operands = new Dictionary<string, int>();
            foreach (var methodOperand in methodOperands)
            {
                if (operands.ContainsKey(methodOperand.Key))
                    operands[methodOperand.Key] += methodOperand.Value;
                else
                    operands.Add(methodOperand.Key, methodOperand.Value);
            }
            return new CodeDictionary(operators, operands);
        }

        public IReadOnlyDictionary<IIdentifierInfo, int> GetIdentifiers()
        {
            var parsingResults = new JavaIdentifiersInClassParser().Parse(this);
            var identifiers = new Dictionary<IIdentifierInfo, int>();
            foreach (var parsingResult in parsingResults)
            {
                identifiers.Add(parsingResult.Key, parsingResult.Value);
            }
            foreach (var methodInfo in GetMethods())
            {
                foreach (var keyValuePair in methodInfo.GetVariables())
                {
                    identifiers.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            return identifiers;
        }
    }
}

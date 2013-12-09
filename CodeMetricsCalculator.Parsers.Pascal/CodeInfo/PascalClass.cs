using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;
using IFieldInfo = CodeMetricsCalculator.Parsers.CodeInfo.IFieldInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.CodeInfo
{
    public class PascalClass : PascalCode, IClassInfo
    {
        private readonly string _name;
        private IReadOnlyCollection<IMethodInfo> _methods;
        private readonly string _declaration;
        private readonly string _implementation;

        public PascalClass(string name, string declaration, string implementation)
            : base(declaration + Environment.NewLine + implementation)
        {
            Contract.Requires(name != null);
            Contract.Requires(declaration != null);
            Contract.Requires(implementation != null);

            _name = name;
            _declaration = declaration;
            _implementation = implementation;
        }

        public string Name
        {
            get { return _name; }
        }
        
        public string Declaration {
            get { return _declaration; }
        }

        public string Implementation {
            get { return _implementation; }
        }

        private IReadOnlyCollection<IFieldInfo> _fields;
        public IReadOnlyCollection<IFieldInfo> GetFields()
        {
            return _fields ?? (_fields = new PascalFieldParser().Parse(this));
        }

        public IReadOnlyCollection<IMethodInfo> GetMethods()
        {
            return _methods ?? (_methods = new PascalMethodParser().Parse(this));
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

        private IReadOnlyDictionary<IIdentifierInfo, int> _identifiers;
        public IReadOnlyDictionary<IIdentifierInfo, int> GetIdentifiers()
        {
            if (_identifiers != null)
                return _identifiers;
            var parsingResults = new PascalIdentifiersInClassParser().Parse(this);
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
            _identifiers = identifiers;
            return _identifiers;
        }
    }
}

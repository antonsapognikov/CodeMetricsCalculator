﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.CodeInfo
{
    internal class PascalMethod : PascalIdentifier, IMethodInfo
    {
        private readonly IClassInfo _class;
        private readonly IReadOnlyCollection<IMethodParameterInfo> _parameters;
        private readonly ITypeInfo _returnType;

        public PascalMethod(ITypeInfo returnType, string name, IEnumerable<IMethodParameterInfo> parameters, string originalSource,
            IClassInfo @class)
            : base(name, originalSource)
        {
            if (returnType == null)
                throw new ArgumentNullException("returnType");
            if (@class == null)
                throw new ArgumentNullException("class");
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            _returnType = returnType;
            _parameters = parameters.ToList().AsReadOnly();
            _class = @class;
        }

        public IClassInfo Class
        {
            get { return _class; }
        }

        public IReadOnlyCollection<IMethodParameterInfo> Parameters
        {
            get { return _parameters; }
        }

        public ITypeInfo ReturnType
        {
            get { return _returnType; }
        }

        public IMethodBodyInfo GetBody()
        {
            var parsingResults = new PascalMethodBodyParser().Parse(this);
            return parsingResults;
        }

        public IReadOnlyCollection<IInputVariable> GetInputVariables()
        {
            var parsingResult = new PascalInputVariableParser().Parse(this);
            return parsingResult;
        }

        private IReadOnlyDictionary<IVariableInfo, int> _variables;

        public IReadOnlyDictionary<IVariableInfo, int> GetVariables()
        {
            if (_variables != null)
                return _variables;
            var parsingResult = new PascalVariablesParser().Parse(this);
            _variables = parsingResult.ToDictionary<KeyValuePair<PascalVariable, int>, IVariableInfo, int>(
                    keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
            return _variables;
        }

        private CodeDictionary _codeDictionary;

        public CodeDictionary GetMethodDictionary()
        {
            if (_codeDictionary != null)
                return _codeDictionary;
            _codeDictionary = new PascalCodeDictionaryParser().Parse(this);
            return _codeDictionary;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    internal class JavaMethod : JavaIdentifier, IMethodInfo
    {
        private readonly IClassInfo _class;
        private readonly IReadOnlyCollection<IMethodParameterInfo> _parameters;
        private readonly ITypeInfo _returnType;

        public JavaMethod(ITypeInfo returnType, string name, IEnumerable<IMethodParameterInfo> parameters, string originalSource,
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
            var parsingResults = new JavaMethodBodyParser().Parse(this);
            return parsingResults;
        }

        public IReadOnlyCollection<IInputVariable> GetInputVariables()
        {
            var parsingResult = new JavaInputVariableParser().Parse(this);
            return parsingResult;
        }

        public IReadOnlyDictionary<IVariableInfo, int> GetVariables()
        {
            var parsingResult = new JavaVariablesParser().Parse(this);
            return
                parsingResult.ToDictionary<KeyValuePair<JavaVariable, int>, IVariableInfo, int>(
                    keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
        }

        public CodeDictionary GetMethodDictionary()
        {
            var codeDictionary = new JavaCodeDictionaryParser().Parse(this);
            return codeDictionary;
        }
    }
}

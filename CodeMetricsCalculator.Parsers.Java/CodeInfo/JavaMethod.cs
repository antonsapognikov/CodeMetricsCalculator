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

        public JavaMethod(string name, IEnumerable<IMethodParameterInfo> parameters, string originalSource,
            IClassInfo @class)
            : base(name, originalSource)
        {
            if (@class == null)
                throw new ArgumentNullException("class");
            if (parameters == null)
                throw new ArgumentNullException("parameters");

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

        public IReadOnlyDictionary<IIdentifierInfo, int> GetIdentifiers()
        {
            var parsingResult = new JavaIdentifiersInMethodParser().Parse(this);
            return
                parsingResult.ToDictionary<KeyValuePair<JavaIdentifier, int>, IIdentifierInfo, int>(
                    keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
        }

        public CodeDictionary GetMethodDictionary()
        {
            var codeDictionary = new JavaCodeDictionaryParser().Parse(this);
            return codeDictionary;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    internal class JavaMethod : JavaCode, IMethodInfo
    {
        private readonly IClassInfo _class;
        private readonly string _name;
        private readonly IReadOnlyCollection<IMethodParameterInfo> _parameters;

        public JavaMethod(string name, IEnumerable<IMethodParameterInfo> parameters, string originalSource,
            IClassInfo @class) : base(originalSource)
        {
            if (@class == null)
                throw new ArgumentNullException("class");
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (name == null)
                throw new ArgumentNullException("name");

            _name = name;
            _parameters = parameters.ToList().AsReadOnly();
            _class = @class;
        }

        public IClassInfo Class
        {
            get { return _class; }
        }

        public string Name
        {
            get { return _name; }
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

        public CodeDictionary GetMethodDictionary()
        {
            var codeDictionary = new JavaCodeDictionaryParser().Parse(this);
            return codeDictionary;
        }
    }
}

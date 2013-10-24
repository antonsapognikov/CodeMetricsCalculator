using System;
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
            var parsingResults = new JavaMethodParser().Parse(this);
            return parsingResults;
        }
    }
}

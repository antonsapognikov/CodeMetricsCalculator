using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    class JavaMethod : JavaCode, IMethodInfo
    {
        private readonly IClassInfo _class;
        private readonly string _name;

        public JavaMethod(string name, IClassInfo @class, string originalSource) : base(originalSource)
        {
            if (@class == null)
                throw new ArgumentNullException("class");
            if (name == null)
                throw new ArgumentNullException("name");

            _class = @class;
            _name = name;
        }

        public IClassInfo Class
        {
            get { return _class; }
        }

        public string Name
        {
            get { return _name; }
        }

        public IMethodBodyInfo GetBody()
        {
            var parsingResults = new JavaMethodBodyParser().Parse(this);
            return parsingResults;
        }
    }
}

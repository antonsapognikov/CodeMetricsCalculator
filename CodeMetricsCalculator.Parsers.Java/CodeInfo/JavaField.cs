using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    internal class JavaField : JavaVariable, IFieldInfo
    {
        private readonly IClassInfo _class;

        public JavaField(string name, IClassInfo @class, string originalSource) : base(name, originalSource)
        {
            if (@class == null)
                throw new ArgumentNullException("class");

            _class = @class;
        }

        public IClassInfo Class
        {
            get { return _class; }
        }
    }
}

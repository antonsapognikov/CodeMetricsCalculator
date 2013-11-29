using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    internal class JavaVariable : JavaIdentifier, IVariableInfo
    {
        private readonly JavaType _type;

        public JavaVariable(JavaType type, string name, string originalSource)
            : base(name, originalSource)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            _type = type;
        }

        public ITypeInfo Type
        {
            get { return _type; }
        }
    }
}

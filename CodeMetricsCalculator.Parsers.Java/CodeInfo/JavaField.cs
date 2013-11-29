using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    internal class JavaField : JavaIdentifier, IFieldInfo
    {
        private readonly JavaType _type;
        private readonly IClassInfo _class;

        public JavaField(JavaType type, string name, IClassInfo @class, string originalSource) : base(name, originalSource)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (@class == null)
                throw new ArgumentNullException("class");

            _type = type;
            _class = @class;
        }

        public IClassInfo Class
        {
            get { return _class; }
        }

        public ITypeInfo Type
        {
            get { return _type; }
        }
    }
}

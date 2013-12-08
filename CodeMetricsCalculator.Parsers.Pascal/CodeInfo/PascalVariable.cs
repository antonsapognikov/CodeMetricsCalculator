using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.CodeInfo
{
    internal class PascalVariable : PascalIdentifier, IVariableInfo
    {
        private readonly PascalType _type;

        public PascalVariable(PascalType type, string name, string originalSource)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.CodeInfo
{
    internal class PascalMethodParameter : PascalVariable, IMethodParameterInfo
    {
        public PascalMethodParameter(PascalType type, string name, string originalSource)
            : base(type, name, originalSource)
        {
        }
    }
}

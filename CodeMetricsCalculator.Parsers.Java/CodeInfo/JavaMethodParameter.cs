using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    internal class JavaMethodParameter : JavaIdentifier, IMethodParameterInfo
    {
        public JavaMethodParameter(string name, string originalSource) : base(name, originalSource)
        {
        }
    }
}

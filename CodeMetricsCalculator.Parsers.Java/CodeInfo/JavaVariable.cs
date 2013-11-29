using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    internal class JavaVariable : JavaIdentifier
    {
        public JavaVariable(string name, string originalSource)
            : base(name, originalSource)
        {
        }
    }
}

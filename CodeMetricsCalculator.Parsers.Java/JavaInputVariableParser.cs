using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    class JavaInputVariableParser : JavaCodeParser<JavaMethod, IReadOnlyCollection<JavaInputVariable>>
    {
        public override IReadOnlyCollection<JavaInputVariable> Parse(JavaMethod code)
        {
            throw new NotImplementedException();
        }
    }
}

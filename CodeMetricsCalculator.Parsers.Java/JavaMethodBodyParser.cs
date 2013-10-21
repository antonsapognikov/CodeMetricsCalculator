using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    class JavaMethodBodyParser : JavaCodeParser<JavaMethod, JavaMethodBody>, IMethodBodyParser<JavaMethod, JavaMethodBody>
    {
        public override JavaMethodBody Parse(JavaMethod code)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaMethodParser : JavaCodeParser<JavaClass, IReadOnlyCollection<JavaMethod>>,
                                      IMethodParser<JavaClass, JavaMethod>
    {
        public override IReadOnlyCollection<JavaMethod> Parse(JavaClass code)
        {
            throw new NotImplementedException();
        }
    }
}

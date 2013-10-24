using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaFieldParser : JavaCodeParser<JavaClass, IReadOnlyCollection<JavaField>>,
                                     IFieldParser<JavaClass, JavaField>
    {
        public override IReadOnlyCollection<JavaField> Parse(JavaClass code)
        {
            throw new NotImplementedException();
        }
    }
}

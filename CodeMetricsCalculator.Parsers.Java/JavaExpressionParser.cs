using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaExpressionParser : JavaCodeParser<JavaMethodBody, IReadOnlyCollection<JavaExpression>>,
                                          IExpressionParser<JavaMethodBody, JavaExpression>
    {
        public override IReadOnlyCollection<JavaExpression> Parse(JavaMethodBody code)
        {
            throw new NotImplementedException();
        }
    }
}

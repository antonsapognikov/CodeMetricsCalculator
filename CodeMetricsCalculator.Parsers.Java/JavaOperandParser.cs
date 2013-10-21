using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    public class JavaOperandParser : JavaCodeParser<JavaExpression, OperandParsingResult<JavaOperand>>,
                                     IOperandParser<JavaExpression, JavaOperand>
    {
        public override OperandParsingResult<JavaOperand> Parse(JavaExpression code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            throw new NotImplementedException();
        }
    }
}
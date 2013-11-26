using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    public class JavaExpression : JavaCode, IExpressionInfo
    {
        public JavaExpression(string originalSource) : base(originalSource)
        {
        }

        public IReadOnlyDictionary<IOperatorInfo, int> GetOperators()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<IOperandInfo, int> GetOperands()
        {
            throw new NotImplementedException();
        }
    }
}

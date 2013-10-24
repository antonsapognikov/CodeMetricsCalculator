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
            var parsingResults = new JavaOperatorParser().Parse(this);
            var operators =
                parsingResults.ToDictionary<KeyValuePair<JavaOperator, int>, IOperatorInfo, int>(
                    parsingResult => parsingResult.Key, parsingResult => parsingResult.Value);
            return new ReadOnlyDictionary<IOperatorInfo, int>(operators);
        }

        public IReadOnlyDictionary<IOperandInfo, int> GetOperands()
        {
            var parsingResults = new JavaOperandParser().Parse(this);
            var operands =
                parsingResults.ToDictionary<KeyValuePair<JavaOperand, int>, IOperandInfo, int>(
                    parsingResult => parsingResult.Key, parsingResult => parsingResult.Value);
            return new ReadOnlyDictionary<IOperandInfo, int>(operands);
        }
    }
}

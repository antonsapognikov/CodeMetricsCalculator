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

        public IReadOnlyCollection<Tuple<IOperatorInfo, int>> GetOperators()
        {
            //todo: возможно кэшировать результаты; не создавать парсер явно, а получать его откуда-то
            var parsingResults = new JavaOperatorParser().Parse(this);
            var operators = parsingResults.Select(pair => new Tuple<IOperatorInfo, int>(pair.Key, pair.Value)).ToList();
            return new ReadOnlyCollection<Tuple<IOperatorInfo, int>>(operators);
        }

        public IReadOnlyCollection<Tuple<IOperandInfo, int>> GetOperands()
        {
            var parsingResults = new JavaOperandParser().Parse(this);
            var operands = parsingResults.Select(pair => new Tuple<IOperandInfo, int>(pair.Key, pair.Value)).ToList();
            return new ReadOnlyCollection<Tuple<IOperandInfo, int>>(operands);
        }
    }
}

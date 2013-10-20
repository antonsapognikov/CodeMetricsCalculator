using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    //todo: придумать что-то лучше Typle
    public interface IExpressionInfo : ICodeInfo
    {
        IReadOnlyCollection<Tuple<IOperatorInfo, int>> GetOperators();

        IReadOnlyCollection<Tuple<IOperandInfo, int>> GetOperands();
    }
}

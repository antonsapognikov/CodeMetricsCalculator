using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    /// <summary>
    ///     The number of repetitions of the given operator.
    /// </summary>
    public class OperatorParsingResult<TOperatorInfo> : Dictionary<TOperatorInfo, int>
        where TOperatorInfo : IOperatorInfo
    {
    }
}

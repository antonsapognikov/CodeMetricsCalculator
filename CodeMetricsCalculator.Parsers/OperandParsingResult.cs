using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    /// <summary>
    ///     The number of repetitions of the given operand.
    /// </summary>
    public class OperandParsingResult<TOperandInfo> : 
        Dictionary<TOperandInfo, int> 
        where TOperandInfo : IOperandInfo
    {
    }
}

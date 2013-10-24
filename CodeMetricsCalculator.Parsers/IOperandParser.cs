using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    public interface IOperandParser<in TParsingCode, TOperandInfo> :
        ICodeParser<TParsingCode, OperandParsingResult<TOperandInfo>>
        where TParsingCode : IMemberInfo
        where TOperandInfo : IOperandInfo
    {
    }
}

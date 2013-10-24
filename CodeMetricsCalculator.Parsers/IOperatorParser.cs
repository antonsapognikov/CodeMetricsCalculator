using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    public interface IOperatorParser<in TParsingCode, TOperatorInfo> :
        ICodeParser<TParsingCode, OperatorParsingResult<TOperatorInfo>>
        where TParsingCode : IMemberInfo
        where TOperatorInfo : IOperatorInfo
    {
    }
}

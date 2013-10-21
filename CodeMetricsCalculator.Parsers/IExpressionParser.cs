using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    public interface IExpressionParser<in TParsingCode, out TExpressionInfo> :
        ICodeParser<TParsingCode, IReadOnlyCollection<TExpressionInfo>>
        where TParsingCode : IMemberInfo
        where TExpressionInfo : IExpressionInfo
    {
    }
}

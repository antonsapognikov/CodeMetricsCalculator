using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    public interface IMethodBodyParser<in TParsingCode, out TMethodBodyInfo> :
        ICodeParser<TParsingCode, TMethodBodyInfo>
        where TParsingCode : IMemberInfo
        where TMethodBodyInfo : IMethodBodyInfo
    {
    }
}

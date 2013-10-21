using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    public interface IMethodParser<in TParsingCode, out TMethodInfo> :
        ICodeParser<TParsingCode, IReadOnlyCollection<TMethodInfo>>
        where TParsingCode : IMemberInfo
        where TMethodInfo : IMethodInfo
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    public interface IClassParser<in TParsingCode, out TClassInfo> :
        ICodeParser<TParsingCode, IReadOnlyCollection<TClassInfo>>
        where TParsingCode : IMemberInfo
        where TClassInfo : IClassInfo
    {
    }
}

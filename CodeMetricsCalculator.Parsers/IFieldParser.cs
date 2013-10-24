using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    public interface IFieldParser<in TParsingCode, out TFieldInfo> :
        ICodeParser<TParsingCode, IReadOnlyCollection<TFieldInfo>>
        where TParsingCode : IMemberInfo
        where TFieldInfo : IFieldInfo
    {
    }
}

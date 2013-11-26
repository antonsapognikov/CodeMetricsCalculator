using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    public interface ICodeDictionaryParser<in TMethodInfo> : ICodeParser<TMethodInfo, CodeDictionary>
        where TMethodInfo : IMethodInfo
    {
    }
}

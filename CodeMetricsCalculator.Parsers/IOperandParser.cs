using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    public interface IOperandParser<in TCode> : ICodeParser<TCode, OperandParsingResult> where TCode : IMemberInfo
    {
    }
}

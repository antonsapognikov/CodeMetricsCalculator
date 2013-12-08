using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Exceptions;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal
{
    internal class PascalMethodBodyParser : PascalCodeParser<PascalMethod, PascalMethodBody>,
        IMethodBodyParser<PascalMethod, PascalMethodBody>
    {
        public override PascalMethodBody Parse(PascalMethod code)
        {
            if (code == null)
                throw new NullReferenceException("code");

            var methodSource = code.NormalizedSource;
            var methodBodySource = ParseMethodBodySource(methodSource);
            return new PascalMethodBody(code, methodBodySource);
        }

        private string ParseMethodBodySource(string sources)
        {
            var methodBodyBeginIndex = sources.IndexOf("begin", StringComparison.OrdinalIgnoreCase);
            if (methodBodyBeginIndex == -1)
                throw new ParsingException("No opening bracket after method declaration.");
            var methodBodyClosingBracketIndex = FindClosingBracketIndex(sources, "begin", "end;",
                methodBodyBeginIndex);
            if (methodBodyClosingBracketIndex == -1)
                throw new ParsingException("No closing bracket for method.");
            var varIndex = sources.IndexOf("var", StringComparison.OrdinalIgnoreCase);
            var constIndex = sources.IndexOf("const", StringComparison.OrdinalIgnoreCase);
            if (varIndex != -1 && varIndex < methodBodyBeginIndex)
                methodBodyBeginIndex = varIndex;
            if (constIndex != -1 && constIndex < methodBodyBeginIndex)
                methodBodyBeginIndex = constIndex;
            var methodBodySource = sources.Substring(methodBodyBeginIndex,
                methodBodyClosingBracketIndex - methodBodyBeginIndex - 4);
            return methodBodySource;
        }
    }
}

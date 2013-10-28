using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Exceptions;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaMethodBodyParser : JavaCodeParser<JavaMethod, JavaMethodBody>,
        IMethodBodyParser<JavaMethod, JavaMethodBody>
    {
        public override JavaMethodBody Parse(JavaMethod code)
        {
            if (code == null)
                throw new NullReferenceException("code");

            var methodSource = code.NormalizedSource;
            var methodBodySource = ParseMethodBodySource(methodSource);
            return new JavaMethodBody(code, methodBodySource);
        }

        private string ParseMethodBodySource(string sources)
        {
            var methodBodyOpeningBracketIndex = sources.IndexOf('{');
            if (methodBodyOpeningBracketIndex == -1)
                throw new ParsingException("No opening bracket after method declaration.");
            var methodBodyClosingBracketIndex = FindClosingBracketIndex(sources, "{", "}", methodBodyOpeningBracketIndex);
            if (methodBodyClosingBracketIndex == -1)
                throw new ParsingException("No closing bracket for method.");
            var methodBodySource = sources.Substring(methodBodyOpeningBracketIndex + 1,
                methodBodyClosingBracketIndex - methodBodyOpeningBracketIndex - 1);
            return methodBodySource;
        }
    }
}

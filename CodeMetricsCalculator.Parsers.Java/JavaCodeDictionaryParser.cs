using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaCodeDictionaryParser : JavaCodeParser<JavaMethod, CodeDictionary>, ICodeDictionaryParser<JavaMethod>
    {
        public override CodeDictionary Parse(JavaMethod javaMethod)
        {
            Contract.Requires(javaMethod != null);

            var operators = JavaOperator.Operators;
            var keywordBasedOperators = operators.Where(@operator => @operator.IsKeywordBase).ToList();
            var notKeywordBasedOperators = operators.Where(@operator => !@operator.IsKeywordBase).ToList();

            return null;
        }
    }
}

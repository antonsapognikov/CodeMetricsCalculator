using System;

namespace CodeMetricsCalculator.Parsers.Java
{
    public class JavaOperatorParser : JavaCodeParser<OperatorParsingResult>
    {
        public override OperatorParsingResult Parse(JavaCode code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            var parsingResult = new OperatorParsingResult();
            string source = code.NormolizedSource;
            foreach (JavaOperator javaOperator in JavaOperator.Operators)
            {
                parsingResult.Add(javaOperator, javaOperator.ParsingRegex.Matches(source).Count);
            }

            return parsingResult;
        }
    }
}
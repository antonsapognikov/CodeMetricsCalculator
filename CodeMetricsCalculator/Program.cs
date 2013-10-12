using System;
using CodeMetricsCalculator.Parsers.Java;

namespace CodeMetricsCalculator
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var javaOperatorParser = new JavaOperatorParser();
            const string source = "for (int j = 0; !(j < buttons.length); j++)" +
                                  "for (int k = 0; (k < buttons[j].length) || !(j < buttons.length); k++)" +
                                  "for (int m = 0; m < alternativeNames.length; m++)" +
                                  "if (buttons[j][k].getText().equals(alternativeNames[m][0]))" +
                                  "buttons[j][k].setText(alternativeNames[m][1]);";
            var javaCode = new JavaCode(source);
            OperatorParsingResult parsingResult = javaOperatorParser.Parse(javaCode);

            Console.WriteLine("Original source: ");
            Console.WriteLine(source);
            Console.WriteLine();
            Console.WriteLine("Normolized source:");
            Console.WriteLine(javaCode.NormolizedSource);
            Console.WriteLine();
            Console.WriteLine("Operators: ");
            foreach (var result in parsingResult)
            {
                Console.WriteLine("{0} {1} - {2}", result.Key.GetType().Name, result.Key.NormolizedSource, result.Value);
            }
            Console.ReadKey();
        }
    }
}
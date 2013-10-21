using System;
using CodeMetricsCalculator.Parsers;
using CodeMetricsCalculator.Parsers.Java;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator
{
    public class Program
    {
        private static void Main(string[] args)
        {
            const string source = "for (int j = 0; !(j < buttons.length); j++)" +
                                  "for (int k = 0; (k < buttons[j].length) || !(j < buttons.length); k++)" +
                                  "for (int m = 0; m < alternativeNames.length; m++)" +
                                  "if (buttons[j][k].getText().equals(alternativeNames[m][0]))" +
                                  "buttons[j][k].setText(alternativeNames[m][1]);";
            var javaExpression = new JavaExpression(source); //вообще говоря, тут надо создавать JavaMethodBody
            //и парсить его на JavaExpression's... но пока JavaMethodMody нету

            var operators = javaExpression.GetOperators();

            Console.WriteLine("Original source: ");
            Console.WriteLine(source);
            Console.WriteLine();
            Console.WriteLine("Normolized source:");
            Console.WriteLine(javaExpression.NormalizedSource);
            Console.WriteLine();
            Console.WriteLine("Operators: ");
            foreach (var operatorInfo in operators)
            {
                Console.WriteLine("{0} ({1}, {2}) {3} - {4}",
                                  operatorInfo.Item1.GetType().Name,
                                  operatorInfo.Item1.OperationType,
                                  operatorInfo.Item1.Syntax,
                                  operatorInfo.Item1.Name, operatorInfo.Item2);
            }
            Console.ReadKey();
        }
    }
}
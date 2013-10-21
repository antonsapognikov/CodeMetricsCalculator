using System;
using System.IO;
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
            //и парсить его на JavaExpression's... но пока парсинг экспрешшенов из боди не реализован

            var operators = javaExpression.GetOperators();

            /*Console.WriteLine("Original source: ");
            Console.WriteLine(source);
            Console.WriteLine();
            Console.WriteLine("Normolized source:");
            Console.WriteLine(javaExpression.NormalizedSource);*/
            Console.WriteLine();
            Console.WriteLine("#########################################");
            Console.WriteLine();
            Console.WriteLine("Operators: ");
            foreach (var operatorInfo in operators)
            {
                Console.WriteLine("{0} ({1}, {2}) {3} - {4}",
                                  operatorInfo.Key.GetType().Name,
                                  operatorInfo.Key.OperationType,
                                  operatorInfo.Key.Syntax,
                                  operatorInfo.Key.Name,
                                  operatorInfo.Value);
            }

            var classesSource = Resource.TestJavaCode;
            var classesCode = new JavaCode(classesSource);
            var classes = new JavaClassParser().Parse(classesCode);

            /*Console.WriteLine("Original source: ");
            Console.WriteLine(classesSource);
            Console.WriteLine();
            Console.WriteLine("Normolized source:");
            Console.WriteLine(classesCode.NormalizedSource);*/
            Console.WriteLine();
            Console.WriteLine("#########################################");
            Console.WriteLine();
            Console.WriteLine("Classes: ");
            foreach (var classInfo in classes)
            {
                Console.WriteLine("Name - {0}{1}Sources:{1}{2}",
                                  classInfo.Name,
                                  Environment.NewLine,
                                  classInfo.NormalizedSource);
            }

            Console.ReadKey();
        }
    }
}
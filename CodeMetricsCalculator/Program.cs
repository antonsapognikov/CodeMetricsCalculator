using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeMetricsCalculator.Parsers;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var classesSource = Resource.TestJavaCode;
            var classesCode = new JavaCode(classesSource);
            var classes = new JavaClassParser().Parse(classesCode);
            Console.WriteLine("Classes: ");
            foreach (var classInfo in classes)
            {
                Console.WriteLine("###########################");
                PrintClass(classInfo);
            }

            Console.ReadKey();
        }

        private static void PrintClass(IClassInfo classInfo)
        {
            Console.WriteLine("Name - {0}{1}Sources:{1}\t{2}",
                    classInfo.Name,
                    Environment.NewLine,
                    classInfo.NormalizedSource);
            var methods = classInfo.GetMethods();
            if (!methods.Any())
                Console.WriteLine("There is no methods");
            else
            {
                Console.WriteLine("Methods:");
                foreach (var methodInfo in methods)
                {
                    PrintMethod(methodInfo);
                    Console.WriteLine("***************************");
                }
            }
        }

        private static void PrintMethod(IMethodInfo methodInfo)
        {
            Console.WriteLine("Name - {0}{1}Sources:{1}\t{2}",
                        methodInfo.Name,
                        Environment.NewLine,
                        methodInfo.NormalizedSource);
            PrintMethodBody(methodInfo.GetBody());
            var parameters = methodInfo.Parameters;
            if (!parameters.Any())
                Console.WriteLine("There is no parameters");
            else
            {
                Console.WriteLine("Parameters:");
                foreach (var variableInfo in parameters)
                {
                    Console.WriteLine("*\t name - {0}, source - {1}", variableInfo.Name, variableInfo.NormalizedSource);
                }
            }
        }

        private static void PrintVariables(IEnumerable<IVariableInfo> variables)
        {
            
        }

        private static void PrintMethodBody(IMethodBodyInfo methodBodyInfo)
        {
            Console.WriteLine("MethodBody: {0}\t\t{1}", Environment.NewLine, methodBodyInfo.NormalizedSource);
            /*
            var expressions = methodBodyInfo.GetExpressions();
            if (!expressions.Any())
                Console.WriteLine("There is no expressions");
            else
            {
                Console.WriteLine("Expressions:");
                foreach (var expressionInfo in expressions)
                {
                    PrintExpression(expressionInfo);
                }
            }*/
        }

        private static void PrintExpression(IExpressionInfo expressionInfo)
        {
            Console.WriteLine("\t*{0}\t\t{1}", Environment.NewLine, expressionInfo.NormalizedSource);
            var operators = expressionInfo.GetOperators();
            if (!operators.Any())
                Console.WriteLine("There is no operators");
            else
            {
                Console.WriteLine("Operators:");
                foreach (var operatorInfo in operators)
                {
                    PrintOperator(operatorInfo.Key);
                }
            }
        }

        private static void PrintOperator(IOperatorInfo operatorInfo)
        {
            Console.WriteLine("{0} - {1}", operatorInfo.GetType().Name, operatorInfo.Name);
        }
    }
}
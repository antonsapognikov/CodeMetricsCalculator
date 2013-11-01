using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            var normalized = classesCode.NormalizedSource;
            var classes = new JavaClassParser().Parse(classesCode);
            var expressions = classes.Select(@class => @class.GetMethods())
                .Aggregate(AggregateMethods)
                .Select(info => info.GetBody())
                .Select(info => info.GetExpressions())
                .Aggregate(
                    AggregateExpressions).ToList();
            Console.WriteLine("All expressions: ");
            foreach (var expressionInfo in expressions)
            {
                Console.WriteLine(expressionInfo.NormalizedSource);
                Console.WriteLine("Operators:");
                foreach (var operatorInfo in expressionInfo.GetOperators())
                {
                    PrintOperator(operatorInfo.Key, operatorInfo.Value);
                }
                Console.WriteLine("-------------------");
                //PrintExpression(expressionInfo);
            }
            Console.WriteLine("Press any key to print classes and their members.");
            Console.ReadKey();
            Console.WriteLine("Classes: ");
            foreach (var classInfo in classes)
            {
                Console.WriteLine("###########################");
                PrintClass(classInfo);
            }

            Console.ReadKey();
        }

        private static IReadOnlyCollection<IExpressionInfo> AggregateExpressions(IReadOnlyCollection<IExpressionInfo> readOnlyCollection, IReadOnlyCollection<IExpressionInfo> expressionInfos)
        {
            var list = new List<IExpressionInfo>();
            list.AddRange(readOnlyCollection);
            list.AddRange(expressionInfos);
            return new ReadOnlyCollection<IExpressionInfo>(list);
        }

        private static IReadOnlyCollection<IMethodInfo> AggregateMethods(IReadOnlyCollection<IMethodInfo> readOnlyCollection,
            IReadOnlyCollection<IMethodInfo> onlyCollection)
        {
            var list = new List<IMethodInfo>();
            list.AddRange(readOnlyCollection);
            list.AddRange(onlyCollection);
            return new ReadOnlyCollection<IMethodInfo>(list);
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
            Console.WriteLine("MethodBody: {0}{1}", Environment.NewLine, methodBodyInfo.NormalizedSource);
            
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
            }
        }

        private static void PrintExpression(IExpressionInfo expressionInfo)
        {
            Console.WriteLine("\t*{0}{1}", Environment.NewLine, expressionInfo.NormalizedSource);
            var operators = expressionInfo.GetOperators();
            if (!operators.Any())
                Console.WriteLine("There is no operators");
            else
            {
                Console.WriteLine("Operators:");
                foreach (var operatorInfo in operators)
                {
                    PrintOperator(operatorInfo.Key, operatorInfo.Value);
                }
            }
        }

        private static void PrintOperator(IOperatorInfo operatorInfo, int count)
        {
            Console.WriteLine("{0} - {1} - {2}", operatorInfo.GetType().Name, operatorInfo.Name, count);
        }
    }
}
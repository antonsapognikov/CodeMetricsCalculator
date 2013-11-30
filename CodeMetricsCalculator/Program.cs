using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Metrics;
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
            
            foreach (var javaClass in classes)
            {
                Console.WriteLine("Chepin: " + ChepinMetricCalculator.Calculate(javaClass));
                Console.WriteLine("Average spen: " + SpenMetricCalculator.CalculateAverage(javaClass 
                    ));
                var spens = SpenMetricCalculator.Calculate(javaClass);
                Console.WriteLine("Spens:");
                foreach (var keyValuePair in spens)
                {
                    Console.WriteLine("{0} - {1}", keyValuePair.Key.Name, keyValuePair.Value);
                }
                var dictionary = javaClass.GetIdentifiers();
                Console.WriteLine(javaClass.NormalizedSource);
                foreach (var keyValuePair in dictionary)
                {
                    Console.WriteLine("{0} - {1}", keyValuePair.Key.Name, keyValuePair.Value);
                }
                Console.WriteLine();
                Console.WriteLine("##############################################");
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            var methods = classes.SelectMany(@class => @class.GetMethods());
            foreach (var methodInfo in methods)
            {
                var codeDictionary = methodInfo.GetMethodDictionary();
                Console.WriteLine(methodInfo.NormalizedSource);
                Console.WriteLine("Operators:");
                foreach (var keyValuePair in codeDictionary.Operators)
                {
                    Console.WriteLine("{0} - {1}", keyValuePair.Key, keyValuePair.Value);
                }
                Console.WriteLine("Operands:");
                foreach (var keyValuePair in codeDictionary.Operands)
                {
                    Console.WriteLine("{0} - {1}", keyValuePair.Key, keyValuePair.Value);
                }
                Console.WriteLine();
                Console.WriteLine("##############################################");
                Console.WriteLine();
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
            var fields = classInfo.GetFields();
            if (!fields.Any())
                Console.WriteLine("There is no fields");
            else
            {
                Console.WriteLine("Fileds:");
                foreach (var fieldInfo in fields)
                {
                    PrintField(fieldInfo);
                }
            }

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

        private static void PrintField(IFieldInfo fieldInfo)
        {
            Console.WriteLine("{0} - {1}", fieldInfo.Name, fieldInfo.NormalizedSource);
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

        private static void PrintVariables(IEnumerable<IIdentifierInfo> variables)
        {
            
        }

        private static void PrintMethodBody(IMethodBodyInfo methodBodyInfo)
        {
            Console.WriteLine("MethodBody: {0}{1}", Environment.NewLine, methodBodyInfo.NormalizedSource);
            
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

            var operands = expressionInfo.GetOperands();
            if (!operands.Any())
                Console.WriteLine("There is no operands");
            else
            {
                Console.WriteLine("Operands:");
                foreach (var operandInfo in operands)
                {
                    PrintOperand(operandInfo.Key, operandInfo.Value);
                }
            }
        }

        private static void PrintOperand(IOperandInfo operandInfo, int count)
        {
            Console.WriteLine("{0} - {1} - {2}", operandInfo.GetType().Name, operandInfo.Name, count);
        }

        private static void PrintOperator(IOperatorInfo operatorInfo, int count)
        {
            Console.WriteLine("{0} - {1} - {2}", operatorInfo.GetType().Name, operatorInfo.Name, count);
        }
    }
}
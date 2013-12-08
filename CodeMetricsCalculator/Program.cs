using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.Pascal;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var classesSource = Resource.TestPascalCode;
            var pascalCode = new PascalCode(classesSource);
            var classes = new PascalClassParser().Parse(pascalCode);
            foreach (var pascalClass in classes)
            {
                var methods = pascalClass.GetMethods();
                foreach (var methodInfo in methods)
                {
                    var codeDictionary = methodInfo.GetMethodDictionary();
                    GC.KeepAlive(codeDictionary);
                }
                foreach (var methodInfo in methods)
                {
                    var body = methodInfo.GetBody();
                    GC.KeepAlive(body);
                }
                GC.KeepAlive(methods);
                Debugger.Break();
            }
         /*   var classesCode = new JavaCode(classesSource);
            var normalized = classesCode.NormalizedSource;
            var classes = new JavaClassParser().Parse(classesCode);
            
            foreach (var javaClass in classes)
            {
                var m = javaClass.GetMethods();
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
            }*/
            Console.ReadKey();

        }
     /*   
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
        
        private static void PrintOperand(IOperandInfo operandInfo, int count)
        {
            Console.WriteLine("{0} - {1} - {2}", operandInfo.GetType().Name, operandInfo.Name, count);
        }

        private static void PrintOperator(IOperatorInfo operatorInfo, int count)
        {
            Console.WriteLine("{0} - {1} - {2}", operatorInfo.GetType().Name, operatorInfo.Name, count);
        }*/
    }
}
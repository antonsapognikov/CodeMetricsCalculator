using System;
using System.IO;
using System.Linq;
using CodeMetricsCalculator.Parsers;
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
                Console.WriteLine("Name - {0}{1}Sources:{1}\t{2}",
                    classInfo.Name,
                    Environment.NewLine,
                    classInfo.NormalizedSource);
                Console.WriteLine("***************************");
                var methods = classInfo.GetMethods();
                if (!methods.Any())
                    Console.WriteLine("There is no methods");
                else
                {
                    Console.WriteLine("Methods:");
                    foreach (var methodInfo in methods)
                    {
                        Console.WriteLine("Name - {0}{1}Sources:{1}\t{2}",
                            methodInfo.Name,
                            Environment.NewLine,
                            methodInfo.NormalizedSource);
                        var methodBodySource = methodInfo.GetBody().NormalizedSource;
                        Console.WriteLine("MethodBody: {0}\t\t{1}", Environment.NewLine, methodBodySource);
                        Console.WriteLine("***************************");
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
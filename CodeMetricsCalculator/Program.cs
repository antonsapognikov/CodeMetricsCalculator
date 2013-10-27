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
            var classesSource = Resource.TestJavaCode;
            var classesCode = new JavaCode(classesSource);
            var classes = new JavaClassParser().Parse(classesCode);

            Console.WriteLine("Classes: ");
            foreach (var classInfo in classes)
            {
                Console.WriteLine("Name - {0}{1}Sources:{1}{2}",
                                  classInfo.Name,
                                  Environment.NewLine,
                                  classInfo.NormalizedSource);
                Console.WriteLine("***************************");
                Console.WriteLine("Methods:");
                foreach (var methodInfo in classInfo.GetMethods())
                {
                    Console.WriteLine("Name - {0}{1}Sources:{1}{2}",
                                      methodInfo.Name,
                                      Environment.NewLine,
                                      methodInfo.NormalizedSource);
                    Console.WriteLine("***************************");
                }
            }

            Console.ReadKey();
        }
    }
}
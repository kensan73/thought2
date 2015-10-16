using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistanceCalculator;

namespace ThoughtworksTrain
{
    class Program
    {
        static void Main(string[] args)
        {
            var graphTraverser = new Calculator(new RouteTraverser.Traverser());

            var inputGraph = new Dictionary<string, int>
            {
                {"AB", 5},
                {"BC", 4},
                {"CD", 8},
                {"DC", 8},
                {"DE", 6},
                {"AD", 5},
                {"CE", 2},
                {"EB", 3},
                {"AE", 7}
            };
            ThoughtTest1(graphTraverser, inputGraph);
            ThoughtTest2(graphTraverser, inputGraph);
            ThoughtTest3(graphTraverser, inputGraph);
            ThoughtTest4(graphTraverser, inputGraph);
            ThoughtTest5(graphTraverser, inputGraph);
        }

        private static void ThoughtTest5(Calculator graphTraverser, Dictionary<string, int> inputGraph)
        {
            var result = graphTraverser.InvokeForPath(inputGraph, "AED");
            if (result != -1)
            {
                Console.WriteLine("Test 5 failed.");
                return;
            }
        }
        private static void ThoughtTest4(Calculator graphTraverser, Dictionary<string, int> inputGraph)
        {
            var result = graphTraverser.InvokeForPath(inputGraph, "AEBCD");
            if (result != 22)
            {
                Console.WriteLine("Test 4 failed.");
            }
        }

        private static void ThoughtTest3(Calculator graphTraverser, Dictionary<string, int> inputGraph)
        {
            var result = graphTraverser.InvokeForPath(inputGraph, "ADC");
            if (result != 13)
            {
                Console.WriteLine("Test 3 failed.");
                return;
            }
        }

        private static void ThoughtTest2(Calculator graphTraverser, Dictionary<string, int> inputGraph)
        {
            var result = graphTraverser.InvokeForPath(inputGraph, "AD");
            if (result != 5)
            {
                Console.WriteLine("Test 2 failed.");
                return;
            }
        }

        private static void ThoughtTest1(Calculator graphTraverser, Dictionary<string, int> inputGraph)
        {
            var result = graphTraverser.InvokeForPath(inputGraph, "ABC");
            if (result != 9)
            {
                Console.WriteLine("Test 1 failed.");
                return;
            }
        }
    }
}

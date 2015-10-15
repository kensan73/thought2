using System;
using System.Collections.Generic;
using System.Linq;
using RouteTraverser;

namespace DistanceCalculator
{
    public class Calculator
    {
        private readonly ITraverseGraph _traverser;

        public Calculator(ITraverseGraph traverser)
        {
            _traverser = traverser;
        }

        public int InvokeForPath(Dictionary<string, int> graph, string myPath)
        {
            var traversals = _traverser.Invoke(graph);

            if (traversals.Any(traversal => traversal.Count == 1 && traversal[0] == myPath))
            {
                return graph[myPath];
            }

            foreach (var traversal in traversals.Where(t => t.Count == myPath.Length-1))
            {
                if (StartLocationsAreEqual(myPath, traversal) && EndLocationsAreEqual(myPath, traversal))
                    return CalculateDistanceOfTraversal(graph, traversal);
            }

            return -1;
        }

        private static int CalculateDistanceOfTraversal(Dictionary<string, int> graph, List<string> traversal)
        {
            return traversal.Select(t => graph[t]).Sum();
        }

        private static bool EndLocationsAreEqual(string myPath, List<string> traversal)
        {
            return traversal.Last().Last() == myPath.Last();
        }

        private static bool StartLocationsAreEqual(string myPath, List<string> traversal)
        {
            return traversal.First().First() == myPath.First();
        }
    }
}

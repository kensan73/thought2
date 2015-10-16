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

            foreach (var traversal in traversals.Where(t => t.Count >= myPath.Length-1))
            {
                if (TraversalMatchesPath(myPath, traversal))
                    return CalculateDistanceOfTraversal(graph, traversal, myPath.Length);
            }

            return -1;
        }
        public List<List<string>> InvokeForStartEndAndLessThanOrEqual(Dictionary<string, int> graph, string startEnd, int threshold)
        {
            var result = new List<List<string>>();

            var traversals = _traverser.Invoke(graph);

            const int minThreshold = 2;

            if (!traversals.Any() || !traversals.Any(t => t.Count >= minThreshold)) return result;

            for (var i = minThreshold; i != threshold; ++i)
            {
                var range = traversals.GetRange(0, i-1);
                if (!range.Any(t => t.Count >= minThreshold)) continue;
                var filteredTraversals = range.Where(t => t.Count >= minThreshold).Where(t => StartAndEndLocationsMatch(startEnd, t));
                foreach (var traversal in filteredTraversals)
                {
                    result.Add(traversal);
                }
            }

            return result;
        }

        private static bool StartAndEndLocationsMatch(string startEnd, List<string> t)
        {
            var tfirst = t.First()[0];
            var startendFirst = startEnd.First();
            var tsecondtolast = t[t.Count-2][1];
            var startendLast = startEnd.Last();
            return tfirst == startendFirst && tsecondtolast == startendLast;
        }

        private static bool TraversalMatchesPath(string myPath, List<string> traversal)
        {
            var i = 0;
            for(; i != myPath.Length-1; ++i)
                if (myPath[i] != traversal[i][0])
                    return false;

            return traversal[i-1][1] == myPath.Last();
//            return StartLocationsAreEqual(startEnd, traversal) && EndLocationsAreEqual(startEnd, traversal);
        }

        private static int CalculateDistanceOfTraversal(Dictionary<string, int> graph, List<string> traversal, int length)
        {
            return traversal.GetRange(0, length-1).Select(t => graph[t]).Sum();
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

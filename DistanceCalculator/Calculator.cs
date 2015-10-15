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

            foreach (var traversal in traversals.Where(t => t.Count == myPath.Length))
            {
                if (traversal[0][0] == myPath[0] && traversal[traversal.Count - 1][1] == myPath[1])
                    return traversal.Select(t => graph[t]).Sum();
//                var myPathAndTraversal = myPath.Zip(traversal, (p, t) => new { Path = p, Traversal = t });
//                var isDifferent = myPathAndTraversal.Any(nw => nw.Path != nw.Traversal[0]);
//                if (isDifferent == false)
//                    return graph[myPath];
            }

            return -1;
        }
    }
}

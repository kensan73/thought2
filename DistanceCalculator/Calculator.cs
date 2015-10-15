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

            return -1;
        }
    }
}

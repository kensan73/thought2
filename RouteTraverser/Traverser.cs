using System.Collections.Generic;
using System.Linq;
using RouteTraverserAuxStrategies;

namespace RouteTraverser
{
    public class Traverser : ITraverseGraph
    {
        private readonly ITraverseAux _auxTraverser;

        public Traverser(ITraverseAux auxTraverser)
        {
            _auxTraverser = auxTraverser;
        }

        public List<List<string>> Invoke(Dictionary<string, int> inputGraph)
        {
            if(!inputGraph.Any())
                return new List<List<string>>();

            return Traverse(inputGraph);
        }

        private List<List<string>> Traverse(Dictionary<string, int> inputGraph)
        {
            var traversals = new List<List<string>>();

            foreach (var startLeg in inputGraph.Keys)
            {
                var traversal = new List<string>();
                _auxTraverser.Invoke(startLeg, inputGraph, ref traversal);
                if(traversal.Any())
                    traversals.Add(traversal);
            }

            return traversals;
        }
    }
}
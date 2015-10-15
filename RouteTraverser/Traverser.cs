using System.Collections.Generic;
using System.Linq;

namespace RouteTraverser
{
    public class Traverser
    {
        private readonly IShortCircuitLoops _shortCircuiter;

        public Traverser(IShortCircuitLoops shortCircuiter)
        {
            _shortCircuiter = shortCircuiter;
        }

        public List<List<string>> Invoke(Dictionary<string, int> inputGraph)
        {
            if(!inputGraph.Any())
                return new List<List<string>>();

//            if (inputGraph.Count == 1)
//                return new List<List<string>>
//                {
//                    {new List<string> {inputGraph.Keys.First()}}
//                };

            return Traverse(inputGraph);
        }

        private List<List<string>> Traverse(Dictionary<string, int> inputGraph)
        {
            var traversals = new List<List<string>>();

            foreach (var startLeg in inputGraph.Keys)
            {
                var traversal = new List<string>();
                TraverseAux(startLeg, inputGraph, ref traversal);
                if(traversal != null)
                    traversals.Add(traversal);
            }

            return traversals;
        }

        private void TraverseAux(string startLeg, Dictionary<string, int> inputGraph, ref List<string> traversal)
        {
            // once you enter this function you have traversed from startLeg[0]
            // ..to startLeg[1] so update bookkeeping here
            traversal.Add(startLeg);

            if (_shortCircuiter.ShouldExitLoop(startLeg, inputGraph, traversal.Count))
                return;
//            if (traversal.Count > inputGraph.Keys.Count*3)
//            {
//                return;
//            }

            var currentLoc = startLeg[1];
            var possibleDests = inputGraph.Keys.Where(k => k[0] == currentLoc).ToList();

            foreach (var nextDest in possibleDests)
            {
                TraverseAux(nextDest, inputGraph, ref traversal);
            }
        }
    }
}
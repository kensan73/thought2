using System;
using System.Collections.Generic;
using System.Linq;

namespace RouteTraverserAuxStrategies
{
    public class CircularPathTraverser : ITraverseAux
    {
        public void Invoke(string startLeg, Dictionary<string, int> inputGraph, ref List<string> traversal)
        {
            // once you enter this function you have traversed from startLeg[0]
            // ..to startLeg[1] so update bookkeeping here
            traversal.Add(startLeg);

            if (traversal.Count > inputGraph.Keys.Count * 3)
            {
                return;
            }

            var currentLoc = startLeg[1];
            var possibleDests = inputGraph.Keys.Where(k => k[0] == currentLoc).ToList();

            foreach (var nextDest in possibleDests)
            {
                Invoke(nextDest, inputGraph, ref traversal);
            }
        }
    }
}

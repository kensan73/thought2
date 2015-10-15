using System.Collections.Generic;

namespace RouteTraverser
{
    public interface IShortCircuitLoops
    {
        bool ShouldExitLoop(string startLeg, Dictionary<string, int> inputGraph, int depth);
    }
}
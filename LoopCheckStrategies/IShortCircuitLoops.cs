using System.Collections.Generic;

namespace LoopCheckStrategies
{
    public interface IShortCircuitLoops
    {
        bool ShouldExitLoop(string startLeg, Dictionary<string, int> inputGraph, int depth);
    }
}
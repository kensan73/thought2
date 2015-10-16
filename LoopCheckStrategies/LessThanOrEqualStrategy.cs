using System;
using System.Collections.Generic;

namespace LoopCheckStrategies
{
    public class LessThanOrEqualStrategy : IShortCircuitLoops
    {
        private readonly int _threshold;

        public LessThanOrEqualStrategy(int threshold)
        {
            _threshold = threshold;
        }

        public bool ShouldExitLoop(string startLeg, Dictionary<string, int> inputGraph, int depth)
        {
            return depth > _threshold;
        }
    }
}

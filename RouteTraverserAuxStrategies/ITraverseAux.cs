using System.Collections.Generic;

namespace RouteTraverserAuxStrategies
{
    public interface ITraverseAux
    {
        void Invoke(string startLeg, Dictionary<string, int> inputGraph, ref List<string> traversal);
    }
}

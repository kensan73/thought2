using System.Collections.Generic;

namespace RouteTraverser
{
    public interface ITraverseGraph
    {
        List<List<string>> Invoke(Dictionary<string, int> inputGraph);
    }
}
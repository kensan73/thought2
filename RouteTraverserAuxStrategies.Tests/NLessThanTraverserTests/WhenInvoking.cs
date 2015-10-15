using System.Collections.Generic;
using NUnit.Framework;

namespace RouteTraverserAuxStrategies.Tests.NLessThanTraverserTests
{
    public partial class NLessThanTraverserTests
    {
        [TestFixture]
        public class WhenInvoking
        {
            [SetUp]
            public void SetUp()
            {
            }

            [TearDown]
            public void TearDown()
            {

            }

            [Test]
            public void NoPathsReturnsNoTraversals()
            {
                var auxer = new NLessThanTraverser();
                var traversal = new List<string>();

                auxer.Invoke("", new Dictionary<string, int>(), ref traversal);

                Assert.That(traversal, Is.Empty);
            }

            [Test]
            public void OnePathReturnsOneTraversal()
            {
                var auxer = new NLessThanTraverser();
                var traversal = new List<string>();
                const string leg1 = "AE";
                const int distance1 = 34;
                var inputGraph = new Dictionary<string, int> {{leg1, distance1}};

                auxer.Invoke("AE", inputGraph, ref traversal);

                Assert.That(traversal.Count, Is.EqualTo(1));
                Assert.That(traversal[0] == leg1);
            }

            [Test]
            public void TwoPathsReturnsOneTraversal()
            {
                var auxer = new NLessThanTraverser();
                var traversal = new List<string>();
                const string leg1 = "AE";
                const int distance1 = 34;
                var inputGraph = new Dictionary<string, int> {{leg1, distance1}};

                auxer.Invoke("AE", inputGraph, ref traversal);

                Assert.That(traversal.Count, Is.EqualTo(1));
                Assert.That(traversal[0] == leg1);
            }
        }
    }
}

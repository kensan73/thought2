using System.Collections.Generic;
using NUnit.Framework;

namespace RouteTraverser.Tests
{
    public partial class RouteTraverserTests
    {
        [TestFixture]
        public class WhenInvoking
        {
            private Traverser _traverser;

            [SetUp]
            public void Setup()
            {
                _traverser = new Traverser();
            }

            [TearDown]
            public void Teardown()
            {
                
            }

            [Test]
            public void DepthSearchOnEmptyGraphReturnsEmptyTraversal()
            {
                var emptyInput = new Dictionary<string, int>();

                var result = _traverser.Invoke(emptyInput);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void DepthSearchOnOneNodeGraphReturnsOneTraversal()
            {
                var sourceDest = "AB";
                var oneNode = new Dictionary<string, int>
                {
                    {sourceDest, 5}
                };

                var result = _traverser.Invoke(oneNode);

                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0][0], Is.EqualTo(sourceDest));
            }

            [Test]
            public void DepthSearchOnTwoNodeConnectedGraphReturnsOneTraversalTwoNodes()
            {
                const string sourceDest = "AC";
                const string sourceDest2 = "CD";

                var oneNode = new Dictionary<string, int>
                {
                    {sourceDest, 5},
                    {sourceDest2, 11}
                };

                var result = _traverser.Invoke(oneNode);

                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result[0][0], Is.EqualTo(sourceDest));
                Assert.That(result[0][1], Is.EqualTo(sourceDest2));
            }

            [Test]
            public void AvoidsInfiniteTraversal()
            {
                const string sourceDest = "AC";
                const string sourceDest2 = "CA";

                var oneNode = new Dictionary<string, int>
                {
                    {sourceDest, 5},
                    {sourceDest2, 11}
                };

                var result = _traverser.Invoke(oneNode);

                Assert.That(result.Count, Is.EqualTo(0));
            }
        }
    }
}
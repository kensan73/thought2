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

            [Test]
            public void MultiplePathsAreTraversed()
            {
                const string sourceDest = "AC";
                const string sourceDest2 = "CD";
                const string sourceDest3 = "BE";
                const string sourceDest4 = "EF";
                const string sourceDest5 = "FG";

                var oneNode = new Dictionary<string, int>
                {
                    {sourceDest, 5},
                    {sourceDest2, 11},
                    {sourceDest3, 7},
                    {sourceDest4, 13},
                    {sourceDest5, 123},
                };

                var result = _traverser.Invoke(oneNode);

                Assert.That(result.Count, Is.EqualTo(5));

                Assert.That(result[0][0], Is.EqualTo(sourceDest));
                Assert.That(result[0][1], Is.EqualTo(sourceDest2));

                Assert.That(result[1][0], Is.EqualTo(sourceDest2));

                Assert.That(result[2][0], Is.EqualTo(sourceDest3));
                Assert.That(result[2][1], Is.EqualTo(sourceDest4));
                Assert.That(result[2][2], Is.EqualTo(sourceDest5));
                
                Assert.That(result[3][0], Is.EqualTo(sourceDest4));
                Assert.That(result[3][1], Is.EqualTo(sourceDest5));

                Assert.That(result[4][0], Is.EqualTo(sourceDest5));
            }
        }
    }
}
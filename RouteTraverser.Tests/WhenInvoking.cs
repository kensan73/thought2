using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using RouteTraverserAuxStrategies;

namespace RouteTraverser.Tests
{
    public partial class TraverserTests
    {
        [TestFixture]
        public class WhenInvoking
        {
            private Traverser _traverser;
            private ITraverseAux _auxTraverser;

            [SetUp]
            public void Setup()
            {
                _auxTraverser = MockRepository.GenerateStrictMock<ITraverseAux>();

                _traverser = new Traverser(_auxTraverser);
            }

            [TearDown]
            public void Teardown()
            {
                _auxTraverser.VerifyAllExpectations();
            }

            [Test]
            public void EmptyGraphResultsInNoTraversal()
            {
                var emptyInput = new Dictionary<string, int>();

                var result = _traverser.Invoke(emptyInput);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void OneNodePassedToAux_ReturnsAuxResultsEvenIfItsEmpty()
            {
                var startLeg = "AB";
                var oneNode = new Dictionary<string, int>
                {
                    {startLeg, 5}
                };
                var traversal = new List<string>();

                _auxTraverser.Expect(a => a.Invoke(startLeg, oneNode, ref traversal)).OutRef(new List<string>());

                var result = _traverser.Invoke(oneNode);

                Assert.That(result.Count, Is.EqualTo(0));
            }

            [Test]
            public void OneNodePassedToAux_ReturnsAuxResults()
            {
                var startLeg = "AB";
                var oneNode = new Dictionary<string, int>
                {
                    {startLeg, 5}
                };
                var traversal = new List<string>();

                _auxTraverser.Expect(a => a.Invoke(startLeg, oneNode, ref traversal)).OutRef(new List<string>{startLeg});

                var result = _traverser.Invoke(oneNode);

                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0][0], Is.EqualTo(startLeg));
            }

            [Test]
            public void OneNodePassedToAux_ReturnsMultipleAuxResults()
            {
                var startLeg = "AB";
                var nextLeg = "CD";
                var lastLeg = "CZ";
                var oneNode = new Dictionary<string, int>
                {
                    {startLeg, 5},
                    {lastLeg, 77},
                    {nextLeg, 73}
                };
                var traversal = new List<string>();
                var traversalOneResult = new List<string> { startLeg };
                var traversalTwoResult = new List<string> { lastLeg, nextLeg };
                var traversalOneResultNextLeg = new List<string> { nextLeg };

                _auxTraverser.Expect(a => a.Invoke(startLeg, oneNode, ref traversal)).OutRef(traversalOneResult);
                _auxTraverser.Expect(a => a.Invoke(lastLeg, oneNode, ref traversal)).OutRef(traversalTwoResult);
                _auxTraverser.Expect(a => a.Invoke(nextLeg, oneNode, ref traversal)).OutRef(traversalOneResultNextLeg);

                var result = _traverser.Invoke(oneNode);

                Assert.That(result.Count, Is.EqualTo(3));
                Assert.That(result.First().First(), Is.EqualTo(startLeg));
                
                Assert.That(result[1].First(), Is.EqualTo(lastLeg));
                Assert.That(result[1].Last(), Is.EqualTo(nextLeg));

                Assert.That(result.Last().Last(), Is.EqualTo(nextLeg));
            }

//            [Test]
//            public void DepthSearchOnTwoNodeConnectedGraphReturnsOneTraversalTwoNodes()
//            {
//                const string sourceDest = "AC";
//                const string sourceDest2 = "CD";
//
//                var oneNode = new Dictionary<string, int>
//                {
//                    {sourceDest, 5},
//                    {sourceDest2, 11}
//                };
//
//                var result = _traverser.Invoke(oneNode);
//
//                Assert.That(result.Count, Is.EqualTo(2));
//                Assert.That(result[0][0], Is.EqualTo(sourceDest));
//                Assert.That(result[0][1], Is.EqualTo(sourceDest2));
//            }
//
//            [Test]
//            public void AvoidsInfiniteTraversal()
//            {
//                const string sourceDest = "AC";
//                const string sourceDest2 = "CA";
//
//                var oneNode = new Dictionary<string, int>
//                {
//                    {sourceDest, 5},
//                    {sourceDest2, 11}
//                };
//
//                var result = _traverser.Invoke(oneNode);
//
//                Assert.That(result.Count, Is.EqualTo(2));
//
//                Assert.That(result.First().First(), Is.EqualTo(sourceDest));
//                Assert.That(result.First().Last(), Is.EqualTo(sourceDest));
//
//                Assert.That(result.Last().First(), Is.EqualTo(sourceDest2));
//                Assert.That(result.Last().Last(), Is.EqualTo(sourceDest2));
//            }
//
//            [Test]
//            public void MultiplePathsAreTraversed()
//            {
//                const string sourceDest = "AC";
//                const string sourceDest2 = "CD";
//                const string sourceDest3 = "BE";
//                const string sourceDest4 = "EF";
//                const string sourceDest5 = "FG";
//
//                var oneNode = new Dictionary<string, int>
//                {
//                    {sourceDest, 5},
//                    {sourceDest2, 11},
//                    {sourceDest3, 7},
//                    {sourceDest4, 13},
//                    {sourceDest5, 123},
//                };
//
//                var result = _traverser.Invoke(oneNode);
//
//                Assert.That(result.Count, Is.EqualTo(5));
//
//                Assert.That(result[0][0], Is.EqualTo(sourceDest));
//                Assert.That(result[0][1], Is.EqualTo(sourceDest2));
//
//                Assert.That(result[1][0], Is.EqualTo(sourceDest2));
//
//                Assert.That(result[2][0], Is.EqualTo(sourceDest3));
//                Assert.That(result[2][1], Is.EqualTo(sourceDest4));
//                Assert.That(result[2][2], Is.EqualTo(sourceDest5));
//                
//                Assert.That(result[3][0], Is.EqualTo(sourceDest4));
//                Assert.That(result[3][1], Is.EqualTo(sourceDest5));
//
//                Assert.That(result[4][0], Is.EqualTo(sourceDest5));
//            }
        }
    }
}
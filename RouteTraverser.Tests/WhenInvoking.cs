using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace RouteTraverser.Tests
{
    public partial class TraverserTests
    {
        [TestFixture]
        public class WhenInvoking
        {
            private Traverser _traverser;
            private IShortCircuitLoops _shortCircuiter;

            [SetUp]
            public void Setup()
            {
                _shortCircuiter = MockRepository.GenerateStrictMock<IShortCircuitLoops>();

                _traverser = new Traverser(_shortCircuiter);
            }

            [TearDown]
            public void Teardown()
            {
                _shortCircuiter.VerifyAllExpectations();
            }


            [Test]
            public void DepthSearchOnEmptyGraphReturnsEmptyTraversal()
            {
                var emptyInput = new Dictionary<string, int>();

                var result = _traverser.Invoke(emptyInput);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void SearchOneNodeThenExit()
            {
                const string startLeg = "AB";
                const string anotherLeg = "DE";
                var inputGraph = new Dictionary<string, int>
                {
                    {startLeg, 5},
                    {anotherLeg, 77}
                };
                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLeg, inputGraph, 1)).Return(true);
                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLeg, inputGraph, 1)).Return(true);

                var result = _traverser.Invoke(inputGraph);

                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result[0][0], Is.EqualTo(startLeg));
                Assert.That(result[1][0], Is.EqualTo(anotherLeg));
            }

            [Test]
            public void SearchOneNodeButHasAnotherLegShortCircuits()
            {
                const string startLeg = "AB";
                const string startLegConnector = "BC";
                const string anotherLeg = "DE";
                const string anotherLegConnector = "EG";
                var inputGraph = new Dictionary<string, int>
                {
                    {startLeg, 5},
                    {startLegConnector, 77},
                    {anotherLeg, 77},
                    {anotherLegConnector, 7899}
                };
                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLeg, inputGraph, 1)).Return(true);
                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLegConnector, inputGraph, 1)).Return(true);
                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLeg, inputGraph, 1)).Return(true);
                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLegConnector, inputGraph, 1)).Return(true);

                var result = _traverser.Invoke(inputGraph);

                Assert.That(result.Count, Is.EqualTo(4));
                Assert.That(result[0][0], Is.EqualTo(startLeg));
                Assert.That(result[1][0], Is.EqualTo(startLegConnector));
                Assert.That(result[2][0], Is.EqualTo(anotherLeg));
                Assert.That(result[3][0], Is.EqualTo(anotherLegConnector));
            }

            [Test]
            public void SearchOneNodeButHasAnotherLeg_DontShortCircuit()
            {
                const string startLeg = "AB";
                const string startLegConnector = "BC";
                const string anotherLeg = "DE";
                const string anotherLegConnector = "EG";
                var inputGraph = new Dictionary<string, int>
                {
                    {startLeg, 5},
                    {startLegConnector, 77},
                    {anotherLeg, 77},
                    {anotherLegConnector, 7899}
                };
                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLeg, inputGraph, 1)).Return(false);
                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLegConnector, inputGraph, 2)).Return(false);

                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLegConnector, inputGraph, 1)).Return(false);

                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLeg, inputGraph, 1)).Return(false);
                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLegConnector, inputGraph, 2)).Return(false);

                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLegConnector, inputGraph, 1)).Return(false);

                var result = _traverser.Invoke(inputGraph);

                Assert.That(result.Count, Is.EqualTo(4));

                Assert.That(result[0][0], Is.EqualTo(startLeg));
                Assert.That(result[0][1], Is.EqualTo(startLegConnector));

                Assert.That(result[1][0], Is.EqualTo(startLegConnector));

                Assert.That(result[2][0], Is.EqualTo(anotherLeg));
                Assert.That(result[2][1], Is.EqualTo(anotherLegConnector));

                Assert.That(result[3][0], Is.EqualTo(anotherLegConnector));
            }

//            [Test]
//            public void DepthSearchOnTwoNodeConnectedGraphReturnsOneTraversalTwoNodes()
//            {
//                const string sourceDest = "AC";
//                const string sourceDest2 = "CD";
//                var oneNode = new Dictionary<string, int>
//                {
//                    {sourceDest, 5},
//                    {sourceDest2, 11}
//                };
//
//                _shortCircuiter.Expect(s => s.ShouldExitLoop())
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
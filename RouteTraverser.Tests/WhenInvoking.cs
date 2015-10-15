using System.Collections.Generic;
using LoopCheckStrategies;
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
                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLeg, inputGraph, 0)).Return(false);
                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLeg, inputGraph, 0)).Return(false);

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
                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLeg, inputGraph, 0)).Return(false);
                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLegConnector, inputGraph, 1)).Return(true);

                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLegConnector, inputGraph, 0)).Return(false);

                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLeg, inputGraph, 0)).Return(false);
                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLegConnector, inputGraph, 1)).Return(true);

                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLegConnector, inputGraph, 0)).Return(true);

                var result = _traverser.Invoke(inputGraph);

                Assert.That(result.Count, Is.EqualTo(3));
                Assert.That(result[0][0], Is.EqualTo(startLeg));
                Assert.That(result[1][0], Is.EqualTo(startLegConnector));
                Assert.That(result[2][0], Is.EqualTo(anotherLeg));
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
                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLeg, inputGraph, 0)).Return(false);
                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLegConnector, inputGraph, 1)).Return(false);

                _shortCircuiter.Expect(s => s.ShouldExitLoop(startLegConnector, inputGraph, 0)).Return(false);

                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLeg, inputGraph, 0)).Return(false);
                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLegConnector, inputGraph, 1)).Return(false);

                _shortCircuiter.Expect(s => s.ShouldExitLoop(anotherLegConnector, inputGraph, 0)).Return(false);

                var result = _traverser.Invoke(inputGraph);

                Assert.That(result.Count, Is.EqualTo(4));

                Assert.That(result[0][0], Is.EqualTo(startLeg));
                Assert.That(result[0][1], Is.EqualTo(startLegConnector));

                Assert.That(result[1][0], Is.EqualTo(startLegConnector));

                Assert.That(result[2][0], Is.EqualTo(anotherLeg));
                Assert.That(result[2][1], Is.EqualTo(anotherLegConnector));

                Assert.That(result[3][0], Is.EqualTo(anotherLegConnector));
            }
        }
    }
}
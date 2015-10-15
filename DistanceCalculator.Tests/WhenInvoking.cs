using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using RouteTraverser;

namespace DistanceCalculator.Tests
{
    public partial class DistanceCalculatorTests
    {
        [TestFixture]
        public class WhenInvoking
        {
            private ITraverseGraph _traverser;
            private Calculator _calcer;

            [SetUp]
            public void Setup()
            {
                _traverser = MockRepository.GenerateStrictMock<ITraverseGraph>();
                _calcer = new Calculator(_traverser);
            }

            [TearDown]
            public void Teardown()
            {
               _traverser.VerifyAllExpectations(); 
            }

            [Test]
            public void HandlesNoTraverserResults()
            {
                var graph = new Dictionary<string, int>();
                const string myPath = "AZ";

                _traverser.Expect(t => t.Invoke(graph)).Return(new List<List<string>>());

                var result = _calcer.InvokeForPath(graph, myPath);

                Assert.That(result, Is.EqualTo(-1));
            }

            [Test]
            public void HandlesOneTraverserResultThatDoesntMatch()
            {
                var graph = new Dictionary<string, int>();
                const string myPath = "AZ";

                var oneResult = new List<List<string>>{new List<string>{"AB"}};

                _traverser.Expect(t => t.Invoke(graph)).Return(oneResult);

                var result = _calcer.InvokeForPath(graph, myPath);

                Assert.That(result, Is.EqualTo(-1));
            }

            [Test]
            public void HandlesOneTraverserResultThatDoesMatch()
            {
                const string leg = "AZ";
                const int distance = 73;
                var graph = new Dictionary<string, int> { {leg, distance} };
                const string myPath = leg;

                var oneResult = new List<List<string>>{new List<string>{leg}};

                _traverser.Expect(t => t.Invoke(graph)).Return(oneResult);

                var result = _calcer.InvokeForPath(graph, myPath);

                Assert.That(result, Is.EqualTo(distance));
            }
        }
    }
}

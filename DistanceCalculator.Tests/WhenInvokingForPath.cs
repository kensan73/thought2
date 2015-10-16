using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using RouteTraverser;

namespace DistanceCalculator.Tests
{
    public partial class DistanceCalculatorTests
    {
        [TestFixture]
        public class WhenInvokingForPath
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

            [Test]
            public void HandlesTwoLegResultThatDoesntMatch()
            {
                const string leg = "AZ";
                const int distance = 73;
                const string leg2 = "ZD";
                const int distance2 = 34;
                var graph = new Dictionary<string, int> { {leg, distance}, {leg2, distance2} };
                const string myPath = "AB";

                var oneResult = new List<List<string>>{new List<string>{leg, leg2}};

                _traverser.Expect(t => t.Invoke(graph)).Return(oneResult);

                var result = _calcer.InvokeForPath(graph, myPath);

                Assert.That(result, Is.EqualTo(-1));
            }

            [Test]
            public void HandlesThreeLegResultThatDoesntMatch()
            {
                const string leg = "AZ";
                const int distance = 73;
                const string leg2 = "ZD";
                const int distance2 = 34;
                const string leg3 = "ZM";
                const int distance3 = 399;
                var graph = new Dictionary<string, int> { {leg, distance}, {leg2, distance2}, {leg3, distance3} };
                const string myPath = "AB";

                var oneResult = new List<List<string>>{new List<string>{leg, leg2, leg3}};

                _traverser.Expect(t => t.Invoke(graph)).Return(oneResult);

                var result = _calcer.InvokeForPath(graph, myPath);

                Assert.That(result, Is.EqualTo(-1));
            }

            [Test]
            public void HandlesTwoLegResultThatDoesMatch()
            {
                const string leg = "AZ";
                const int distance = 73;
                const string leg2 = "ZD";
                const int distance2 = 34;
                var graph = new Dictionary<string, int> { {leg, distance}, {leg2, distance2} };
                const string myPath = "AZD";

                var oneResult = new List<List<string>>{new List<string>{leg, leg2}};

                _traverser.Expect(t => t.Invoke(graph)).Return(oneResult);

                var result = _calcer.InvokeForPath(graph, myPath);

                Assert.That(result, Is.EqualTo(distance + distance2));
            }

            [Test]
            public void HandlesTwoLegResultThatDoesMatch_SkipsNonmatchResult()
            {
                const string leg = "AZ";
                const int distance = 73;
                const string leg2 = "ZD";
                const int distance2 = 34;

                const string leg3 = "AV";
                const int distance3 = 773;
                const string leg4 = "VE";
                const int distance4 = 3422;
                var graph = new Dictionary<string, int> { {leg, distance}, {leg2, distance2}, {leg3, distance3}, {leg4, distance4} };
                const string myPath = "AZD";

                var twoTrips = new List<List<string>> {new List<string> {leg3, leg4}, new List<string> {leg, leg2}};

                _traverser.Expect(t => t.Invoke(graph)).Return(twoTrips);

                var result = _calcer.InvokeForPath(graph, myPath);

                Assert.That(result, Is.EqualTo(distance + distance2));
            }

            [Test]
            public void HandlesThreeLegResultThatDoesMatch()
            {
                const string leg = "AZ";
                const int distance = 73;
                const string leg2 = "ZD";
                const int distance2 = 34;
                const string leg3 = "DM";
                const int distance3 = 399;
                var graph = new Dictionary<string, int> { { leg, distance }, { leg2, distance2 }, { leg3, distance3 } };
                const string myPath = "AZDM";

                var oneResult = new List<List<string>>{new List<string>{leg, leg2, leg3}};

                _traverser.Expect(t => t.Invoke(graph)).Return(oneResult);

                var result = _calcer.InvokeForPath(graph, myPath);

                Assert.That(result, Is.EqualTo(distance + distance2 + distance3));
            }

            [Test]
            public void HandlesThreeLegResultMiddleDoesntMatch()
            {
                const string leg = "AZ";
                const int distance = 73;
                const string leg2 = "ZE";
                const int distance2 = 34;
                const string leg3 = "EM";
                const int distance3 = 399;
                var graph = new Dictionary<string, int> { { leg, distance }, { leg2, distance2 }, { leg3, distance3 } };
                const string myPath = "AZDM";

                var oneResult = new List<List<string>>{new List<string>{leg, leg2, leg3}};

                _traverser.Expect(t => t.Invoke(graph)).Return(oneResult);

                var result = _calcer.InvokeForPath(graph, myPath);

                Assert.That(result, Is.EqualTo(-1));
            }

            [Test]
            public void HandlesLongerMatchingLegResultMiddle()
            {
                const string leg = "AZ";
                const int distance = 73;
                const string leg2 = "ZD";
                const int distance2 = 34;
                const string leg3 = "DM";
                const int distance3 = 399;
                const string leg4 = "ME";
                const int distance4 = 393;
                var graph = new Dictionary<string, int>
                {
                    {leg, distance},
                    {leg2, distance2},
                    {leg3, distance3},
                    {leg4, distance4}
                };
                const string myPath = "AZDM";

                var oneResult = new List<List<string>>{new List<string>{leg, leg2, leg3, leg4}};

                _traverser.Expect(t => t.Invoke(graph)).Return(oneResult);

                var result = _calcer.InvokeForPath(graph, myPath);

                Assert.That(result, Is.EqualTo(distance + distance2 + distance3));
            }
        }
    }
}

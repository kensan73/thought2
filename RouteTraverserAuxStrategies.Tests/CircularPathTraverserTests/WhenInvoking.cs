using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace RouteTraverserAuxStrategies.Tests.CircularPathTraverserTests
{
    public partial class CircularPathTraverserTests
    {
        [TestFixture]
        
        public class WhenInvoking
        {
            private CircularPathTraverser _traverser;

            [SetUp]
            public void SetUp()
            {
                _traverser = new CircularPathTraverser();
            }

            [TearDown]
            public void TearDown()
            {

            }

            [Test]
            public void OnePathReturnsOne()
            {
                var traversal = new List<string>();
                var startLeg = "AB";

                _traverser.Invoke(startLeg, new Dictionary<string, int>{{startLeg, 3}}, ref traversal);

                Assert.That(traversal.Count, Is.EqualTo(1));
                Assert.That(traversal.First(), Is.EqualTo(startLeg));
            }

            [Test]
            public void TwoPathReturnsOneMatch()
            {
                var traversal = new List<string>();
                var startLeg = "AB";
                var startLegNotA = "CD";

                _traverser.Invoke(startLeg, new Dictionary<string, int>{{startLeg, 3}, {startLegNotA, 7}}, ref traversal);

                Assert.That(traversal.Count, Is.EqualTo(1));
                Assert.That(traversal.First(), Is.EqualTo(startLeg));
            }

            [Test]
            public void TwoLegsReturnsOne()
            {
                var traversal = new List<string>();
                var startLeg = "AB";
                var nextLeg = "BC";

                _traverser.Invoke(startLeg, new Dictionary<string, int> { {nextLeg, 7}, { startLeg, 3 } }, ref traversal);

                Assert.That(traversal.Count, Is.EqualTo(2));
                Assert.That(traversal.First(), Is.EqualTo(startLeg));
                Assert.That(traversal.Last(), Is.EqualTo(nextLeg));
            }

            [Test]
            public void GuardsAgainstCircularPathReturnsThePath()
            {
                var traversal = new List<string>();
                var startLeg = "AB";
                var nextLeg = "BA";
                var inputGraph = new Dictionary<string, int> { {nextLeg, 7}, { startLeg, 3 } };

                _traverser.Invoke(startLeg, inputGraph, ref traversal);

                Assert.That(traversal.Count, Is.EqualTo(inputGraph.Count*3+1));
                Assert.That(traversal.First(), Is.EqualTo(startLeg));
                Assert.That(traversal.Last(), Is.EqualTo(startLeg));
            }
        }
    }
}

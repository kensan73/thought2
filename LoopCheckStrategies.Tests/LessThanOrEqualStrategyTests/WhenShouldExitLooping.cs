using System.Collections.Generic;
using NUnit.Framework;

namespace LoopCheckStrategies.Tests.LessThanOrEqualStrategyTests
{
    public partial class LessThanOrEqualStrategyTests
    {
        [TestFixture]
        public class WhenInvoking
        {
            private const int Threshold = 3;
            private const string StartLeg = "AB";
            private LessThanOrEqualStrategy _strategy;
            private Dictionary<string, int> _inputGraph;

            [SetUp]
            public void SetUp()
            {
                _inputGraph = new Dictionary<string, int>();
                _strategy = new LessThanOrEqualStrategy(Threshold);
            }

            [TearDown]
            public void TearDown()
            {

            }

            [Test]
            public void IsLessThanThreshold()
            {
                var result = _strategy.ShouldExitLoop(StartLeg, _inputGraph, Threshold - 1);

                Assert.That(result, Is.False);
            }

            [Test]
            public void EqualThreshold()
            {
                var result = _strategy.ShouldExitLoop(StartLeg, _inputGraph, Threshold);

                Assert.That(result, Is.False);
            }

            [Test]
            public void ExceedsThreshold()
            {
                var result = _strategy.ShouldExitLoop(StartLeg, _inputGraph, Threshold+1);

                Assert.That(result, Is.True);
            }
        }
    }
}

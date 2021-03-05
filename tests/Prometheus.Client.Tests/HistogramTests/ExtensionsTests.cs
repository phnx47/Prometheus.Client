using System;
using NSubstitute;
using Xunit;

namespace Prometheus.Client.Tests.HistogramTests
{
    public class ExtensionsTests
    {
        [Fact]
        public void ObserveWithTs()
        {
            var metric = Substitute.For<IHistogram>();
            var ts = DateTimeOffset.UtcNow;
            var inc = 2;

            metric.Observe(inc, ts);

            metric.Received().Observe(inc, ts.ToUnixTimeMilliseconds());
        }

        [Fact]
        public void UnlabelledObserve()
        {
            var family = MockFamily();
            var val = 2;

            family.Observe(val);

            family.Unlabelled.Received().Observe(val);
        }

        [Fact]
        public void UnlabelledObserveWithTs()
        {
            var family = MockFamily();
            var ts = 123;
            var val = 2;

            family.Observe(val, ts);

            family.Unlabelled.Received().Observe(val, ts);
        }

        [Fact]
        public void UnlabelledObserveWithTsDate()
        {
            var family = MockFamily();
            var ts = DateTimeOffset.UtcNow;
            var val = 2;

            family.Observe(val, ts);

            family.Unlabelled.Received().Observe(val, ts.ToUnixTimeMilliseconds());
        }

        [Fact]
        public void UnlabelledTupleObserve()
        {
            var family = MockFamilyTuple();
            var val = 2;

            family.Observe(val);

            family.Unlabelled.Received().Observe(val);
        }

        [Fact]
        public void UnlabelledTupleObserveWithTs()
        {
            var family = MockFamilyTuple();
            var ts = 123;
            var val = 2;

            family.Observe(val, ts);

            family.Unlabelled.Received().Observe(val, ts);
        }

        [Fact]
        public void UnlabelledTupleObserveWithTsDate()
        {
            var family = MockFamilyTuple();
            var ts = DateTimeOffset.UtcNow;
            var val = 2;

            family.Observe(val, ts);

            family.Unlabelled.Received().Observe(val, ts.ToUnixTimeMilliseconds());
        }

        private IMetricFamily<IHistogram> MockFamily()
        {
            var metric = Substitute.For<IHistogram>();
            var family = Substitute.For<IMetricFamily<IHistogram>>();
            family.Unlabelled.Returns(metric);

            return family;
        }

        private IMetricFamily<IHistogram, (string, string)> MockFamilyTuple()
        {
            var metric = Substitute.For<IHistogram>();
            var family = Substitute.For<IMetricFamily<IHistogram, (string, string)>>();
            family.Unlabelled.Returns(metric);

            return family;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.Benchmarks.Counter
{
    [MemoryDiagnoser]
    [CoreJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class CounterCollection
    {
        private CollectorRegistry _registry;
        private MemoryStream _stream;

        [GlobalSetup]
        public void Setup()
        {
            _registry = new CollectorRegistry();
            var factory = new MetricFactory(_registry);
            var counter = factory.CreateCounter("counter", string.Empty, "label");
            counter.Inc();
            counter.WithLabels("test").Inc(2);

            _stream = new MemoryStream();
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _stream.Seek(0, SeekOrigin.Begin);
        }

        [Benchmark]
        public void Collect()
        {
            ScrapeHandler.ProcessAsync(_registry, _stream).GetAwaiter().GetResult();
        }
    }
}

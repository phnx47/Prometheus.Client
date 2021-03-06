[bmac]: https://www.buymeacoffee.com/phnx47
[ko-fi]: https://ko-fi.com/phnx47
[patreon]: https://www.patreon.com/phnx47

# Prometheus.Client

[![NuGet](https://img.shields.io/nuget/v/Prometheus.Client.svg)](https://www.nuget.org/packages/Prometheus.Client)
[![NuGet](https://img.shields.io/nuget/dt/Prometheus.Client.svg)](https://www.nuget.org/packages/Prometheus.Client)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT)

[![CodeFactor](https://www.codefactor.io/repository/github/prom-client-net/prom-client/badge)](https://www.codefactor.io/repository/github/prom-client-net/prom-client)
[![CI](https://img.shields.io/github/workflow/status/prom-client-net/prom-client/%F0%9F%92%BF%20CI%20Master?label=CI&logo=github)](https://github.com/prom-client-net/prom-client/actions/workflows/master.yml)
[![codecov](https://codecov.io/gh/prom-client-net/prom-client/branch/master/graph/badge.svg)](https://codecov.io/gh/prom-client-net/prom-client)

.NET Client library for [prometheus.io](https://prometheus.io/)  

It was started as a fork of [prometheus-net](https://github.com/prometheus-net/prometheus-net), but over time the library was evolved into a different product. Our main goals:

- Keep possibility of rapid development.
- Extensibility is one of the core values, together with performance and minimal allocation.
- We are open for suggestions and new ideas, contribution is always welcomed.

## Performance comparison with prometheus-net

![General use case benchmarks](/docs/benchmarks/generalcase.png)
Find more details on [benchmarks description](/docs/benchmarks/GeneralUseCase.md)

## Installation

```shell script
dotnet add package Prometheus.Client
```

## Configuration

[Examples](https://github.com/prom-client-net/prom-examples)

[Prometheus Docs](https://prometheus.io/docs/introduction/overview/)

## Quick start

1) Add IMetricFactory and ICollectorRegistry into DI container with extension library Prometheus.Client.DependencyInjection

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMetricFactory();
}
```

2) Add metrics endpoint

With Prometheus.Client.AspNetCore:

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    app.UsePrometheusServer();
}
```

Without extensions:

```c#
[Route("[controller]")]
public class MetricsController : Controller
{
    private readonly ICollectorRegistry _registry;

    public MetricsController(ICollectorRegistry registry)
    {
        _registry = registry;
    }

    [HttpGet]
    public async Task Get()
    {
        Response.StatusCode = 200;
        await using var outputStream = Response.Body;
        await ScrapeHandler.ProcessAsync(_registry, outputStream);
    }
}
```

For collect http requests, use Prometheus.Client.HttpRequestDurations.
It does not depend of Prometheus.Client.AspNetCore, however together it's very convenient to use:

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    app.UsePrometheusServer();
    app.UsePrometheusRequestDurations(); 
}
```

### Instrumenting

Four types of metric are offered: Counter, Gauge, Summary and Histogram.
See the documentation on [metric types](http://prometheus.io/docs/concepts/metric_types/)
and [instrumentation best practices](http://prometheus.io/docs/practices/instrumentation/#counter-vs.-gauge-vs.-summary)
on how to use them.

### Counter

Counters go up, and reset when the process restarts.

```c#
var counter = metricFactory.CreateCounter("myCounter", "some help about this");
counter.Inc(5.5);
```

### Gauge

Gauges can go up and down.

```c#
var gauge = metricFactory.CreateGauge("gauge", "help text");
gauge.Inc(3.4);
gauge.Dec(2.1);
gauge.Set(5.3);
```

### Summary

Summaries track the size and number of events.

```c#
var summary = metricFactory.CreateSummary("mySummary", "help text");
summary.Observe(5.3);
```

### Histogram

Histograms track the size and number of events in buckets.
This allows for aggregatable calculation of quantiles.

```c#
var hist = metricFactory.CreateHistogram("my_histogram", "help text", buckets: new[] { 0, 0.2, 0.4, 0.6, 0.8, 0.9 });
hist.Observe(0.4);
```

The default buckets are intended to cover a typical web/rpc request from milliseconds to seconds.
They can be overridden passing in the `buckets` argument.

### Labels

All metrics can have labels, allowing grouping of related time series.

See the best practices on [naming](http://prometheus.io/docs/practices/naming/)
and [labels](http://prometheus.io/docs/practices/instrumentation/#use-labels).

Taking a counter as an example:

```c#
var counter = metricFactory.CreateCounter("myCounter", "help text", labelNames: new []{ "method", "endpoint"});
counter.WithLabels("GET", "/").Inc();
counter.WithLabels("POST", "/cancel").Inc();
```

Since v4 there is alternative new way to provide a labels via ValueTuple that allow to reduce memory allocation:

```c#
var counter = metricFactory.CreateCounter("myCounter", "help text", labelNames: ("method", "endpoint"));
counter.WithLabels(("GET", "/")).Inc();
counter.WithLabels(("POST", "/cancel")).Inc();
```

## Extensions

AspNetCore Middleware: [Prometheus.Client.AspNetCore](https://github.com/prom-client-net/prom-client-aspnetcore)

```shell script
dotnet add package Prometheus.Client.AspNetCore
```

Standalone host: [Prometheus.Client.MetricServer](https://github.com/prom-client-net/prom-client-metricserver)

```shell script
dotnet add package Prometheus.Client.MetricServer
```

Push metrics to a PushGateway: [Prometheus.Client.MetricPusher](https://github.com/prom-client-net/prom-client-metricpusher)

```shell script
dotnet add package Prometheus.Client.MetricPusher
```

Collect http requests duration: [Prometheus.Client.HttpRequestDurations](https://github.com/prom-client-net/prom-client-httprequestdurations)

```shell script
dotnet add package Prometheus.Client.HttpRequestDurations
```

## Contribute

Contributions to the package are always welcome!

- Report any bugs or issues you find on the [issue tracker](https://github.com/prom-client-net/prom-client/issues).
- You can grab the source code at the package's [git repository](https://github.com/prom-client-net/prom-client).

## Support

If you like what I'm accomplishing, feel free to buy me a coffee

[<img align="left" alt="phnx47 | Buy Me a Coffe" width="32px" src="https://raw.githubusercontent.com/phnx47/files/master/button-sponsors/bmac0.png" />][bmac]
[<img align="left" alt="phnx47 | Kofi" width="32px" src="https://raw.githubusercontent.com/phnx47/files/master/button-sponsors/kofi0.png" />][ko-fi]
[<img align="left" alt="phnx47 | Patreon" width="32px" src="https://raw.githubusercontent.com/phnx47/files/master/button-sponsors/patreon0.png" />][patreon]

&nbsp;

## JetBrains - you're cool!

We much appreciate free Rider's licenses provided by JetBrains to support our library.

## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).

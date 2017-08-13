﻿// <copyright file="FitleredMetricsEndpointStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.TrackingMiddleware;
using App.Metrics.Counter;
using App.Metrics.Filtering;
using App.Metrics.Formatters.Json;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Integration.Facts.Startup
{
    public class FitleredMetricsEndpointStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMetricsEndpoint();
            app.UseMetricsTextEndpoint();
            app.UseMetricsAllMiddleware();

            SetupAppBuilder(app, env, loggerFactory);

            RecordSomeMetrics();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsOptions = new MetricsOptions
                                    {
                                        MetricsEnabled = true,
                                        DefaultOutputMetricsFormatter = new MetricsJsonOutputFormatter()
                                    };

            var endpointOptions = new MetricsEndpointsOptions();
            var trackingOptions = new MetricsTrackingMiddlewareOptions();

            SetupServices(
                services,
                appMetricsOptions,
                trackingOptions,
                endpointOptions,
                new DefaultMetricsFilter().WhereType(MetricType.Counter));
        }

        private void RecordSomeMetrics()
        {
            var counterOptions = new CounterOptions
                                 {
                                     Name = "test_counter",
                                     MeasurementUnit = Unit.Requests,
                                     Tags = new MetricTags("tag1", "value")
                                 };

            var meterOptions = new MeterOptions
                               {
                                   Name = "test_meter",
                                   MeasurementUnit = Unit.None,
                                   Tags = new MetricTags("tag2", "value")
                               };

            var timerOptions = new TimerOptions
                               {
                                   Name = "test_timer",
                                   MeasurementUnit = Unit.Requests
                               };

            var histogramOptions = new HistogramOptions
                                   {
                                       Name = "test_histogram",
                                       MeasurementUnit = Unit.Requests
                                   };

            var gaugeOptions = new GaugeOptions
                               {
                                   Name = "test_gauge"
                               };

            Metrics.Measure.Counter.Increment(counterOptions);
            Metrics.Measure.Meter.Mark(meterOptions);
            Metrics.Measure.Timer.Time(timerOptions, () => Metrics.Clock.Advance(TimeUnit.Milliseconds, 10));
            Metrics.Measure.Histogram.Update(histogramOptions, 5);
            Metrics.Measure.Gauge.SetValue(gaugeOptions, () => 8);
        }
    }
}
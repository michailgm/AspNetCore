﻿// <copyright file="DisabledMetricsTestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.TrackingMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Integration.Facts.Startup
{
    public class DisabledMetricsTestStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMetricsEndpoint();
            app.UseMetricsAllMiddleware();

            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsOptions = new MetricsOptions
                                    {
                                        DefaultContextLabel = "testing",
                                        MetricsEnabled = false
                                    };

            var endpointsOptions = new MetricsEndpointsOptions
                                   {
                                       MetricsTextEndpointEnabled = true,
                                       MetricsEndpointEnabled = true,
                                       PingEndpointEnabled = true
                                   };

            var trackingOptions = new MetricsTrackingMiddlewareOptions();

            SetupServices(services, appMetricsOptions, trackingOptions, endpointsOptions);
        }
    }
}
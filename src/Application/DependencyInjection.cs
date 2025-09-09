// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AiplBlazor.Application.Common.ExceptionHandlers;
using AiplBlazor.Application.Common.PublishStrategies;
using AiplBlazor.Application.Pipeline;
using AiplBlazor.Application.Pipeline.PreProcessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiplBlazor.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
      
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.NotificationPublisherType = typeof(ChannelBasedNoWaitPublisher);
            config.AddRequestPreProcessor(typeof(IRequestPreProcessor<>), typeof(ValidationPreProcessor<>));
            config.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
            config.AddOpenBehavior(typeof(FusionCacheBehaviour<,>));
            config.AddOpenBehavior(typeof(CacheInvalidationBehaviour<,>));

        });
      

        return services;
    }
    
}
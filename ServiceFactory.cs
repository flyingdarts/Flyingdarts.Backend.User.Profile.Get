
using System;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;

public static class ServiceFactory
{
    public static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddValidatorsFromAssemblyContaining<GetUserProfileQueryValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserProfileQuery).Assembly));
        return services.BuildServiceProvider();
    }
}
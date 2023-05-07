
using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using Flyingdarts.Shared;

public static class ServiceFactory
{
    public static ServiceProvider GetServiceProvider()
    {
        var configuration = new ConfigurationBuilder()
            .AddSystemsManager($"/{System.Environment.GetEnvironmentVariable("EnvironmentName")}/Application")
            .Build();
        var services = new ServiceCollection();
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonDynamoDB>(configuration.GetAWSOptions("DynamoDb"));
        services.AddOptions<ApplicationOptions>();
        services.AddTransient<IDynamoDBContext, DynamoDBContext>();
        services.AddValidatorsFromAssemblyContaining<GetUserProfileQueryValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserProfileQuery).Assembly));
        return services.BuildServiceProvider();
    }
}
using System.Linq;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Flyingdarts.Persistence;
using Flyingdarts.Shared;
using MediatR;
using Microsoft.Extensions.Options;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, APIGatewayProxyResponse>
{
    private readonly IDynamoDBContext _dbContext;
    private readonly ApplicationOptions _applicationOptions;
    public GetUserProfileQueryHandler(IDynamoDBContext dbContext, IOptions<ApplicationOptions> applicationOptions)
    {
        _dbContext = dbContext;
        _applicationOptions = applicationOptions.Value;
    }
    public async Task<APIGatewayProxyResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var queryItems = await _dbContext.FromQueryAsync<User>(QueryConfig(request.CognitoUserId), _applicationOptions.ToOperationConfig())
            .GetRemainingAsync(cancellationToken);

        // Handle the query results
        if (queryItems == null || !queryItems.Any())
            return new APIGatewayProxyResponse
            {
                StatusCode = 404
            };
        // At least one item was found
        // Use the fetched object(s) as needed
        var result = queryItems.First().Profile;

        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(new { action = "v2/user/profile/get", message = result })
        };

    }
    private static QueryOperationConfig QueryConfig(string cognitoUserId)
    {
        var queryFilter = new QueryFilter("PK", QueryOperator.Equal, Constants.User);
        queryFilter.AddCondition("SK", QueryOperator.BeginsWith, cognitoUserId);
        return new QueryOperationConfig { Filter = queryFilter };
    }
}
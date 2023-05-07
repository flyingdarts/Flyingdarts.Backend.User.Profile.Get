using System.Collections.Generic;
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
    private readonly ApplicationOptions _applicationOptions;
    private readonly IDynamoDBContext _dbContext;
    public GetUserProfileQueryHandler(IDynamoDBContext dbContext, IOptions<ApplicationOptions> applicationOptions)
    {
        _dbContext = dbContext;
        _applicationOptions = applicationOptions.Value;
    }
    public async Task<APIGatewayProxyResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        // Define the partition key value for the items you want to query
        string partitionKey = Constants.User;
        string sortKeyBeginsWithValue = request.CognitoUserId;

        // Create a query expression to fetch the items
        QueryOperationConfig queryConfig = new QueryOperationConfig
        {
            KeyExpression = new Expression
            {
                ExpressionStatement = "PartitionKey = :pk and begins_with(SortKey, :sk)",
                ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>
                {
                    { ":PK", partitionKey },
                    { ":SK", sortKeyBeginsWithValue }
                }
            }
        };
        // Call the QueryAsync method to execute the query
        IEnumerable<User> queryResults = await _dbContext.QueryAsync<User>(queryConfig, _applicationOptions.ToOperationConfig()).GetRemainingAsync(cancellationToken);

        // Handle the query results
        if (queryResults == null || !queryResults.Any())
            return new APIGatewayProxyResponse
            {
                StatusCode = 404
            };
        // At least one item was found
        // Use the fetched object(s) as needed
        var result = queryResults.First().Profile;

        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(result)
        };

    }
}
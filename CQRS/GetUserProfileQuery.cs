using Amazon.Lambda.APIGatewayEvents;
using MediatR;

public class GetUserProfileQuery : IRequest<APIGatewayProxyResponse>
{
    public string CognitoUserId { get; set; }
}
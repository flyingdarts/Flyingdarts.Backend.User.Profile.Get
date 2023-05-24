using Amazon.Lambda.APIGatewayEvents;
using MediatR;

public class GetUserProfileQuery : IRequest<APIGatewayProxyResponse>
{
    public string UserId { get; set; }
}
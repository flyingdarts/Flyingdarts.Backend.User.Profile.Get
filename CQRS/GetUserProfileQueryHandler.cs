using Amazon.Lambda.APIGatewayEvents;
using System.Threading;
using System.Threading.Tasks;
using Flyingdarts.Persistence;
using MediatR;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, APIGatewayProxyResponse>
{
    public GetUserProfileQueryHandler()
    {
    }
    public async Task<APIGatewayProxyResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        return new APIGatewayProxyResponse { StatusCode = 200 };
    }
}
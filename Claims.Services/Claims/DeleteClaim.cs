using Claims.Core;
using Claims.Infrastructure.AuditContext;
using Claims.Infrastructure.CosmosDb;
using MediatR;

namespace Claims.Services.Covers
{
    public class DeleteClaim
    {
        public class Request : IRequest<Response>
        {
            public string Id { get; set; }
        }

        public class Response
        {

        }

        public class Handle : IRequestHandler<Request, Response>
        {
            private readonly ICosmosDbService _cosmosDbService;
            private readonly IAuditer _auditer;

            public Handle(ICosmosDbService cosmosDbService, IAuditer auditer)
            {
                _auditer = auditer;
                _cosmosDbService = cosmosDbService;
            }

            async Task<Response> IRequestHandler<Request, Response>.Handle(Request request, CancellationToken cancellationToken)
            {
                await _auditer.AuditClaim(request.Id, "DELETE");
                await _cosmosDbService.DeleteItemAsync<Claim>(request.Id);

                return new Response();
            }
        }
    }
}

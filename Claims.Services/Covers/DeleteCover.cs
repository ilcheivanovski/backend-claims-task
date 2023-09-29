using Claims.Core;
using Claims.Infrastructure.AuditContext;
using MediatR;
using Microsoft.Azure.Cosmos;

namespace Claims.Services.Covers
{
    public class DeleteCover
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
            private readonly Container _container;
            private readonly IAuditer _auditer;

            public Handle(CosmosClient cosmosClient, IAuditer auditer)
            {
                _auditer = auditer;
                _container = cosmosClient?.GetContainer("ClaimDb", "Cover")
                     ?? throw new ArgumentNullException(nameof(cosmosClient));
            }

            async Task<Response> IRequestHandler<Request, Response>.Handle(Request request, CancellationToken cancellationToken)
            {
                await _auditer.AuditCover(request.Id, "DELETE");
                await _container.DeleteItemAsync<Cover>(request.Id, new(request.Id));

                return new Response();
            }
        }
    }
}

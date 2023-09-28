using Claims.Core;
using Claims.Infrastructure.AuditContext;
using Claims.Infrastructure.CosmosDb;
using FluentValidation;
using MediatR;

namespace Claims.Services.Claims
{
    public class CreateClaim
    {
        public class Request : IRequest<Response>
        {
            public string CoverId { get; set; }
            public string Name { get; set; }
            public ClaimType Type { get; set; }
            public decimal DamageCost { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.DamageCost).NotEmpty().ExclusiveBetween(0, 100001);
            }
        }



        public class Response
        {
            public string Id { get; set; }
            public string CoverId { get; set; }
            public DateTime Created { get; set; }
            public string Name { get; set; }
            public ClaimType Type { get; set; }
            public decimal DamageCost { get; set; }
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

                var validator = new Validator();
                var validationResult = await validator.ValidateAsync(request);
                var relatedCover = await _cosmosDbService.GetItemAsync<Cover>(request.CoverId);

                if (
                    validationResult.IsValid && relatedCover != null &&
                   (DateOnly.FromDateTime(DateTime.UtcNow) < relatedCover.StartDate ||
                    DateOnly.FromDateTime(DateTime.UtcNow) > relatedCover.EndDate)
                    )
                {
                    //throw new FluentValidation.ValidationException("Created date must be within the period of the related Cover");
                }

                var claim = new Claim();
                claim.Add(request.CoverId, request.Name, request.Type, request.DamageCost);

                await _cosmosDbService.AddItemAsync<Claim>(claim, claim.Id);
                //await _auditer.AuditClaim(claim.Id, "POST");

                return new Response()
                {
                    Id = claim.Id,
                    CoverId = claim.CoverId,
                    Created = claim.Created,
                    Name = claim.Name,
                    Type = claim.Type
                };
            }
        }
    }
}

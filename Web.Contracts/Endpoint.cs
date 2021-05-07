using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Contracts
{
    public static class Endpoint
    {
        public static class WithRequest<TRequest>
        {
            public abstract class WithResponse<TResponse> : EndpointBase
            {
                public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request, CancellationToken token = default);
            }

            public abstract class WithoutResponse : EndpointBase
            {
                public abstract Task<ActionResult> HandleAsync(TRequest request, CancellationToken token = default);
            }
        }

        public static class WithoutRequest
        {
            public abstract class WithResponse<TResponse> : EndpointBase
            {
                public abstract Task<ActionResult<TResponse>> HandleAsync(CancellationToken token = default);
            }

            public abstract class WithoutResponse : EndpointBase
            {
                public abstract Task<ActionResult> HandleAsync(CancellationToken token = default);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrilogyAvivaTest.Services.Rest
{
    public interface IRestService
    {
        Task<(ResultStatus status, TResponse payload, string rawResponse)> GetAsync<TResponse>(string url);
        Task<(ResultStatus status, TResponse payload, string rawResponse)> PostAsync<TRequest, TResponse>(TRequest request, string url);

        // TODO: PUT, DELETE.
    }

    public enum ResultStatus
    {
        Success = 0,
        ConnectionFailed,
        Unauthorized,
        BadResponse,
        BadPayload,
        // Map HttpStatus codes to additional enum members here ...
        Other
    }
}

using Application.DTOs.Log;
using Application.Parameters;
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface ILoggerService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Task<PagedResponse<IEnumerable<GetLogDto>>> GetLogsPagedAsync(RequestParameter parameter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public void LogInfo(CreateLogRequest request);
    }
}
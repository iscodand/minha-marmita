using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTOs.Log;
using Application.Parameters;
using Application.Wrappers;
using Domain.Entities;
using Serilog;

namespace Infrastructure.Shared.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILoggerRepository _loggerRepository;

        public LoggerService(ILoggerRepository loggerRepository)
        {
            _loggerRepository = loggerRepository;
        }

        public async Task<PagedResponse<IEnumerable<GetLogDto>>> GetLogsPagedAsync(RequestParameter parameter)
        {
            IEnumerable<ApplicationLog> logs = await _loggerRepository.GetPagedResponseAsync(
                parameter.PageNumber,
                parameter.PageSize
            );
            int totalItems = await _loggerRepository.CountAsync();

            return new PagedResponse<IEnumerable<GetLogDto>>(
                data: GetLogDto.Map(logs),
                pageNumber: parameter.PageNumber,
                pageSize: parameter.PageSize,
                totalItems: totalItems
            );
        }

        public void LogInfo(CreateLogRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

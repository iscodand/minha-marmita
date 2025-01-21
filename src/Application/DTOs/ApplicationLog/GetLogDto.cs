using Domain.Entities;

namespace Application.DTOs.Log
{
    public class GetLogDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Action { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }

        public static IEnumerable<GetLogDto> Map(IEnumerable<ApplicationLog> logs)
        {
            return logs.Select(log => new GetLogDto()
            {
                Id = log.Id,
                Message = log.Message,
                TimeStamp = log.TimeStamp,
                Action = log.Action,
                UserId = log.UserId,
                CompanyId = log.CompanyId
            });
        }
    }
}
namespace Application.DTOs.Log
{
    public class CreateLogRequest
    {
        public string Action { get; set; }
        public string Message { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }

        public CreateLogRequest()
        {

        }

        public CreateLogRequest(string request, string response)
        {
            Request = request;
            Response = response;
        }
    }
}
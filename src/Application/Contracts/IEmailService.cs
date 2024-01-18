using Application.DTOs.Email;

namespace Application.Contracts
{
    public interface IEmailService
    {
        public Task SendMailAsync(SendMailRequest request);
    }
}
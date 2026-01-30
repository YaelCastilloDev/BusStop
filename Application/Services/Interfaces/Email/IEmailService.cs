using Application.DTOs.Auth;
namespace Application.Services.Interfaces.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequestDto request);

    }
}

using Application.Models;

namespace Application.Contracts.Gateways
{
    public interface IEmailSender
    {
         Task<bool> SendEmail(Email email);
    }
}
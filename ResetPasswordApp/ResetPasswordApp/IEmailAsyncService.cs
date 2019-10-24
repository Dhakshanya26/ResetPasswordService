using System.Threading.Tasks;

namespace ResetPasswordApp
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string toAddress, string subject, string toName, string resetPasswordUrl);

    }
}

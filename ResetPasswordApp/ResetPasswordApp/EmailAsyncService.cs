using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ResetPasswordApp
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private string _applicationEmail;
        private string _applicationeName;
        private string _applicationpassword;
        private readonly IConfiguration _configuration;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            var emailSection = _configuration.GetSection("email");

            _applicationEmail = emailSection.GetSection("adminEmail").Value;
            _applicationeName = "Qudoosupport";
            _applicationpassword = emailSection.GetSection("adminPassword").Value;
        }



        public async Task<bool> SendEmail(string toAddress, string subject, string toName, string resetPasswordUrl)
        {

            //For testing
            toAddress = "dhakshanya26@gmail.com";
            _logger.LogInformation($"Sending Email to  {toAddress}");
            var fromAddress = new MailAddress(_applicationEmail, _applicationeName);
            string fromPassword = _applicationpassword;
            var emailContent = new StringBuilder();
            using (var streamReader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "ResetPassword.html")))
            {
                var line = streamReader.ReadLine();
                while (line != null)
                {
                    emailContent.Append(line);
                    line = streamReader.ReadLine();
                }
                streamReader.Close();
            }

            emailContent.Replace("{{USERNAME}}", toName)
                .Replace("{{RESETPASSWORD_URL}}", resetPasswordUrl);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress.Address, toAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = emailContent.ToString(),
            })
            {
                try
                {
                    await smtp.SendMailAsync(message);
                    return true;
                }
                catch (Exception)
                {
                    _logger.LogInformation($"Error occurred on sending Email to  {toAddress}");
                    return false;
                }
            }
        }
    }
}

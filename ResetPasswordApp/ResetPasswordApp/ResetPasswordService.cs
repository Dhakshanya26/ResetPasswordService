using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ResetPasswordApp
{
    public class ResetPasswordService
    {
        private MyDbContext _dbContext;
        private readonly IEmailService _emailService;
        private ILogger<ResetPasswordService> _logger;
        public ResetPasswordService(MyDbContext dbContext, IEmailService emailService, ILogger<ResetPasswordService> logger)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task SendPasswordResetEmail()
        {
            try
            {
                string emailSubject = "Resend Password";
                string resetPasswordUrl = "resetPasswordUrl";
                var userList = _dbContext.Users.FromSqlRaw("select Id,UserName as Name,EmailAddress From Users;").ToList();
                if (userList.Any())
                {
                    foreach (var user in userList)
                    {
                        try
                        {
                            if (CheckWhetherEmailSentDetail(user, emailSubject))
                            {
                                await _emailService.SendEmail(user.EmailAddress, emailSubject, user.Name,
                                    resetPasswordUrl);
                                _logger.LogInformation($"Email sent to {user.Name}");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation($"Exception occurred on sending email to {user.EmailAddress}");
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception occurred on sending email");
                throw ex;
            }
        }

        private void AddSentEmailDetail(UserDto userDto, string resetPasswordUrl, string subject, bool isSuccess)
        {
            if (userDto != null && !string.IsNullOrEmpty(userDto.EmailAddress))
            {
                _dbContext.SentEmailDtos.Add(new SentEmailDto()
                {
                    EmailXml = resetPasswordUrl,
                    IsSentSuccessful = isSuccess,
                    Recipient = userDto.EmailAddress,
                    SentOn = DateTime.Now,
                    Subject = subject
                });
                _dbContext.SaveChanges();
            }
        }

        private bool CheckWhetherEmailSentDetail(UserDto userDto, string subject)
        {
            if (userDto != null && !string.IsNullOrEmpty(userDto.EmailAddress))
            {
                var response = _dbContext.SentEmailDtos.Where(x => x.Recipient == userDto.EmailAddress
                                                                   && x.Subject == subject).ToList();
                return response.Any();
            }

            return false;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Application.Common.Models;

namespace NutritionalRecipeBook.Infrastructure.Services
{
    public class EmailSender : IEmailSender<User>
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailSender(IOptionsSnapshot<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }

        public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        {
            var smtpClient = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailConfiguration.UserName, _emailConfiguration.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfiguration.From, _emailConfiguration.FromName),
                Subject = "Confirm Email",
                Body = confirmationLink
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }
    }
}

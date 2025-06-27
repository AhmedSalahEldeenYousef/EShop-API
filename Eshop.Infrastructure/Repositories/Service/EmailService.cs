using Eshop.Core.DTO;
using Eshop.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net.Mail;
namespace Eshop.Infrastructure.Repositories.Service
{
    public class EmailService : IEmailService
    {
        //SMTP ==> Simple mail transfer protocol

        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(EmailDto emailDto)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("My EShop", _configuration["EmailSettings:From"]));
            message.Subject = emailDto.Subject;
            message.To.Add(new MailboxAddress(emailDto.To, emailDto.To));
            // message.Body = new TextPart("plain")
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDto.Content
            };

            //message.From.Add(new MailboxAddress(emailDto.From, emailDto.From));
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(
                        _configuration["EmailSettings:smtp"],
                        int.Parse(_configuration["EmailSettings:Port"]),
                        true
                        //MailKit.Security.SecureSocketOptions.StartTls).ConfigureAwait(false);
                        );

                    await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                    await smtp.SendAsync(message);

                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    throw new Exception("Error sending email", ex);
                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }
        }
    }
}

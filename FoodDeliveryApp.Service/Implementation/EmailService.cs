using FoodDeliveryApp.Domain.Email;
using FoodDeliveryApp.Service.Interface;
using MimeKit;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using FoodDeliveryApp.Repository;
using Microsoft.Extensions.Options;

namespace FoodDeliveryApp.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ApplicationDbContext _context;

        public EmailService(IOptions<EmailSettings> emailSettings, ApplicationDbContext context)
        {
            _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SendEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SmtpUserName));
            emailMessage.Sender = new MailboxAddress(_emailSettings.EmailDisplayName, _emailSettings.SmtpUserName);
            emailMessage.To.Add(new MailboxAddress(message.MailTo, message.MailTo));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = message.Content };

            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    await smtp.ConnectAsync(
                        _emailSettings.SmtpServer,
                        _emailSettings.SmtpServerPort,
                        _emailSettings.Enablessl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto
                    );

                    await smtp.AuthenticateAsync(_emailSettings.SmtpUserName, _emailSettings.SmtpPassword);
                    await smtp.SendAsync(emailMessage);
                    await smtp.DisconnectAsync(true);

                    message.Status = true;
                    _context.EmailMessages.Add(message);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                message.Status = false;
                _context.EmailMessages.Add(message);
                await _context.SaveChangesAsync();
                throw new ApplicationException("Failed to send email", ex);
            }
        }
    }
}

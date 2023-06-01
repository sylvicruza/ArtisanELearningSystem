using System.Net.Mail;
using System.Net;

namespace ArtisanELearningSystem.Utils
{
    public class EmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailSender(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var fromAddress = new MailAddress(_smtpUsername, "ArtisanELearningSystemAdmin");
            var toAddress = new MailAddress(to);
            var smtp = new SmtpClient
            {
                Host = _smtpServer,
                Port = _smtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _smtpPassword)
            };
            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            await smtp.SendMailAsync(message);
        }


        // Usage Create an email account for FeedBackSystem.
        // var emailService = new EmailService("smtp.gmail.com", 587, "your-email@gmail.com", "your-password");
        // await emailService.SendEmailAsync(user.Email, "Welcome to MyApp", "Thank you for signing up for MyApp!");
    }
}

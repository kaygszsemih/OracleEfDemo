using Microsoft.Extensions.Options;
using OracleEfDemo.Dtos;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace OracleEfDemo.Helpers
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail)
        {
            var smptClient = new SmtpClient
            {
                Host = _emailSettings.Host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                From = new MailAddress(_emailSettings.Email, "Lidersan Info"),
                Subject = "Lidersan| Şifre Sıfırlama Bağlantısı",
                Body = @$"
                  <h4>Şifrenizi yenilemek için aşağıdaki linkte tıklayınız.</h4><br/>
                  <p><a href='{resetPasswordEmailLink}'>şifre yenileme link</a></p>"
            };

            mailMessage.To.Add(ToEmail);

            await smptClient.SendMailAsync(mailMessage);
        }
    }
}

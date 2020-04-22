namespace SmtpScaler
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public sealed class SmtpClientWrapper : IDisposable
    {
        private readonly SmtpClient smtpClient;
        private readonly EmailSettings emailSettings;

        public bool Busy { get; private set; }

        public SmtpClientWrapper(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings ?? throw new ArgumentNullException(nameof(emailSettings));

            this.smtpClient = new SmtpClient(emailSettings.ServerIp, emailSettings.Port)
            {
                UseDefaultCredentials = false,
                EnableSsl = emailSettings.EnableSsl,
                Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password)
            };
        }

        public void SetAsBusy()
        {
            this.Busy = true;
        }

        public async Task SendAsync(string subject, string body, params string[] recipients)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException(nameof(subject), "value cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException(nameof(body), "value cannot be null or empty");
            }

            if (recipients == null || recipients.Length == 0 || !recipients.Any(m => !string.IsNullOrWhiteSpace(m)))
            {
                throw new ArgumentException("Email should contain at least 1 valid recipient");
            }

            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(emailSettings.Sender, "No Reply");
                    mailMessage.Sender = mailMessage.From;
                    mailMessage.Body = body;
                    mailMessage.Subject = subject;

                    mailMessage.IsBodyHtml = true;

                    foreach (var recipient in recipients)
                    {
                        if (!string.IsNullOrWhiteSpace(recipient))
                        {
                            mailMessage.To.Add(recipient);
                        }
                    }

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            finally
            {
                this.Busy = false;
            }
        }

        public void Dispose()
        {
            smtpClient.Dispose();
        }
    }
}
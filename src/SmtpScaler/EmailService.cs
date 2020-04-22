namespace SmtpScaler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class EmailService : IDisposable
    {
        private readonly List<SmtpClientWrapper> wrappers;
        private readonly EmailSettings emailSettings;
        private readonly object locker = new Object();

        public EmailService(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
            this.wrappers = new List<SmtpClientWrapper>();
        }

        public async Task SendAsync(string subject, string body, params string[] recipients)
        {
            SmtpClientWrapper wrapper;

            lock (locker)
            {
                wrapper = PromoteWrapper();
                Console.WriteLine($"wrappers count {this.wrappers.Count}");
            }

            await wrapper.SendAsync(subject, body, recipients);
        }

        private SmtpClientWrapper PromoteWrapper()
        {
            var wrapper = this.wrappers.FirstOrDefault(m => !m.Busy);
            
            if (wrapper == null)
            {
                wrapper = new SmtpClientWrapper(emailSettings);
                this.wrappers.Add(wrapper);
            }

            wrapper.SetAsBusy();
            return wrapper;
        }

        public void Dispose()
        {
            if (this.wrappers.Any())
            {
                foreach (var wrapper in this.wrappers)
                {
                    wrapper.Dispose();
                }
            }
        }
    }
}
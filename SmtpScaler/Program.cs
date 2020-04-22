using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmtpScaler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var emails = EmailProvider.Generate(300);
            var emailSettings = Config.GetSection<EmailSettings>(sectionName: "emailSettings");
            var emailService = new EmailService(emailSettings);

            emails.AsParallel().ForAll(async m =>
            {
                await emailService.SendAsync(m, $"Some content {Guid.NewGuid()}", m);
                Console.WriteLine(m);
            });

            "Hit enter to exit".Print(ConsoleColor.Yellow);
            Console.ReadLine();
        }
    }
}

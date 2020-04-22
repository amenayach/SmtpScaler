using System;
using System.Diagnostics;
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
            var stopwatch = new Stopwatch();

            using (var emailService = new EmailService(emailSettings, 40))
            {
                stopwatch.Start();
                var tasks = emails.AsParallel().Select(async m =>
                {
                    await emailService.SendAsync(m, $"Some content {Guid.NewGuid()}", m);
                    Console.WriteLine(m);
                });

                await Task.WhenAll(tasks);

                stopwatch.Stop();
                
                $"Elapsed {stopwatch.ElapsedMilliseconds}ms".Print();
            }

            "Hit enter to exit".Print(ConsoleColor.Yellow);
            Console.ReadLine();
        }
    }
}

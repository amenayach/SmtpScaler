namespace SmtpScaler
{
    using System;
    using System.Threading.Tasks;

    public static class Extensions
    {
        public static void Print(this string message, ConsoleColor color = ConsoleColor.Green)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }

        public static async Task Retry(Func<Task> action, int times =  5)
        {
            var retry = 0;

            while (retry++ < times)
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex)
                {
                    if (retry >=  times )
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}

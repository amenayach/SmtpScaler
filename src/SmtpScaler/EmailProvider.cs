namespace SmtpScaler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EmailProvider
    {
        public static IEnumerable<string> Generate(int count)
        {
            if (count < 1)
            {
                return Array.Empty<string>();
            }

            return Enumerable.Range(1, count).Select(m => $"mail{m}@yopmail.com");
        }
    }
}
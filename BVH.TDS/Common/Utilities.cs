using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BVH.TDS
{
    public static class Utilities
    {
        private static Random rand = new Random();
        public static string RandomString(int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var c = pool[rand.Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }

        public static string GetCookie(CookieContainer container, string url, string cookieName)
        {
            var result = String.Empty;

            IEnumerable<Cookie> responseCookies = container.GetCookies(new Uri(url)).Cast<Cookie>();
            if (responseCookies != null)
            {
                var cookie = responseCookies.FirstOrDefault(_ => _.Name == cookieName);
                if (cookie != null)
                {
                    result = cookie.Value;
                }

            }

            return result;
        }
    }
}

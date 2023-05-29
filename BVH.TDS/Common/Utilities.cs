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

            //const string pool = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string poolLower = "abcdefghijklmnopqrstuvwxyz";
            const string poolNum = "0123456789";
            const string poolUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string poolSymbol = "@$!";
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                int selectPool = i > 3 ? rand.Next(0, 5) % 4 : i;
                var c = new char();
                if (selectPool == 0)
                {
                    c = poolLower[rand.Next(0, poolLower.Length)];
                }
                else if (selectPool == 1)
                {
                    c = poolNum[rand.Next(0, poolNum.Length)];
                }
                else if (selectPool == 2)
                {
                    c = poolUpper[rand.Next(0, poolUpper.Length)];
                }
                else if (selectPool == 3)
                {
                    c = poolSymbol[rand.Next(0, poolSymbol.Length)];
                }
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

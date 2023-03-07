using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AnyCaptchaHelper;
using System.Text.RegularExpressions;

namespace BVH.TDS.Proxy
{
    public class CaptchaProxy
    {
        private static string CAPTCHA_API_Key = "a832c1afb5e7455e9889154fb518f590";

        public string ResolveCaptcha(string siteKey, string siteUrl)
        {
            var recaptchaV3ProxylessRequest = new AnyCaptcha().RecaptchaV3Proxyless(CAPTCHA_API_Key, siteKey, siteUrl, "add_uid", false);
            if (!recaptchaV3ProxylessRequest.IsSuccess)
            {
                throw new Exception("resolve captcha failed");
            }

            return recaptchaV3ProxylessRequest.Result;
        }
    }
}

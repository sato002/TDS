﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace BVH.TDS
{
    public class TDSProxy
    {
        private static string TDS_URL = "https://traodoisub.com";
        private static string COOKIE_NAME = "PHPSESSID";
        private CookieContainer _cookieContainer { get; set; }
        private HttpClient _client { get; set; }
        private HttpClientHandler _handler { get; set; }
        public TDSProxy()
        {
            SetupHttpRequest();
        }

        private void SetupHttpRequest()
        {
            _cookieContainer = new CookieContainer();
            _handler = new HttpClientHandler() { CookieContainer = _cookieContainer };
            _client = new HttpClient(_handler) { BaseAddress = new Uri(TDS_URL) };
            _client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36");
            _client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
        }

        public async Task<TDSAccountInfor> GetAccInfor(AccountInfor row)
        {
            try
            {
                row.State = "Đang đợi lấy token";
                var payload = new FormUrlEncodedContent(new[] {
                            new KeyValuePair<string, string>("username", row.Username),
                            new KeyValuePair<string, string>("password", row.Password),
                        });
                var response = await _client.PostAsync("/scr/login2.php", payload);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                if (String.IsNullOrEmpty(content))
                {
                    throw new Exception("content null");
                }
                return JsonConvert.DeserializeObject<TDSAccountInfor>(content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Login(AccountInfor row)
        {
            try
            {
                row.State = "Đang đợi lấy token";
                var payload = new FormUrlEncodedContent(new[] {
                            new KeyValuePair<string, string>("username", row.Username),
                            new KeyValuePair<string, string>("password", row.Password),
                        });
                var result = await _client.PostAsync("/scr/login.php", payload);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();
                if (String.IsNullOrEmpty(content))
                {
                    throw new Exception("content null");
                }
                var cookie = Utilities.GetCookie(_cookieContainer, TDS_URL, COOKIE_NAME);
                if (String.IsNullOrEmpty(cookie))
                {
                    throw new Exception("cookie null");
                }
            }
            catch (Exception ex)
            {
                row.State = $"Lỗi khi đăng nhập: {ex.Message}!";
                throw ex;
            }
        }

        public async Task<string> ChangePassword(AccountInfor row, string newPass)
        {
            var payload = new FormUrlEncodedContent(new[] {
                                new KeyValuePair<string, string>("oldpass", row.Password),
                                new KeyValuePair<string, string>("newpass", newPass),
                                new KeyValuePair<string, string>("renewpass", newPass)
                            });
            var result = await _client.PostAsync($"/scr/doipass.php", payload);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<TDSResponse<TDSNhanXu>> NhanXuTim(AccountInfor row)
        {
            var result = await _client.GetAsync($"/api/coin/?type=TIKTOK_LIKE&id=TIKTOK_LIKE_API&access_token={row.AccessToken}");
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            if (String.IsNullOrEmpty(content))
            {
                throw new Exception("content null");
            }
            return JsonConvert.DeserializeObject< TDSResponse<TDSNhanXu>>(content);
        }
        public async Task<string> TangXu(AccountInfor row, string userNhanXu, string soXu)
        {
            var payload = new FormUrlEncodedContent(new[] {
                                new KeyValuePair<string, string>("usernhan", userNhanXu),
                                new KeyValuePair<string, string>("xutang", soXu)
                            });

            var result = await _client.PostAsync($"/view/tangxu/tangxu.php", payload);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> GetGcaptchaKey(string accessToken)
        {
            string siteKey = String.Empty;

            var resultAutoclick = await _client.GetAsync($"/api/autoclick/?access_token={accessToken}&type=tiktok_follow");
            resultAutoclick.EnsureSuccessStatusCode();

            var html = await resultAutoclick.Content.ReadAsStringAsync();
            string pattern = @"grecaptcha.execute\('(.*?)',";
            foreach (Match match in Regex.Matches(html, pattern))
            {
                if (match.Success && match.Groups.Count > 0)
                {
                    siteKey = match.Groups[1].Value;
                }
            }

            if (String.IsNullOrEmpty(siteKey))
            {
                throw new Exception("sitekey is null");
            }

            return siteKey;
        }

        public async Task<TDSResponse<TDSAccountInfor>> ConfigTiktok(AccountInfor row, string captchaPass)
        {
            var payload = new FormUrlEncodedContent(new[] {
                                new KeyValuePair<string, string>("recaptcha_response", captchaPass)
                            });

            var result = await _client.PostAsync($"/api/?fields=tiktok_run&id={row.TikUsername}&access_token={row.AccessToken}", payload);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TDSResponse<TDSAccountInfor>>(content);
        }


    }
}

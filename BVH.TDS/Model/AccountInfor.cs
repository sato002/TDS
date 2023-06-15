namespace BVH.TDS
{
    public class AccountInfor
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
        public string QuickLink {
            get
            {
                string value = "";
                if (!string.IsNullOrEmpty(AccessToken))
                {
                    value = $"https://traodoisub.com/api/autoclick/?access_token={AccessToken}&type=tiktok_follow";
                }
                return value;
            }
        }
        public string TikUsername { get; set; }
        public int Coin { get ; set; }
        public string State { get; set; }
        public int CoinDie { get; set; }
    }
}

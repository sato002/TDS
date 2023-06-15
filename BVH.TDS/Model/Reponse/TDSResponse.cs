namespace BVH.TDS
{
    public class TDSResponse
    {
        public int success { get; set; }
    }
    public class TDSResponse<T> : TDSResponse
    {
        public T data { get; set; }
    }
    public class TDSAccountInfor
    {
        public string user { get; set; }
        public string xu { get; set; }
        public string access_token { get; set; }
        public string error { get; set; }
        public string xudie { get; set; }
    }
    public class TDSNhanXu
    {
        public string xu { get; set; }
        public string job_success { get; set; }
        public string xu_them { get; set; }
        public string msg { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BVH.TDS
{
    public class TDSResponse2
    {
        public bool success { get; set; }
    }

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
        public string xudie { get; set; }
    }

    public class TDSLogin2
    {
        public string user { get; set; }
        public string xu { get; set; }
        public string access_token { get; set; }
        public string error { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public string request_token { get; set; }
        public string session_id { get; set; }
    }
}

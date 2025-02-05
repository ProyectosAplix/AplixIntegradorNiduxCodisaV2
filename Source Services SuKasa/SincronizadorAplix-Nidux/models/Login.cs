using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorAplix_Nidux.models
{
    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
        public int storeId { get; set; }
    }

    public class ResponseLogin
    {
        public string token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}

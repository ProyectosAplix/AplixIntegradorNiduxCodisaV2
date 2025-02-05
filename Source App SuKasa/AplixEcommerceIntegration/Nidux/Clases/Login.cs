using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    class Login
    {
        public string token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }
    }
}

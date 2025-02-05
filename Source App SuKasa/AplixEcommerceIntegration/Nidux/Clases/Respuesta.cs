using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    public class Respuesta
    {
        public string estado { get; set; }
        public string id { get; set; }
        public string comentarios { get; set; }
        public string sku { get; set; }
        public bool isSuccessful { get; set; }
        public string message { get; set; }
        public IList<string> error { get; set; }
    }
}

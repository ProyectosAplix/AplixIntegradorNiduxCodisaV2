using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.WooCommerce.Clases
{
     public class BitacorasWoocomerce
    {

        public string id_bitacora { get; set; }
        public string fecha { get; set; }
        public string procedimiento { get; set; }
        public string error  { get; set; }
        public string modulo { get; set; }


    }

    public class BitacorasFechasWoocomerce
    {
        public string fecha_ini { get; set; }
        public string fecha_fin { get; set; }

    }

}

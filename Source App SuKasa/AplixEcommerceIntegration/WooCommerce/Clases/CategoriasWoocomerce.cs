using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.WooCommerce.Clases
{
    public class CategoriasWoocomerce
    {

        public int id { get; set; }
        public int parent { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string description { get; set; }
        public string estado { get; set; }


    }


    public class BusquedaCategoriasWoocomerce
    {
        public string dato { get; set; }
        public string parametro { get; set; }

    }


}

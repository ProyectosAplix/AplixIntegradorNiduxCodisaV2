using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.WooCommerce.Clases
{
    public class TerminosAtributoWoocomerce
    {

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string slug { get; set; }
        public int codigo_atributo { get; set; }

    }

    public class TerminosAtributosCompletoWoocomerce
    {
        public int id_atributo { get; set; }
        public string name_atributo { get; set; }
        public int id_termino_atributo { get; set; }
        public string name_termino_atributo { get; set; }
        public string description_termino_atributo { get; set; }
        public string parametro { get; set; }
        public string slug { get; set; }
        public string estado { get; set; }

    }

    public class BusquedaTerminoAtributosWoocomerce
    {
        public string dato { get; set; }
        public string parametro { get; set; }

    }


}

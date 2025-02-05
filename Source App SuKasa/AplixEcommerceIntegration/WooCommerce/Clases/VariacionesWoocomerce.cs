using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.WooCommerce.Clases
{
    public class VariacionesWoocomerce
    {
        public int id { get; set; }
        public string sku { get; set; }
        public string articulo { get; set; }
  

    }


    public class ArtributosArticulos {

        public string articulo { get; set; }
        public int id_atributo { get; set; }
        public int id_termino_atributo { get; set; }  
        public string estado { get; set; }

    }

    public class ArticulosVariaciones
    {
        public string articulo { get; set; }
        public string articulo_padre { get; set; }
        public int id_termino { get; set; }
        public string id { get; set; }
        public string precio { get; set; }
        public int cantidad { get; set; }
        public string estado { get; set; }
        public string peso { get; set; }
        public string nombre { get; set; }
        public string descuento { get; set; }
        public string termino_lista { get; set; }

        //este valor simplemente se usa para no convertir esas cantidades a enteros en los greed
        public string cantidad_en_greed { get; set; }
        public string usa_stock { get; set; }
        public string sincroniza { get; set; }



    }


    public class VariacionesMantenimientoWoocomerce {

        public int id { get; set; }
        public string descripcion { get; set; }
        public int sku { get; set; }
        public string price { get; set; }
        public string regular_price { get; set; }
        public int status { get; set; }
        public string manage_stock { get; set; }
        public string weight { get; set; }
        public List<AtributosVariacionesWoocomerce> attributes { get; set; }


    }


    public class AtributosVariacionesWoocomerce {

        public int id { get; set; }
        public string name { get; set; }
        public string option { get; set; }

    }


    public class CrearVariacionesWoocomerce {
        public string description { get; set; }
        public string sku { get; set; }
        public string sale_price { get; set; }
        public string regular_price { get; set; }
        public string status { get; set; }
        public bool manage_stock { get; set; }
        public int stock_quantity { get; set; }
        public string weight { get; set; }
        public List<AtributosVariacionesWoocomerce> attributes { get; set; }

    }

    public class ActualizarVariacionWoocomerce {

        public int id { get; set; }
        public string description { get; set; }
        public string sku { get; set; }
        public string sale_price { get; set; }
        public string regular_price { get; set; }
        public string status { get; set; }
        public bool manage_stock { get; set; }
        public int stock_quantity { get; set; }
        public string weight { get; set; }
        public List<AtributosVariacionesWoocomerce> attributes { get; set; }
    }

        public class PadresVariacionWoocomerce
        {
            public int id { get; set; }
            public string sku { get; set; }

        }

    public class RespuestaIdVariacionWoocomerce {
        public int id { get; set; }
        public string sku { get; set; }

    }

}

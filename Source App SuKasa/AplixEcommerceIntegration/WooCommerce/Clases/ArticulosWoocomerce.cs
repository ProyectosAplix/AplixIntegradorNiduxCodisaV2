using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.WooCommerce.Clases
{
    public class ArticulosWoocomerce
    {

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string short_description { get; set; }
        public string sku { get; set; }
        public string type { get; set; }
        public string sale_price { get; set; }
        public string regular_price { get; set; }
        public string stock_quantity { get; set; }
        public string stock_status { get; set; }
        public string tax_class { get; set; }
        public string weight { get; set; }
        public List<CategoriasArticulosWoocomerce> categories { get; set; }
        public string sincroniza { get; set; }
        public string status { get; set; }
        public bool manage_stock { get; set; }
        public bool featured { get; set; }

    }

    public class CategoriasArticulosWoocomerce
    {
        public int id { get; set; }
    }

    public class ArticulosBusquedaWoocomerce
    {

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string short_description { get; set; }
        public string sku { get; set; }
        public string type { get; set; }
        public string sale_price { get; set; }
        public string regular_price { get; set; }
        public string stock_quantity { get; set; }
        public string stock_status { get; set; }
        public string tax_class { get; set; }
        public string weight { get; set; }
        public string sincroniza { get; set; }
        public string status { get; set; }
        public string manage_stock { get; set; }
        public string featured { get; set; }
        public string categories { get; set; }

        public string dato { get; set; }

        public string parametro { get; set; }


    }

    public class EstadosArticuloWoocomerce
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }

    public class EstadosStockWoocomerce
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }


    public class TipoArticuloWoocomerce
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }


    public class AtributosWoocomerceActualizar
    {
        public int id { get; set; }
        public bool visible { get; set; }
        public bool variation { get; set; }
        public IList<string> options { get; set; }

    }
    public class AtributosPadreWoocomerce
    {
        public IList<AtributosWoocomerceActualizar> attributes { get; set; }


    }

    public class ActualizarArticulosAtributos
    {

        public int id { get; set; }
        public string sku { get; set; }
    }

    public class AtributosPadre
    {
        public int id { get; set; }

    }

    public class ValoresAtributosPadre
    {
        public string name { get; set; }

    }

}

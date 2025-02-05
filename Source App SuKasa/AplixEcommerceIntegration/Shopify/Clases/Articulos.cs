using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Shopify.Clases
{
    //Objetos para la lista que se va editando
    public class Articulos
    {
        public string sku { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string peso { get; set; }
        public string cantidad { get; set; }
        public string precio { get; set; }
        public string descuento { get; set; }
        public string impuesto { get; set; }
        public string estado { get; set; }
        public string sincronizar { get; set; }
        public string id_shopify { get; set; }

    }

    //Objetos para la lista que nos traemos del API de azrticulos sin variantes
    public class Products
    {
        public string title { get; set; }
        public string body_html { get; set; }
        public string status { get; set; }
        public string published_scope { get; set; } //global
        public List<Variant> variants { get; set; }
    }

    public class Variant
    {
        public string price { get; set; }
        public string sku { get; set; }
        public string inventory_policy { get; set; }//deny
        public string compare_at_price { get; set; }
        public string inventory_management { get; set; }//shopify
        public string taxable { get; set; }
        public string weight { get; set; }
        public string weight_unit { get; set; }//kg
        public string inventory_quantity { get; set; }
    }

    //Objetos para el id de respuesta de Shopify
    public class respuesta
    {
        public Product product { get; set; }
    }

    public class Product
    {
        public string id { get; set; }

        public List<Variant_Respuesta> variants { get; set; }
    }

    public class Variant_Respuesta
    {
        public string id { get; set; }
        public string sku { get; set; }
    }


    //objetos para la lista de los articulos con variantes que nos traemos del API
    public class Articulos_Padre
    {
        public string sku { get; set; }
        public string title { get; set; }
        public string body_html { get; set; }
        public string status { get; set; }
        public string published_scope { get; set; }
    }
    public class Articulos_Variantes
    {
        public Product_Variantes product { get; set; }
    }

    public class Product_Variantes
    {
        public string title { get; set; }
        public string body_html { get; set; }
        public string status { get; set; }
        public string published_scope { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Variant_Variantes> variants { get; set; }
        public List<Option_Variantes> options { get; set; }
    }

    public class Variant_Variantes
    {
        public string option1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string option2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string option3 { get; set; }
        public string price { get; set; }
        public string sku { get; set; }
        public string inventory_policy { get; set; }
        public string inventory_management { get; set; }
        public int inventory_quantity { get; set; }
    }

    public class Option_Variantes
    {
        public string name { get; set; }
        public string [] values { get; set; }
    }

    //objetos que nos traemos del API del articulo que acaba de ser modificado sin variantes

    public class productos
    {
        public productos_edit product { get; set; }
    }

    public class productos_edit
    {
        public long id { get; set; }
        public string title { get; set; }
        public string body_html { get; set; }
        public string status { get; set; }
        public List<Variantes_edit> variants { get; set; }
    }

    public class Variantes_edit
    {
        public string price { get; set; }
        public string sku { get; set; }
        public string inventory_policy { get; set; }//deny
        public string compare_at_price { get; set; }
        public string inventory_management { get; set; }//shopify
        public bool taxable { get; set; }
        public decimal weight { get; set; }
        public string weight_unit { get; set; }//kg
        public int inventory_quantity { get; set; }
    }

    //objetos que nos ayudan con la actualizacion de los articulos que se man modificado para articulos con variantes
    public class productos_variantes
    {
        public productos_edit_variantes product { get; set; }
    }

    public class productos_edit_variantes
    {
        public long id { get; set; }
        public string title { get; set; }
        public string body_html { get; set; }
        public string status { get; set; }
        public List<Productos_Variantes> variants { get; set; }
        public List<Opciones> options { get; set; }
    }

    public class Productos_Variantes
    {
        public string price { get; set; }
        public string sku { get; set; }
        public string option1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string option2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string option3 { get; set; }
        public int inventory_quantity { get; set; }
    }

    public class Opciones
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] values { get; set; }

    }

    //objetos para eliminar las variantes de un articulo

    public class Variantes_Delete
    {
        public string articulo { get; set; }
        public string variante { get; set; }
    }

    //objetos para la obtencion de los productod de shopify

    public class shopify
    {
        public List<productos_shopify> products { get; set; }
    }

    public class productos_shopify
    {
        public long id { get; set; }
        public string title { get; set; }
        public string body_html { get; set; }
        public string status { get; set; }
        public List<variants_shopify> variants { get; set; }
        public List<Opciones_shopify> options { get; set; }
    }

    public class variants_shopify
    {
        public string id { get; set; }
        public string price { get; set; }
        public string sku { get; set; }
        public string compare_at_price { get; set; }
        public string taxable { get; set; }
        public string weight { get; set; }
        public string inventory_quantity { get; set; }
        public int position { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
    }

    public class Opciones_shopify
    {
        public string name { get; set; }
        public string[] values { get; set; }

    }

}

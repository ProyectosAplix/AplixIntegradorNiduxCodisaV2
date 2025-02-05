using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Shopify.Clases
{
    //Clases para agregar colecciones a shopify
    public class Colecciones
    {
        public CustomCollection custom_collection { get; set; }
    }

    public class CustomCollection
    {
        public string id { get; set; }
        public string title { get; set; }
        public string body_html { get; set; }
        
    }

    //Clases con los objetos que nos ayudan a llenar la lista para cuando modifican alguna coleccion en la tabla y para las busquedas
    public class Coleccion
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string activo { get; set; }
    }

    //Clases que obtiene toda la lista de las colecciones
    public class Collection
    {
        public List<CustomCollections> custom_collections { get; set; }
    }

    public class CustomCollections
    {
        public string id { get; set; }
        public string title { get; set; }
        public string body_html { get; set; }

    }

    //Clases que obtiene las colecciones
    public class collect
    {
        public string product_id { get; set; }
        public string collection_id { get; set; }
        public string sku { get; set; }
    }

    //Clases para la respuesta de las colecciones
    public class respuesta_coleccion
    {
        public collects collect { get; set; }
    }

    public class collects
    {
        public string id { get; set; }
    }

    //objetos para las colecciones asociadas al articulo

    public class product_colecciones
    {
        public List<product_collects> collects { get; set; }
    }

    public class product_collects
    {
        public long id { get; set; }
        public long collection_id { get; set; }
        public long product_id { get; set; }
    }
}

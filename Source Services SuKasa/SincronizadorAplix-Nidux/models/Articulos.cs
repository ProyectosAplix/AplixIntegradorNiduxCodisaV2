using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorAplix_Nidux.models
{

    public class productos
    {
        public List<Articulos> products { get; set; }
    }
    public class Articulos
    {
        public string id { get; set; }

        public string id_marca { get; set; }

        public int[] categorias { get; set; }

        public string sku { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }

        public string precio { get; set; }

        public string costo_shipping_individual { get; set; }

        public string peso_producto { get; set; }

        public string porcentaje_oferta { get; set; }

        public string estado_de_producto { get; set; }

        public string es_destacado { get; set; }

        public int stock_principal { get; set; }

        public string video_youtube_url { get; set; }

        public string ocultar_indicador_stock { get; set; }

        public string producto_permite_reservacion { get; set; }

        public string limite_para_reservar_en_carrito { get; set; }

        public string porcentaje_para_reservar { get; set; }

        public string usar_gif_en_homepage { get; set; }

        public string gif_tiempo_transicion { get; set; }

        public int impuesto_producto { get; set; }

        public string[] tags { get; set; }

        public string[] seo_tags { get; set; }

        public List<traduccion> traducciones { get; set; }
    }

    public class traduccion
    {
        public string lang_id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

    }

    public class Respuesta
    {

        public string estado { get; set; }
        public string id { get; set; }
        public string comentarios { get; set; }
        public string sku { get; set; }
        //public bool isSuccessful { get; set; }
        //public string message { get; set; }
        public IList<string> error { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplixAPI.Models
{
    public class articulos
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

        public List<traduccion> traducciones { get; set; }

        public List<variaciones> variations { get; set; }
    }

    public class traduccion
    {
        public string lang_id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

    }

    public class variaciones
    {
        public int variationId { get; set; }
        public string nombre { get; set; }
        public string variationSubstring { get; set; }
        public string sku { get; set; }
        public int precio { get; set; }
        public int stock { get; set; }
        public int peso { get; set; }
        public string product_name_full { get; set; }

    }

    public class productos
    {
        public List<articulos> products { get; set; }
    }
}
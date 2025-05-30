using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplixAPI.Models
{
    public class articuloPrueba
    {
        public Producto producto { get; set; }
    }
    public class Variations
    {
        public string variationId { get; set; } /*id nidux hijo*/
        public string nombre { get; set; } /*U_APX_NOMBRE*/
        public string variationSubstring { get; set; } /*atributos*/
        public string sku { get; set; } /*buscamos en softland*/
        public int precio { get; set; }
        public int stock { get; set; }
        public int peso { get; set; }
        public string product_name_full { get; set; }

    }

    public class Traduccion
    {
        public string lang_id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

    }
    public class Producto
    {
        public string id { get; set; }

        public string id_marca { get; set; }

        public int[] categorias { get; set; }

        public string sku { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }

        public string precio { get; set; }

        public string costo_shipping_individual { get; set; }

        public int peso_producto { get; set; }

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

        public List<Traduccion> traducciones { get; set; }

        public List<Variations> variations { get; set; }

    }
}
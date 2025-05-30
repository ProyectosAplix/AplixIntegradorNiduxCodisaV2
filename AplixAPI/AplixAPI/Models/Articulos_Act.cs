using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplixAPI.Models
{
    public class Articulos_Act
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

        public int impuesto_producto { get; set; }

        public string usar_gif_en_homepage { get; set; }

        public string gif_tiempo_transicion { get; set; }

        public string[] tags { get; set; }

        public string[] seo_tags { get; set; }

        public string cabys { get; set; }

        public string codigo_tarifa { get; set; }

        public string Skip_factura { get; set; }

        public List<traduccion_act> traducciones { get; set; }

        public List<dimensiones> dimensiones { get; set; }
        
    }
    public class traduccion_act
    {
        public string lang_id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

    }

    public class dimensiones
    {
        public string longitud { get; set; }
        public string ancho { get; set; }
        public string alto { get; set; }
    }

    public class productos_act
    {
        public List<articulos> products { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    //Nada más se usa para la lista de la tabla para la funcion de buscar
    public class Articulos
    {
        public string id { get; set; }

        public string id_marca { get; set; }

        public string categorias { get; set; }

        public string sku { get; set; }

        public string nombre { get; set; }

        public string nombre_nidux { get; set; }

        public string descripcion { get; set; }

        public string descripcion_nidux { get; set; }

        public string precio { get; set; }

        public string costo_shipping_individual { get; set; }

        public string peso_producto { get; set; }

        public string porcentaje_oferta { get; set; }

        public string estado_de_producto { get; set; }

        public string es_destacado { get; set; }

        public string stock_principal { get; set; }

        public string video_youtube_url { get; set; }

        public string ocultar_indicador_stock { get; set; }

        public string producto_permite_reservacion { get; set; }

        public string limite_para_reservar_en_carrito { get; set; }

        public string porcentaje_para_reservar { get; set; }

        public string usar_gif_en_homepage { get; set; }

        public string gif_tiempo_transicion { get; set; }

        public string lang_id { get; set; }

        public string lang_nombre { get; set; }

        public string lang_descripcion { get; set; }

        public string impuesto { get; set; }

        public string activo { get; set; }

        public string atributos { get; set; }

        public string id_padre { get; set; }

        public string id_hijo { get; set; }

        public string tags { get; set; }

        public string seo_tags { get; set; }


    }

    //objetos que nos sirven para sincronizar los articulos a Nidux
    public class Articulos_Nidux
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

        public string[] tags { get; set; }

        public string[] seo_tags { get; set; }

        public int impuesto_producto { get; set; }

        public List<traduccion_act> traducciones { get; set; }
    }
    public class traduccion_act
    {
        public string lang_id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

    }

    //Variaciones
    public class Variaciones_Padres
    {
        public string padre { get; set; }
        public string ID { get; set; }
        public string sku_hijo { get; set; }
    }
    public class Valores_Hijos_Atributos
    {
        public int[] id_atributos { get; set; }
    }

    //objetos que nos ayudan a traer los artoculos de nidux para nuestras tablas
    public class productos
    {
        public List<articulos> products { get; set; }
    }

    public class articulos
    {
        public string id { get; set; }

        public string id_marca { get; set; }

        public int[] categorias { get; set; }

        public string sku { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }

        public string precio { get; set; }

        public string fecha_creacion { get; set; }

        public string fecha_modificacion { get; set; }

        public string costo_shipping_individual { get; set; }

        public decimal peso_producto { get; set; }

        public string porcentaje_oferta { get; set; }

        public string estado_de_producto { get; set; }

        public string es_destacado { get; set; }

        public int stock_principal { get; set; }

        public int grado_importancia { get; set; }

        public string video_youtube_url { get; set; }

        public string ocultar_indicador_stock { get; set; }

        public string producto_permite_reservacion { get; set; }

        public string limite_para_reservar_en_carrito { get; set; }

        public string porcentaje_para_reservar { get; set; }

        public string impuesto_producto { get; set; }

        public string usar_gif_en_homepage { get; set; }

        public string gif_tiempo_transicion { get; set; }

        public string[] tags { get; set; }

        public string[] seo_tags { get; set; }

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
        public decimal precio { get; set; }
        public int stock { get; set; }
        public decimal peso { get; set; }
        public string product_name_full { get; set; }

    }

    //para eliminar los hijos que son padres pero que nada más son hijos
    public class articulos_eliminados
    {
        public string SKU { get; set; }
    }
}

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
        public string ID { get; set; }
    }


    public class ProductosResponse
    {
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int productos_por_pagina { get; set; }
        public int paginas_disponibles { get; set; }
        public Dictionary<string, Product> Products { get; set; }
    }

    public class Product
    {
        public int id { get; set; }
        public int brand_id { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string product_desc_short { get; set; }
        public string product_description { get; set; }
        public decimal product_price { get; set; }
        public DateTime product_date { get; set; }
        public DateTime product_last_updated { get; set; }
        public decimal product_shipping { get; set; }
        public decimal product_weight { get; set; }
        public decimal product_sale { get; set; }
        public int product_status { get; set; }
        public int product_home { get; set; }
        public int product_stock { get; set; }
        public string product_video { get; set; }
        public int product_hidestock { get; set; }
        public int product_reserve { get; set; }
        public int? product_reserve_limit { get; set; }
        public decimal? product_reserve_percentage { get; set; }
        public decimal? product_tax { get; set; }
        public int product_priority { get; set; }
        public int product_gif_enable { get; set; }
        public int product_gif_transition { get; set; }
        public string package_id { get; set; }
        public string measured_code_package { get; set; }
        public string measured_package_total { get; set; }
        public string measured_sale_mode { get; set; }
        public string measured_code_sale { get; set; }
        public string measured_sale_unit { get; set; }
        public string measured_sale_min { get; set; }
        public string measured_sale_max { get; set; }
        public Dictionary<string, string> product_tags { get; set; }
        public Dictionary<string, string> product_keywords { get; set; }
        public Dictionary<string, Variation> variations { get; set; }
        public CabysL cabys { get; set; }
        public Dictionary<string, string> codigos_alternos { get; set; }
        public Dictionary<string, string> categorias { get; set; }
        public Dictionary<string, string> imagenes { get; set; }
        public int stockVariation { get; set; }
        public Dictionary<string, TraduccionA> traducciones { get; set; }
    }

    public class CabysL
    {
        public string Cabys { get; set; }
        public string CodigoTarifa { get; set; }
        public int SkipFactura { get; set; }
    }

    public class TraduccionA
    {
        public int product_id { get; set; }
        public int lang_id { get; set; }
        public string product_name { get; set; }
        public string product_desc_short { get; set; }
        public string product_description { get; set; }
    }

    public class Variation
    {
        public int VariationId { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Substring { get; set; }
        public Dictionary<string, Attribute> Attributes { get; set; }
    }

    public class Attribute
    {
        public int attribute_id { get; set; }
        public string attribute_name { get; set; }
        public string attribute_style { get; set; }
        public string value_name { get; set; }
    }

    public partial class Articulos2
    {
        public Producto Producto { get; set; }
    }
    #region Para la consulta a NIDUX de datos en art padres
    //Para la consulta a NIDUX de datos en art padres
    //lo que esta comentado es para evitar errores ya que solo necesito en este caso el id y la descripcion
    //lo dejo porque si se necesitara algun otro dato ya esta hecha la estructura de objetos y clases solo descomentar el que se necesita
    public partial class Producto
    {
        public long id { get; set; }
        //public long IdMarca { get; set; }
        //public string Sku { get; set; }
        //public string Nombre { get; set; }
        public string product_description { get; set; }
        //public string Precio { get; set; }
        //public DateTimeOffset FechaCreacion { get; set; }
        //public DateTimeOffset FechaModificacion { get; set; }
        //public long CostoShippingIndividual { get; set; }
        //public long PesoProducto { get; set; }
        public long product_sale { get; set; }
        //public long EstadoDeProducto { get; set; }
        //public long EsDestacado { get; set; }
        //public long StockPrincipal { get; set; }
        //public Uri VideoYoutubeUrl { get; set; }
        //public long OcultarIndicadorStock { get; set; }
        //public long ProductoPermiteReservacion { get; set; }
        //public long LimiteParaReservarEnCarrito { get; set; }
        //public long PorcentajeParaReservar { get; set; }
        //public long ImpuestoProducto { get; set; }
        //public long GradoImportancia { get; set; }
        //public long UsarGifEnHomepage { get; set; }
        //public long GifTiempoTransicion { get; set; }
        public Dictionary<string, int> Categorias { get; set; }
        //public List<object> SeoTags { get; set; }
        //public List<object> Tags { get; set; }
        //public List<object> CodigosAlternos { get; set; }
        ////public Cabys Cabys { get; set; }
        //public List<Traduccione> Traducciones { get; set; }
    }
    //public partial class Cabys
    //{
    //    public string CabysCabys { get; set; }
    //    public string CodigoTarifa { get; set; }
    //    public long? SkipFactura { get; set; }
    //}
    //public partial class Traduccione
    //{
    //    public long LangId { get; set; }
    //    public string Nombre { get; set; }
    //    public object ProductDescShort { get; set; }
    //    public string Descripcion { get; set; }
    //}
    #endregion
}

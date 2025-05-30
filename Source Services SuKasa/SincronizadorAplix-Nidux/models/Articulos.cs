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

    #region Variaciones
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

    #endregion

    #region Eliminar Art
    //para eliminar los hijos que son padres pero que nada más son hijos
    public class articulos_eliminados
    {
        public string SKU { get; set; }
        public string ID { get; set; }
    }
    #endregion

    public class Atributos_Padre
    {
        public int[] atributos { get; set; }
    }

    public class Variaciones
    {
        public int id { get; set; }
        public int[] id_valores_atributos { get; set; }
        public string sku_variacion { get; set; }
        public decimal peso { get; set; }
        public int stock { get; set; }
        public decimal precio { get; set; }

    }
}

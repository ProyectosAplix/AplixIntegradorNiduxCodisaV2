using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplixAPI.Models
{
    public class Estado
    {
        public int orderId { get; set; }
        public int nuevo_estado { get; set; }
    }

    public class Pedidos
    {
        public Ordenes ordenes { get; set; }
    }

    public class Ordenes
    {
        public int totalRegistros { get; set; }
        public int totalRegistrosAMostrarConFiltros { get; set; }
        public int paginas_totales { get; set; }
        public int pagina_actual { get; set; }
        public Dictionary<string, Data> data { get; set; }
    }

    public partial class Data
    {
        public int orderId { get; set; }
        public string consumer_name { get; set; }
        public string consumer_lastname { get; set; }
        public string consumer_identification { get; set; }
        public string consumer_email { get; set; }
        public string consumer_tel { get; set; }
        public string consumer_cel { get; set; }
        public int consumer_anon { get; set; }
        public string order_tax_amount { get; set; }
        public DateTime order_date { get; set; }
        public string orderGiftpointsUsed { get; set; }
        public string order_status { get; set; }
        public string tipo_envio { get; set; }
        public string costo_shipping { get; set; }
        public string tasa_impuesto_shipping { get; set; }
        public string order_trackingCode { get; set; }
        public string moneda { get; set; }
        public string moneda_fe { get; set; }
        public string codigo_metodo_pago { get; set; }
        public string observaciones { get; set; }
        public string codigo_autorizacion { get; set; }
        public string ip_origen { get; set; }
        public string order_payment_status { get; set; }
        public string method_type { get; set; }
        public string total { get; set; }
        public string cuponUsado { get; set; }
        public string cuponTipo { get; set; }
        public string sucursal { get; set; }
        public string order_pickup { get; set; }
        public string costo_total_shipping { get; set; }
        public Dictionary<string, Detalles> Detalles { get; set; }
        public Direcciones direcciones { get; set; }
    }

    public class Detalles
    {
        public int orderId { get; set; }
        public int id_producto { get; set; }
        public string id_variacion { get; set; }
        public string sku { get; set; }
        public string nombre_producto { get; set; }
        public string precio { get; set; }
        public int cantidad { get; set; }
        public string porcentaje_descuento { get; set; }
        public string subtotal_descuento { get; set; }
        public string subtotal_linea { get; set; }
        public int impuesto { get; set; }
        public string subtotal_impuestos { get; set; }
        public Cabys cabys { get; set; }
    }

    public class Cabys
    {
        public string cabys { get; set; }
        public string codigoTarifa { get; set; }
        public bool skipFactura { get; set; }
    }

    public class Direcciones
    {
        public Envio envio { get; set; }
        public Facturacion facturacion { get; set; }
    }

    public class Envio
    {
        public string nombre_destinatario { get; set; }
        public string apellido_destinatario { get; set; }
        public string identificacion { get; set; }
        public string tipo_identificacion { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string movil { get; set; }
        public string pais { get; set; }
        public string provincia { get; set; }
        public string canton { get; set; }
        public string distrito { get; set; }
        public string detalle_direccion { get; set; }
        public string ciudad { get; set; }
        public string zip { get; set; }
        public string geo_latitud { get; set; }
        public string geo_longitud { get; set; }
    }

    public class Facturacion
    {
        public string nombre_destinatario { get; set; }
        public string identificacion { get; set; }
        public string tipo_identificacion { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string movil { get; set; }
        public string pais { get; set; }
        public string provincia { get; set; }
        public string canton { get; set; }
        public string distrito { get; set; }
        public string detalle_direccion { get; set; }
        public string ciudad { get; set; }
        public string zip { get; set; }
        public string geo_latitud { get; set; }
        public string geo_longitud { get; set; }
        public int provincia_fe { get; set; }
        public int? canton_fe { get; set; }
        public int? distrito_fe { get; set; }
    }
}
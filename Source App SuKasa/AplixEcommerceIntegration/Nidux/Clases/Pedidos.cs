using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    //está en una clase aparte
    //public class Estados
    //{
    //    public int orderId { get; set; }

    //    public int nuevo_estado { get; set; }
    //}
    public class Pedidos
    {
        public Ordenes ordenes { get; set; }
    }
    public class Ordenes
    {
        public IList<int> totalRegistros { get; set; }
        public IList<int> totalRegistrosAMostrarConFiltros { get; set; }
        public IList<int> paginas_totales { get; set; }
        public IList<int> pagina_actual { get; set; }
        public IList<Data> data { get; set; }
    }
    public class Data
    {
        public int orderId { get; set; }
        public int? wish_id { get; set; }
        public string cliente { get; set; }
        public string identificacion { get; set; }
        public string correo { get; set; }
        public string telefono_fijo { get; set; }
        public string telefono_movil { get; set; }
        public int es_anonimo { get; set; }
        public string monto_impuestos { get; set; }
        public DateTime fecha_orden { get; set; }
        public string orderGiftpointsUsed { get; set; }
        public string estado_orden { get; set; }
        public string moneda { get; set; }
        public string observaciones { get; set; }
        public string codigo_autorizacion { get; set; }
        public string ip_origen { get; set; }
        public string estado_pago { get; set; }
        public string medio_pago { get; set; }
        public string total { get; set; }
        public string cuponUsado { get; set; }
        public string cuponTipo { get; set; }
        public string sucursal { get; set; }
        public string recoger_sucursal { get; set; }
        public string tipo_envio { get; set; }
        public string costo_total_shipping { get; set; }
        public string tasa_impuesto_shipping { get; set; }
        public string codigo_metodo_pago { get; set; }
        public string moneda_fe { get; set; }
        public IList<Detalles> detalles { get; set; }
        public Direcciones direcciones { get; set; }
    }
    public class Detalles
    {
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
    }
    public class Direcciones
    {
        public Envio envio { get; set; }
        public Facturacion facturacion { get; set; }
    }
    public class Envio
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
        public int canton_fe { get; set; }
        public int distrito_fe { get; set; }
    }
}

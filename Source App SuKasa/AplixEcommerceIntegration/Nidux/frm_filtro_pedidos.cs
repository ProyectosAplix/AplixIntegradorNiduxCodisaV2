using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplixEcommerceIntegration.Nidux
{
    public partial class frm_filtro_pedidos : Form
    {
        public int n = 0;
        public frm_filtro_pedidos()
        {
            InitializeComponent();
        }

        private void btnFiltro_Pedidos_Click(object sender, EventArgs e)
        {
            try
            {
                actualizarFiltrosPedidos_BD();
                n = 1;
                this.Close();
            }
            catch (Exception)
            {
                n = 0;
                throw;
            }
        }

        private void frm_filtro_pedidos_Load(object sender, EventArgs e)
        {
            cargarCheckBox_FiltroPedidos();
        }

        private void cargarCheckBox_FiltroPedidos()
        {
            try
            {

                List<Clases.filtro_pedidos> filtro_Pedidos = new List<Clases.filtro_pedidos>();
                Metodos.met_filtro_pedidos met_Filtro_Pedidos = new Metodos.met_filtro_pedidos();

                filtro_Pedidos = met_Filtro_Pedidos.obtenerFiltroPedidos();

                // LOS CHECKBOX POR DEFECTO APARECEN ACTIVOS
                // ACA PREGUNTA SI EL VALOR EN BD ES 0 Y DESACTIVA LOS CHECKBOXS
                if (filtro_Pedidos[0].numero_orden == 0) { chk_numero_orden.Checked = false; }
                if (filtro_Pedidos[0].nombre_cliente == 0) { chk_nombre_cliente.Checked = false; }
                if (filtro_Pedidos[0].identificacion == 0) { chk_identificacion.Checked = false; }
                if (filtro_Pedidos[0].correo_cliente == 0) { chk_correo_cliente.Checked = false; }
                if (filtro_Pedidos[0].tel_fijo == 0) { chk_tel_fijo.Checked = false; }
                if (filtro_Pedidos[0].tel_movil == 0) { chk_tel_movil.Checked = false; }
                if (filtro_Pedidos[0].comprador_anonimo == 0) { chk_comprador_anonimo.Checked = false; }
                if (filtro_Pedidos[0].monto_impuesto == 0) { chk_monto_impuesto.Checked = false; }
                if (filtro_Pedidos[0].fecha == 0) { chk_fecha.Checked = false; }
                if (filtro_Pedidos[0].order_gif == 0) { chk_order_gif.Checked = false; }

                if (filtro_Pedidos[0].estado_orden == 0) { chk_estado_orden.Checked = false; }
                if (filtro_Pedidos[0].moneda == 0) { chk_moneda.Checked = false; }
                if (filtro_Pedidos[0].observaciones == 0) { chk_observaciones.Checked = false; }
                if (filtro_Pedidos[0].cod_autorizacion == 0) { chk_cod_autorizacion.Checked = false; }
                if (filtro_Pedidos[0].ip_origen == 0) { chk_ip_origen.Checked = false; }
                if (filtro_Pedidos[0].estado_pago == 0) { chk_estado_pago.Checked = false; }
                if (filtro_Pedidos[0].medio_pago == 0) { chk_medio_pago.Checked = false; }
                if (filtro_Pedidos[0].total_orden == 0) { chk_total_orden.Checked = false; }
                if (filtro_Pedidos[0].uso_cupon == 0) { chk_uso_cupon.Checked = false; }
                if (filtro_Pedidos[0].tipo_cupon == 0) { chk_tipo_cupon.Checked = false; }

                if (filtro_Pedidos[0].sucursal == 0) { chk_sucursal.Checked = false; }
                if (filtro_Pedidos[0].recoger_sucursal == 0) { chk_recoger_sucursal.Checked = false; }
                if (filtro_Pedidos[0].metodo_pago == 0) { chk_metodo_pago.Checked = false; }
                if (filtro_Pedidos[0].moneda_facturacion == 0) { chk_moneda_facturacion.Checked = false; }
                if (filtro_Pedidos[0].tipo_envio == 0) { chk_tipo_envio.Checked = false; }
                if (filtro_Pedidos[0].costo_envio == 0) { chk_costo_envio.Checked = false; }
                if (filtro_Pedidos[0].costo_impuesto_envio == 0) { chk_costo_impuesto_envio.Checked = false; }
                if (filtro_Pedidos[0].nombre_destinatario == 0) { chk_nombre_destinatario.Checked = false; }
                if (filtro_Pedidos[0].identificacion_envio == 0) { chk_identificacion_envio.Checked = false; }
                if (filtro_Pedidos[0].tipo_identi_envio == 0) { chk_tipo_identi_envio.Checked = false; }

                if (filtro_Pedidos[0].correo_envio == 0) { chk_correo_envio.Checked = false; }
                if (filtro_Pedidos[0].telefono_envio == 0) { chk_telefono_envio.Checked = false; }
                if (filtro_Pedidos[0].tel_movil_envio == 0) { chk_tel_movil_envio.Checked = false; }
                if (filtro_Pedidos[0].pais_envio == 0) { chk_pais_envio.Checked = false; }
                if (filtro_Pedidos[0].provincia_envio == 0) { chk_provincia_envio.Checked = false; }
                if (filtro_Pedidos[0].canton_envio == 0) { chk_canton_envio.Checked = false; }
                if (filtro_Pedidos[0].distrito_envio == 0) { chk_distrito_envio.Checked = false; }
                if (filtro_Pedidos[0].detalle_direccion_envio == 0) { chk_detalle_dir_envio.Checked = false; }
                if (filtro_Pedidos[0].ciudad_envio == 0) { chk_ciudad_envio.Checked = false; }
                if (filtro_Pedidos[0].codigo_zip_envio == 0) { chk_codigo_zip_envio.Checked = false; }

                if (filtro_Pedidos[0].posicion_latitud == 0) { chk_posicion_latitud.Checked = false; }
                if (filtro_Pedidos[0].posicion_longitud == 0) { chk_posicion_longitud.Checked = false; }
                if (filtro_Pedidos[0].nombre_destinatario_fac == 0) { chk_nombre_dest_fac.Checked = false; }
                if (filtro_Pedidos[0].identificacion_fac == 0) { chk_identificacion_fac.Checked = false; }
                if (filtro_Pedidos[0].tipo_id_fac == 0) { chk_tipo_id_fac.Checked = false; }
                if (filtro_Pedidos[0].correo_fac == 0) { chk_correo_fac.Checked = false; }
                if (filtro_Pedidos[0].telefono_fac == 0) { chk_telefono_fac.Checked = false; }
                if (filtro_Pedidos[0].tel_movil_fac == 0) { chk_tel_movil_fac.Checked = false; }
                if (filtro_Pedidos[0].pais_fac == 0) { chk_pais_fac.Checked = false; }
                if (filtro_Pedidos[0].provincia_fac == 0) { chk_provincia_fac.Checked = false; }

                if (filtro_Pedidos[0].canton_fac == 0) { chk_canton_fac.Checked = false; }
                if (filtro_Pedidos[0].distrito_fac == 0) { chk_distrito_fac.Checked = false; }
                if (filtro_Pedidos[0].detalle_dir_fac == 0) { chk_detalle_dir_fac.Checked = false; }
                if (filtro_Pedidos[0].ciudad_fac == 0) { chk_ciudad_fac.Checked = false; }
                if (filtro_Pedidos[0].codigo_zip_fac == 0) { chk_cod_zip_fac.Checked = false; }
                if (filtro_Pedidos[0].posicion_latitud_fac == 0) { chk_posicion_latitud_fac.Checked = false; }
                if (filtro_Pedidos[0].posicion_longitud_fac == 0) { chk_posicion_longitud_fac.Checked = false; }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void actualizarFiltrosPedidos_BD()
        {
            try
            {
                Clases.filtro_pedidos filtro_Pedidos = new Clases.filtro_pedidos();
                Metodos.met_filtro_pedidos met_Filtro_Pedidos = new Metodos.met_filtro_pedidos();

                filtro_Pedidos.numero_orden = Convert.ToInt32(chk_numero_orden.Checked);
                filtro_Pedidos.nombre_cliente = Convert.ToInt32(chk_nombre_cliente.Checked);
                filtro_Pedidos.identificacion = Convert.ToInt32(chk_identificacion.Checked);
                filtro_Pedidos.correo_cliente = Convert.ToInt32(chk_correo_cliente.Checked);
                filtro_Pedidos.tel_fijo = Convert.ToInt32(chk_tel_fijo.Checked);
                filtro_Pedidos.tel_movil = Convert.ToInt32(chk_tel_movil.Checked);
                filtro_Pedidos.comprador_anonimo = Convert.ToInt32(chk_comprador_anonimo.Checked);
                filtro_Pedidos.monto_impuesto = Convert.ToInt32(chk_monto_impuesto.Checked);
                filtro_Pedidos.fecha = Convert.ToInt32(chk_fecha.Checked);
                filtro_Pedidos.order_gif = Convert.ToInt32(chk_order_gif.Checked);

                filtro_Pedidos.estado_orden = Convert.ToInt32(chk_estado_orden.Checked);
                filtro_Pedidos.moneda = Convert.ToInt32(chk_moneda.Checked);
                filtro_Pedidos.observaciones = Convert.ToInt32(chk_observaciones.Checked);
                filtro_Pedidos.cod_autorizacion = Convert.ToInt32(chk_cod_autorizacion.Checked);
                filtro_Pedidos.ip_origen = Convert.ToInt32(chk_ip_origen.Checked);
                filtro_Pedidos.estado_pago = Convert.ToInt32(chk_estado_pago.Checked);
                filtro_Pedidos.medio_pago = Convert.ToInt32(chk_medio_pago.Checked);
                filtro_Pedidos.total_orden = Convert.ToInt32(chk_total_orden.Checked);
                filtro_Pedidos.uso_cupon = Convert.ToInt32(chk_uso_cupon.Checked);
                filtro_Pedidos.tipo_cupon = Convert.ToInt32(chk_tipo_cupon.Checked);

                filtro_Pedidos.sucursal = Convert.ToInt32(chk_sucursal.Checked);
                filtro_Pedidos.recoger_sucursal = Convert.ToInt32(chk_recoger_sucursal.Checked);
                filtro_Pedidos.metodo_pago = Convert.ToInt32(chk_metodo_pago.Checked);
                filtro_Pedidos.moneda_facturacion = Convert.ToInt32(chk_moneda_facturacion.Checked);
                filtro_Pedidos.tipo_envio = Convert.ToInt32(chk_tipo_envio.Checked);
                filtro_Pedidos.costo_envio = Convert.ToInt32(chk_costo_envio.Checked);
                filtro_Pedidos.costo_impuesto_envio = Convert.ToInt32(chk_costo_impuesto_envio.Checked);
                filtro_Pedidos.nombre_destinatario = Convert.ToInt32(chk_nombre_destinatario.Checked);
                filtro_Pedidos.identificacion_envio = Convert.ToInt32(chk_identificacion_envio.Checked);
                filtro_Pedidos.tipo_identi_envio = Convert.ToInt32(chk_tipo_identi_envio.Checked);

                filtro_Pedidos.correo_envio = Convert.ToInt32(chk_correo_envio.Checked);
                filtro_Pedidos.telefono_envio = Convert.ToInt32(chk_telefono_envio.Checked);
                filtro_Pedidos.tel_movil_envio = Convert.ToInt32(chk_tel_movil_envio.Checked);
                filtro_Pedidos.pais_envio = Convert.ToInt32(chk_pais_envio.Checked);
                filtro_Pedidos.provincia_envio = Convert.ToInt32(chk_provincia_envio.Checked);
                filtro_Pedidos.canton_envio = Convert.ToInt32(chk_canton_envio.Checked);
                filtro_Pedidos.distrito_envio = Convert.ToInt32(chk_distrito_envio.Checked);
                filtro_Pedidos.detalle_direccion_envio = Convert.ToInt32(chk_detalle_dir_envio.Checked);
                filtro_Pedidos.ciudad_envio = Convert.ToInt32(chk_ciudad_envio.Checked);
                filtro_Pedidos.codigo_zip_envio = Convert.ToInt32(chk_codigo_zip_envio.Checked);

                filtro_Pedidos.posicion_latitud = Convert.ToInt32(chk_posicion_latitud.Checked);
                filtro_Pedidos.posicion_longitud = Convert.ToInt32(chk_posicion_longitud.Checked);
                filtro_Pedidos.nombre_destinatario_fac = Convert.ToInt32(chk_nombre_dest_fac.Checked);
                filtro_Pedidos.identificacion_fac = Convert.ToInt32(chk_identificacion_fac.Checked);
                filtro_Pedidos.tipo_id_fac = Convert.ToInt32(chk_tipo_id_fac.Checked);
                filtro_Pedidos.correo_fac = Convert.ToInt32(chk_correo_fac.Checked);
                filtro_Pedidos.telefono_fac = Convert.ToInt32(chk_telefono_fac.Checked);
                filtro_Pedidos.tel_movil_fac = Convert.ToInt32(chk_tel_movil_fac.Checked);
                filtro_Pedidos.pais_fac = Convert.ToInt32(chk_pais_fac.Checked);
                filtro_Pedidos.provincia_fac = Convert.ToInt32(chk_provincia_fac.Checked);

                filtro_Pedidos.canton_fac = Convert.ToInt32(chk_canton_fac.Checked);
                filtro_Pedidos.distrito_fac = Convert.ToInt32(chk_distrito_fac.Checked);
                filtro_Pedidos.detalle_dir_fac = Convert.ToInt32(chk_detalle_dir_fac.Checked);
                filtro_Pedidos.ciudad_fac = Convert.ToInt32(chk_ciudad_fac.Checked);
                filtro_Pedidos.codigo_zip_fac = Convert.ToInt32(chk_cod_zip_fac.Checked);
                filtro_Pedidos.posicion_latitud_fac = Convert.ToInt32(chk_posicion_latitud_fac.Checked);
                filtro_Pedidos.posicion_longitud_fac = Convert.ToInt32(chk_posicion_longitud_fac.Checked);

                met_Filtro_Pedidos.actualizarFiltroPedidos(filtro_Pedidos);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}

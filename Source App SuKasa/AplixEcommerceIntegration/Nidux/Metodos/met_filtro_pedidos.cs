using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplixEcommerceIntegration.Globales;
using System.Configuration;


namespace AplixEcommerceIntegration.Nidux.Metodos
{
    public class met_filtro_pedidos
    {
        //Establecer la instancia con la clase Conexion
        private Conexion v_conexion = new Conexion();
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];

        SqlDataReader v_leer;
        SqlCommand v_comando;
        DataTable v_tabla = new DataTable();

        // OBTIENE EL VALOR DE LOS CAMPOS DE LA TABLA 'FILTRO_PEDIDOS_TABLAS_PROPIAS' 
        // MEDIANTE UN SP LLAMADO 'OBTENER_FILTRO_PEDIDOS_TP' PARA REALIZAR EL FILTRO 
        public List<Clases.filtro_pedidos> obtenerFiltroPedidos()
        {
            try
            {
                List<Clases.filtro_pedidos> lista = new List<Clases.filtro_pedidos>();

                v_comando = new SqlCommand();
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "" + com + ".OBTENER_FILTRO_PEDIDOS_TP";
                v_comando.CommandTimeout = 0;
                v_comando.CommandType = CommandType.StoredProcedure;
                v_leer = v_comando.ExecuteReader();

                while (v_leer.Read())
                {
                    lista.Add(

                       new Clases.filtro_pedidos()
                       {
                           numero_orden = Convert.ToInt32(v_leer[1]),
                           nombre_cliente = Convert.ToInt32(v_leer[2]),
                           identificacion = Convert.ToInt32(v_leer[3]),
                           correo_cliente = Convert.ToInt32(v_leer[4]),
                           tel_fijo = Convert.ToInt32(v_leer[5]),
                           tel_movil = Convert.ToInt32(v_leer[6]),
                           comprador_anonimo = Convert.ToInt32(v_leer[7]),
                           monto_impuesto = Convert.ToInt32(v_leer[8]),
                           fecha = Convert.ToInt32(v_leer[9]),
                           order_gif = Convert.ToInt32(v_leer[10]),
                           estado_orden = Convert.ToInt32(v_leer[11]),
                           moneda = Convert.ToInt32(v_leer[12]),
                           observaciones = Convert.ToInt32(v_leer[13]),
                           cod_autorizacion = Convert.ToInt32(v_leer[14]),
                           ip_origen = Convert.ToInt32(v_leer[15]),
                           estado_pago = Convert.ToInt32(v_leer[16]),
                           medio_pago = Convert.ToInt32(v_leer[17]),
                           total_orden = Convert.ToInt32(v_leer[18]),
                           uso_cupon = Convert.ToInt32(v_leer[19]),
                           tipo_cupon = Convert.ToInt32(v_leer[20]),
                           sucursal = Convert.ToInt32(v_leer[21]),
                           recoger_sucursal = Convert.ToInt32(v_leer[22]),
                           metodo_pago = Convert.ToInt32(v_leer[23]),
                           moneda_facturacion = Convert.ToInt32(v_leer[24]),
                           tipo_envio = Convert.ToInt32(v_leer[25]),
                           costo_envio = Convert.ToInt32(v_leer[26]),
                           costo_impuesto_envio = Convert.ToInt32(v_leer[27]),
                           nombre_destinatario = Convert.ToInt32(v_leer[28]),
                           identificacion_envio = Convert.ToInt32(v_leer[29]),
                           tipo_identi_envio = Convert.ToInt32(v_leer[30]),
                           correo_envio = Convert.ToInt32(v_leer[31]),
                           telefono_envio = Convert.ToInt32(v_leer[32]),
                           tel_movil_envio = Convert.ToInt32(v_leer[33]),
                           pais_envio = Convert.ToInt32(v_leer[34]),
                           provincia_envio = Convert.ToInt32(v_leer[35]),
                           canton_envio = Convert.ToInt32(v_leer[36]),
                           distrito_envio = Convert.ToInt32(v_leer[37]),
                           detalle_direccion_envio = Convert.ToInt32(v_leer[38]),
                           ciudad_envio = Convert.ToInt32(v_leer[39]),
                           codigo_zip_envio = Convert.ToInt32(v_leer[40]),
                           posicion_latitud = Convert.ToInt32(v_leer[41]),
                           posicion_longitud = Convert.ToInt32(v_leer[42]),
                           nombre_destinatario_fac = Convert.ToInt32(v_leer[43]),
                           identificacion_fac = Convert.ToInt32(v_leer[44]),
                           tipo_id_fac = Convert.ToInt32(v_leer[45]),
                           correo_fac = Convert.ToInt32(v_leer[46]),
                           telefono_fac = Convert.ToInt32(v_leer[47]),
                           tel_movil_fac = Convert.ToInt32(v_leer[48]),
                           pais_fac = Convert.ToInt32(v_leer[49]),
                           provincia_fac = Convert.ToInt32(v_leer[50]),
                           canton_fac = Convert.ToInt32(v_leer[51]),
                           distrito_fac = Convert.ToInt32(v_leer[52]),
                           detalle_dir_fac = Convert.ToInt32(v_leer[53]),
                           ciudad_fac = Convert.ToInt32(v_leer[54]),
                           codigo_zip_fac = Convert.ToInt32(v_leer[55]),
                           posicion_latitud_fac = Convert.ToInt32(v_leer[56]),
                           posicion_longitud_fac = Convert.ToInt32(v_leer[57])
                       }
                       );
                }

                v_conexion.CerrarConexion();
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        // ACTUALIZA EL FILTRO DE ARTICULOS BASADO EN LA SELECCION DEL FORMULARIO 'frm_filtro_pedidos'
        public void actualizarFiltroPedidos(Clases.filtro_pedidos filtro)
        {
            try
            {
                v_comando = new SqlCommand();
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "" + com + ".ACTUALIZAR_FILTRO_PEDIDOS_TP";
                v_comando.CommandTimeout = 0;
                v_comando.CommandType = CommandType.StoredProcedure;

                v_comando.Parameters.AddWithValue("@numero_orden", filtro.numero_orden);
                v_comando.Parameters.AddWithValue("@nombre_cliente", filtro.nombre_cliente);
                v_comando.Parameters.AddWithValue("@identificacion", filtro.identificacion);
                v_comando.Parameters.AddWithValue("@correo_cliente", filtro.correo_cliente);
                v_comando.Parameters.AddWithValue("@tel_fijo", filtro.tel_fijo);
                v_comando.Parameters.AddWithValue("@tel_movil", filtro.tel_movil);
                v_comando.Parameters.AddWithValue("@comprador_anonimo", filtro.comprador_anonimo);
                v_comando.Parameters.AddWithValue("@monto_impuesto", filtro.monto_impuesto);
                v_comando.Parameters.AddWithValue("@fecha", filtro.fecha);
                v_comando.Parameters.AddWithValue("@order_gif", filtro.order_gif);

                v_comando.Parameters.AddWithValue("@estado_orden", filtro.estado_orden);
                v_comando.Parameters.AddWithValue("@moneda", filtro.moneda);
                v_comando.Parameters.AddWithValue("@observaciones", filtro.observaciones);
                v_comando.Parameters.AddWithValue("@cod_autorizacion", filtro.cod_autorizacion);
                v_comando.Parameters.AddWithValue("@ip_origen", filtro.ip_origen);
                v_comando.Parameters.AddWithValue("@estado_pago", filtro.estado_pago);
                v_comando.Parameters.AddWithValue("@medio_pago", filtro.medio_pago);
                v_comando.Parameters.AddWithValue("@total_orden", filtro.total_orden);
                v_comando.Parameters.AddWithValue("@uso_cupon", filtro.uso_cupon);
                v_comando.Parameters.AddWithValue("@tipo_cupon", filtro.tipo_cupon);

                v_comando.Parameters.AddWithValue("@sucursal", filtro.sucursal);
                v_comando.Parameters.AddWithValue("@recoger_sucursal", filtro.recoger_sucursal);
                v_comando.Parameters.AddWithValue("@metodo_pago", filtro.metodo_pago);
                v_comando.Parameters.AddWithValue("@moneda_facturacion", filtro.moneda_facturacion);
                v_comando.Parameters.AddWithValue("@tipo_envio", filtro.tipo_envio);
                v_comando.Parameters.AddWithValue("@costo_envio", filtro.costo_envio);
                v_comando.Parameters.AddWithValue("@costo_impuesto_envio", filtro.costo_impuesto_envio);
                v_comando.Parameters.AddWithValue("@nombre_destinatario", filtro.nombre_destinatario);
                v_comando.Parameters.AddWithValue("@identificacion_envio", filtro.identificacion_envio);
                v_comando.Parameters.AddWithValue("@tipo_identi_envio", filtro.tipo_identi_envio);

                v_comando.Parameters.AddWithValue("@correo_envio", filtro.correo_envio);
                v_comando.Parameters.AddWithValue("@telefono_envio", filtro.telefono_envio);
                v_comando.Parameters.AddWithValue("@tel_movil_envio", filtro.tel_movil_envio);
                v_comando.Parameters.AddWithValue("@pais_envio", filtro.pais_envio);
                v_comando.Parameters.AddWithValue("@provincia_envio", filtro.provincia_envio);
                v_comando.Parameters.AddWithValue("@canton_envio", filtro.canton_envio);
                v_comando.Parameters.AddWithValue("@distrito_envio", filtro.distrito_envio);
                v_comando.Parameters.AddWithValue("@detalle_direccion_envio", filtro.detalle_direccion_envio);
                v_comando.Parameters.AddWithValue("@ciudad_envio", filtro.ciudad_envio);
                v_comando.Parameters.AddWithValue("@codigo_zip_envio", filtro.codigo_zip_envio);

                v_comando.Parameters.AddWithValue("@posicion_latitud", filtro.posicion_latitud);
                v_comando.Parameters.AddWithValue("@posicion_longitud", filtro.posicion_longitud);
                v_comando.Parameters.AddWithValue("@nombre_destinatario_fac", filtro.nombre_destinatario_fac);
                v_comando.Parameters.AddWithValue("@identificacion_fac", filtro.identificacion_fac);
                v_comando.Parameters.AddWithValue("@tipo_id_fac", filtro.tipo_id_fac);
                v_comando.Parameters.AddWithValue("@correo_fac", filtro.correo_fac);
                v_comando.Parameters.AddWithValue("@telefono_fac", filtro.telefono_fac);
                v_comando.Parameters.AddWithValue("@tel_movil_fac", filtro.tel_movil_fac);
                v_comando.Parameters.AddWithValue("@pais_fac", filtro.pais_fac);
                v_comando.Parameters.AddWithValue("@provincia_fac", filtro.provincia_fac);

                v_comando.Parameters.AddWithValue("@canton_fac", filtro.canton_fac);
                v_comando.Parameters.AddWithValue("@distrito_fac", filtro.distrito_fac);
                v_comando.Parameters.AddWithValue("@detalle_dir_fac", filtro.detalle_dir_fac);
                v_comando.Parameters.AddWithValue("@ciudad_fac", filtro.ciudad_fac);
                v_comando.Parameters.AddWithValue("@codigo_zip_fac", filtro.codigo_zip_fac);
                v_comando.Parameters.AddWithValue("@posicion_latitud_fac", filtro.posicion_latitud_fac);
                v_comando.Parameters.AddWithValue("@posicion_longitud_fac", filtro.posicion_longitud_fac);

                v_comando.ExecuteNonQuery();
                v_conexion.CerrarConexion();

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}

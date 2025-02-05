using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AplixEcommerceIntegration.Nidux.Metodos
{
    public class met_filtro_articulos
    {
        Globales.Conexion v_conexion = new Globales.Conexion();
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];

        SqlDataReader v_leer;
        SqlCommand v_comando;
        DataTable v_tabla = new DataTable();

        public List<Clases.filtro_articulos> obtenerFiltroArticulos()
        {
            try
            {
                List<Clases.filtro_articulos> lista = new List<Clases.filtro_articulos>();

                v_comando = new SqlCommand();
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "" + com + ".OBTENER_FILTRO_ARTICULOS_TP";
                v_comando.CommandTimeout = 0;
                v_comando.CommandType = CommandType.StoredProcedure;
                v_leer = v_comando.ExecuteReader();

                while (v_leer.Read())
                {
                    lista.Add(

                       new Clases.filtro_articulos()
                       {
                           codigo_articulo = Convert.ToInt32(v_leer[1]),
                           nombre = Convert.ToInt32(v_leer[2]),
                           nombre_nidux = Convert.ToInt32(v_leer[3]),
                           descripcion = Convert.ToInt32(v_leer[4]),
                           descripcion_nidux = Convert.ToInt32(v_leer[5]),
                           peso = Convert.ToInt32(v_leer[6]),
                           cantidad = Convert.ToInt32(v_leer[7]),
                           precio = Convert.ToInt32(v_leer[8]),
                           porcentaje_descuento = Convert.ToInt32(v_leer[9]),
                           impuesto_articulo = Convert.ToInt32(v_leer[10]),
                           sincroniza = Convert.ToInt32(v_leer[11]),
                           estado = Convert.ToInt32(v_leer[12]),
                           id_nidux = Convert.ToInt32(v_leer[13]),
                           marca_nidux = Convert.ToInt32(v_leer[14]),
                           categorias = Convert.ToInt32(v_leer[15]),
                           valores_atributos = Convert.ToInt32(v_leer[16]),
                           id_padre = Convert.ToInt32(v_leer[17]),
                           id_hijo = Convert.ToInt32(v_leer[18]),
                           indicador_stock = Convert.ToInt32(v_leer[19]),
                           destacar_articulo = Convert.ToInt32(v_leer[20]),
                           costo_shipping = Convert.ToInt32(v_leer[21]),
                           permite_reserva = Convert.ToInt32(v_leer[22]),
                           porcentaje_reserva = Convert.ToInt32(v_leer[23]),
                           limite_carrito = Convert.ToInt32(v_leer[24]),
                           usa_gif = Convert.ToInt32(v_leer[25]),
                           tiempo_gif = Convert.ToInt32(v_leer[26]),
                           video_youtube = Convert.ToInt32(v_leer[27]),
                           nombre_ingles = Convert.ToInt32(v_leer[28]),
                           descripcion_ingles = Convert.ToInt32(v_leer[29]),
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
        public void actualizarFiltroArticulos(Clases.filtro_articulos filtro)
        {
            try
            {
                v_comando = new SqlCommand();
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = "" + com + ".ACTUALIZAR_FILTRO_ARTICULOS_TP";
                v_comando.CommandTimeout = 0;
                v_comando.CommandType = CommandType.StoredProcedure;

                v_comando.Parameters.AddWithValue("@codigo_articulo", filtro.codigo_articulo);
                v_comando.Parameters.AddWithValue("@nombre", filtro.nombre);
                v_comando.Parameters.AddWithValue("@nombre_nidux", filtro.nombre_nidux);
                v_comando.Parameters.AddWithValue("@descripcion", filtro.descripcion);
                v_comando.Parameters.AddWithValue("@descripcion_nidux", filtro.descripcion_nidux);
                v_comando.Parameters.AddWithValue("@peso", filtro.peso);
                v_comando.Parameters.AddWithValue("@cantidad", filtro.cantidad);
                v_comando.Parameters.AddWithValue("@precio", filtro.precio);
                v_comando.Parameters.AddWithValue("@porcentaje_descuento", filtro.porcentaje_descuento);
                v_comando.Parameters.AddWithValue("@impuesto_articulo", filtro.impuesto_articulo);
                v_comando.Parameters.AddWithValue("@sincroniza", filtro.sincroniza);
                v_comando.Parameters.AddWithValue("@estado", filtro.estado);
                v_comando.Parameters.AddWithValue("@id_nidux", filtro.id_nidux);
                v_comando.Parameters.AddWithValue("@marca_nidux", filtro.marca_nidux);
                v_comando.Parameters.AddWithValue("@categorias", filtro.categorias);
                v_comando.Parameters.AddWithValue("@valores_atributos", filtro.valores_atributos);
                v_comando.Parameters.AddWithValue("@id_padre", filtro.id_padre);
                v_comando.Parameters.AddWithValue("@id_hijo", filtro.id_hijo);
                v_comando.Parameters.AddWithValue("@indicador_stock", filtro.indicador_stock);
                v_comando.Parameters.AddWithValue("@destacar_articulo", filtro.destacar_articulo);
                v_comando.Parameters.AddWithValue("@costo_shipping", filtro.costo_shipping);
                v_comando.Parameters.AddWithValue("@permite_reserva", filtro.permite_reserva);
                v_comando.Parameters.AddWithValue("@porcentaje_reserva", filtro.porcentaje_reserva);
                v_comando.Parameters.AddWithValue("@limite_carrito", filtro.limite_carrito);
                v_comando.Parameters.AddWithValue("@usa_gif", filtro.usa_gif);
                v_comando.Parameters.AddWithValue("@tiempo_gif", filtro.tiempo_gif);
                v_comando.Parameters.AddWithValue("@video_youtube", filtro.video_youtube);
                v_comando.Parameters.AddWithValue("@nombre_ingles", filtro.nombre_ingles);
                v_comando.Parameters.AddWithValue("@descripcion_ingles", filtro.descripcion_ingles);

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

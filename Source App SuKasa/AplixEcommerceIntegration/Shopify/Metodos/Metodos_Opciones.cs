using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using AplixEcommerceIntegration.Globales;

namespace AplixEcommerceIntegration.Shopify.Metodos
{
    class Metodos_Opciones
    {
        //Conexion a base de datos
        Conexion cnn = new Conexion();

        //Métodos que llenan la tabla de las Variantes de los articulos
        public DataTable opciones_articulos()
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.MOSTRAR_OPCIONES_ARTICULOS";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {

                    da.Fill(dt);
                    cnn.CerrarConexion();
                    return dt;
                }
            }
            catch (Exception)
            {
                dt = null;
                cnn.CerrarConexion();
                return dt;
            }
        }

        //Método que llena el ComboBox de las opciones
        public DataTable opciones_ComboBox()
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT NOMBRE FROM SHOPIFY.ARTICULOS_OPCIONES";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {

                    da.Fill(dt);
                    cnn.CerrarConexion();
                    return dt;
                }
            }
            catch (Exception)
            {
                dt = null;
                cnn.CerrarConexion();
                return dt;
            }
        }

        //Método que agrega una nueva opcion a tablas propias
        public int agregar_opciones(string nombre)
        {
            try
            {
                //Verificamos que la opcion no esté repetida
                int n = 0;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT NOMBRE FROM SHOPIFY.ARTICULOS_OPCIONES WHERE NOMBRE LIKE '%" + nombre + "%'";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    n = 1;
                }
                cnn.CerrarConexion();
                if (n == 1)
                {
                   return 2;
                }
                else
                {
                    //Insertamos la nueva opcion
                    SqlCommand scmd = new SqlCommand();
                    scmd.Connection = cnn.AbrirConexion();
                    scmd.CommandText = "SHOPIFY.INSERTAR_OPCIONES";
                    scmd.CommandTimeout = 0;
                    scmd.CommandType = CommandType.StoredProcedure;
                    scmd.Parameters.AddWithValue("@NOMBRE", nombre);
                    scmd.ExecuteNonQuery();
                    cnn.CerrarConexion();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
            
        }

        //Método que agrega el valor de la opcion
        public int agregar_opciones_valores(string nombre, string opcion)
        {
            try
            {
                //Verificamos que la opcion valor no esté repetida
                int n = 0;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT OV.NOMBRE FROM SHOPIFY.ARTICULOS_OPCIONES_VALORES AS OV " +
                                  "INNER JOIN SHOPIFY.ARTICULOS_OPCIONES AS AO ON OV.ID_OPCION = AO.ID " +
                                  "WHERE OV.NOMBRE LIKE '" + nombre +"%' AND AO.NOMBRE = '" + opcion +"'";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    n = 1;
                }
                cnn.CerrarConexion();
                if (n == 1)
                {
                    return 2;
                }
                else
                {
                    //Insertamos la nueva opcion de valores
                    SqlCommand scmd = new SqlCommand();
                    scmd.Connection = cnn.AbrirConexion();
                    scmd.CommandText = "SHOPIFY.INSERTAR_OPCIONES_VALORES";
                    scmd.CommandTimeout = 0;
                    scmd.CommandType = CommandType.StoredProcedure;
                    scmd.Parameters.AddWithValue("@NOMBRE", nombre);
                    scmd.Parameters.AddWithValue("@OPCION", opcion);
                    scmd.ExecuteNonQuery();
                    cnn.CerrarConexion();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //Método que actualiza las variantes de las opciones
        public int actualizar_opciones_valores(string descripcion, int id)
        {
            try
            {
                //Actualizamos la opcion de valor
                SqlCommand scmd = new SqlCommand();
                scmd.Connection = cnn.AbrirConexion();
                scmd.CommandText = "SHOPIFY.ACTUALIZAR_OPCIONES";
                scmd.CommandTimeout = 0;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@NOMBRE", descripcion);
                scmd.Parameters.AddWithValue("@ID", id);
                scmd.ExecuteNonQuery();
                cnn.CerrarConexion();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //Método que elimina el valor de opcion
        public void eliminar_opciones_valores(string id)
        {
            try
            {
                SqlCommand scmd = new SqlCommand();
                scmd.Connection = cnn.AbrirConexion();
                scmd.CommandText = "SHOPIFY.ELIMINAR_OPCIONES";
                scmd.CommandTimeout = 0;
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@ID", id);
                scmd.ExecuteNonQuery();
                cnn.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Método que nos regresa las cantidades y el precio de los articulos variantes
        public DataTable articulos_variantes(string articulo)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SHOPIFY.ARTICULO_VARIANTE_DATOS";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SKU", articulo);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {

                    da.Fill(dt);
                    cnn.CerrarConexion();
                    return dt;
                }
            }
            catch (Exception)
            {
                dt = null;
                cnn.CerrarConexion();
                return dt;
            }
        }
    }
}

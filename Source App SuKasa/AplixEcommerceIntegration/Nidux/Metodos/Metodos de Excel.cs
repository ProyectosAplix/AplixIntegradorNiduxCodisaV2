using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Metodos
{
    public class Metodos_de_Excel
    {

        Globales.Conexion v_conexion = new Globales.Conexion();
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];

        SqlDataReader v_leer;
        SqlCommand v_comando = new SqlCommand();
        DataTable v_tabla = new DataTable();


        //Obtener los datos de configuracion 
        public List<Clases.CategoriasExcel> obtener_datos_de_categorias_excel()
        {

            List<Clases.CategoriasExcel> lista = new List<Clases.CategoriasExcel>();

            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = v_conexion.AbrirConexion();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = com + ".TRAER_CATEGORIAS_EXCEL";
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Clases.CategoriasExcel obj_excel = new Clases.CategoriasExcel();
                    obj_excel.codigo = (rdr["CODIGO_NIDUX"].ToString());
                    obj_excel.nombre = (rdr["NOMBRE"].ToString());
                    obj_excel.padre = (rdr["PADRE"].ToString());
                    lista.Add(obj_excel);

                }


                cmd.Connection = v_conexion.CerrarConexion();

                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Obtener los datos de configuracion 
        public List<Clases.MarcasExcel> obtener_datos_de_marcas_excel()
        {

            List<Clases.MarcasExcel> lista = new List<Clases.MarcasExcel>();

            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = v_conexion.AbrirConexion();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = com + ".TRAER_MARCAS_EXCEL";
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Clases.MarcasExcel obj_marcas = new Clases.MarcasExcel();
                    obj_marcas.codigo = (rdr["CODIGO"].ToString());
                    obj_marcas.nombre = (rdr["DESCRIPCION_NIDUX"].ToString());
                    lista.Add(obj_marcas);

                }

                cmd.Connection = v_conexion.CerrarConexion();

                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Obtener los datos de configuracion 
        public List<Clases.AtributosExcel> obtener_datos_de_atributos_excel()
        {

            List<Clases.AtributosExcel> lista = new List<Clases.AtributosExcel>();

            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = v_conexion.AbrirConexion();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = com + ".TRAER_ATRIBUTOS_EXCEL";
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Clases.AtributosExcel obj_atributos = new Clases.AtributosExcel();
                    obj_atributos.codigo = (rdr["ID"].ToString());
                    obj_atributos.nombre = (rdr["DESCRIPCION"].ToString());
                    obj_atributos.atributo = (rdr["ATRIBUTO"].ToString());
                    lista.Add(obj_atributos);

                }

                cmd.Connection = v_conexion.CerrarConexion();

                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Obtener los datos de configuracion 
        public List<Clases.EstadosExcel> obtener_datos_de_estados_excel()
        {

            List<Clases.EstadosExcel> lista = new List<Clases.EstadosExcel>();

            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = v_conexion.AbrirConexion();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = com + ".TRAER_ESTADOS_ARTICULOS";
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Clases.EstadosExcel obj_marcas = new Clases.EstadosExcel();
                    obj_marcas.id = (rdr["ID"].ToString());
                    obj_marcas.descripcion = (rdr["DESCRIPCION"].ToString());
                    lista.Add(obj_marcas);

                }

                cmd.Connection = v_conexion.CerrarConexion();

                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Validamos que las categorias se encuentren en la base de datos
        public bool validar_categorias_excel(Clases.ArticulosExcel obj_articulos)
        {

            bool bandera = true;
            string mensaje;

            try
            {

                if (obj_articulos.categoria == "" || obj_articulos.categoria == null || obj_articulos.categoria == " ")
                {

                }
                else
                {

                    int[] array_categorias;

                    //array con los valores divididos por la coma
                    array_categorias = Array.ConvertAll(obj_articulos.categoria.ToString().TrimEnd(',').Split(','), int.Parse);

                    int i = 0;

                    while (i < array_categorias.Length)
                    {

                        SqlCommand v_comando = new SqlCommand();
                        DataTable v_tabla = new DataTable();
                        SqlDataReader v_leer;

                        v_comando.Connection = v_conexion.AbrirConexion();
                        v_comando.CommandText = com + ".VALIDAR_CATEGORIAS";
                        v_comando.CommandType = CommandType.StoredProcedure;
                        v_comando.Parameters.AddWithValue("@ID", array_categorias[i]);
                        v_leer = v_comando.ExecuteReader();
                        v_leer.Read();
                        mensaje = v_leer.GetString(v_leer.GetOrdinal("MENSAJE"));
                        v_conexion.CerrarConexion();

                        if (mensaje == "N")
                        {

                            bandera = false;

                        }

                        i++;

                    }

                }

                return bandera;

            }
            catch (Exception)
            {
                return bandera;
            }
        }

        //Validamos que las marcas se encuentren en la base de datos
        public bool validar_marcas_excel(Clases.ArticulosExcel obj_articulos)
        {

            bool bandera = true;
            string mensaje;

            try
            {

                if (obj_articulos.marca == "" || obj_articulos.marca == null || obj_articulos.marca == " ")
                {

                }
                else
                {

                    int[] array_categorias;

                    //array con los valores divididos por la coma
                    array_categorias = Array.ConvertAll(obj_articulos.marca.ToString().TrimEnd(',').Split(','), int.Parse);

                    int i = 0;

                    while (i < array_categorias.Length)
                    {

                        SqlCommand v_comando = new SqlCommand();
                        DataTable v_tabla = new DataTable();
                        SqlDataReader v_leer;

                        v_comando.Connection = v_conexion.AbrirConexion();
                        v_comando.CommandText = com + ".VALIDAR_MARCAS";
                        v_comando.CommandType = CommandType.StoredProcedure;
                        v_comando.Parameters.AddWithValue("@ID", array_categorias[i]);
                        v_leer = v_comando.ExecuteReader();
                        v_leer.Read();
                        mensaje = v_leer.GetString(v_leer.GetOrdinal("MENSAJE"));
                        v_conexion.CerrarConexion();

                        if (mensaje == "N")
                        {

                            bandera = false;

                        }

                        i++;

                    }

                }

                return bandera;

            }
            catch (Exception)
            {
                return bandera;
            }
        }

        //agregamos o actualizamos los datos en el excel
        public void agregar_articulos_excel(Clases.ArticulosExcel obj_articulos)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = com + ".INSERTAR_ARTICULOS_POR_EXCEL";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@SKU", obj_articulos.sku);
                v_comando.Parameters.AddWithValue("@NOMBRE_NIDUX", obj_articulos.nombre_nidux);
                v_comando.Parameters.AddWithValue("@DESCRIPCION_NIDUX", obj_articulos.descripcion_nidux);
                v_comando.Parameters.AddWithValue("@MARCA", obj_articulos.marca);
                v_comando.Parameters.AddWithValue("@CATEGORIA", obj_articulos.categoria);
                v_comando.Parameters.AddWithValue("@PORCENTAJE_RESERVA", obj_articulos.porcentaje_reserva);
                v_comando.Parameters.AddWithValue("@PERMITE_RESERVA", obj_articulos.permite_reserva);
                v_comando.Parameters.AddWithValue("@DESTACADO", obj_articulos.destacado);
                v_comando.Parameters.AddWithValue("@INDICADOR_STOCK", obj_articulos.indicador_stock);
                v_comando.Parameters.AddWithValue("@ESTADO", obj_articulos.estado);
                v_comando.Parameters.AddWithValue("@COSTO_SHIPPING", obj_articulos.costo_shipping);
                v_comando.Parameters.AddWithValue("@LIMITE_CARRITO", obj_articulos.limite_carrito);
                v_comando.Parameters.AddWithValue("@USAR_GIF", obj_articulos.usar_gif);
                v_comando.Parameters.AddWithValue("@TIEMPO_GIF", obj_articulos.tiempo_gif);
                v_comando.Parameters.AddWithValue("@NOMBRE_TRADUCCION", obj_articulos.nombre_traduccion);
                v_comando.Parameters.AddWithValue("@DESCRIPCION_TRADUCCION", obj_articulos.descripcion_traduccion);
                v_comando.Parameters.AddWithValue("@SINCRONIZA", obj_articulos.sincroniza);

                if (obj_articulos.tags == null) { v_comando.Parameters.AddWithValue("@TAGS", ""); } else { v_comando.Parameters.AddWithValue("@TAGS", obj_articulos.tags); }
                if (obj_articulos.seo_tags == null) { v_comando.Parameters.AddWithValue("@SEO_TAGS", "");  } else { v_comando.Parameters.AddWithValue("@SEO_TAGS", obj_articulos.seo_tags);  }
                        

                v_comando.ExecuteNonQuery();
                v_conexion.CerrarConexion();


            }
            catch (Exception)
            {
                throw;
            }
        }

        //Validamos que las marcas se encuentren en la base de datos
        public bool validar_atributos_excel(Clases.VariacionesExcel obj_articulos)
        {

            bool bandera = true;
            string mensaje;

            try
            {

                if (obj_articulos.atributos == "" || obj_articulos.atributos == null || obj_articulos.atributos == " ")
                {

                }
                else
                {

                    int[] array_categorias;

                    //array con los valores divididos por la coma
                    array_categorias = Array.ConvertAll(obj_articulos.atributos.ToString().TrimEnd(',').Split(','), int.Parse);

                    int i = 0;

                    while (i < array_categorias.Length)
                    {

                        SqlCommand v_comando = new SqlCommand();
                        DataTable v_tabla = new DataTable();
                        SqlDataReader v_leer;

                        v_comando.Connection = v_conexion.AbrirConexion();
                        v_comando.CommandText = com + ".VALIDAR_ATRIBUTOS";
                        v_comando.CommandType = CommandType.StoredProcedure;
                        v_comando.Parameters.AddWithValue("@ID", array_categorias[i]);
                        v_leer = v_comando.ExecuteReader();
                        v_leer.Read();
                        mensaje = v_leer.GetString(v_leer.GetOrdinal("MENSAJE"));
                        v_conexion.CerrarConexion();

                        if (mensaje == "N")
                        {

                            bandera = false;

                        }

                        i++;

                    }

                }

                return bandera;

            }
            catch (Exception)
            {
                return bandera;
            }
        }

        //mostrar atributos de un articulo padre que viene desde el excel
        public void agregar_variaciones_excel(Clases.VariacionesExcel obj_articulos)
        {

            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = com + ".INSERTAR_VARIACIONES_EXCEL";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.Parameters.AddWithValue("@SKU_PADRE", obj_articulos.sku_padre);
                v_comando.Parameters.AddWithValue("@SKU_VAR", obj_articulos.sku_variacion);
                v_comando.Parameters.AddWithValue("@ATRIBUTOS", obj_articulos.atributos);
                v_comando.Parameters.AddWithValue("@SINCRONIZA", obj_articulos.sincroniza);

                v_comando.ExecuteNonQuery();
                v_conexion.CerrarConexion();


            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo para emparejar el ERP con las tablas
        public void emparejar_tablas_erp_articulos()
        {
            try
            {
               

                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = com + ".EJECUTAR_SINCRONIZACION_ARTICULOS";
                v_comando.CommandType = CommandType.StoredProcedure;
                v_comando.ExecuteNonQuery();
                v_conexion.CerrarConexion();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo para emparejar el ERP con las tablas esto se refiere a los valores de los articulos (precios, impuestos, descuento y demas)
        public void emparejar_tablas_erp_articulos_emparejar_valores()
        {
            try
            {
                v_comando.Connection = v_conexion.AbrirConexion();
                v_comando.CommandText = com + ".EJECUTAR_SINCRONIZACION_VALORES_ARTICULO";
                v_comando.CommandType = CommandType.StoredProcedure;
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


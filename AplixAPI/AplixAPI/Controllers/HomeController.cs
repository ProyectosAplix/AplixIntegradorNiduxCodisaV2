using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Data.SqlClient;
using AplixAPI.Models;
using System.Text.RegularExpressions;
using System.Diagnostics.PerformanceData;
using System.Net;

namespace AplixAPI.Controllers
{
    /*
     * API con todos los servicios para la sincronizacion de NIDUX-ERP SOFTLAND
     * Su ultima modificacion fue el 23/02/2021
     * Esta version es la que funciona con el método más simple de sincronizar y dandole a la aplicacion más participacion por el CRUD que contiene ella
     */
    public class HomeController : ApiController
    {
        /****** METODOS EN USO, ULTIMA VERSION QUE FUNCIONA CON EL SERVICIO Y APLICACION *******/
        static string com = ConfigurationManager.AppSettings["Company"];

        //METODO QUE RETORNA EL USARIO Y LA CONTRASEÑA DEL API DE NIDUX
        //ES USADO POR EL SERVICIO
        [HttpGet]
        [Route("api/obtener_credenciales")] //FUERA DE USO
        public IHttpActionResult ObtenerCredenciales()//HAY QUE AGREGAR EL STOREID
        {
            var list_Credenciales = new List<Valores_credencial>();
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" +com +".CREDENCIALES_CONEXION";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Valores_credencial val_cre = new Valores_credencial();
                    val_cre.usuario = rdr["USUARIO"].ToString();
                    val_cre.contrasena = rdr["CONTRASENA"].ToString();
                    val_cre.storeId = Int32.Parse(rdr["STOREID"].ToString());
                    list_Credenciales.Add(val_cre);
                }

            }
            return Ok(new { credenciales = list_Credenciales });
        }

        //METODO QUE SE TRAE LOS ATRIBUTOS DE LA TIENDA PARA SER INGRESADOS EN TABLAS PROPIAS
        //ES USADO POR LA APP
        [HttpPost]
        [Route("api/insertar_atributos")]
        public IHttpActionResult Insertar_Atributos(Atributo_Valores atributo)
        {

            if (atributo == null || atributo.atributos == null || atributo.atributos.Count == 0)
            {
                return Content(HttpStatusCode.BadRequest, new { isSuccessful = false, message = "El objeto Atributo o su lista es nula o está vacía." });
            }

            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            int record = 0;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
  
                    foreach (var atributoEntry in atributo.atributos)
                    {
                        SqlCommand cmd = new SqlCommand
                        {
                            Connection = con,
                            CommandType = CommandType.StoredProcedure,
                            CommandText = "" + com + ".INSERTAR_ATRIBUTOS"
                        };


                        cmd.Parameters.AddWithValue("@ID", ((object)atributoEntry.Value.id) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DESCRIPCION", ((object)atributoEntry.Value.attribute_name) ?? DBNull.Value);

                        con.Open();
                        record = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    return Ok(new { isSuccessful = false, message = e.Message.ToString() });
                }
                return Ok(new { isSuccessful = true, json_res = "Atributos agregados con éxito" });
            }
        }


        //METODO QUE ELIMINA LOS VALORES ACTUALES DEL ATRIBUTO DE LAS TABLAS PROPIAS
        //ES USADO POR LA APP
        [HttpDelete]
        [Route("api/eliminar_valores_atributos")]
        public IHttpActionResult Eliminar_Atributos()
        {
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            int record = 0;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    //Eliminamos los valores para ingresarlos de nuevo
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.Connection = con;
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandText = "" + com + ".ELIMINAR_VALORES_ATRIBUTOS";
                    con.Open();
                    record = cmd2.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception e)
                {
                    con.Close();
                    return Content(HttpStatusCode.BadRequest, new { isSuccessful = false, message = e.Message.ToString() });
                }
                return Ok(new { isSuccessful = true, json_res = "Atributos Eliminados con éxito" });
            }
        }

        //METODO QUE INSERTA LOS VALORES DEL ATRIBUTO QU ESTAN EN LA TIENDA EN NUESTRAS TABLAS
        //ES USADO POR LA APP
        [HttpPost]
        [Route("api/insertar_valores_atributos")]
        public IHttpActionResult Insertar_Variaciones(Atributo atributo)
        {
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            int record = 0;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    int n = 0;
                    while (n < atributo.atributos.Count)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "" + com + ".INSERTAR_VALORES_ATRIBUTOS";
                        cmd.Parameters.AddWithValue("@ID_ATRIBUTO", ((object)atributo.atributos[n].id_atributo) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ID_VALOR", ((object)atributo.atributos[n].id_valor_atributo) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DESCRIPCION", ((object)atributo.atributos[n].nombre_valor_atributo) ?? DBNull.Value);
                        con.Open();
                        record = cmd.ExecuteNonQuery();
                        con.Close();
                        n++;
                    }
                }
                catch (Exception e)
                {
                    con.Close();

                    return Content(HttpStatusCode.BadRequest, new { isSuccessful = false, message = e.Message.ToString() });
                }
                return Ok(new { isSuccessful = true, json_res = "Atributos agregados con éxito" });
            }
        }

        //METODO QUE SE TRAE LAS MARCAS DE LA TIENDA PARA INGRESARLAS EN TABLAS PROPIAS
        //ES USADO POR LA APP
        [HttpPost]
        [Route("api/insertar_marcas")]
        public IHttpActionResult Insertar_Marcas(Marcas marcas)
        {

            if (marcas == null || marcas.marcas == null || marcas.marcas.Count == 0)
            {
                return Content(HttpStatusCode.BadRequest, new { isSuccessful = false, message = "El objeto marcas o su lista es nula o está vacía." });
            }


            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            int record = 0;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    foreach (var marcaEntry in marcas.marcas)
                    {
                        SqlCommand cmd = new SqlCommand
                        {
                            Connection = con,
                            CommandType = CommandType.StoredProcedure,
                            CommandText = "" + com + ".INSERTAR_MARCAS_SIMPLE"
                        };

                        // Usando la clave (marcaEntry.Key) y el valor (marcaEntry.Value) del diccionario
                        cmd.Parameters.AddWithValue("@CODIGO", ((object)marcaEntry.Value.id) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NOMBRE", ((object)marcaEntry.Value.brand_name) ?? DBNull.Value);

                        con.Open();
                        record = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    return Ok(new { isSuccessful = false, message = e.Message.ToString() });
                }
                return Ok(new { isSuccessful = true, json_res = "marcas agregadas con éxito" });
            }
        }


        //METODO QUE OBTIENE LAS MARCAS QUE SON EDITADAS EN TABLAS PROPIAS PARA SER ACTUALIZADAS EN NIDUX
        //ES USADA POR EL APP
        //ES USADA POR EL SERVICIO
        [HttpGet]
        [Route("api/actualizar_marcas_simple")]
        public IEnumerable<Marca> Actualizar_Marcas()
        {
            var lista_marcas = new List<Marca>();
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + com + ".ACTUALIZAR_MARCAS_SIMPLE";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Marca marcas = new Marca();
                    marcas.brand_name = rdr["DESCRIPCION_NIDUX"].ToString();
                    marcas.nombre = marcas.brand_name;
                    marcas.codigo_marca = rdr["CODIGO_NIDUX"].ToString();
                    lista_marcas.Add(marcas);
                }
            }
            return lista_marcas;
        }

        //METODO QUE SE TRAE LAS CATEGORIAS DE LA TIENDA PARA INGRESARLAS EN TABLAS PROPIAS
        //ES USADO POR LA APP
        [HttpPost]
        [Route("api/insertar_categorias")]
        public IHttpActionResult Insertar_Categorias(Categorias categorias)
        {
            if (categorias == null || categorias.cats == null || categorias.cats.Count == 0)
            {
                return Content(HttpStatusCode.BadRequest, new { isSuccessful = false, message = "El objeto categorías o su lista es nula o está vacía." });
            }

            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            string company = ConfigurationManager.AppSettings["Company"];
            int record = 0;
            const string reduceMultiSpace = @"[ ]{2,}";

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    foreach (var catEntry in categorias.cats)
                    {
                        // Obtiene la categoría correspondiente del diccionario
                        Cats cat = catEntry.Value;

                        if (cat == null)
                        {
                            return Ok(new { isSuccessful = false, message = $"La categoría con clave {catEntry.Key} es nula." });
                        }

                        // Limpieza de la descripción.
                        string descrip = string.IsNullOrEmpty(cat.category_description)
                            ? ""
                            : Regex.Replace(cat.category_description.Trim(), @"<[^>]+>|&nbsp;", String.Empty);

                        string descrip_final = Regex.Replace(descrip.Replace("\t", " "), reduceMultiSpace, " ");

                        // Configuración del comando SQL.
                        SqlCommand cmd = new SqlCommand(company+".INSERTAR_CATEGORIAS_SIMPLE", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.AddWithValue("@CODIGO", (object)cat.id ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NOMBRE", (object)cat.category_name ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DESCRIPCION", (object)descrip_final ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SUBCATEGORIA", (object)cat.category_father ?? DBNull.Value);

                        // Ejecuta el comando.
                        record = cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                catch (Exception e)
                {
                    con.Close();
                    return Content(HttpStatusCode.BadRequest, new { isSuccessful = false, message = e.Message });
                }
            }

            return Ok(new { isSuccessful = true, json_res = "Categorías agregadas con éxito" });
        }


        //METODO QUE OBTIENE LAS CATEGORIAS QUE SON EDITADAS EN TABLAS PROPIAS PARA SER ACTUALIZADAS EN NIDUX
        //ES USADA POR EL APP ??
        //ES USADA POR EL SERVICIO ??
        [HttpGet]
        [Route("api/actualizar_categorias_nidux_simple")]
        public IEnumerable<Categorias_Padre> GetActualizarCategoriasSimple()
        {
            var listcategorias = new List<Categorias_Padre>();

            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + com + ".MODIFICAR_CATEGORIAS_NIDUX_SIMPLE";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    Categorias_Padre list = new Categorias_Padre();
                    list.codigo_categoria = rdr["CODIGO_NIDUX"].ToString();
                    list.nombre = rdr["NOMBRE"].ToString();
                    list.descripcion = rdr["DESCRIPCION"].ToString();
                    list.categoria_padre = rdr["SUBCATEGORIA_NIDUX"].ToString();
                    listcategorias.Add(list);
                }
            }
            return listcategorias;
        }

        //METODO QUE OBTIENE LOS ARTICULOS QUE SON EDITADOS EN TABLAS PROPIAS PARA SER ACTUALIZADOS EN NIDUX
        //ES USADA EN EL APP
        //ES USADA POR EL SERVICIO
        [HttpGet]
        [Route("api/actualizar_articulos_editados_simple")]
        public IEnumerable<Articulos_Act> Editar_Articulos_Simple() /////////////// CAMBIOSSS
        {
            var listArticulos = new List<Articulos_Act>();
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            const string reduceMultiSpace = @"[ ]{2,}";
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + com + ".ACT_ARTICULOS_EDITADOS_SIMPLE"; 
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    try
                    {
                        Articulos_Act list = new Articulos_Act();
                        var listaTraduc = new List<traduccion_act>();
                        traduccion_act traduc = new traduccion_act();
                        dimensiones dimensiones = new dimensiones();
                        list.dimensiones = new List<dimensiones>();
                        list.id = rdr["ID"].ToString();
                        if (rdr["ID_MARCA"].ToString() == "" || rdr["ID_MARCA"].ToString() == null)
                        {
                            list.id_marca = "0";
                        }
                        else
                        {
                            list.id_marca = rdr["ID_MARCA"].ToString().TrimEnd(',');
                        }
                        if (rdr["ID_CATEGORIAS"].ToString() == "" || rdr["ID_CATEGORIAS"].ToString() == null)
                        {
                            list.categorias = new int[] { 0 };
                        }
                        else
                        {
                            list.categorias = Array.ConvertAll(rdr["ID_CATEGORIAS"].ToString().TrimEnd(',').Split(','), int.Parse);
                        }
                        list.sku = rdr["SKU"].ToString();
                        if (rdr["NOMBRE"].ToString() == "" || rdr["NOMBRE"].ToString() == null)
                        {
                            list.nombre = "Nombre por defecto: " + rdr["SKU"].ToString();
                        }
                        else
                        {
                            string nombre = (Regex.Replace(rdr["NOMBRE"].ToString().Trim(), @"<[^>]+>|&nbsp;", String.Empty));
                            string nombre_final = Regex.Replace(nombre.Replace("\t", " "), reduceMultiSpace, " ");
                            list.nombre = nombre_final;
                        }
                        if (rdr["DESCRIPCION"].ToString() == "" || rdr["DESCRIPCION"].ToString() == null)
                        {
                            list.descripcion = "Descripción por defecto: " + rdr["SKU"].ToString();
                        }
                        else
                        {
                            //string descrip = (Regex.Replace(rdr["DESCRIPCION"].ToString(), @"<[^>]+>|&nbsp;", String.Empty));
                            //string descrip_final = Regex.Replace(descrip.Replace("\t", " "), reduceMultiSpace, " ");
                            list.descripcion = rdr["DESCRIPCION"].ToString();
                        }
                        if (rdr["PRECIO"].ToString() == "" || rdr["PRECIO"].ToString() == null)
                        {
                            list.precio = "0";
                        }
                        else
                        {
                            rdr["PRECIO"].ToString().Replace(',', '.');
                            list.precio = string.Format("{0:0.00}", rdr["PRECIO"]);
                        }
                        if (rdr["COSTO_SHIPPING"].ToString() == "" || rdr["COSTO_SHIPPING"].ToString() == null)
                        {
                            list.costo_shipping_individual = "0";
                        }
                        else
                        {
                            list.costo_shipping_individual = rdr["COSTO_SHIPPING"].ToString();
                        }
                        if (rdr["PESO"].ToString() == "" || rdr["PESO"].ToString() == null)
                        {
                            list.peso_producto = "0";
                        }
                        else
                        {
                            list.peso_producto = (rdr["PESO"].ToString());
                        }
                        if (rdr["DESCUENTO"].ToString() == "" || rdr["DESCUENTO"].ToString() == null)
                        {
                            list.porcentaje_oferta = "0";
                        }
                        else
                        {
                            list.porcentaje_oferta = string.Format("{0:0.00}", rdr["DESCUENTO"]);
                        }
                        list.estado_de_producto = rdr["ESTADO"].ToString();
                        if (rdr["DESTACADO"].ToString() == "" || rdr["DESTACADO"].ToString() == null || rdr["DESTACADO"].ToString() == "N")
                        {
                            list.es_destacado = "0";
                        }
                        else
                        {
                            list.es_destacado = "1";
                        }
                        if (rdr["CANTIDAD"].ToString() == "" || rdr["CANTIDAD"].ToString() == null)
                        {
                            list.stock_principal = 0;
                        }
                        else
                        {
                            list.stock_principal = Convert.ToInt32(Convert.ToDecimal(rdr["CANTIDAD"].ToString()));
                        }
                        list.video_youtube_url = rdr["VIDEO_YOUTUBE"].ToString();
                        if (rdr["INDICADOR_STOCK"].ToString() == "" || rdr["INDICADOR_STOCK"].ToString() == null || rdr["INDICADOR_STOCK"].ToString() == "N")
                        {
                            list.ocultar_indicador_stock = "0";
                        }
                        else
                        {
                            list.ocultar_indicador_stock = "1";
                        }
                        if (rdr["PERMITE_RESERVA"].ToString() == "" || rdr["PERMITE_RESERVA"].ToString() == null || rdr["PERMITE_RESERVA"].ToString() == "N")
                        {
                            list.producto_permite_reservacion = "0";
                        }
                        else
                        {
                            list.producto_permite_reservacion = "1";
                            //rdr["PERMITE_RESERVA"].ToString();
                        }
                        if (rdr["LIMITE_CARRITO"].ToString() == "" || rdr["LIMITE_CARRITO"].ToString() == null)
                        {
                            list.limite_para_reservar_en_carrito = "1";
                        }
                        else
                        {
                            list.limite_para_reservar_en_carrito = rdr["LIMITE_CARRITO"].ToString();
                        }
                        if (rdr["PORCENTAJE_RESERVA"].ToString() == "" || rdr["PORCENTAJE_RESERVA"].ToString() == null)
                        {
                            list.porcentaje_para_reservar = "0";
                        }
                        else
                        {
                            list.porcentaje_para_reservar = rdr["PORCENTAJE_RESERVA"].ToString();
                        }
                        if (rdr["USAR_GIF"].ToString() == "" || rdr["USAR_GIF"].ToString() == null || rdr["USAR_GIF"].ToString() == "N")
                        {
                            list.usar_gif_en_homepage = "0";
                        }
                        else
                        {
                            list.usar_gif_en_homepage = "1";
                            //rdr["USAR_GIF"].ToString();
                        }
                        if (rdr["GIF_TIEMPO"].ToString() == "" || rdr["GIF_TIEMPO"].ToString() == null)
                        {
                            list.gif_tiempo_transicion = "0";
                        }
                        else
                        {
                            list.gif_tiempo_transicion = rdr["GIF_TIEMPO"].ToString();
                        }
                        if (rdr["IMPUESTO"].ToString() == null)
                        {
                            list.impuesto_producto = 0;
                        }
                        else
                        {
                            list.impuesto_producto = Convert.ToInt32(Convert.ToDecimal(rdr["IMPUESTO"].ToString()));
                        }
                        //traduccion
                        if (rdr["ID_TRADUC"].ToString() == "" || rdr["ID_TRADUC"].ToString() == null)
                        {
                            traduc.lang_id = "1";
                            traduc.nombre = "Default Name";
                            traduc.descripcion = "Default Descripcion";
                            listaTraduc.Add(traduc);
                            list.traducciones = listaTraduc;
                        }
                        else
                        {
                            traduc.lang_id = rdr["ID_TRADUC"].ToString();
                            traduc.nombre = rdr["NOMBRE_TRADUC"].ToString();
                            traduc.descripcion = rdr["DESCRIPCION_TRADUC"].ToString();
                            listaTraduc.Add(traduc);
                            list.traducciones = listaTraduc;
                        }

                        if (rdr["TAGS"] == null)
                        {
                            list.tags = new string[] { "" };
                        }
                        else {

                            list.tags  = rdr["TAGS"].ToString().TrimEnd(',').Split(',');
                        }

                        if (rdr["SEO_TAGS"] == null)
                        {
                            list.seo_tags = new string[] { "" };
                        }
                        else {

                            list.seo_tags = rdr["SEO_TAGS"].ToString().TrimEnd(',').Split(',');
                        }

                        if (string.IsNullOrEmpty(rdr["CABYS"].ToString()))
                        {
                            list.cabys = "";
                        }
                        else
                        {
                            list.cabys = rdr["CABYS"].ToString();
                        }

                        if (rdr["CODIGOTARIFA"] == null)
                        {
                            list.codigo_tarifa = "";
                        }
                        else
                        {
                            list.codigo_tarifa = rdr["CODIGOTARIFA"].ToString();
                        }

                        if (rdr["SKIPFACTURA"] == null)
                        {
                            list.Skip_factura = "";
                        }
                        else
                        {
                            list.Skip_factura = rdr["SKIPFACTURA"].ToString();
                        }
                        
                        if (!string.IsNullOrEmpty(rdr["LONGITUD"].ToString()) && !string.IsNullOrEmpty(rdr["ANCHO"].ToString()) && !string.IsNullOrEmpty(rdr["ALTO"].ToString()))
                        {
                            dimensiones.longitud = rdr["LONGITUD"].ToString();
                            dimensiones.ancho = rdr["ANCHO"].ToString();
                            dimensiones.alto = rdr["ALTO"].ToString();
                            list.dimensiones.Add(dimensiones);
                        }
                        else 
                        {
                            list.dimensiones.Add(null);
                        }
                        

                        listArticulos.Add(list);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                    }
                }
            }
            return listArticulos;
        }

        //METODO QUE ACTUALIZA EL ID DE NIDUX EN NUESTRAS TABLAS
        //ES USADA EN EL APP
        //ES USADA POR EL SERVICIO
        [HttpPut]
        [Route("api/actualizar_id_articulos")]
        public IHttpActionResult Update_Id_Articulos(articulos products)
        {
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            int record = 0;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "" + com + ".ACT_ID_ARTICULOS";
                    cmd.Parameters.AddWithValue("@SKU", ((object)products.sku) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ID", ((object)products.id) ?? DBNull.Value);
                    con.Open();
                    record = cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (SqlException e)
                {
                    con.Close();
                    return Content(HttpStatusCode.BadRequest, new { isSuccessful = true, errorArticulo = "Error en el articulo: " + products.id, message = e.Message.ToString() });
                }
                con.Close();
                return Ok(new { isSuccessful = true, message = "El artículo: " + products.sku + " ha sido actualizado con el id de nidux: " + products.id });
            }
        }

        //METODO QUE OBTIENE LOS ARTICULOS PADRES PARA SER INGRESADOS EN NIDUX
        //ES USADA POR EL SERVICIO
        [HttpGet]
        [Route("api/obtener_articulos_padres")]
        public IEnumerable<Variaciones_Padre> articulos_padre() /////////////// CAMBIOSS
        {
            //Cambia el estado del pedido a 1, en proceso
            var listPadre = new List<Variaciones_Padre>();
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + com + ".MOSTRAR_ARTICULOS_PADRE";/**/
                    con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Variaciones_Padre list = new Variaciones_Padre();
                    list.padre = rdr["ARTICULO"].ToString();
                    list.ID = rdr["ID"].ToString();
                    listPadre.Add(list);
                }
            }
            return listPadre;
        }

        //METODO QUE OBTIENE LOS ATRIBUTOS DEL PADRE PARA SER INGRESADOS EN NIDUX
        //ES USADA POR EL SERVICIO
        [HttpGet]
        [Route("api/agregar_atributos_articulos/{padre}")]
        public IHttpActionResult articulos_hijos_atributos(string padre)
        {
            var listHijos = new List<Valores_hijo_atributos>();
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + com + ".AGREGAR_ATRIBUTOS_ARTICULOS";
                cmd.Parameters.AddWithValue("@ID_PADRE", padre);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                Valores_hijo_atributos atributos = new Valores_hijo_atributos();
                List<int> lista_atributos = new List<int>();
                while (rdr.Read())
                {
                    if (rdr["ID_ATRIBUTO"].ToString() == "" || rdr["ID_ATRIBUTO"].ToString() == null)
                    {
                        atributos.id_atributos = new int[] { };
                    }
                    else
                    {
                        lista_atributos.Add(int.Parse(rdr["ID_ATRIBUTO"].ToString()));
                    }
                }
                atributos.id_atributos = lista_atributos.ToArray();
                listHijos.Add(atributos);
            }
            return Ok(listHijos);
        }

        //METODO QUE OBTIENE LOS ARTICULOS HIJOS DE CADA PADRE PARA SER INGRESADOS EN NIDUX
        //ES USADA POR EL SERVICIO
        [HttpGet]
        [Route("api/agregar_articulo_hijo/{padre}")]
        public IHttpActionResult articulos_hijos(string padre)
        {
            //Cambia el estado del pedido a 1, en proceso
            var listHijos = new List<Valores_hijo>();
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + com + ".AGREGAR_VALORES_ATRIBUTOS_ARTICULOS";
                cmd.Parameters.AddWithValue("@ID_PADRE", padre);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Valores_hijo list = new Valores_hijo();
                    list.id_valores_atributos = rdr["ID_VALORES_ATRIBUTOS"].ToString().Split(',');
                    list.sku_variacion = rdr["ARTICULO"].ToString();
                    list.peso = Convert.ToDecimal(string.Format("{0:0.000}", rdr["PESO"]));
                    list.stock = Convert.ToInt32(Convert.ToDecimal(rdr["CANTIDAD"].ToString()));
                    list.precio = Convert.ToDecimal(string.Format("{0:0.000}", rdr["PRECIO"].ToString()));
                    listHijos.Add(list);
                }
            }
            return Ok(listHijos);
        }

        //METODO QUE OBTIENE LOS PEDIDOS DE NIDUX PARA INGRESARLOS EN NUESTRAS TABLAS PROPIAS
        //ES USADA POR EL SERVICIO
        [HttpPost]
        [Route("api/insertar_pedidos")]
        public IHttpActionResult Obtener_Pedidos(Pedidos pedido)
        {
            if (pedido == null || pedido.ordenes == null || pedido.ordenes.data.Count == 0)
            {
                return Content(HttpStatusCode.BadRequest, new { isSuccessful = false, message = "El objeto Pedido o su diccionario de datos está vacío." });
            }

            int record = 0;
            List<string> lista_error = new List<string>();
            int id_last = 0;
            int id_error = 0;
            DateTime day = DateTime.Now;
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                // Iterar sobre cada entrada en el diccionario 'data'
                foreach (var entry in pedido.ordenes.data)
                {
                    try
                    {
                        string orderId = entry.Key;  // La clave es el orderId
                        var orderData = entry.Value; // El valor es un objeto Data

                        id_error = int.Parse(orderId);

                        // Insertar el encabezado del pedido
                        SqlCommand cmd = new SqlCommand
                        {
                            Connection = con,
                            CommandType = CommandType.StoredProcedure,
                            CommandText = com + ".AGREGAR_PEDIDOS"
                        };
                        string nombre_completo = orderData.consumer_name + orderData.consumer_lastname;

                        cmd.Parameters.AddWithValue("@orderId", (object)orderData.orderId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@cliente", (object)nombre_completo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@identificacion", (object)orderData.consumer_identification ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@correo", (object)orderData.consumer_email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@telefono_fijo", (object)orderData.consumer_tel ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@telefono_movil", (object)orderData.consumer_cel ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@es_anonimo", (object)orderData.consumer_anon ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@monto_impuestos", (object)orderData.order_tax_amount ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@fecha_orden", (object)orderData.order_date ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@orderGiftpointsUsed", (object)orderData.orderGiftpointsUsed ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@estado_orden", (object)orderData.order_status ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@moneda", (object)orderData.moneda ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@observaciones", (object)orderData.observaciones ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@codigo_autorizacion", (object)orderData.codigo_autorizacion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ip_origen", (object)orderData.ip_origen ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@estado_pago", (object)orderData.order_payment_status ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@medio_pago", (object)orderData.method_type ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@total", (object)orderData.total ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@cuponUsado", (object)orderData.cuponUsado ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@cuponTipo", (object)orderData.cuponTipo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@sucursal", (object)orderData.sucursal ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@recoger_sucursal", (object)orderData.order_pickup ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@codigo_metodo_pago", (object)orderData.codigo_metodo_pago ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@moneda_fe", (object)orderData.moneda_fe ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@tasa_impuesto_shipping", (object)orderData.tasa_impuesto_shipping ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@costo_total_shipping", (object)orderData.costo_total_shipping ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@tipo_envio", (object)orderData.tipo_envio ?? DBNull.Value);
                        

                        // Datos de envío
                        if (orderData.direcciones.envio != null)
                        {
                            cmd.Parameters.AddWithValue("@nombre_destinatario_envio", (object)orderData.direcciones.envio.nombre_destinatario ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@identificacion_envio", (object)orderData.direcciones.envio.identificacion ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@tipo_identificacion_envio", (object)orderData.direcciones.envio.tipo_identificacion ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@correo_envio", (object)orderData.direcciones.envio.correo ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@telefono_envio",  (object)orderData.direcciones.envio.telefono ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@movil_envio", (object)orderData.direcciones.envio.movil ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@pais_envio", (object)orderData.direcciones.envio.pais ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@provincia_envio", (object)orderData.direcciones.envio.provincia ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@ciudad_envio", (object)orderData.direcciones.envio.ciudad ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@zip_envio", (object)orderData.direcciones.envio.zip ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@canton_envio", (object)orderData.direcciones.envio.canton ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@distrito_envio", (object)orderData.direcciones.envio.canton ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@detalle_direccion_envio", (object)orderData.direcciones.envio.detalle_direccion ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@geo_latitud_envio", (object)orderData.direcciones.envio.geo_latitud ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@geo_longitud_envio", (object)orderData.direcciones.envio.geo_longitud ?? DBNull.Value);
                        }

                        // Datos de facturación
                        if (orderData.direcciones.facturacion != null)
                        {
                            cmd.Parameters.AddWithValue("@nombre_destinatario_fac", (object)orderData.direcciones.facturacion.nombre_destinatario ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@identificacion_fac", (object)orderData.direcciones.facturacion.identificacion ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@tipo_identificacion_fac", (object)orderData.direcciones.facturacion.tipo_identificacion ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@correo_fac", (object)orderData.direcciones.facturacion.correo ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@telefono_fac", (object)orderData.direcciones.facturacion.telefono ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@movil_fac", (object)orderData.direcciones.facturacion.movil ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@pais_fac", (object)orderData.direcciones.facturacion.pais ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@provincia_fac", (object)orderData.direcciones.facturacion.provincia_fe ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@canton_fac", (object)orderData.direcciones.facturacion.canton ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@distrito_fac", (object)orderData.direcciones.facturacion.distrito ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@detalle_direccion_fac", (object)orderData.direcciones.facturacion.detalle_direccion ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@ciudad_fac",  (object)orderData.direcciones.facturacion.ciudad ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@zip_fac", (object)orderData.direcciones.facturacion.zip ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@geo_latitud_fac", (object)orderData.direcciones.facturacion.geo_latitud ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@geo_longitud_fac", (object)orderData.direcciones.facturacion.geo_longitud ?? DBNull.Value);
                        }

                        // Ejecutar el comando de inserción
                        con.Open();
                        record = cmd.ExecuteNonQuery();
                        con.Close();

                        // Insertar las líneas del pedido
                        foreach (var kvp in orderData.Detalles) 
                        {
                            var detalle = kvp.Value; 

                            SqlCommand cmd2 = new SqlCommand
                            {
                                Connection = con,
                                CommandType = CommandType.StoredProcedure,
                                CommandText = com + ".AGREGAR_PEDIDOS_LINEA"
                            };

                            cmd2.Parameters.AddWithValue("@orderId", (object)orderData.orderId ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@id_producto", (object)detalle.id_producto ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@id_variacion", (object)detalle.id_variacion ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@sku", (object)detalle.sku ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@nombre_producto", (object)detalle.nombre_producto ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@precio", (object)detalle.precio ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@cantidad", (object)detalle.cantidad ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@porcentaje_descuento", (object)detalle.porcentaje_descuento ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@subtotal_descuento", (object)detalle.subtotal_descuento ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@subtotal_linea", (object)detalle.subtotal_linea ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@impuesto", (object)detalle.impuesto ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@subtotal_impuestos", (object)detalle.subtotal_impuestos ?? DBNull.Value);

                            con.Open();
                            record = cmd2.ExecuteNonQuery();
                            con.Close();
                        }
                        id_last = int.Parse(orderId);
                    }
                    catch (Exception ex)
                    {
                        lista_error.Add("Fallo en el pedido numero: " + id_error.ToString() + ", Fecha error: " + day.ToString("MM/dd/yy HH:mm:ss"));
                    }
                }
            }

            return Ok(new { isSuccessful = true, message = "Pedidos ingresados con exito. ", error = lista_error });
        }



        //METODO QUE OBTIENE LOS PEDIDOS QUE SON INGRESADOS EN EL ERP PARA CAMBIAR EL ESTADO EN NIDUX
        //ES USADA POR EL SERVICIO
        [HttpGet]
        [Route("api/actualizar_estado_pedido")]
        public IEnumerable<Estado> Estado_Pedido()
        {
            //Canbia el estado del pedido a 1, en proceso
            var listPedidosID = new List<Estado>();
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + com + ".ACT_ESTADO_ORDEN_PEDIDO";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Estado list = new Estado();
                    list.orderId = int.Parse(rdr["ORDERID"].ToString());
                    list.nuevo_estado = 1;
                    listPedidosID.Add(list);
                }
            }
            return listPedidosID;
        }

        //METODO QUE OBTIENE LOS ARTICULOS QUE SE HAN EDITADO EN CANTIDAD PARA ACTUALIZARLOS EN NIDUX
        //ES USADA POR EL SERVICIO
        [HttpGet]
        [Route("api/actualizar_cantidad")]
        public IEnumerable<Stock> GetStock()
        {
            var liststock = new List<Stock>();
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + com + ".ACT_CANTIDAD_ARTICULOS";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Stock list = new Stock();
                    list.sku = rdr["ARTICULO"].ToString();
                    list.stock = decimal.Parse(string.Format("{0:0.000}", rdr["CANTIDAD"]));
                    liststock.Add(list);
                }
            }
            return liststock;
        }

        //METODO QUE OBTIENE LOS ARTICULOS QUE SE HAN EDITADO EN PRECIO PARA ACTUALIZARLOS EN NIDUX
        //ES USADA POR EL SERVICIO
        [HttpGet]
        [Route("api/actualizar_precios")]
        public IEnumerable<Precios> Get_Precio()
        {
            var listPrice = new List<Precios>();
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + com + ".ACT_PRECIO_ARTICULOS";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Precios list = new Precios();
                    list.sku = rdr["ARTICULO"].ToString();
                    list.price = decimal.Parse(string.Format("{0:0.00}", rdr["PRECIO"]));
                    listPrice.Add(list);
                }
            }
            return listPrice;
        }

        //METODO QUE ACTUALIZA LA FECHA DE CONSULTA DE NUESTRAS TABLAS
        //ES USADA POR EL SERVICIO
        //ES USADA POR EL APP
        [HttpGet]
        [Route("api/actualizar_fecha")]
        public IHttpActionResult Update_Registro_Fecha()
        {
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            int record = 0;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "" + com + ".ACT_FECHA_CONSULTA";
                    con.Open();
                    record = cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    con.Close();
                    return Content(HttpStatusCode.BadRequest, new { isSuccessful = false, message = e.ToString() });
                }
                con.Close();
            }
            return Ok(new { isSuccessful = true, message = "Se ha actualizado la fecha de consulta" });
        }

        //METODO QUE ELIMINA LOS ARTICULOS EN NIDUX
        //ES USADA POR EL SERVICIO
        //ES USADA POR EL APP
        [HttpGet]
        [Route("api/eliminar_articulos")]
        public IEnumerable<Eliminar_Art> Eliminar_Articulos()
        {
            var articulos_delete = new List<Eliminar_Art>();
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + com + ".ELIMINAR_ARTICULOS";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Eliminar_Art eliminar_Art = new Eliminar_Art();
                    eliminar_Art.SKU = rdr["ARTICULO"].ToString();
                    eliminar_Art.ID  = rdr["ID"].ToString();
                    articulos_delete.Add(eliminar_Art);
                }
            }
            return articulos_delete;
        }

        [HttpGet]
        [Route("api/eliminar_articulos_padres")]
        public IEnumerable<Eliminar_Art> Eliminar_Articulos_Padres()
        {
            var articulos_delete = new List<Eliminar_Art>();
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "" + com + ".ELIMINAR_ARTICULOS_PADRES";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Eliminar_Art eliminar_Art = new Eliminar_Art();
                    eliminar_Art.SKU = rdr["ARTICULO"].ToString();
                    eliminar_Art.ID = rdr["ID"].ToString();
                    articulos_delete.Add(eliminar_Art);
                }
            }
            return articulos_delete;
        }

        [HttpPut]
        [Route("api/actualizar_articulos_eliminados/{articulo}")]
        public IHttpActionResult Update_Id_Articulos(string articulo)
        {
            string cs = ConfigurationManager.ConnectionStrings["MyCnn"].ConnectionString;
            int record = 0;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "" + com + ".ACTUALIZAR_ARTICULOS_ELIMANADOS";
                    cmd.Parameters.AddWithValue("@ARTICULO", ((object)articulo) ?? DBNull.Value);
                    con.Open();
                    record = cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (SqlException e)
                {
                    con.Close();
                    return Content(HttpStatusCode.BadRequest, new { isSuccessful = true, errorArticulo = "Error en el articulo: " + articulo, message = e.Message.ToString() });
                }
                con.Close();
                return Ok(new { isSuccessful = true, message = "El artículo: " + articulo + " ha sido actualizado" });
            }
        }

      

    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using AplixEcommerceIntegration.Globales;
using Newtonsoft.Json;
using RestSharp;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Data.OracleClient;
using System.Threading;
using AplixEcommerceIntegration.Nidux.Clases;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using AplixEcommerceIntegration.Shopify.Clases;
using System.Web;
using System.Xml.Linq;


namespace AplixEcommerceIntegration.Nidux
{
    public partial class UC_Nidux_SUKASA : UserControl
    {
        Conexion cnn = new Conexion();
        static List<Clases.Articulos> lista_articulos_editados = new List<Clases.Articulos>();
        static List<Clases.Categorias> lista_categorias_editados = new List<Clases.Categorias>();
        static List<Clases.Marca> lista_marcas_editados = new List<Clases.Marca>();
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];
        LogsFile logsFile = new LogsFile();


        //static string valor enviado por combo_box de datagreed
        static string valor_enviado_por_combo_box = "";
        static int columna_combo = 0;
        public UC_Nidux_SUKASA()
        {
            InitializeComponent();
            this.dgvArticulos.Columns["articulo"].Frozen = true;
            this.dgvArticulos.Columns["nombre"].Frozen = true;
            this.dgvPedidos.Columns["ORDERID"].Frozen = true;
        }

        private void UC_Nidux_Load(object sender, EventArgs e)
        {
            CargaDatos_Articulos();
            CargaDatos_Categorias();
            CargaDatos_Marcas();
            CargaDatos_Atributos();
            CargaDatos_Pedidos();
            Activar_Descuentos_Check();

            filtrarArticulos();
            filtrarPedidos();
            InfoLabelConexion();
        }

        /*------------------------ METODOS GENERALES -----------------------------------*/
        private DataTable GetData(string sql)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dr = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dr.Fill(dt);
                cnn.CerrarConexion();
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetDataSP(string sql)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = sql;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dr = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dr.Fill(dt);
                cnn.CerrarConexion();
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Logs

        private FileSystemWatcher watcher;
        private string rutaArchivoSeleccionado;



        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {

            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    System.Threading.Thread.Sleep(100);
                    textBitacora.Text = File.ReadAllText(rutaArchivoSeleccionado);
                }
                catch (IOException)
                {

                }
            });
        }

        #endregion

        #region Inicializar informacion label conexion 
        public void InfoLabelConexion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionString);

            string server = builder.DataSource;
            string database = builder.InitialCatalog;
            string user = builder.UserID;

            labelConexion.Text = $"Conectado a: Server={server}; DB={database}; Usuario={user}";

            string oracleConnStr = ConfigurationManager.ConnectionStrings["conexionOracle"].ConnectionString;
            var oracleBuilder = new OracleConnectionStringBuilder(oracleConnStr);

            string oracleDataSource = oracleBuilder.DataSource;
            string oracleUser = oracleBuilder.UserID;

            labelConexionOracle.Text = $"Conectado a Oracle: DataSource={oracleDataSource}; Usuario={oracleUser}";
        }

        #endregion

        private void btnGuardarConfiguracion_Click(object sender, EventArgs e)
        {
            try
            {
                if (chbActivarSincro.Checked == false)
                {
                    //string query = "UPDATE " + db + ".APLIX.RELACION_COMPANIAS SET SINCRONIZA_DESCUENTOS = 'N'";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "UPDATE " + com + ".RELACION_COMPANIAS SET SINCRONIZA_DESCUENTOS = 'N'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    config.AppSettings.Settings["Act_Descuento"].Value = "N";
                    config.Save(ConfigurationSaveMode.Modified);
                    carga_config();
                    MessageBox.Show("La Sincronización de Descuentos ha sido Desactivada", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cnn.CerrarConexion();
                }
                else
                {
                    //string query = "UPDATE " + db + ".APLIX.RELACION_COMPANIAS SET SINCRONIZA_DESCUENTOS = 'S'";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "UPDATE " + com + ".RELACION_COMPANIAS SET SINCRONIZA_DESCUENTOS = 'S'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    config.AppSettings.Settings["Act_Descuento"].Value = "S";
                    config.Save(ConfigurationSaveMode.Modified);
                    carga_config();
                    MessageBox.Show("La Sincronización de Descuentos ha sido Activada", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cnn.CerrarConexion();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logsFile.WriteLogs(@"
                ==================================================
                🛑 [ERROR AL GUARDAR CONFIGURACIÓN DE DESCUENTOS] 🛑
                --------------------------------------------------
                Mensaje: " + ex.Message.ToString() + @"
                ==================================================");

            }

            //Actualizacion de valores de configuracion
            try
            {
                //SqlCommand cmd = new SqlCommand();
                //cmd.Connection = cnn.AbrirConexion();
                //cmd.CommandText = "UPDATE " + com + ".RELACION_COMPANIAS SET SINCRONIZA_DESCUENTOS = 'S', BODEGA = " + txtbodega.Text  + ", NIVEL_PRECIO = " ;
                //cmd.CommandTimeout = 0;
                //cmd.CommandType = CommandType.Text;
                //cmd.ExecuteNonQuery();
                //Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                //config.AppSettings.Settings["Act_Descuento"].Value = "S";
                //config.Save(ConfigurationSaveMode.Modified);
                //carga_config();
                //MessageBox.Show("La Sincronización de Descuentos ha sido Activada", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //cnn.CerrarConexion();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void carga_config()
        {
            bool activar = false;
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            string descuento = config.AppSettings.Settings["Act_Descuento"].Value;
            if (descuento.Equals("N"))
            {
                activar = false;
            }
            else
            {
                activar = true;
            }
            foreach (DataGridViewRow item in dgvArticulos.Rows)
            {
                item.Cells[8].ReadOnly = activar;
            }
        }

        public void Activar_Descuentos_Check()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            string descuento = config.AppSettings.Settings["Act_Descuento"].Value;
            if (descuento.Equals("N"))
            {
                chbActivarSincro.Checked = false;
            }
            else
            {
                chbActivarSincro.Checked = true;
            }
        }

        /*------------------------ ARTICULOS -----------------------------------*/
        public void CargaDatos_Articulos()
        {
            try
            {
                bool activar = false;
                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                string descuento = config.AppSettings.Settings["Act_Descuento"].Value;
                if (descuento.Equals("N"))
                {
                    activar = false;
                }
                else
                {
                    activar = true;
                }

                DataTable dt = new DataTable();
                dt = GetDataSP("" + com + ".CARGAR_ARTICULOS_APP");
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvArticulos.Rows.Add();

                    dgvArticulos.Rows[n].Cells[0].Value = item["ARTICULO"].ToString();
                    dgvArticulos.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                    dgvArticulos.Rows[n].Cells[2].Value = item["NOMBRE_NIDUX"].ToString();
                    dgvArticulos.Rows[n].Cells[3].Value = item["DESCRIPCION"].ToString();
                    dgvArticulos.Rows[n].Cells[4].Value = item["DESCRIPCION_NIDUX"].ToString();

                    dgvArticulos.Rows[n].Cells[5].Value = item["PESO"].ToString();
                    dgvArticulos.Rows[n].Cells[6].Value = item["CANTIDAD"].ToString();
                    dgvArticulos.Rows[n].Cells[7].Value = item["PRECIO"].ToString();
                    dgvArticulos.Rows[n].Cells[8].Value = item["DESCUENTO"].ToString();
                    dgvArticulos.Rows[n].Cells[8].ReadOnly = activar;

                    dgvArticulos.Rows[n].Cells[9].Value = item["IMPUESTO"].ToString();

                    dgvArticulos.Rows[n].Cells[10].Value = item["ACTIVO"].ToString();
                    dgvArticulos.Rows[n].Cells[11].Value = item["ESTADO"].ToString();

                    dgvArticulos.Rows[n].Cells[12].Value = item["ID"].ToString();
                    dgvArticulos.Rows[n].Cells[13].Value = item["ID_MARCA"].ToString();
                    dgvArticulos.Rows[n].Cells[14].Value = item["ID_CATEGORIAS"].ToString();
                    dgvArticulos.Rows[n].Cells[15].Value = item["ID_VALORES_ATRIBUTOS"].ToString();
                    dgvArticulos.Rows[n].Cells[16].Value = item["ID_PADRE"].ToString();
                    dgvArticulos.Rows[n].Cells[17].Value = item["ID_HIJO"].ToString();
                    dgvArticulos.Rows[n].Cells[18].Value = item["INDICADOR_STOCK"].ToString();
                    dgvArticulos.Rows[n].Cells[19].Value = item["DESTACADO"].ToString();
                    dgvArticulos.Rows[n].Cells[20].Value = item["COSTO_SHIPPING"].ToString();
                    dgvArticulos.Rows[n].Cells[21].Value = item["PERMITE_RESERVA"].ToString();
                    dgvArticulos.Rows[n].Cells[22].Value = item["PORCENTAJE_RESERVA"].ToString();
                    dgvArticulos.Rows[n].Cells[23].Value = item["LIMITE_CARRITO"].ToString();
                    dgvArticulos.Rows[n].Cells[24].Value = item["USAR_GIF"].ToString();
                    dgvArticulos.Rows[n].Cells[25].Value = item["GIF_TIEMPO"].ToString();
                    dgvArticulos.Rows[n].Cells[26].Value = item["VIDEO_YOUTUBE"].ToString();
                    dgvArticulos.Rows[n].Cells[27].Value = item["NOMBRE_TRADUC"].ToString();
                    dgvArticulos.Rows[n].Cells[28].Value = item["DESCRIPCION_TRADUC"].ToString();
                    dgvArticulos.Rows[n].Cells[29].Value = item["TAGS"].ToString();
                    dgvArticulos.Rows[n].Cells[30].Value = item["SEO_TAGS"].ToString();
                }
                //ESTADO
                DataTable dt_estado = this.GetData("SELECT ID, DESCRIPCION FROM " + com + ".ARTICULO_ESTADO");
                foreach (DataGridViewRow row in dgvArticulos.Rows)
                {
                    DataGridViewComboBoxCell comboBoxCell = (row.Cells[11] as DataGridViewComboBoxCell);///llamar a la tabla que hago en la vista
                    foreach (DataRow drow in dt_estado.Rows)
                    {
                        string id = drow[0].ToString();
                        comboBoxCell.Items.Add(drow[1]);
                        if (row.Cells[11].Value.ToString() == id)
                        {
                            comboBoxCell.Value = drow[1];
                        }
                    }
                }
                //foreach para el llenado de los checkbox
                foreach (DataGridViewRow row in dgvArticulos.Rows)
                {
                    DataGridViewCheckBoxCell comboBoxCell = (row.Cells[10] as DataGridViewCheckBoxCell);///ACTIVO
                    if (row.Cells[10].Value.ToString() == "S")
                    {
                        comboBoxCell.Value = true;
                    }
                    else
                    {
                        comboBoxCell.Value = false;
                    }
                    DataGridViewCheckBoxCell comboBoxCellindicador = (row.Cells[18] as DataGridViewCheckBoxCell);///INDICADOR_STOCK
                    if (row.Cells[18].Value.ToString() == "S")
                    {
                        comboBoxCellindicador.Value = true;
                    }
                    else
                    {
                        comboBoxCellindicador.Value = false;
                    }
                    DataGridViewCheckBoxCell comboBoxCellDestacado = (row.Cells[19] as DataGridViewCheckBoxCell);///DESTACADO
                    if (row.Cells[19].Value.ToString() == "S")
                    {
                        comboBoxCellDestacado.Value = true;
                    }
                    else
                    {
                        comboBoxCellDestacado.Value = false;
                    }
                    DataGridViewCheckBoxCell comboBoxCellReserva = (row.Cells[21] as DataGridViewCheckBoxCell);///PERMITE_RESERVA
                    if (row.Cells[21].Value.ToString() == "S")
                    {
                        comboBoxCellReserva.Value = true;
                    }
                    else
                    {
                        comboBoxCellReserva.Value = false;
                    }
                    DataGridViewCheckBoxCell comboBoxCellGif = (row.Cells[24] as DataGridViewCheckBoxCell);///USAR_GIF
                    if (row.Cells[24].Value.ToString() == "S")
                    {
                        comboBoxCellGif.Value = true;
                    }
                    else
                    {
                        comboBoxCellGif.Value = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logsFile.WriteLogs(@"
                ==================================================
                🛑 [ERROR EN CARGA DE ARTÍCULOS APP] 🛑
                --------------------------------------------------
                Mensaje: " + ex.Message.ToString() + @"
                ==================================================");
            }
        }

        public void Buscar_Articulos(string articulo)
        {
            try
            {
                bool activar = false;
                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                string descuento = config.AppSettings.Settings["Act_Descuento"].Value;
                if (descuento.Equals("N"))
                {
                    activar = false;
                }
                else
                {
                    activar = true;
                }

                dgvArticulos.Rows.Clear();
                if (cbbArticulos.Text.Equals("Nombre"))
                {
                    //buscamos por nombre
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "SELECT AR.ARTICULO,AR.NOMBRE, AR.NOMBRE_NIDUX, AR.DESCRIPCION, AR.DESCRIPCION_NIDUX, AR.PESO,AR.ACTIVO,AR.ESTADO,AR.ID,AR.ID_MARCA,AR.ID_CATEGORIAS,AR.ID_VALORES_ATRIBUTOS,AR.ID_PADRE,AR.ID_HIJO," +
                        " AR.INDICADOR_STOCK,AR.DESTACADO,AR.COSTO_SHIPPING,AR.PERMITE_RESERVA,AR.PORCENTAJE_RESERVA,AR.LIMITE_CARRITO,AR.USAR_GIF,AR.GIF_TIEMPO,AR.VIDEO_YOUTUBE," +
                        " AR.NOMBRE_TRADUC,AR.DESCRIPCION_TRADUC, AR.PESO, AC.CANTIDAD, AP.PRECIO, AP.DESCUENTO, AR.IMPUESTO FROM " + com + ".ARTICULOS AR INNER JOIN " + com + ".ARTICULOS_CANTIDAD AS AC ON AR.ARTICULO = AC.ARTICULO " +
                        " INNER JOIN " + com + ".ARTICULOS_PRECIO AS AP ON AR.ARTICULO = AP.ARTICULO WHERE AR.NOMBRE LIKE '%" + articulo + "%' AND AR.ACTIVO = 'S'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dgvArticulos.Rows.Add();

                        Clases.Articulos product = lista_articulos_editados.FirstOrDefault(x => x.sku == dt.Rows[n]["ARTICULO"].ToString());

                        if (product == null)
                        {
                            dgvArticulos.Rows[n].Cells[0].Value = item["ARTICULO"].ToString();
                            dgvArticulos.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                            dgvArticulos.Rows[n].Cells[2].Value = item["NOMBRE_NIDUX"].ToString();
                            dgvArticulos.Rows[n].Cells[3].Value = item["DESCRIPCION"].ToString();
                            dgvArticulos.Rows[n].Cells[4].Value = item["DESCRIPCION_NIDUX"].ToString();

                            dgvArticulos.Rows[n].Cells[5].Value = item["PESO"].ToString();
                            dgvArticulos.Rows[n].Cells[6].Value = item["CANTIDAD"].ToString();
                            dgvArticulos.Rows[n].Cells[7].Value = item["PRECIO"].ToString();
                            dgvArticulos.Rows[n].Cells[8].Value = item["DESCUENTO"].ToString();
                            dgvArticulos.Rows[n].Cells[8].ReadOnly = activar;

                            dgvArticulos.Rows[n].Cells[9].Value = item["IMPUESTO"].ToString();

                            dgvArticulos.Rows[n].Cells[10].Value = item["ACTIVO"].ToString();
                            dgvArticulos.Rows[n].Cells[11].Value = item["ESTADO"].ToString();

                            dgvArticulos.Rows[n].Cells[12].Value = item["ID"].ToString();
                            dgvArticulos.Rows[n].Cells[13].Value = item["ID_MARCA"].ToString();
                            dgvArticulos.Rows[n].Cells[14].Value = item["ID_CATEGORIAS"].ToString();
                            dgvArticulos.Rows[n].Cells[15].Value = item["ID_VALORES_ATRIBUTOS"].ToString();
                            dgvArticulos.Rows[n].Cells[16].Value = item["ID_PADRE"].ToString();
                            dgvArticulos.Rows[n].Cells[17].Value = item["ID_HIJO"].ToString();
                            dgvArticulos.Rows[n].Cells[18].Value = item["INDICADOR_STOCK"].ToString();
                            dgvArticulos.Rows[n].Cells[19].Value = item["DESTACADO"].ToString();
                            dgvArticulos.Rows[n].Cells[20].Value = item["COSTO_SHIPPING"].ToString();
                            dgvArticulos.Rows[n].Cells[21].Value = item["PERMITE_RESERVA"].ToString();
                            dgvArticulos.Rows[n].Cells[22].Value = item["PORCENTAJE_RESERVA"].ToString();
                            dgvArticulos.Rows[n].Cells[23].Value = item["LIMITE_CARRITO"].ToString();
                            dgvArticulos.Rows[n].Cells[24].Value = item["USAR_GIF"].ToString();
                            dgvArticulos.Rows[n].Cells[25].Value = item["GIF_TIEMPO"].ToString();
                            dgvArticulos.Rows[n].Cells[26].Value = item["VIDEO_YOUTUBE"].ToString();
                            dgvArticulos.Rows[n].Cells[27].Value = item["NOMBRE_TRADUC"].ToString();
                            dgvArticulos.Rows[n].Cells[28].Value = item["DESCRIPCION_TRADUC"].ToString();
                        }
                        else
                        {
                            dgvArticulos.Rows[n].Cells[0].Value = product.sku;
                            dgvArticulos.Rows[n].Cells[1].Value = product.nombre;
                            dgvArticulos.Rows[n].Cells[2].Value = product.nombre_nidux;
                            dgvArticulos.Rows[n].Cells[3].Value = product.descripcion;
                            dgvArticulos.Rows[n].Cells[4].Value = product.descripcion_nidux;

                            dgvArticulos.Rows[n].Cells[5].Value = product.peso_producto;
                            dgvArticulos.Rows[n].Cells[6].Value = product.stock_principal;
                            dgvArticulos.Rows[n].Cells[7].Value = product.precio;
                            dgvArticulos.Rows[n].Cells[8].Value = product.porcentaje_oferta;
                            dgvArticulos.Rows[n].Cells[8].ReadOnly = activar;

                            dgvArticulos.Rows[n].Cells[9].Value = product.impuesto;

                            dgvArticulos.Rows[n].Cells[10].Value = product.activo;
                            dgvArticulos.Rows[n].Cells[11].Value = product.estado_de_producto;

                            dgvArticulos.Rows[n].Cells[12].Value = product.id;
                            dgvArticulos.Rows[n].Cells[13].Value = product.id_marca;
                            dgvArticulos.Rows[n].Cells[14].Value = product.categorias;
                            dgvArticulos.Rows[n].Cells[15].Value = product.atributos;
                            dgvArticulos.Rows[n].Cells[16].Value = product.id_padre;
                            dgvArticulos.Rows[n].Cells[17].Value = product.id_hijo;
                            dgvArticulos.Rows[n].Cells[18].Value = product.ocultar_indicador_stock;
                            dgvArticulos.Rows[n].Cells[19].Value = product.es_destacado;
                            dgvArticulos.Rows[n].Cells[20].Value = product.costo_shipping_individual;
                            dgvArticulos.Rows[n].Cells[21].Value = product.producto_permite_reservacion;
                            dgvArticulos.Rows[n].Cells[22].Value = product.porcentaje_para_reservar;
                            dgvArticulos.Rows[n].Cells[23].Value = product.limite_para_reservar_en_carrito;
                            dgvArticulos.Rows[n].Cells[24].Value = product.usar_gif_en_homepage;
                            dgvArticulos.Rows[n].Cells[25].Value = product.gif_tiempo_transicion;
                            dgvArticulos.Rows[n].Cells[26].Value = product.video_youtube_url;
                            dgvArticulos.Rows[n].Cells[27].Value = product.lang_nombre;
                            dgvArticulos.Rows[n].Cells[28].Value = product.lang_descripcion;
                        }

                    }
                    //ESTADO
                    DataTable dt_estado = this.GetData("SELECT ID, DESCRIPCION FROM " + com + ".ARTICULO_ESTADO");
                    foreach (DataGridViewRow row in dgvArticulos.Rows)
                    {
                        DataGridViewComboBoxCell comboBoxCell = (row.Cells[11] as DataGridViewComboBoxCell);///llamar a la tabla que hago en la vista
                        foreach (DataRow drow in dt_estado.Rows)
                        {
                            string id = drow[0].ToString();
                            comboBoxCell.Items.Add(drow[1]);
                            if (row.Cells[11].Value.ToString() == id)
                            {
                                comboBoxCell.Value = drow[1];
                            }
                        }
                    }

                    //foreach para el llenado de los checkbox
                    foreach (DataGridViewRow row in dgvArticulos.Rows)
                    {
                        DataGridViewCheckBoxCell comboBoxCell = (row.Cells[10] as DataGridViewCheckBoxCell);///ACTIVO
                        if (row.Cells[10].Value.ToString() == "S" || row.Cells[10].Value.Equals("True"))
                        {
                            comboBoxCell.Value = true;
                        }
                        else
                        {
                            comboBoxCell.Value = false;
                        }
                        DataGridViewCheckBoxCell comboBoxCellindicador = (row.Cells[18] as DataGridViewCheckBoxCell);///INDICADOR_STOCK
                        if (row.Cells[18].Value.ToString() == "S" || row.Cells[18].Value.Equals("True"))
                        {
                            comboBoxCellindicador.Value = true;
                        }
                        else
                        {
                            comboBoxCellindicador.Value = false;
                        }
                        DataGridViewCheckBoxCell comboBoxCellDestacado = (row.Cells[19] as DataGridViewCheckBoxCell);///DESTACADO
                        if (row.Cells[19].Value.ToString() == "S" || row.Cells[19].Value.Equals("True"))
                        {
                            comboBoxCellDestacado.Value = true;
                        }
                        else
                        {
                            comboBoxCellDestacado.Value = false;
                        }
                        DataGridViewCheckBoxCell comboBoxCellReserva = (row.Cells[21] as DataGridViewCheckBoxCell);///PERMITE_RESERVA
                        if (row.Cells[21].Value.ToString() == "S" || row.Cells[21].Value.Equals("True"))
                        {
                            comboBoxCellReserva.Value = true;
                        }
                        else
                        {
                            comboBoxCellReserva.Value = false;
                        }
                        DataGridViewCheckBoxCell comboBoxCellGif = (row.Cells[24] as DataGridViewCheckBoxCell);///USAR_GIF
                        if (row.Cells[24].Value.ToString() == "S" || row.Cells[24].Value.Equals("True"))
                        {
                            comboBoxCellGif.Value = true;
                        }
                        else
                        {
                            comboBoxCellGif.Value = false;
                        }
                    }
                }
                else
                {
                    //buscamos por código
                    //buscamos por nombre
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "SELECT AR.ARTICULO, AR.NOMBRE, AR.NOMBRE_NIDUX, AR.DESCRIPCION, AR.DESCRIPCION_NIDUX, AR.PESO,AR.ACTIVO,AR.ESTADO,AR.ID,AR.ID_MARCA,AR.ID_CATEGORIAS,AR.ID_VALORES_ATRIBUTOS,AR.ID_PADRE,AR.ID_HIJO," +
                        " AR.INDICADOR_STOCK,AR.DESTACADO,AR.COSTO_SHIPPING,AR.PERMITE_RESERVA,AR.PORCENTAJE_RESERVA,AR.LIMITE_CARRITO,AR.USAR_GIF,AR.GIF_TIEMPO,AR.VIDEO_YOUTUBE," +
                        " AR.NOMBRE_TRADUC,AR.DESCRIPCION_TRADUC, AR.PESO, AC.CANTIDAD, AP.PRECIO, AP.DESCUENTO, AR.IMPUESTO, AR.TAGS, AR.SEO_TAGS FROM " + com + ".ARTICULOS AR INNER JOIN " + com + ".ARTICULOS_CANTIDAD AS AC ON AR.ARTICULO = AC.ARTICULO " +
                        " INNER JOIN " + com + ".ARTICULOS_PRECIO AS AP ON AR.ARTICULO = AP.ARTICULO WHERE AR.ARTICULO LIKE '%" + articulo + "%' AND AR.ACTIVO = 'S'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dgvArticulos.Rows.Add();

                        Clases.Articulos product = lista_articulos_editados.FirstOrDefault(x => x.sku == dt.Rows[n]["ARTICULO"].ToString());

                        if (product == null)
                        {
                            dgvArticulos.Rows[n].Cells[0].Value = item["ARTICULO"].ToString();
                            dgvArticulos.Rows[n].Cells[1].Value = item["NOMBRE"].ToString();
                            dgvArticulos.Rows[n].Cells[2].Value = item["NOMBRE_NIDUX"].ToString();
                            dgvArticulos.Rows[n].Cells[3].Value = item["DESCRIPCION"].ToString();
                            dgvArticulos.Rows[n].Cells[4].Value = item["DESCRIPCION_NIDUX"].ToString();

                            dgvArticulos.Rows[n].Cells[5].Value = item["PESO"].ToString();
                            dgvArticulos.Rows[n].Cells[6].Value = item["CANTIDAD"].ToString();
                            dgvArticulos.Rows[n].Cells[7].Value = item["PRECIO"].ToString();
                            dgvArticulos.Rows[n].Cells[8].Value = item["DESCUENTO"].ToString();
                            dgvArticulos.Rows[n].Cells[8].ReadOnly = activar;

                            dgvArticulos.Rows[n].Cells[9].Value = item["IMPUESTO"].ToString();

                            dgvArticulos.Rows[n].Cells[10].Value = item["ACTIVO"].ToString();
                            dgvArticulos.Rows[n].Cells[11].Value = item["ESTADO"].ToString();

                            dgvArticulos.Rows[n].Cells[12].Value = item["ID"].ToString();
                            dgvArticulos.Rows[n].Cells[13].Value = item["ID_MARCA"].ToString();
                            dgvArticulos.Rows[n].Cells[14].Value = item["ID_CATEGORIAS"].ToString();
                            dgvArticulos.Rows[n].Cells[15].Value = item["ID_VALORES_ATRIBUTOS"].ToString();
                            dgvArticulos.Rows[n].Cells[16].Value = item["ID_PADRE"].ToString();
                            dgvArticulos.Rows[n].Cells[17].Value = item["ID_HIJO"].ToString();
                            dgvArticulos.Rows[n].Cells[18].Value = item["INDICADOR_STOCK"].ToString();
                            dgvArticulos.Rows[n].Cells[19].Value = item["DESTACADO"].ToString();
                            dgvArticulos.Rows[n].Cells[20].Value = item["COSTO_SHIPPING"].ToString();
                            dgvArticulos.Rows[n].Cells[21].Value = item["PERMITE_RESERVA"].ToString();
                            dgvArticulos.Rows[n].Cells[22].Value = item["PORCENTAJE_RESERVA"].ToString();
                            dgvArticulos.Rows[n].Cells[23].Value = item["LIMITE_CARRITO"].ToString();
                            dgvArticulos.Rows[n].Cells[24].Value = item["USAR_GIF"].ToString();
                            dgvArticulos.Rows[n].Cells[25].Value = item["GIF_TIEMPO"].ToString();
                            dgvArticulos.Rows[n].Cells[26].Value = item["VIDEO_YOUTUBE"].ToString();
                            dgvArticulos.Rows[n].Cells[27].Value = item["NOMBRE_TRADUC"].ToString();
                            dgvArticulos.Rows[n].Cells[28].Value = item["DESCRIPCION_TRADUC"].ToString();
                            dgvArticulos.Rows[n].Cells[29].Value = item["TAGS"].ToString();
                            dgvArticulos.Rows[n].Cells[30].Value = item["SEO_TAGS"].ToString();

                        }
                        else
                        {
                            dgvArticulos.Rows[n].Cells[0].Value = product.sku;
                            dgvArticulos.Rows[n].Cells[1].Value = product.nombre;
                            dgvArticulos.Rows[n].Cells[2].Value = product.nombre_nidux;
                            dgvArticulos.Rows[n].Cells[3].Value = product.descripcion;
                            dgvArticulos.Rows[n].Cells[4].Value = product.descripcion_nidux;

                            dgvArticulos.Rows[n].Cells[5].Value = product.peso_producto;
                            dgvArticulos.Rows[n].Cells[6].Value = product.stock_principal;
                            dgvArticulos.Rows[n].Cells[7].Value = product.precio;
                            dgvArticulos.Rows[n].Cells[8].Value = product.porcentaje_oferta;
                            dgvArticulos.Rows[n].Cells[8].ReadOnly = activar;

                            dgvArticulos.Rows[n].Cells[9].Value = product.impuesto;

                            dgvArticulos.Rows[n].Cells[10].Value = product.activo;
                            dgvArticulos.Rows[n].Cells[11].Value = product.estado_de_producto;

                            dgvArticulos.Rows[n].Cells[12].Value = product.id;
                            dgvArticulos.Rows[n].Cells[13].Value = product.id_marca;
                            dgvArticulos.Rows[n].Cells[14].Value = product.categorias;
                            dgvArticulos.Rows[n].Cells[15].Value = product.atributos;
                            dgvArticulos.Rows[n].Cells[16].Value = product.id_padre;
                            dgvArticulos.Rows[n].Cells[17].Value = product.id_hijo;
                            dgvArticulos.Rows[n].Cells[18].Value = product.ocultar_indicador_stock;
                            dgvArticulos.Rows[n].Cells[19].Value = product.es_destacado;
                            dgvArticulos.Rows[n].Cells[20].Value = product.costo_shipping_individual;
                            dgvArticulos.Rows[n].Cells[21].Value = product.producto_permite_reservacion;
                            dgvArticulos.Rows[n].Cells[22].Value = product.porcentaje_para_reservar;
                            dgvArticulos.Rows[n].Cells[23].Value = product.limite_para_reservar_en_carrito;
                            dgvArticulos.Rows[n].Cells[24].Value = product.usar_gif_en_homepage;
                            dgvArticulos.Rows[n].Cells[25].Value = product.gif_tiempo_transicion;
                            dgvArticulos.Rows[n].Cells[26].Value = product.video_youtube_url;
                            dgvArticulos.Rows[n].Cells[27].Value = product.lang_nombre;
                            dgvArticulos.Rows[n].Cells[28].Value = product.lang_descripcion;
                            dgvArticulos.Rows[n].Cells[29].Value = product.tags;
                            dgvArticulos.Rows[n].Cells[30].Value = product.seo_tags;
                        }

                    }
                    //ESTADO
                    DataTable dt_estado = this.GetData("SELECT ID, DESCRIPCION FROM " + com + ".ARTICULO_ESTADO");
                    foreach (DataGridViewRow row in dgvArticulos.Rows)
                    {
                        DataGridViewComboBoxCell comboBoxCell = (row.Cells[11] as DataGridViewComboBoxCell);///llamar a la tabla que hago en la vista
                        foreach (DataRow drow in dt_estado.Rows)
                        {
                            string id = drow[0].ToString();
                            comboBoxCell.Items.Add(drow[1]);
                            if (row.Cells[11].Value.ToString() == id)
                            {
                                comboBoxCell.Value = drow[1];
                            }
                        }
                    }

                    //foreach para el llenado de los checkbox
                    foreach (DataGridViewRow row in dgvArticulos.Rows)
                    {
                        DataGridViewCheckBoxCell comboBoxCell = (row.Cells[10] as DataGridViewCheckBoxCell);///ACTIVO
                        if (row.Cells[10].Value.ToString() == "S" || row.Cells[10].Value.Equals("True"))
                        {
                            comboBoxCell.Value = true;
                        }
                        else
                        {
                            comboBoxCell.Value = false;
                        }
                        DataGridViewCheckBoxCell comboBoxCellindicador = (row.Cells[18] as DataGridViewCheckBoxCell);///INDICADOR_STOCK
                        if (row.Cells[18].Value.ToString() == "S" || row.Cells[18].Value.Equals("True"))
                        {
                            comboBoxCellindicador.Value = true;
                        }
                        else
                        {
                            comboBoxCellindicador.Value = false;
                        }
                        DataGridViewCheckBoxCell comboBoxCellDestacado = (row.Cells[19] as DataGridViewCheckBoxCell);///DESTACADO
                        if (row.Cells[19].Value.ToString() == "S" || row.Cells[19].Value.Equals("True"))
                        {
                            comboBoxCellDestacado.Value = true;
                        }
                        else
                        {
                            comboBoxCellDestacado.Value = false;
                        }
                        DataGridViewCheckBoxCell comboBoxCellReserva = (row.Cells[21] as DataGridViewCheckBoxCell);///PERMITE_RESERVA
                        if (row.Cells[21].Value.ToString() == "S" || row.Cells[21].Value.Equals("True"))
                        {
                            comboBoxCellReserva.Value = true;
                        }
                        else
                        {
                            comboBoxCellReserva.Value = false;
                        }
                        DataGridViewCheckBoxCell comboBoxCellGif = (row.Cells[24] as DataGridViewCheckBoxCell);///USAR_GIF
                        if (row.Cells[24].Value.ToString() == "S" || row.Cells[24].Value.Equals("True"))
                        {
                            comboBoxCellGif.Value = true;
                        }
                        else
                        {
                            comboBoxCellGif.Value = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                logsFile.WriteLogs($@"
                ===============================
                🔍 [ERROR AL BUSCAR ARTÍCULO] 🔍
                -------------------------------
                Mensaje: {ex.Message}
                ===============================");

            }
        }

        private void txtBuscarArticulos_TextChanged(object sender, EventArgs e)
        {
            Buscar_Articulos(txtBuscarArticulos.Text.ToString());
        }

        private void dgvArticulos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Clases.Articulos articulos = new Clases.Articulos();

            articulos.sku = dgvArticulos.CurrentRow.Cells[0].Value.ToString();
            articulos.nombre = dgvArticulos.CurrentRow.Cells[1].Value.ToString();
            articulos.nombre_nidux = dgvArticulos.CurrentRow.Cells[2].Value.ToString();
            articulos.descripcion = dgvArticulos.CurrentRow.Cells[3].Value.ToString();
            articulos.descripcion_nidux = dgvArticulos.CurrentRow.Cells[4].Value.ToString();
            articulos.peso_producto = dgvArticulos.CurrentRow.Cells[5].Value.ToString();
            articulos.stock_principal = dgvArticulos.CurrentRow.Cells[6].Value.ToString();
            articulos.precio = dgvArticulos.CurrentRow.Cells[7].Value.ToString();
            articulos.porcentaje_oferta = dgvArticulos.CurrentRow.Cells[8].Value.ToString();
            articulos.impuesto = dgvArticulos.CurrentRow.Cells[9].Value.ToString();
            articulos.activo = dgvArticulos.CurrentRow.Cells[10].Value.ToString();
            articulos.estado_de_producto = dgvArticulos.CurrentRow.Cells[11].Value.ToString();
            articulos.id = dgvArticulos.CurrentRow.Cells[12].Value.ToString();
            articulos.id_marca = dgvArticulos.CurrentRow.Cells[13].Value.ToString();
            articulos.categorias = dgvArticulos.CurrentRow.Cells[14].Value.ToString();
            articulos.atributos = dgvArticulos.CurrentRow.Cells[15].Value.ToString();
            articulos.id_padre = dgvArticulos.CurrentRow.Cells[16].Value.ToString();
            articulos.id_hijo = dgvArticulos.CurrentRow.Cells[17].Value.ToString();
            articulos.ocultar_indicador_stock = dgvArticulos.CurrentRow.Cells[18].Value.ToString();
            articulos.es_destacado = dgvArticulos.CurrentRow.Cells[19].Value.ToString();
            articulos.costo_shipping_individual = dgvArticulos.CurrentRow.Cells[20].Value.ToString();
            articulos.producto_permite_reservacion = dgvArticulos.CurrentRow.Cells[21].Value.ToString();
            articulos.porcentaje_para_reservar = dgvArticulos.CurrentRow.Cells[22].Value.ToString();
            articulos.limite_para_reservar_en_carrito = dgvArticulos.CurrentRow.Cells[23].Value.ToString();
            articulos.usar_gif_en_homepage = dgvArticulos.CurrentRow.Cells[24].Value.ToString();
            articulos.gif_tiempo_transicion = dgvArticulos.CurrentRow.Cells[25].Value.ToString();
            articulos.video_youtube_url = dgvArticulos.CurrentRow.Cells[26].Value.ToString();
            articulos.lang_nombre = dgvArticulos.CurrentRow.Cells[27].Value.ToString();
            articulos.lang_descripcion = dgvArticulos.CurrentRow.Cells[28].Value.ToString();

            if (dgvArticulos.CurrentRow.Cells[29].Value == null) { articulos.tags = ""; } else { articulos.tags = dgvArticulos.CurrentRow.Cells[29].Value.ToString(); }

            if (dgvArticulos.CurrentRow.Cells[30].Value == null) { articulos.seo_tags = ""; } else { articulos.seo_tags = dgvArticulos.CurrentRow.Cells[30].Value.ToString(); }


            Clases.Articulos product = lista_articulos_editados.FirstOrDefault(x => x.sku == this.dgvArticulos.CurrentRow.Cells[0].Value.ToString());

            if (product == null)
            {
                lista_articulos_editados.Add(articulos);
            }
            else
            {
                lista_articulos_editados.RemoveAll(x => x.sku == (articulos.sku));
                lista_articulos_editados.Add(articulos);
            }
        }

        private void dgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvArticulos.EndEdit();
            Clases.Articulos articulos = new Clases.Articulos();
            //Sincronizar
            if (e.ColumnIndex == 11 || e.ColumnIndex == 19 || e.ColumnIndex == 20 || e.ColumnIndex == 22 || e.ColumnIndex == 25)
            {
                articulos.sku = dgvArticulos.CurrentRow.Cells[0].Value.ToString();
                articulos.nombre = dgvArticulos.CurrentRow.Cells[1].Value.ToString();
                articulos.nombre_nidux = dgvArticulos.CurrentRow.Cells[2].Value.ToString();
                articulos.descripcion = dgvArticulos.CurrentRow.Cells[3].Value.ToString();
                articulos.descripcion_nidux = dgvArticulos.CurrentRow.Cells[4].Value.ToString();
                articulos.peso_producto = dgvArticulos.CurrentRow.Cells[5].Value.ToString();
                articulos.stock_principal = dgvArticulos.CurrentRow.Cells[6].Value.ToString();
                articulos.precio = dgvArticulos.CurrentRow.Cells[7].Value.ToString();
                articulos.porcentaje_oferta = dgvArticulos.CurrentRow.Cells[8].Value.ToString();
                articulos.impuesto = dgvArticulos.CurrentRow.Cells[9].Value.ToString();
                articulos.activo = dgvArticulos.CurrentRow.Cells[10].Value.ToString();
                articulos.estado_de_producto = dgvArticulos.CurrentRow.Cells[11].Value.ToString();
                articulos.id = dgvArticulos.CurrentRow.Cells[12].Value.ToString();
                articulos.id_marca = dgvArticulos.CurrentRow.Cells[13].Value.ToString();
                articulos.categorias = dgvArticulos.CurrentRow.Cells[14].Value.ToString();
                articulos.atributos = dgvArticulos.CurrentRow.Cells[15].Value.ToString();
                articulos.id_padre = dgvArticulos.CurrentRow.Cells[16].Value.ToString();
                articulos.id_hijo = dgvArticulos.CurrentRow.Cells[17].Value.ToString();
                articulos.ocultar_indicador_stock = dgvArticulos.CurrentRow.Cells[18].Value.ToString();
                articulos.es_destacado = dgvArticulos.CurrentRow.Cells[19].Value.ToString();
                articulos.costo_shipping_individual = dgvArticulos.CurrentRow.Cells[20].Value.ToString();
                articulos.producto_permite_reservacion = dgvArticulos.CurrentRow.Cells[21].Value.ToString();
                articulos.porcentaje_para_reservar = dgvArticulos.CurrentRow.Cells[22].Value.ToString();
                articulos.limite_para_reservar_en_carrito = dgvArticulos.CurrentRow.Cells[23].Value.ToString();
                articulos.usar_gif_en_homepage = dgvArticulos.CurrentRow.Cells[24].Value.ToString();
                articulos.gif_tiempo_transicion = dgvArticulos.CurrentRow.Cells[25].Value.ToString();
                articulos.video_youtube_url = dgvArticulos.CurrentRow.Cells[26].Value.ToString();
                articulos.lang_nombre = dgvArticulos.CurrentRow.Cells[27].Value.ToString();
                articulos.lang_descripcion = dgvArticulos.CurrentRow.Cells[28].Value.ToString();
                articulos.tags = dgvArticulos.CurrentRow.Cells[29].Value.ToString();
                articulos.seo_tags = dgvArticulos.CurrentRow.Cells[30].Value.ToString();

                Clases.Articulos product = lista_articulos_editados.FirstOrDefault(x => x.sku == this.dgvArticulos.CurrentRow.Cells[0].Value.ToString());

                if (product == null)
                {
                    lista_articulos_editados.Add(articulos);
                }
                else
                {
                    lista_articulos_editados.RemoveAll(x => x.sku == (articulos.sku));
                    lista_articulos_editados.Add(articulos);
                }
            }
        }

        private void dgvArticulos_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewComboBoxEditingControl dgvCombo = e.Control as DataGridViewComboBoxEditingControl;
            columna_combo = this.dgvArticulos.CurrentCell.ColumnIndex;

            if (dgvCombo != null)
            {
                //
                // se remueve el handler previo que pudiera tener asociado, a causa ediciones previas de la celda
                // evitando asi que se ejecuten varias veces el evento
                //
                dgvCombo.SelectedIndexChanged -= new EventHandler(dvgCombo_SelectedIndexChanged);

                dgvCombo.SelectedIndexChanged += new EventHandler(dvgCombo_SelectedIndexChanged);
            }
        }

        private void dvgCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            if (combo.SelectedValue == null) { } else { valor_enviado_por_combo_box = combo.SelectedItem.ToString(); }

            if (columna_combo == 12)
            {
                Clases.Articulos articulos = new Clases.Articulos();

                articulos.sku = dgvArticulos.CurrentRow.Cells[0].Value.ToString();
                articulos.nombre = dgvArticulos.CurrentRow.Cells[1].Value.ToString();
                articulos.nombre_nidux = dgvArticulos.CurrentRow.Cells[2].Value.ToString();
                articulos.descripcion = dgvArticulos.CurrentRow.Cells[3].Value.ToString();
                articulos.descripcion_nidux = dgvArticulos.CurrentRow.Cells[4].Value.ToString();
                articulos.peso_producto = dgvArticulos.CurrentRow.Cells[5].Value.ToString();
                articulos.stock_principal = dgvArticulos.CurrentRow.Cells[6].Value.ToString();
                articulos.precio = dgvArticulos.CurrentRow.Cells[7].Value.ToString();
                articulos.porcentaje_oferta = dgvArticulos.CurrentRow.Cells[8].Value.ToString();
                articulos.impuesto = dgvArticulos.CurrentRow.Cells[9].Value.ToString();
                articulos.activo = dgvArticulos.CurrentRow.Cells[10].Value.ToString();
                articulos.estado_de_producto = dgvArticulos.CurrentRow.Cells[11].Value.ToString();
                articulos.id = dgvArticulos.CurrentRow.Cells[12].Value.ToString();
                articulos.id_marca = dgvArticulos.CurrentRow.Cells[13].Value.ToString();
                articulos.categorias = dgvArticulos.CurrentRow.Cells[14].Value.ToString();
                articulos.atributos = dgvArticulos.CurrentRow.Cells[15].Value.ToString();
                articulos.id_padre = dgvArticulos.CurrentRow.Cells[16].Value.ToString();
                articulos.id_hijo = dgvArticulos.CurrentRow.Cells[17].Value.ToString();
                articulos.ocultar_indicador_stock = dgvArticulos.CurrentRow.Cells[18].Value.ToString();
                articulos.es_destacado = dgvArticulos.CurrentRow.Cells[19].Value.ToString();
                articulos.costo_shipping_individual = dgvArticulos.CurrentRow.Cells[20].Value.ToString();
                articulos.producto_permite_reservacion = dgvArticulos.CurrentRow.Cells[21].Value.ToString();
                articulos.porcentaje_para_reservar = dgvArticulos.CurrentRow.Cells[22].Value.ToString();
                articulos.limite_para_reservar_en_carrito = dgvArticulos.CurrentRow.Cells[23].Value.ToString();
                articulos.usar_gif_en_homepage = dgvArticulos.CurrentRow.Cells[24].Value.ToString();
                articulos.gif_tiempo_transicion = dgvArticulos.CurrentRow.Cells[25].Value.ToString();
                articulos.video_youtube_url = dgvArticulos.CurrentRow.Cells[26].Value.ToString();
                articulos.lang_nombre = dgvArticulos.CurrentRow.Cells[27].Value.ToString();
                articulos.lang_descripcion = dgvArticulos.CurrentRow.Cells[28].Value.ToString();
                articulos.tags = dgvArticulos.CurrentRow.Cells[29].Value.ToString();
                articulos.seo_tags = dgvArticulos.CurrentRow.Cells[30].Value.ToString();

                Clases.Articulos product = lista_articulos_editados.FirstOrDefault(x => x.sku == this.dgvArticulos.CurrentRow.Cells[0].Value.ToString());

                if (product == null)
                {
                    lista_articulos_editados.Add(articulos);
                }
                else
                {
                    lista_articulos_editados.RemoveAll(x => x.sku == (articulos.sku));
                    lista_articulos_editados.Add(articulos);
                }
            }
        }

        private void btnAgregarArticulos_Click(object sender, EventArgs e)
        {
            Activar_Articulos ar = new Activar_Articulos();
            ar.ShowDialog();
            if (ar.n == 1)
            {
                dgvArticulos.Rows.Clear();
                CargaDatos_Articulos();
            }
        }

        private void btnGuardarArticulos_Click(object sender, EventArgs e)
        {
            try
            {
                if (lista_articulos_editados.Count == 0)
                {
                    MessageBox.Show("No se ha editado ningún Artículo", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    logsFile.WriteLogs(@"
                    ===========================================
                    💾 [INICIO ACTUALIZACIÓN DE ARTÍCULOS APP] 💾
                    -------------------------------------------");
                    foreach (Clases.Articulos item in lista_articulos_editados)
                    {
                        try { 

                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn.AbrirConexion();

                            cmd.CommandText = "" + com + ".ACT_ARTICULOS_APP";
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@ARTICULO", item.sku);
                            cmd.Parameters.AddWithValue("@NOMBRE_NIDUX", item.nombre_nidux);
                            cmd.Parameters.AddWithValue("@DESCRIPCION_NIDUX", item.descripcion_nidux);
                            if (item.costo_shipping_individual == "")
                            {
                                cmd.Parameters.AddWithValue("@SHIPPING", 0.00);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@SHIPPING", Convert.ToDouble(item.costo_shipping_individual)); //item.costo_shipping_individual
                            }
                            if (item.porcentaje_para_reservar == "")
                            {
                                cmd.Parameters.AddWithValue("@PORCENTAJE", 0.00);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@PORCENTAJE", Convert.ToDouble(item.porcentaje_para_reservar));//item.porcentaje_para_reservar
                            }
                            if (item.limite_para_reservar_en_carrito == "")
                            {
                                cmd.Parameters.AddWithValue("@CARRITO", "0");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@CARRITO", item.limite_para_reservar_en_carrito);
                            }
                            if (item.gif_tiempo_transicion == "")
                            {
                                cmd.Parameters.AddWithValue("@TIEMPO_GIF", "0");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@TIEMPO_GIF", item.gif_tiempo_transicion);
                            }

                            cmd.Parameters.AddWithValue("@VIDEO", item.video_youtube_url);
                            cmd.Parameters.AddWithValue("@NOMBRE_TRA", item.lang_nombre);
                            cmd.Parameters.AddWithValue("@DESCRIP_TRADUC", item.lang_descripcion);

                            cmd.Parameters.AddWithValue("@DESCUENTO", Convert.ToDouble(item.porcentaje_oferta));

                            cmd.Parameters.AddWithValue("@ESTADO", item.estado_de_producto);

                            if (item.activo.Equals("True"))
                            {
                                cmd.Parameters.AddWithValue("@ACTIVO", "S");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@ACTIVO", "N");
                            }
                            if (item.ocultar_indicador_stock.Equals("True"))
                            {
                                cmd.Parameters.AddWithValue("@INDICADOR", "S");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@INDICADOR", "N");
                            }
                            if (item.es_destacado.Equals("True"))
                            {
                                cmd.Parameters.AddWithValue("@DESTACADO", "S");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@DESTACADO", "N");
                            }
                            if (item.producto_permite_reservacion.Equals("True"))
                            {
                                cmd.Parameters.AddWithValue("@RESERVA", "S");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@RESERVA", "N");
                            }
                            if (item.usar_gif_en_homepage.Equals("True"))
                            {
                                cmd.Parameters.AddWithValue("@GIF", "S");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@GIF", "N");
                            }

                            cmd.Parameters.AddWithValue("@TAGS", item.tags);

                            cmd.Parameters.AddWithValue("@SEO_TAGS", item.seo_tags);

                            cmd.ExecuteNonQuery();
                            cnn.CerrarConexion();
                            logsFile.WriteLogs($"✅ Artículo actualizado: {item.sku}");
                        }
                        catch (Exception ex)
                        {
                            logsFile.WriteLogs($@"
                            🛑 [ERROR AL ACTUALIZAR ARTÍCULO APP: {item.sku}]
                            Mensaje: {ex.Message}
                            -------------------------------------------");
                        }
                     }


                    logsFile.WriteLogs(@"
                    -------------------------------------------
                    ✅ [FIN ACTUALIZACIÓN DE ARTÍCULOS APP]
                    ===========================================");
                    MessageBox.Show("Artículos Actualizados con Éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvArticulos.Rows.Clear();
                    CargaDatos_Articulos();
                    lista_articulos_editados.Clear();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Mensaje de Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logsFile.WriteLogs($@"
                ===========================================
                🛑 [ERROR AL GUARDAR ARTÍCULOS] 🛑
                -------------------------------------------
                {ex.ToString()}
                ===========================================");

            }
        }

        private void btnActualizarArticulos_Click(object sender, EventArgs e)
        {
            dgvArticulos.Rows.Clear();
            CargaDatos_Articulos();
            Buscar_Articulos(txtBuscarArticulos.Text.ToString());
            MessageBox.Show("Datos de Artículos refrescados", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSincronizarArticulos_Click(object sender, EventArgs e)
        {
            List<Clases.Articulos_Nidux> lista = new List<Clases.Articulos_Nidux>();
            int contador1 = 0;


            logsFile.WriteLogs($@"
           ===========================================
           ▶️ [INICIO DE SINCRONIZACIÓN DE ARTÍCULOS]
           ===========================================");

            Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();

            string token = obj_metodos.Obtener_Token();
            if (token.Equals("Error en login"))
            {
                MessageBox.Show("Error en el login", "Mensaje de Error v4", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //BEBEMUNDO
                #region PADRES
                try
                {

                    var client = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_articulos_editados_simple");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response1 = client.Execute(request);
                    lista = JsonConvert.DeserializeObject<List<Clases.Articulos_Nidux>>(response1.Content);

                    if ((int)response1.StatusCode == 200)
                    {
                        if (lista.Count() > 0)
                        {
                            int contadorPeticionesArticulos = 0;
                            string Exitopaso = "";
                            string errorNoPAso = "";
                            while (contador1 < lista.Count())
                            {
                                #region Se consulta la Descripcion de NIDUX
                                //para poder conservar el formato de la descripcion se va a consultar el
                                //articulo para sacar la descripcion tal cual esta en NIDUX
                                if (!String.IsNullOrEmpty(lista[contador1].id))
                                {
                                    if (contadorPeticionesArticulos == 99)
                                    {
                                        Thread.Sleep(60000);
                                        contadorPeticionesArticulos = 0;
                                    }

                                    string id = lista[contador1].id;

                                    var clientget = new RestClient("https://api.nidux.dev/v3/products/" + id);
                                    clientget.Timeout = -1;
                                    var requestget = new RestRequest(Method.GET);
                                    requestget.AddHeader("Authorization", "Bearer " + token);
                                    IRestResponse responseget = clientget.Execute(requestget);

                                    if ((int)responseget.StatusCode == 200)
                                    {
                                        //MessageBox.Show(responseget.Content);
                                        Articulos2 lista_articulosget = JsonConvert.DeserializeObject<Articulos2>(responseget.Content);
                                        //logsFile.WriteLogs($"Categorias a asignar: {lista_articulosget.Producto.Categorias}");
                                        lista[contador1].descripcion = lista_articulosget.Producto.product_description;

                                    }
                                    else
                                    {
                                        //logsFile.WriteLogs($"Fallo al obtener la Categoria de NIDUX del articulo: {lista[contador1].id}");
                                    }
                                }
                                #endregion

                                if (contadorPeticionesArticulos == 295)
                                {
                                    Thread.Sleep(60000);
                                    contadorPeticionesArticulos = 0;
                                }

                                string json = JsonConvert.SerializeObject(lista[contador1], Formatting.Indented);
                                //si el id viene vacio es porque hay que insertarlo
                                if (lista[contador1].id == "")
                                {

                                    string name = lista[contador1].nombre.Replace("\"", "\\\"");
                                    string description = lista[contador1].descripcion
                                     .Replace("\"", "\\\"")
                                     .Replace("\r\n", "\\n")
                                     .Replace("\n", "\\n")
                                     .Replace("\t", "\\t");
                                    string code = HttpUtility.HtmlEncode(lista[contador1].sku);
                                    string stockPrincipal = lista[contador1].stock_principal.ToString().Replace(",", ".");
                                    string precio = lista[contador1].precio.ToString().Replace(",", ".");
                                    string costoShipping = lista[contador1].costo_shipping_individual != null
                                            ? lista[contador1].costo_shipping_individual.ToString().Replace(",", ".") : "0";
                                    string peso = lista[contador1].peso_producto != null
                                            ? lista[contador1].peso_producto.ToString().Replace(",", ".") : "0";
                                    string porcentaje_oferta = lista[contador1].porcentaje_oferta != null
                                             ? lista[contador1].porcentaje_oferta.ToString().Replace(",", ".") : "0";



                                    string jsonBody = $@"{{
                                        ""add"": [
                                            {{
                                                ""brand_id"": {lista[contador1].id_marca},
                                                ""categorias"": [{string.Join(",", lista[contador1].categorias)}],
                                                ""product_code"": ""{code}"",
                                                ""product_name"": ""{name}"",
                                                ""product_description"": ""{description}"",
                                                ""product_price"": ""{precio}"",
                                                ""product_shipping"": {costoShipping},
                                                ""product_weight"": {peso},
                                                ""product_sale"": {porcentaje_oferta},
                                                ""product_status"": {lista[contador1].estado_de_producto ?? "0"},
                                                ""product_home"": {lista[contador1].es_destacado ?? "0"},
                                                ""product_stock"": {stockPrincipal},
                                                ""product_video"": ""{lista[contador1].video_youtube_url}"",
                                                ""product_hidestock"": {lista[contador1].ocultar_indicador_stock ?? "0"},
                                                ""product_reserve"": {lista[contador1].producto_permite_reservacion ?? "0"},
                                                ""product_reserve_limit"": {lista[contador1].limite_para_reservar_en_carrito ?? "0"},
                                                ""product_reserve_percentage"": {lista[contador1].porcentaje_para_reservar ?? "0"},
                                                ""product_tax"": {lista[contador1].impuesto_producto},
                                                ""seo_tags"": [{string.Join(",", lista[contador1].seo_tags.Select(tag => $"\"{tag}\""))}],
                                                ""tags"": [{string.Join(",", lista[contador1].tags.Select(tag => $"\"{tag}\""))}]   
                                            }}
                                        ]
                                    }}";

                                    logsFile.WriteLogs($@"
                               ===========================================
                               ➕ [AGREGANDO NUEVO ARTÍCULO]
                               -------------------------------------------
                               SKU: {lista[contador1].sku}
                               Nombre: {lista[contador1].nombre}              
                               ===========================================");

                                    var client1 = new RestClient("https://api.nidux.dev/v3/products/batch");
                                    client1.Timeout = -1;
                                    var request1 = new RestRequest(Method.POST);
                                    request1.AddHeader("Authorization", "Bearer " + token);
                                    request1.AddHeader("Content-Type", "application/json");
                                    request1.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
                                    IRestResponse response2 = client1.Execute(request1);

                                    if ((int)response2.StatusCode == 200)
                                    {
                                        Clases.Respuesta m = JsonConvert.DeserializeObject<Clases.Respuesta>(response2.Content);
                                        Clases.Respuesta res = new Clases.Respuesta();
                                        res.id = m.id;
                                        res.sku = lista[contador1].sku;
                                        string json_respuesta = JsonConvert.SerializeObject(res, Formatting.Indented);

                                        if (!string.IsNullOrEmpty(res.id))
                                        {
                                            var client2 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_id_articulos");
                                            client2.Timeout = -1;
                                            var request2 = new RestRequest(Method.PUT);
                                            request2.AddParameter("application/json", json_respuesta, ParameterType.RequestBody);
                                            IRestResponse response3 = client2.Execute(request2);

                                            if ((int)response3.StatusCode == 200)
                                            {
                                                logsFile.WriteLogs($@"
                                               ===========================================
                                               ✅ [ARTÍCULO AGREGADO CON ÉXITO]
                                               -------------------------------------------
                                               SKU: {res.sku} | ID: {res.id}
                                               ===========================================");
                                            }
                                            else
                                            {
                                                MessageBox.Show(response3.Content.ToString(), "Mensaje de Error v5", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                logsFile.WriteLogs($@"
                                               🛑 [ERROR AL AGREGAR ARTÍCULO]
                                               -------------------------------------------
                                               Respuesta: {response3.Content}
                                               ===========================================");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(response2.Content.ToString(), "Mensaje de Error v6", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        logsFile.WriteLogs($@"
                                       🛑 [ERROR AL AGREGAR ARTÍCULO]
                                       -------------------------------------------
                                       Respuesta: {response2.Content}
                                       ===========================================");
                                    }
                                }
                                else
                                {
                                    string name = lista[contador1].nombre.Replace("\"", "\\\"");
                                    string description = lista[contador1].descripcion
                                     .Replace("\"", "\\\"")
                                     .Replace("\r\n", "\\n")
                                     .Replace("\n", "\\n")
                                     .Replace("\t", "\\t");
                                    string code = HttpUtility.HtmlEncode(lista[contador1].sku);
                                    string stockPrincipal = lista[contador1].stock_principal.ToString().Replace(",", ".");
                                    string precio = lista[contador1].precio.ToString().Replace(",", ".");
                                    string costoShipping = lista[contador1].costo_shipping_individual != null
                                            ? lista[contador1].costo_shipping_individual.ToString().Replace(",", ".") : "0";
                                    string peso = lista[contador1].peso_producto != null
                                            ? lista[contador1].peso_producto.ToString().Replace(",", ".") : "0";
                                    string porcentaje_oferta = lista[contador1].porcentaje_oferta != null
                                             ? lista[contador1].porcentaje_oferta.ToString().Replace(",", ".") : "0";


                                    string jsonBody = $@"{{
                                        ""id"": {lista[contador1].id},
                                        ""brand_id"": {lista[contador1].id_marca},
                                        ""categorias"": [{string.Join(",", lista[contador1].categorias)}],
                                        ""product_code"": ""{code}"",
                                        ""product_name"": ""{name}"",
                                        ""operation_type"": ""replace"",
                                        ""product_description"": ""{description}"",
                                        ""product_price"": ""{precio}"",
                                        ""product_shipping"": {costoShipping},
                                        ""product_weight"": {peso},
                                        ""product_sale"": {porcentaje_oferta},
                                        ""product_status"": {lista[contador1].estado_de_producto ?? "0"},
                                        ""product_home"": {lista[contador1].es_destacado ?? "0"},
                                        ""product_stock"": {stockPrincipal},
                                        ""product_video"": ""{lista[contador1].video_youtube_url}"",
                                        ""product_hidestock"": {lista[contador1].ocultar_indicador_stock ?? "0"},
                                        ""product_reserve"": {lista[contador1].producto_permite_reservacion ?? "0"},
                                        ""product_reserve_limit"": {lista[contador1].limite_para_reservar_en_carrito ?? "0"},
                                        ""product_reserve_percentage"": {lista[contador1].porcentaje_para_reservar ?? "0"},
                                        ""product_tax"": {lista[contador1].impuesto_producto},
                                        ""seo_tags"": [{string.Join(",", lista[contador1].seo_tags.Select(tag => $"\"{tag}\""))}],
                                        ""tags"": [{string.Join(",", lista[contador1].tags.Select(tag => $"\"{tag}\""))}]
                                        }}
                                        ]
                                    }}";

                                    logsFile.WriteLogs($@"
                               ===========================================
                               ➕ [ACTUALIZANDO ARTÍCULO]
                               -------------------------------------------
                               SKU: {lista[contador1].sku}
                               Nombre: {lista[contador1].nombre}              
                               ===========================================");

                                    var client4 = new RestClient("https://api.nidux.dev/v3/products/"); // nidux pregunta
                                    client4.Timeout = -1;
                                    var request4 = new RestRequest(Method.PATCH);
                                    request4.AddHeader("Authorization", "Bearer " + token);
                                    request4.AddHeader("Content-Type", "application/json");
                                    request4.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
                                    IRestResponse response4 = client4.Execute(request4);

                                    if ((int)response4.StatusCode == 200)
                                    {
                                        //Articulo editado con exito
                                        Exitopaso += lista[contador1].id + " \\||// ";
                                        logsFile.WriteLogs($@"
                                           ===========================================
                                           ✅ [ID ARTÍCULO ACTUALIZADO CON ÉXITO]
                                           ===========================================");
                                    
                                    }
                                    else
                                    {
                                        int aux = contador1;
                                        if (aux + 10 == contador1)
                                        {
                                            errorNoPAso += "\n";
                                        }
                                        errorNoPAso += lista[contador1].id + " error fue " + response4.Content.ToString() + " || ";
                                        logsFile.WriteLogs($@"
                                       🛑 [ERROR AL ACTUALIZAR ID ARTÍCULO]
                                       -------------------------------------------
                                       Respuesta: {response4.Content}
                                       ===========================================");
                                        //MessageBox.Show(response4.Content.ToString(), "Mensaje de Error v7", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                contadorPeticionesArticulos++;
                                contador1++;
                            }

                            //MessageBox.Show(Exitopaso, "Sincronizados con exito");
                            //MessageBox.Show(errorNoPAso, "Error Sincronizados");
                        }
                    }
                    else
                    {
                        MessageBox.Show(response1.Content.ToString(), "Mensaje de Error v8", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logsFile.WriteLogs($@"
                       🛑 [ERROR AL AGREGAR ARTÍCULO]
                       -------------------------------------------
                       Respuesta: {response1.Content}
                       ===========================================");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Mensaje de Error v9", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logsFile.WriteLogs($@"
                       🛑 [ERROR AL AGREGAR ARTÍCULO]
                       -------------------------------------------
                       Respuesta: {ex.Message.ToString()}
                       ===========================================");
                }
                logsFile.WriteLogs($@"
       ===========================================
       ✅ [ARTÍCULOS SINCRONIZADOS CON NIDUX]
       ===========================================");
                #endregion 
                //agregar articulos hijos
                #region VARIACIONES
                try
                {
                    logsFile.WriteLogs($@"
                   ===========================================
                   🔄 [INICIO DE SINCRONIZACIÓN DE VARIACIONES]
                   ===========================================");
                    var client5 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/obtener_articulos_padres");
                    client5.Timeout = -1;
                    var request5 = new RestRequest(Method.GET);
                    request5.AddHeader("Content-Type", "application/json");
                    IRestResponse response5 = client5.Execute(request5);

                    if ((int)response5.StatusCode == 200)
                    {
                        int contador_padres = 0;

                        List<Clases.Variaciones_Padres> lista_padres = JsonConvert.DeserializeObject<List<Clases.Variaciones_Padres>>(response5.Content);

                        if (lista_padres.Count > 0)
                        {
                            int contadorArticulosHijos = 0;

                            while (contador_padres < lista_padres.Count)
                            {
                                if (contadorArticulosHijos == 295)
                                {
                                    Thread.Sleep(60000);
                                    contadorArticulosHijos = 0;
                                }

                                //Obtenemos los padres
                                logsFile.WriteLogs($@"
                               ===========================================
                               🔍 [CONSULTANDO ATRIBUTOS DEL ARTÍCULO PADRE]
                               -------------------------------------------
                               Artículo Padre ID: {lista_padres[contador_padres].padre}
                               ===========================================");

                                var client6 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/agregar_atributos_articulos/" + lista_padres[contador_padres].padre);
                                client6.Timeout = -1;
                                var request6 = new RestRequest(Method.GET);
                                request6.AddHeader("Content-Type", "application/json");
                                IRestResponse response6 = client6.Execute(request6);

                                if ((int)response6.StatusCode == 200)
                                {
                                    List<Clases.Valores_Hijos_Atributos> lista2 = JsonConvert.DeserializeObject<List<Clases.Valores_Hijos_Atributos>>(response6.Content);

                                    if (lista2[0].id_atributos.Length > 0)
                                    {

                                        // Empieza el metodo de insertar atributos



                                        var client_atributos = new RestClient($"https://api.nidux.dev/v3/products/{lista_padres[contador_padres].ID}/attribs");
                                        client_atributos.Timeout = -1;
                                        var request_atributos = new RestRequest(Method.DELETE);
                                        request_atributos.AddHeader("Authorization", "Bearer " + token);
                                        IRestResponse response_atributos = client_atributos.Execute(request_atributos);

                                        if ((int)response_atributos.StatusCode == 200)
                                    {

                                            string v_atributos_id = "[" + String.Join(",", lista2[0].id_atributos.Select(p => p.ToString()).ToArray()) + "]";
                                            var client7 = new RestClient("https://api.nidux.dev/v3/products/" + lista_padres[contador_padres].ID + "/attribs");
                                        client7.Timeout = -1;
                                        var request7 = new RestRequest(Method.POST);
                                        request7.AddHeader("Authorization", "Bearer " + token);
                                        request7.AddParameter("application/json", "{\r\n    \"atributos\": " + v_atributos_id + "\r\n}", ParameterType.RequestBody);
                                        IRestResponse response7 = client7.Execute(request7);

                                        if ((int)response7.StatusCode == 200)
                                        {

                                            // Consume las variciones de ese articulo
                                            var client8 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/agregar_articulo_hijo/" + lista_padres[contador_padres].padre);

                                            client8.Timeout = -1;
                                            var request8 = new RestRequest(Method.GET);
                                            IRestResponse response8 = client8.Execute(request8);
                                            // richTextBox1.AppendText("\n" + response8.Content);



                                            if ((int)response8.StatusCode == 200)
                                            {
                                                    string res = response8.Content;

                                                    var encapsulador_json_variacion = new
                                                    {
                                                        action = "replace",
                                                        variations = res
                                                    };

                                                    string json_variacion = JsonConvert.SerializeObject(encapsulador_json_variacion, Formatting.Indented);
                      
                                                //Inserta las variaciones en nidux
                                                var client9 = new RestClient("https://api.nidux.dev/v3/products/" + lista_padres[contador_padres].ID + "/variations");
                                                client9.Timeout = -1;
                                                var request9 = new RestRequest(Method.POST);
                                                request9.AddHeader("Authorization", "Bearer " + token);
                                                request9.AddParameter("application/json", json_variacion, ParameterType.RequestBody);
                                                IRestResponse response9 = client9.Execute(request9);

                                                if ((int)response9.StatusCode == 200)
                                                {
                                                        //Se actualiza el ID del Hijo
                                                        logsFile.WriteLogs($@"
                                                           ===========================================
                                                           ✅ [VARIACIONES AÑADIDAS AL ARTÍCULO]
                                                           -------------------------------------------
                                                           Artículo Padre ID: {lista_padres[contador_padres].ID}
                                                           ===========================================");

                                                    }// fin del if de response 9
                                                else
                                                {
                                                    MessageBox.Show(response9.Content.ToString(), "Mensaje de Error v10", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                        logsFile.WriteLogs($@"
                                                           🛑 [ERROR AL AÑADIR VARIACIONES AL ARTÍCULO]
                                                           -------------------------------------------
                                                           Respuesta: {response9.Content}
                                                           ===========================================");
                                                    }
                                            } // fin del if de response8
                                            else
                                            {
                                                    logsFile.WriteLogs($@"
                                                           🛑 [ERROR AL AÑADIR VARIACIONES AL ARTÍCULO]
                                                           -------------------------------------------
                                                           Respuesta: {response8.Content}
                                                           ===========================================");
                                                    MessageBox.Show(response8.Content.ToString(), "Mensaje de Error v11", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }// fin del if de response7
                                        else
                                        {
                                                logsFile.WriteLogs($@"
                                                           🛑 [ERROR AL AÑADIR VARIACIONES AL ARTÍCULO]
                                                           -------------------------------------------
                                                           Respuesta: {response7.Content}
                                                           ===========================================");
                                                MessageBox.Show(response7.Content.ToString(), "Mensaje de Error v12", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    else
                                    {
                                            logsFile.WriteLogs($@"
                                                           🛑 [ERROR AL AÑADIR VARIACIONES AL ARTÍCULO]
                                                           -------------------------------------------
                                                           Respuesta: {response6.Content}
                                                           ===========================================");
                                            MessageBox.Show(response6.Content.ToString(), $"[ERR] ERROR AL LIMPIAR ATRIBUTOS: {response_atributos.Content}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                }// fin del if de response6
                                else
                                {
                                    MessageBox.Show(response6.Content.ToString(), "Mensaje de Error v13", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    logsFile.WriteLogs($@"
                                                           🛑 [ERROR AL AÑADIR VARIACIONES AL ARTÍCULO]
                                                           -------------------------------------------
                                                           Respuesta: {response6.Content}
                                                           ===========================================");
                                }
                                
                                contadorArticulosHijos++;
                                contador_padres++;
                            }// fin del while
                        }// fin del if de lista de padres
                    } // fin del if de response.5
                    else
                    {
                        MessageBox.Show(response5.Content.ToString(), "Mensaje de Error v14", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logsFile.WriteLogs($@"
                                                           🛑 [ERROR AL AÑADIR VARIACIONES AL ARTÍCULO]
                                                           -------------------------------------------
                                                           Respuesta: {response5.Content}
                                                           ===========================================");
                    }
                    MessageBox.Show("Artículos Sincronizados Con Nidux", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                } //fin del try
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Mensaje de Error v15", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                logsFile.WriteLogs($@"
                                   ===========================================
                                   ✅ [VARIACIONES SINCRONIZADOS CON NIDUX]
                                   ===========================================");
                #endregion
                //revisamos si algun articulo que no es padre se metio como tal para luego solo dejarlo como hijo
                try
                {
                    var client10 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/eliminar_articulos_padres");
                    client10.Timeout = -1;
                    var request10 = new RestRequest(Method.GET);
                    request10.AddHeader("Content-Type", "application/json");
                    IRestResponse response10 = client10.Execute(request10);

                    if ((int)response10.StatusCode == 200)
                    {
                        List<Clases.articulos_eliminados> lista_eliminar = JsonConvert.DeserializeObject<List<Clases.articulos_eliminados>>(response10.Content);
                        int contador_elimanados = 0;

                        int contadorArticulosEliminados = 0;

                        while (contador_elimanados < lista_eliminar.Count)
                        {
                            if (contadorArticulosEliminados == 295)
                            {
                                Thread.Sleep(60000);
                                contadorArticulosEliminados = 0;
                            }

                            var client = new RestClient("https://api.nidux.dev/v3/products/" + lista_eliminar[contador_elimanados].ID);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.DELETE);
                            request.AddHeader("Authorization", "Bearer " + token);
                            IRestResponse response = client.Execute(request);

                            if ((int)response.StatusCode == 200)
                            {
                                var client11 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_articulos_eliminados/" + lista_eliminar[contador_elimanados].SKU);
                                client11.Timeout = -1;
                                var request11 = new RestRequest(Method.PUT);
                                request11.AddHeader("Content-Type", "application/json");
                                IRestResponse response11 = client11.Execute(request11);

                                if ((int)response11.StatusCode == 200)
                                {

                                }
                                else
                                {
                                    MessageBox.Show("Error en el método de actualizar artículos para eliminar padres", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            contadorArticulosEliminados++;
                            contador_elimanados++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error en el método de obtener artículos para eliminar padres", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //actualizamos la fecha
                try
                {
                    var client1 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_fecha");
                    client1.Timeout = -1;
                    var request1 = new RestRequest(Method.GET);
                    IRestResponse response1 = client1.Execute(request1);
                    if ((int)response1.StatusCode == 200)
                    {

                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar la fecha de consulta", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnObtenerArticulos_Click(object sender, EventArgs e)
        {
            const string reduceMultiSpace = @"[ ]{2,}";
            Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
            string token = obj_metodos.Obtener_Token();
            if (token.Equals("Error en login"))
            {
                MessageBox.Show("Error en el login", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                logsFile.WriteLogs(@"
                ===========================================
                📦 [INICIO OBTENCIÓN DE ARTÍCULOS] 📦
                -------------------------------------------");
                // Inicio de Articulos
                var client = new RestClient("https://api.nidux.dev/v3/products/");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + token);
                IRestResponse response = client.Execute(request);

                if ((int)response.StatusCode == 200)
                {
                    ProductosResponse lista_articulos = JsonConvert.DeserializeObject<ProductosResponse>(response.Content);
                    if (lista_articulos.Products.Count > 0)
                    {
                        try
                        {
                            int n = 0;
                            int i = 0;
                            while (n < lista_articulos.Products.Count)
                            {
                                

                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = cnn.AbrirConexion();
                                cmd.CommandText = "" + com + ".INSERTAR_ARTICULOS_TIENDA_APP";
                                cmd.CommandTimeout = 0;
                                cmd.CommandType = CommandType.StoredProcedure;

                                var product = lista_articulos.Products.ElementAt(n).Value;
                                logsFile.WriteLogs($"🔄 Insertando artículo SKU: {product.product_code} {product.product_name}");

                                cmd.Parameters.AddWithValue("@ID", product.id);
                                cmd.Parameters.AddWithValue("@ID_MARCA", (object)product.brand_id ?? 0);
                                cmd.Parameters.AddWithValue("@SKU", product.product_code);
                                cmd.Parameters.AddWithValue("@NOMBRE", product.product_name);

                                string description = string.IsNullOrEmpty(product.product_description) ? "" :
                                    Regex.Replace(Regex.Replace(product.product_description, @"<[^>]+>|&nbsp;", string.Empty).Replace("\t", " "), reduceMultiSpace, " ");
                                cmd.Parameters.AddWithValue("@DESCRIPCION", description);
                                cmd.Parameters.AddWithValue("@PRECIO", product.product_price);
                                cmd.Parameters.AddWithValue("@COSTO_SHIPPING", product.product_shipping);
                                cmd.Parameters.AddWithValue("@PESO", product.product_weight);
                                cmd.Parameters.AddWithValue("@OFERTA", product.product_sale);
                                cmd.Parameters.AddWithValue("@ESTADO", product.product_status);
                                cmd.Parameters.AddWithValue("@DESTACADO", product.product_home);
                                cmd.Parameters.AddWithValue("@STOCK", product.product_stock);
                                cmd.Parameters.AddWithValue("@VIDEO", product.product_video ?? "");
                                cmd.Parameters.AddWithValue("@INDICADOR", product.product_hidestock);
                                cmd.Parameters.AddWithValue("@RESERVA", product.product_reserve);
                                cmd.Parameters.AddWithValue("@CARRITO", product.product_reserve_limit ?? 0);
                                cmd.Parameters.AddWithValue("@POR_RESERVA", product.product_reserve_percentage ?? 0);
                                cmd.Parameters.AddWithValue("@USAR_GIF", product.product_gif_enable);
                                cmd.Parameters.AddWithValue("@TIEMPO_GIF", product.product_gif_transition);
                                cmd.Parameters.AddWithValue("@IMPUESTO", product.product_tax ?? 0);

                                cmd.Parameters.AddWithValue("@CATEGORIAS", product.categorias == null ? "0" : string.Join(",", product.categorias.Values));

               
                                if (product.traducciones.Count > 0)
                                {
                                    var translation = product.traducciones.Values.First(); // Assuming only one translation
                                    cmd.Parameters.AddWithValue("@LANG_NOMBRE", translation.product_name);
                                    string langDescription = Regex.Replace(translation.product_description, @"<[^>]+>|&nbsp;", string.Empty);
                                    langDescription = Regex.Replace(langDescription.Replace("\t", " "), reduceMultiSpace, " ");
                                    cmd.Parameters.AddWithValue("@LANG_DESCRIP", langDescription);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@LANG_NOMBRE", "Default Name");
                                    cmd.Parameters.AddWithValue("@LANG_DESCRIP", "Default Description");
                                }

                                string cadena_tags = product.product_tags != null ? string.Join(",", product.product_tags.Values) : "";
                                cmd.Parameters.AddWithValue("@TAGS", cadena_tags);

                                // Handle product keywords (SEO tags)
                                string cadena_seotags = product.product_keywords != null ? string.Join(",", product.product_keywords.Values) : "";
                                cmd.Parameters.AddWithValue("@SEO_TAGS", cadena_seotags);




                                cmd.Parameters.AddWithValue("@CABYS", product.cabys?.Cabys ?? "");
                                cmd.Parameters.AddWithValue("@CODIGOTARIFA", product.cabys?.CodigoTarifa ?? "");
                                cmd.Parameters.AddWithValue("@SkipFactura", product.cabys?.SkipFactura ?? 0);

                                cmd.ExecuteNonQuery();
                                cnn.CerrarConexion();
                                //INSERTAR VARIACIONES de articulo

                                if (product.variations != null && product.variations.Count > 0)
                                {
                                    logsFile.WriteLogs($"🧬 Variaciones encontradas para: {product.product_code}");
                                    while (i < product.variations.Count)
                                    {
                                       
                                        var variation = product.variations.Values.ElementAt(i); // Access variation via dictionary
                                        SqlCommand cmd_va = new SqlCommand
                                        {
                                            Connection = cnn.AbrirConexion(),
                                            CommandText = "" + com + ".INSERTAR_ARTICULOS_VARIACIONES_TIENDA_APP",
                                            CommandTimeout = 0,
                                            CommandType = CommandType.StoredProcedure
                                        };
                                        logsFile.WriteLogs($"🧬 Variaciones encontradas para: {variation.Sku}");
                                        cmd_va.Parameters.AddWithValue("@SKU_PADRE", product.product_code);
                                        cmd_va.Parameters.AddWithValue("@ID", variation.VariationId);
                                        cmd_va.Parameters.AddWithValue("@ATRIBUTOS", string.Join(",", variation.Attributes.Values.Select(a => a.value_name)));
                                        cmd_va.Parameters.AddWithValue("@SKU", variation.Sku);
                                        cmd_va.Parameters.AddWithValue("@PRECIO", variation.Price);
                                        cmd_va.Parameters.AddWithValue("@STOCK", variation.Stock);
                                        cmd_va.Parameters.AddWithValue("@PESO", variation.Price); // Assuming Peso is stored in Price property
                                        cmd_va.Parameters.AddWithValue("@IMPUESTO", product.product_tax ?? 0);
                                        cmd_va.ExecuteNonQuery();
                                        cnn.CerrarConexion();
                                        i++;
                                    }
                                }
                                logsFile.WriteLogs($"✅ Artículo insertado: {product.product_code}");

                                n++;

                            }
                            logsFile.WriteLogs(@"
                            -------------------------------------------
                            ✅ [FIN OBTENCIÓN DE ARTÍCULOS EXITOSA]
                            ===========================================");
                            MessageBox.Show("Artículos agregada con éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvArticulos.Rows.Clear();
                            CargaDatos_Articulos();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al procesar los artículos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            logsFile.WriteLogs($@"
                            🛑 [ERROR AL PROCESAR ARTÍCULOS]
                            Mensaje: {ex.Message}
                            -------------------------------------------");
                        }
                        
                    }
                }
                else
                {
                    MessageBox.Show("Error: " + response.Content, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logsFile.WriteLogs("⚠️ [AVISO] No se encontraron artículos en la respuesta de Nidux.");
                }
            }
        }

        private void btnFiltarArticulos_Click(object sender, EventArgs e)
        {
            frm_filtro_articulos frm_Filtro_Articulos = new frm_filtro_articulos();
            frm_Filtro_Articulos.ShowDialog();
            if (frm_Filtro_Articulos.n == 1)
            {
                filtrarArticulos();
            }
        }

        public void filtrarArticulos()
        {
            try
            {
                List<Clases.filtro_articulos> filtroTabla = new List<Clases.filtro_articulos>();
                Metodos.met_filtro_articulos met_articulos = new Metodos.met_filtro_articulos();

                // Guarda en el objeto filtroTabla los datos traidos desde BD
                filtroTabla = met_articulos.obtenerFiltroArticulos();

                // Obtiene la cantidad de columnas que contiene el datagridview (dgvArticulos)
                var numColumns = dgvArticulos.Columns.Count;

                // Muestra todas las columnas del datagridview (dgvArticulos)
                for (int i = 0; i < numColumns; i++)
                {
                    dgvArticulos.Columns[i].Visible = true;
                }

                #region OCULTAR COLUMNAS
                // Consulta si el valor traido de BD es 0 (falso) y siendo asi oculta la columna
                if (filtroTabla[0].codigo_articulo == 0) { dgvArticulos.Columns[0].Visible = false; }

                if (filtroTabla[0].nombre == 0) { dgvArticulos.Columns[1].Visible = false; }

                if (filtroTabla[0].nombre_nidux == 0) { dgvArticulos.Columns[2].Visible = false; }

                if (filtroTabla[0].descripcion == 0) { dgvArticulos.Columns[3].Visible = false; }

                if (filtroTabla[0].descripcion_nidux == 0) { dgvArticulos.Columns[4].Visible = false; }

                if (filtroTabla[0].peso == 0) { dgvArticulos.Columns[5].Visible = false; }

                if (filtroTabla[0].cantidad == 0) { dgvArticulos.Columns[6].Visible = false; }

                if (filtroTabla[0].precio == 0) { dgvArticulos.Columns[7].Visible = false; }

                if (filtroTabla[0].porcentaje_descuento == 0) { dgvArticulos.Columns[8].Visible = false; }

                if (filtroTabla[0].impuesto_articulo == 0) { dgvArticulos.Columns[9].Visible = false; }

                if (filtroTabla[0].sincroniza == 0) { dgvArticulos.Columns[10].Visible = false; }

                if (filtroTabla[0].estado == 0) { dgvArticulos.Columns[11].Visible = false; }

                if (filtroTabla[0].id_nidux == 0) { dgvArticulos.Columns[12].Visible = false; }

                if (filtroTabla[0].marca_nidux == 0) { dgvArticulos.Columns[13].Visible = false; }

                if (filtroTabla[0].categorias == 0) { dgvArticulos.Columns[14].Visible = false; }

                if (filtroTabla[0].valores_atributos == 0) { dgvArticulos.Columns[15].Visible = false; }

                if (filtroTabla[0].id_padre == 0) { dgvArticulos.Columns[16].Visible = false; }

                if (filtroTabla[0].id_hijo == 0) { dgvArticulos.Columns[17].Visible = false; }

                if (filtroTabla[0].indicador_stock == 0) { dgvArticulos.Columns[18].Visible = false; }

                if (filtroTabla[0].destacar_articulo == 0) { dgvArticulos.Columns[19].Visible = false; }

                if (filtroTabla[0].costo_shipping == 0) { dgvArticulos.Columns[20].Visible = false; }

                if (filtroTabla[0].permite_reserva == 0) { dgvArticulos.Columns[21].Visible = false; }

                if (filtroTabla[0].porcentaje_reserva == 0) { dgvArticulos.Columns[22].Visible = false; }

                if (filtroTabla[0].limite_carrito == 0) { dgvArticulos.Columns[23].Visible = false; }

                if (filtroTabla[0].usa_gif == 0) { dgvArticulos.Columns[24].Visible = false; }

                if (filtroTabla[0].tiempo_gif == 0) { dgvArticulos.Columns[25].Visible = false; }

                if (filtroTabla[0].video_youtube == 0) { dgvArticulos.Columns[26].Visible = false; }

                if (filtroTabla[0].nombre_ingles == 0) { dgvArticulos.Columns[27].Visible = false; }

                if (filtroTabla[0].descripcion_ingles == 0) { dgvArticulos.Columns[28].Visible = false; }

                #endregion OCULTAR COLUMNAS
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void dgvArticulos_DoubleClick(object sender, EventArgs e)
        {
            string cod_articulo = dgvArticulos.SelectedCells[0].Value.ToString();
            Asignar_Marcas mc = new Asignar_Marcas(cod_articulo);
            Asignar_Categorias ca = new Asignar_Categorias(cod_articulo);
            Asignar_Atributos at = new Asignar_Atributos(cod_articulo);
            Asignar_Padres pp = new Asignar_Padres(cod_articulo);
            try
            {
                DataGridView dgv = (DataGridView)sender;
                DataGridViewCell cell = dgv.CurrentCell;
                if (cell.RowIndex > -1 && cell.ColumnIndex == 13)
                {
                    if (cell.ColumnIndex == 13)//marcas
                    {
                        mc.ShowDialog();
                        if (mc.n == 1)
                        {
                            dgvArticulos[13, cell.RowIndex].Value = mc.codMarca;
                        }
                    }
                }
                if (cell.RowIndex > -1 && cell.ColumnIndex == 14)
                {
                    if (cell.ColumnIndex == 14)//categorias
                    {
                        ca.ShowDialog();
                        if (ca.n == 1)
                        {
                            dgvArticulos[14, cell.RowIndex].Value = ca.codCat;

                        }
                    }
                }
                if (cell.RowIndex > -1 && cell.ColumnIndex == 15)
                {
                    if (cell.ColumnIndex == 15)//atributos
                    {
                        at.ShowDialog();
                        if (at.n == 1)
                        {
                            dgvArticulos[15, cell.RowIndex].Value = at.atributosCodigos;
                        }
                    }
                }
                //padres
                if (cell.RowIndex > -1 && cell.ColumnIndex == 16)
                {
                    if (cell.ColumnIndex == 16)
                    {
                        pp.ShowDialog();
                        if (pp.n == 1)
                        {
                            //dgvArticulos.Rows.Clear();
                            //CargaDatos_Articulos();
                            dgvArticulos[16, cell.RowIndex].Value = pp.articulo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOracleSincro_Click(object sender, EventArgs e)//listo
        {
            //CEMACO
            string art = string.Empty;
            List<string> ListError = new List<string>();
            try
            {
                //obtener valores de oracle
                string query = "SELECT NO_ARTI, DESCRIPCION, PESO, CARACTERISTICAS, PRECIO, PORC_DESC, EXISTENCIA, NIDUX_COD_CATEGORIA, NIDUX_COD_MARCA, DESTACADO, TAGS, SEO_TAG, URL_VIDEO, RECORDDATE, ESTADO FROM naf5m.NIDUX_OBTENER_PRODUCTO_SUKASA";
                OracleCommand cmd = new OracleCommand();

                cmd.Connection = cnn.open();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                OracleDataAdapter da = new OracleDataAdapter();
                da.SelectCommand = cmd;

                DataTable dt = new DataTable();
                da.Fill(dt);

                cnn.close();

                //inserto valores de articulos en tablas intermedias SQL SERVER
                foreach (DataRow item in dt.Rows)
                {
                    try
                    {
                        SqlCommand scmd = new SqlCommand();
                        scmd.Connection = cnn.AbrirConexion();
                        //insertado en tabla articulos
                        scmd.CommandText = com + ".INSERTAR_ARTICULOS_CODISA";
                        scmd.CommandTimeout = 0;
                        scmd.CommandType = CommandType.StoredProcedure;
                        art = item["NO_ARTI"].ToString();//Es para el catch
                        scmd.Parameters.AddWithValue("@ARTICULO", item["NO_ARTI"].ToString());
                        //limpiamos de caracteres especiales
                        string nombre = item["DESCRIPCION"].ToString().Replace("\n", "");
                        string nombre_limpio = nombre.Replace("\"", "");
                        scmd.Parameters.AddWithValue("@NOMBRE", nombre_limpio);
                        //limpiamos de caracteres especiales
                        string descripcion = item["CARACTERISTICAS"].ToString().Replace("\n", "");
                        string descripcion_limpia = descripcion.Replace("\"", "");
                        scmd.Parameters.AddWithValue("@DESCRIPCION", descripcion_limpia);
                        scmd.Parameters.AddWithValue("@PESO", Convert.ToDecimal(item["PESO"].ToString()));
                        scmd.Parameters.AddWithValue("@IMPUESTO", 0);
                        scmd.Parameters.AddWithValue("@CANTIDAD", Convert.ToDecimal(item["EXISTENCIA"].ToString()));
                        string precio = item["PRECIO"].ToString().Replace(',', '.');
                        //MessageBox.Show($"Articulo: {item["NO_ARTI"].ToString()} precio: {item["PRECIO"].ToString()} PRECIO CONVER: {Convert.ToDecimal(precio)}");
                        scmd.Parameters.AddWithValue("@PRECIO", item["PRECIO"].ToString());
                        string descuento = item["PORC_DESC"].ToString().Replace('.', ',');
                        scmd.Parameters.AddWithValue("@DESCUENTO", Convert.ToDecimal(descuento));
                        scmd.Parameters.AddWithValue("@CATEGORIAS", item["NIDUX_COD_CATEGORIA"].ToString());
                        scmd.Parameters.AddWithValue("@MARCA", item["NIDUX_COD_MARCA"].ToString());

                        //validamos el estado segun la cantidad
                        //if (Convert.ToDecimal(item["EXISTENCIA"].ToString()) > 0)
                        //{
                        //    scmd.Parameters.AddWithValue("@ESTADO", 3);
                        //}
                        //else
                        //{
                        //    scmd.Parameters.AddWithValue("@ESTADO", 2);
                        //}
                        scmd.Parameters.AddWithValue("@ESTADO", item["ESTADO"].ToString());
                        scmd.Parameters.AddWithValue("@DESTACADO", item["DESTACADO"].ToString());
                        scmd.Parameters.AddWithValue("@TAGS", item["TAGS"].ToString());
                        scmd.Parameters.AddWithValue("@SEOTAG", item["SEO_TAG"].ToString());
                        scmd.Parameters.AddWithValue("@VIDEO", item["URL_VIDEO"].ToString());
                        scmd.Parameters.AddWithValue("@RECORDDATE", Convert.ToDateTime(item["RECORDDATE"]));
                        scmd.ExecuteNonQuery();
                        scmd.Parameters.Clear();
                        cnn.CerrarConexion();
                    }
                    catch (Exception ex)
                    {
                        ListError.Add($"Articulo: {art} Error: {ex.Message}");
                    }
                }

                MessageBox.Show("Artículos actualizados");
                cnn.CerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Articulo: {art} ERROR: {ex.Message}");
            }
            finally
            {
                if (ListError.Count > 0)
                {
                    string errores = string.Empty;
                    foreach (var error in ListError)
                    {
                        errores += error + " \n";
                    }
                    MessageBox.Show($"Articulos con errores: \n {errores}");

                }
            }
        }

        /*------------------------ Categorias -----------------------------------*/

        public void CargaDatos_Categorias()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT NOMBRE, DESCRIPCION, CODIGO_NIDUX, SUBCATEGORIA_NIDUX FROM " + com + ".CATEGORIAS";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cnn.CerrarConexion();

                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvCategorias.Rows.Add();

                    dgvCategorias.Rows[n].Cells[0].Value = item["NOMBRE"].ToString();//nombre
                    dgvCategorias.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                    dgvCategorias.Rows[n].Cells[2].Value = item["CODIGO_NIDUX"].ToString();
                    dgvCategorias.Rows[n].Cells[3].Value = item["SUBCATEGORIA_NIDUX"].ToString();
                    dgvCategorias.Rows[n].Cells[4].Value = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarCategorias_Click(object sender, EventArgs e)
        {
            Agregar_Categorias cat = new Agregar_Categorias();
            cat.ShowDialog();
            if (cat.n == 1)
            {
                dgvCategorias.Rows.Clear();
                CargaDatos_Categorias();
            }
        }

        private void btnGuardarCategorias_Click(object sender, EventArgs e)
        {
            try
            {
                if (lista_categorias_editados.Count() == 0)
                {
                    MessageBox.Show("No se ha editado ningúna Categoría", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    foreach (Clases.Categorias item in lista_categorias_editados)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cnn.AbrirConexion();
                        cmd.CommandText = "" + com + ".ACTUALIZAR_CATEGORIAS_APP_SIMPLE";
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NOMBRE", item.nombre);
                        cmd.Parameters.AddWithValue("@DESCRIPCION", item.descripcion);
                        cmd.Parameters.AddWithValue("@CODIGO", Convert.ToInt32(item.codigo_categoria));
                        cmd.ExecuteNonQuery();
                    }
                    cnn.CerrarConexion();
                    MessageBox.Show("Categorias Actualizadas con Éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lista_categorias_editados.Clear();
                    dgvCategorias.Rows.Clear();
                    CargaDatos_Categorias();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarCategorias_Click(object sender, EventArgs e)
        {
            dgvCategorias.Rows.Clear();
            CargaDatos_Categorias();
            MessageBox.Show("Datos de Categorías refrescados", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSincronizarCategorias_Click(object sender, EventArgs e)
        {
            try
            {
                logsFile.WriteLogs($@"
       ===========================================
       ▶️  [INICIO DE SINCRONIZACIÓN DE CATEGORÍAS]
       ===========================================");

                Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
                string token = obj_metodos.Obtener_Token();

                // Comprobamos si hubo error al obtener el token
                if (token.Equals("Error en login"))
                {
                    logsFile.WriteLogs($@"
       🛑 [ERROR EN LOGIN]
       -------------------------------------------
       Error al obtener el token de Nidux.
       ===========================================");

                    MessageBox.Show("Error en el login", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Conectamos al API para obtener las categorías
                    var client = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_categorias_nidux_simple");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);

                    if ((int)response.StatusCode == 200)
                    {
                        int cont_cat = 0;
                        List<Clases.Categorias> lista_categorias = JsonConvert.DeserializeObject<List<Clases.Categorias>>(response.Content);

                        if (lista_categorias.Count > 0)
                        {
                            while (cont_cat < lista_categorias.Count)
                            {
                                logsFile.WriteLogs($@"
       🔄 [SINCRONIZANDO CATEGORÍA]
       -------------------------------------------
       Sincronizando categoría: {lista_categorias[cont_cat].nombre} (Código: {lista_categorias[cont_cat].codigo_categoria}).
       ===========================================");

                                string jsonBody = $@"
                        {{
                           ""category_name"":""{lista_categorias[cont_cat].nombre}"",
                           ""category_father"":{lista_categorias[cont_cat].categoria_padre},
                           ""category_description"":""<p>{lista_categorias[cont_cat].descripcion}</p>"",
                           ""category_status"":1,
                           ""category_weight"":0,
                           ""mallId"":0,
                           ""traducciones"":[
                              {{
                                 ""idioma"":1,
                                 ""nombre"":""Name Default"",
                                 ""descripcion"":""<p>Description Default</p>""
                              }}
                           ]
                        }}";

                                var client2 = new RestClient("https://api.nidux.dev/v3/categories/" + lista_categorias[cont_cat].codigo_categoria);
                                client2.Timeout = -1;
                                var request2 = new RestRequest(Method.PUT);
                                request2.AddHeader("Authorization", "Bearer " + token);
                                request2.AddHeader("Content-Type", "application/json");
                                request2.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
                                IRestResponse response2 = client2.Execute(request2);

                                if ((int)response2.StatusCode == 200)
                                {
                                    logsFile.WriteLogs($@"
       ✅ [CATEGORÍA SINCRONIZADA EXITOSAMENTE]
       -------------------------------------------
       Categoría con código {lista_categorias[cont_cat].codigo_categoria} sincronizada con éxito.
       ===========================================");
                                }
                                else
                                {
                                    logsFile.WriteLogs($@"
       🛑 [ERROR AL SINCRONIZAR CATEGORÍA]
       -------------------------------------------
       Error al sincronizar categoría con código {lista_categorias[cont_cat].codigo_categoria}. Respuesta: {response2.Content}
       ===========================================");

                                    MessageBox.Show("Error al sincronizar categoría", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                cont_cat++;
                            }

                            // Actualizamos la fecha de sincronización
                            var client3 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_fecha");
                            client3.Timeout = -1;
                            var request3 = new RestRequest(Method.GET);
                            IRestResponse response3 = client3.Execute(request3);

                            if ((int)response3.StatusCode == 200)
                            {
                                logsFile.WriteLogs($@"
       ✅ [FECHA ACTUALIZADA]
       -------------------------------------------
       Fecha de sincronización actualizada correctamente.
       ===========================================");
                            }
                            else
                            {
                                logsFile.WriteLogs($@"
       🛑 [ERROR AL ACTUALIZAR FECHA]
       -------------------------------------------
       Error al actualizar la fecha de sincronización. Respuesta: {response3.Content}
       ===========================================");

                                MessageBox.Show("Error al actualizar la fecha", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            logsFile.WriteLogs($@"
       🛑 [SIN CATEGORÍAS PARA ACTUALIZAR]
       -------------------------------------------
       No hay categorías disponibles para actualizar.
       ===========================================");

                            MessageBox.Show("No hay Categorías para actualizar", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        logsFile.WriteLogs($@"
       🛑 [ERROR AL CONECTAR AL API]
       -------------------------------------------
       Error al conectar con el API para obtener categorías. Respuesta: {response.Content}
       ===========================================");

                        MessageBox.Show("Error en el Api Propio Método actualizar categorias nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    logsFile.WriteLogs($@"
       ===========================================
       ✅ [SINCRONIZACIÓN DE CATEGORÍAS FINALIZADA]
       ===========================================");
                    MessageBox.Show("Categorías Actualizadas con Éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                logsFile.WriteLogs($@"
       🛑 [EXCEPCIÓN EN SINCRONIZACIÓN DE CATEGORÍAS]
       -------------------------------------------
       {ex.ToString()}
       ===========================================");

                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnObtenerCategorias_Click(object sender, EventArgs e)
        {
            try
            {
                logsFile.WriteLogs($@"
       ===========================================
       🔄 [INICIO DE OBTENCIÓN DE CATEGORÍAS NIDUX]
       ===========================================");
                Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
                string token = obj_metodos.Obtener_Token();
                //empezamos la sincronizacion de las categorias desde el api de aplix a Nidux
                if (token.Equals("Error en login"))
                {
                    MessageBox.Show("Error en el login", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //Nos traemos las categorias
                    var client = new RestClient("https://api.nidux.dev/v3/categories/");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddHeader("Content-Type", "application/json");
                    IRestResponse response = client.Execute(request);

                    if ((int)response.StatusCode == 200)
                    {


                        //Insertamos con el api las categorias
                        var client2 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/insertar_categorias");
                        client2.Timeout = -1;
                        var request2 = new RestRequest(Method.POST);
                        request2.AddParameter("application/json", response.Content, ParameterType.RequestBody);
                        IRestResponse response2 = client2.Execute(request2);

                        if ((int)response2.StatusCode == 200)
                        {
                            logsFile.WriteLogs($@"
                       ===========================================
                       ✅ [CATEGORÍAS INSERTADAS CON ÉXITO]
                       -------------------------------------------
                       Categorías insertadas en el sistema con éxito.
                       ===========================================");
                            MessageBox.Show("Categorias Actualizadas con Éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvCategorias.Rows.Clear();
                            CargaDatos_Categorias();
                        }
                        else
                        {
                            logsFile.WriteLogs($@"
                               🛑 [ERROR AL INSERTAR CATEGORÍAS]
                               -------------------------------------------
                               Error al insertar categorías en el API propio.
                               Respuesta: {response2.Content}
                               ===========================================");
                            MessageBox.Show("Error en el Api Propio Método insertar categorias a tablas", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        logsFile.WriteLogs($@"
                       🛑 [ERROR AL OBTENER CATEGORÍAS DE NIDUX]
                       -------------------------------------------
                       Error al obtener las categorías desde el API de Nidux.
                       Respuesta: {response.Content}
                       ===========================================");
                        MessageBox.Show("Error en el Api nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarCategorias_Click(object sender, EventArgs e)
        {
            try
            {
                logsFile.WriteLogs($@"
                   ===========================================
                   🛑 [INICIO DE ELIMINACIÓN DE CATEGORÍAS]
                   ===========================================");
                string codigos = "";
                string nombre = "";
                int n = 0;
                for (int i = 0; i < lista_categorias_editados.Count; i++)
                {
                    if (lista_categorias_editados[i].activo.Equals("True"))
                    {
                        codigos = lista_categorias_editados[i].codigo_categoria + "," + codigos;//codigo de nidux
                        nombre = lista_categorias_editados[i].nombre + "," + nombre;//nombre de nidux
                    }
                }

                if (codigos == "")
                {
                    logsFile.WriteLogs($@"
                   🛑 [NINGUNA CATEGORÍA SELECCIONADA]
                   -------------------------------------------
                   No se ha seleccionado ninguna categoría para eliminar.
                   ===========================================");
                    MessageBox.Show("No se ha seleccionada ninguna categoría para Eliminar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    codigos = codigos.Remove(codigos.Length - 1);
                    nombre = nombre.Remove(nombre.Length - 1);
                    DialogResult result =
                    MessageBox.Show("¿Está seguro que desea Eliminar Categorías? Las Categorías que se van a Eliminar son: " + nombre, "Mensaje de Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        logsFile.WriteLogs($@"
                       ===========================================
                       🔄 [CONFIRMACIÓN DE ELIMINACIÓN]
                       -------------------------------------------
                       Categorías seleccionadas para eliminación: {nombre}.
                       ===========================================");
                        Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
                        string token = obj_metodos.Obtener_Token();
                        //empezamos la sincronizacion de las categorias desde el api de aplix a Nidux
                        if (token.Equals("Error en login"))
                        {
                            MessageBox.Show("Error en el login del API de Nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            string[] codigos_categorias = codigos.Split(',');
                            foreach (string item in codigos_categorias)
                            {
                                //Empezamos a Eliminar en Nidux
                                var client = new RestClient("https://api.nidux.dev/v3/categories/" + item);
                                client.Timeout = -1;
                                var request = new RestRequest(Method.DELETE);
                                request.AddHeader("Authorization", "Bearer " + token);
                                request.AddHeader("Content-Type", "application/json");
                                IRestResponse response = client.Execute(request);

                                if ((int)response.StatusCode == 200)
                                {
                                    Clases.Respuesta_Categoria lista_respuesta = JsonConvert.DeserializeObject<Clases.Respuesta_Categoria>(response.Content);

                                    SqlCommand cmd = new SqlCommand();
                                    cmd.Connection = cnn.AbrirConexion();
                                    cmd.CommandText = "" + com + ".ELIMINAR_CATEGORIAS_ARTICULO_APP";
                                    cmd.CommandTimeout = 0;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@CODIGO", Convert.ToInt32(item));
                                    cmd.ExecuteNonQuery();
                                    n = 1;

                                    //eliminamos las categorias de CODISA
                                    string query = "DELETE FROM naf5m.NIDUX_CATEGORIAS WHERE COD_CATEGORIA_NIDUX = " + item;
                                    OracleCommand cmds = new OracleCommand();
                                    cmds.Connection = cnn.open();
                                    cmds.CommandText = query;
                                    cmds.CommandTimeout = 0;
                                    cmds.CommandType = CommandType.Text;
                                    cmds.ExecuteNonQuery();
                                    cnn.close();

                                    logsFile.WriteLogs($@"
                                   ✅ [CATEGORÍA ELIMINADA EXITOSAMENTE]
                                   -------------------------------------------
                                   Categoría con código {item} eliminada correctamente.
                                   ===========================================");
                                }
                                else
                                {
                                    
                                    Clases.Respuesta_Categoria lista_respuesta = JsonConvert.DeserializeObject<Clases.Respuesta_Categoria>(response.Content);
                                    logsFile.WriteLogs($@"
                                   🛑 [ERROR AL ELIMINAR CATEGORÍA]
                                   -------------------------------------------
                                   Error al eliminar categoría con código {item}. Comentarios: {lista_respuesta.comentarios}
                                   ===========================================");
                                    MessageBox.Show(lista_respuesta.comentarios, "Mensaje de Error V2", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            cnn.CerrarConexion();
                            if (n == 1)
                            {
                                logsFile.WriteLogs($@"
                                   ===========================================
                                   ✅ [CATEGORÍAS ELIMINADAS CON ÉXITO]
                                   -------------------------------------------
                                   Categorías eliminadas correctamente.
                                   ===========================================");
                                MessageBox.Show("Categorías Elimanas con éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lista_categorias_editados.Clear();
                                dgvCategorias.Rows.Clear();
                                CargaDatos_Categorias();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logsFile.WriteLogs($@"
       🛑 [EXCEPCIÓN AL ELIMINAR CATEGORÍAS]
       -------------------------------------------
       {ex.ToString()}
       ===========================================");
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error V3", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Buscar_Categorias(string categoria)
        {
            try
            {
                dgvCategorias.Rows.Clear();
                if (cbbCategorias.Text.Equals("Nombre"))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "SELECT NOMBRE, DESCRIPCION, CODIGO_NIDUX, SUBCATEGORIA_NIDUX FROM " + com + ".CATEGORIAS WHERE NOMBRE LIKE '%" + categoria + "%'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cnn.CerrarConexion();
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dgvCategorias.Rows.Add();

                        Clases.Categorias product = lista_categorias_editados.FirstOrDefault(x => x.codigo_categoria == dt.Rows[n]["CODIGO_NIDUX"].ToString());

                        if (product == null)
                        {
                            dgvCategorias.Rows[n].Cells[0].Value = item["NOMBRE"].ToString();//nombre
                            dgvCategorias.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                            dgvCategorias.Rows[n].Cells[2].Value = item["CODIGO_NIDUX"].ToString();
                            dgvCategorias.Rows[n].Cells[3].Value = item["SUBCATEGORIA_NIDUX"].ToString();
                            dgvCategorias.Rows[n].Cells[4].Value = false;
                        }
                        else
                        {
                            dgvCategorias.Rows[n].Cells[0].Value = product.nombre;
                            dgvCategorias.Rows[n].Cells[1].Value = product.descripcion;
                            dgvCategorias.Rows[n].Cells[2].Value = product.codigo_categoria;
                            dgvCategorias.Rows[n].Cells[3].Value = product.categoria_padre;
                            dgvCategorias.Rows[n].Cells[4].Value = product.activo;
                        }
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "SELECT NOMBRE, DESCRIPCION, CODIGO_NIDUX, SUBCATEGORIA_NIDUX FROM " + com + ".CATEGORIAS WHERE CODIGO_NIDUX LIKE '%" + categoria + "%'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cnn.CerrarConexion();
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dgvCategorias.Rows.Add();

                        Clases.Categorias product = lista_categorias_editados.FirstOrDefault(x => x.codigo_categoria == dt.Rows[n]["CODIGO_NIDUX"].ToString());

                        if (product == null)
                        {
                            dgvCategorias.Rows[n].Cells[0].Value = item["NOMBRE"].ToString();//nombre
                            dgvCategorias.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                            dgvCategorias.Rows[n].Cells[2].Value = item["CODIGO_NIDUX"].ToString();
                            dgvCategorias.Rows[n].Cells[3].Value = item["SUBCATEGORIA_NIDUX"].ToString();
                            dgvCategorias.Rows[n].Cells[4].Value = false;
                        }
                        else
                        {
                            dgvCategorias.Rows[n].Cells[0].Value = product.nombre;
                            dgvCategorias.Rows[n].Cells[1].Value = product.descripcion;
                            dgvCategorias.Rows[n].Cells[2].Value = product.codigo_categoria;
                            dgvCategorias.Rows[n].Cells[3].Value = product.categoria_padre;
                            dgvCategorias.Rows[n].Cells[4].Value = product.activo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void txtBuscarCategorias_TextChanged(object sender, EventArgs e)
        {
            Buscar_Categorias(txtBuscarCategorias.Text.ToString());
        }

        private void dgvCategorias_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Clases.Categorias cat = new Clases.Categorias();

            cat.nombre = this.dgvCategorias.CurrentRow.Cells[0].Value.ToString();
            cat.descripcion = this.dgvCategorias.CurrentRow.Cells[1].Value.ToString();
            cat.codigo_categoria = this.dgvCategorias.CurrentRow.Cells[2].Value.ToString();
            cat.categoria_padre = this.dgvCategorias.CurrentRow.Cells[3].Value.ToString();
            cat.activo = this.dgvCategorias.CurrentRow.Cells[4].Value.ToString();

            Clases.Categorias product = lista_categorias_editados.FirstOrDefault(x => x.codigo_categoria == this.dgvCategorias.CurrentRow.Cells[2].Value.ToString());

            if (product == null)
            {
                lista_categorias_editados.Add(cat);
            }
            else
            {
                lista_categorias_editados.RemoveAll(x => x.codigo_categoria == (cat.codigo_categoria));
                lista_categorias_editados.Add(cat);
            }
        }

        private void dgvCategorias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvCategorias.EndEdit();
            if (e.ColumnIndex == 4)
            {
                Clases.Categorias cat = new Clases.Categorias();

                cat.nombre = this.dgvCategorias.CurrentRow.Cells[0].Value.ToString();
                cat.descripcion = this.dgvCategorias.CurrentRow.Cells[1].Value.ToString();
                cat.codigo_categoria = this.dgvCategorias.CurrentRow.Cells[2].Value.ToString();
                cat.categoria_padre = this.dgvCategorias.CurrentRow.Cells[3].Value.ToString();
                cat.activo = this.dgvCategorias.CurrentRow.Cells[4].Value.ToString();

                Clases.Categorias product = lista_categorias_editados.FirstOrDefault(x => x.codigo_categoria == this.dgvCategorias.CurrentRow.Cells[2].Value.ToString());

                if (product == null)
                {
                    lista_categorias_editados.Add(cat);
                }
                else
                {
                    lista_categorias_editados.RemoveAll(x => x.codigo_categoria == (cat.codigo_categoria));
                    lista_categorias_editados.Add(cat);
                }
                dgvCategorias.EndEdit();
            }
        }

        private void btnCategorias_Oracle_Click(object sender, EventArgs e)
        {
            DataTable dt = GetDataSP(com + ".INSERTAR_CATEGORIAS_CODISA");

            try
            {
                string grupo = ConfigurationManager.AppSettings["grupo"];
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cnn.open();

                foreach (DataRow item in dt.Rows)
                {
                    cmd.CommandText = "NAF5M.PKG_NIDUX_WEB.POST_CATEGORIAS";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("PGRUPO", OracleType.VarChar, 10).Value = grupo;
                    cmd.Parameters.Add("PCOD_CATEGORIA_NIDUX", OracleType.Number, 3).Value = Convert.ToInt32(item["CODIGO_NIDUX"].ToString());
                    cmd.Parameters.Add("PDESC_CATEGORIA_NIDUX", OracleType.VarChar, 100).Value = item["NOMBRE_CAT"].ToString();
                    cmd.Parameters.Add("PCOD_SUBCATEGORIA_NIDUX", OracleType.Number, 3).Value = Convert.ToInt32(item["SUBCATEGORIA_NIDUX"].ToString());
                    cmd.Parameters.Add("PDESC_SUBCATEGORIA_NIDUX", OracleType.VarChar, 100).Value = item["SUBNOMBRE_CAT"].ToString();
                    cmd.Parameters.Add("pMENSAJE", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                cnn.close();
                MessageBox.Show("Categorías Actualizadas con Éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /*------------------------ Marcas -----------------------------------*/

        public void CargaDatos_Marcas()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT DESCRIPCION_NIDUX, CODIGO_NIDUX FROM " + com + ".MARCAS";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvMarcas.Rows.Add();

                    dgvMarcas.Rows[n].Cells[0].Value = item["DESCRIPCION_NIDUX"].ToString();
                    dgvMarcas.Rows[n].Cells[1].Value = item["CODIGO_NIDUX"].ToString();
                    dgvMarcas.Rows[n].Cells[2].Value = false;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarMarcas_Click(object sender, EventArgs e)
        {
            Agregar_Marcas marcas = new Agregar_Marcas();
            marcas.ShowDialog();
            if (marcas.n == 1)
            {
                dgvMarcas.Rows.Clear();
                CargaDatos_Marcas();
            }
        }

        private void btnGuardarMarcas_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Clases.Marca item in lista_marcas_editados)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "" + com + ".ACTUALIZAR_MARCAS_APP_SIMPLE";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOMBRE", item.nombre);
                    cmd.Parameters.AddWithValue("@ID", item.id);
                    cmd.ExecuteNonQuery();
                    cnn.CerrarConexion();
                }
                lista_marcas_editados.Clear();
                MessageBox.Show("Marcas Actualizadas con Éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvMarcas.Rows.Clear();
                CargaDatos_Marcas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarMarcas_Click(object sender, EventArgs e)
        {
            dgvMarcas.Rows.Clear();
            CargaDatos_Marcas();
            MessageBox.Show("Datos de Marcas refrescados", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSincronizarMarcas_Click(object sender, EventArgs e)
        {
            try
            {
                logsFile.WriteLogs($@"
       ===========================================
       ▶️ [INICIO DE SINCRONIZACIÓN DE MARCAS]
       ===========================================");

                Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
                string token = obj_metodos.Obtener_Token();

                // Verificamos si hubo error al obtener el token
                if (token.Equals("Error en login"))
                {
                    logsFile.WriteLogs($@"
       🛑 [ERROR EN LOGIN]
       -------------------------------------------
       Error al obtener el token de Nidux.
       ===========================================");

                    MessageBox.Show("Error en el login", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int cont_marcas = 0;

                    // Consume las marcas
                    var client8 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_marcas_simple");
                    client8.Timeout = -1;
                    var request8 = new RestRequest(Method.GET);
                    IRestResponse response8 = client8.Execute(request8);

                    if ((int)response8.StatusCode == 200)
                    {
                        List<Clases.Marca> lista_marcas = JsonConvert.DeserializeObject<List<Clases.Marca>>(response8.Content);

                        if (lista_marcas.Count > 0)
                        {
                            while (cont_marcas < lista_marcas.Count)
                            {
                                var jsonMarcas = new
                                {
                                    brand_name = lista_marcas[cont_marcas].nombre
                                };

                                logsFile.WriteLogs($@"
                           ----------------------------------------------------------------------
                            Sincronizando marca: {lista_marcas[cont_marcas].nombre} (Código: {lista_marcas[cont_marcas].codigo_marca}). 
                           -----------------------------------------------------------------------");

                                var jsonBody = JsonConvert.SerializeObject(jsonMarcas, Formatting.Indented);

                                if (!String.IsNullOrEmpty(lista_marcas[cont_marcas].codigo_marca))
                                {
                                    // Actualizamos la marca en Nidux
                                    var client = new RestClient("https://api.nidux.dev/v3/brands/" + lista_marcas[cont_marcas].codigo_marca);
                                    client.Timeout = -1;
                                    var request = new RestRequest(Method.PUT);
                                    request.AddHeader("Authorization", "Bearer " + token);
                                    request.AddHeader("Content-Type", "application/json");
                                    request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
                                    IRestResponse response = client.Execute(request);

                                    if ((int)response.StatusCode != 200)
                                    {
                                        logsFile.WriteLogs($@"
       🛑 [ERROR AL ACTUALIZAR MARCA]
       -------------------------------------------
       Error al actualizar la marca con código {lista_marcas[cont_marcas].codigo_marca}. Respuesta: {response.Content}
       ===========================================");

                                        MessageBox.Show("Error al actualizar marca", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    // Insertamos la nueva marca en Nidux
                                    var client = new RestClient("https://api.nidux.dev/v3/brands/");
                                    client.Timeout = -1;
                                    var request = new RestRequest(Method.POST);
                                    request.AddHeader("Authorization", "Bearer " + token);
                                    request.AddHeader("Content-Type", "application/json");
                                    request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
                                    IRestResponse response = client.Execute(request);

                                    if ((int)response.StatusCode == 200)
                                    {
                                        Clases.Respuesta_Marcas lista_respuesta = JsonConvert.DeserializeObject<Clases.Respuesta_Marcas>(response.Content);
                                        int id = lista_respuesta.id;

                                        SqlCommand cmd = new SqlCommand();
                                        cmd.Connection = cnn.AbrirConexion();
                                        cmd.CommandText = "" + com + ".ACT_ID_MARCAS";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@NOMBRE", lista_marcas[cont_marcas].nombre);
                                        cmd.Parameters.AddWithValue("@CODIGO", id);
                                        cmd.ExecuteNonQuery();
                                        cnn.CerrarConexion();
                                    }
                                    else
                                    {
                                        logsFile.WriteLogs($@"
       🛑 [ERROR AL INGRESAR MARCA]
       -------------------------------------------
       Error al ingresar marca: {lista_marcas[cont_marcas].nombre}. Respuesta: {response.Content}
       ===========================================");

                                        MessageBox.Show("Error al ingresar marcas en Nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }

                                cont_marcas++;
                            }

                            // Actualizamos la fecha
                            var client3 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_fecha");
                            client3.Timeout = -1;
                            var request3 = new RestRequest(Method.GET);
                            IRestResponse response3 = client3.Execute(request3);

                            if ((int)response3.StatusCode == 200)
                            {
                                logsFile.WriteLogs($@"
       ✅ [FECHA ACTUALIZADA]
       -------------------------------------------
       Fecha de sincronización actualizada correctamente.
       ===========================================");
                            }
                            else
                            {
                                logsFile.WriteLogs($@"
       🛑 [ERROR AL ACTUALIZAR FECHA]
       -------------------------------------------
       Error al actualizar la fecha de sincronización. Respuesta: {response3.Content}
       ===========================================");

                                MessageBox.Show("Error al actualizar la fecha", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            logsFile.WriteLogs($@"
       🛑 [SIN MARCAS PARA ACTUALIZAR]
       -------------------------------------------
       No hay marcas disponibles para actualizar.
       ===========================================");

                            MessageBox.Show("No hay marcas para actualizar", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        logsFile.WriteLogs($@"
       🛑 [ERROR AL CONECTAR AL API]
       -------------------------------------------
       Error al conectar con el API para actualizar marcas. Respuesta: {response8.Content}
       ===========================================");

                        MessageBox.Show("Error en el Api Propio Método actualizar marcas nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    logsFile.WriteLogs($@"
       ===========================================
       ✅ [SINCRONIZACIÓN DE MARCAS FINALIZADA]
       ===========================================");

                    MessageBox.Show("Marcas Actualizadas con Éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                logsFile.WriteLogs($@"
       🛑 [EXCEPCIÓN EN SINCRONIZACIÓN DE MARCAS]
       -------------------------------------------
       {ex.ToString()}
       ===========================================");

                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnObtenerMarcas_Click(object sender, EventArgs e)
        {
            try
            {
                Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
                string token = obj_metodos.Obtener_Token();
                //empezamos la sincronizacion de las categorias desde el api de aplix a Nidux
                if (token.Equals("Error en login"))
                {
                    MessageBox.Show("Error en el login", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //Nos traemos las categorias
                    var client = new RestClient("https://api.nidux.dev/v3/brands/");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddHeader("Content-Type", "application/json");
                    IRestResponse response = client.Execute(request);

                    if ((int)response.StatusCode == 200)
                    {
                        //Insertamos con el api las categorias
                        var client2 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/insertar_marcas");
                        client2.Timeout = -1;
                        var request2 = new RestRequest(Method.POST);
                        request2.AddParameter("application/json", response.Content, ParameterType.RequestBody);
                        IRestResponse response2 = client2.Execute(request2);

                        if ((int)response2.StatusCode == 200)
                        {
                            //terminamos el flujo al ingresar en CODISA
                            Clases.MarcasNidux lista_respuesta = JsonConvert.DeserializeObject<Clases.MarcasNidux>(response.Content);

                            //revisamos si hay marcas en el json
                            if (lista_respuesta.marcas.Count > 0)
                            {
                                int n = 0;
                                OracleCommand cmd = new OracleCommand();
                                while (n < lista_respuesta.marcas.Count)
                                {
                                    cmd.Connection = cnn.open();
                                    cmd.CommandText = "NAF5M.PKG_NIDUX_WEB.update_marcas";
                                    cmd.CommandTimeout = 0;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("PDESCRIPCION", OracleType.VarChar, 30).Value = lista_respuesta.marcas[n].nombre;
                                    cmd.Parameters.Add("PCOD_MARCA_NIUDX", OracleType.Number, 5).Value = lista_respuesta.marcas[n].id;
                                    cmd.Parameters.Add("pMENSAJE", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;
                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();
                                    cnn.close();
                                    n++;
                                }
                            }
                            MessageBox.Show("Marcas Actualizadas con Éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvMarcas.Rows.Clear();
                            CargaDatos_Marcas();
                        }
                        else
                        {
                            MessageBox.Show("Error en el Api Propio Método insertar marcas a tablas", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error en el Api nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarMarcas_Click(object sender, EventArgs e)
        {
            try
            {
                string codigos = "";
                string nombre = "";
                int n = 0;
                for (int i = 0; i < lista_marcas_editados.Count; i++)
                {
                    if (lista_marcas_editados[i].activo.Equals("True"))
                    {
                        codigos = lista_marcas_editados[i].id + "," + codigos;//codigo de nidux
                        nombre = lista_marcas_editados[i].nombre + "," + nombre;//nombre de nidux
                    }
                }
                if (codigos == "")
                {
                    MessageBox.Show("No se ha seleccionada ninguna Marca para Eliminar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    codigos = codigos.Remove(codigos.Length - 1);
                    nombre = nombre.Remove(nombre.Length - 1);
                    DialogResult result =
                    MessageBox.Show("¿Está seguro que desea Eliminar Marcas? Las Marcas que se van a Eliminar son: " + nombre, "Mensaje de Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
                        string token = obj_metodos.Obtener_Token();
                        //empezamos la sincronizacion de las categorias desde el api de aplix a Nidux
                        if (token.Equals("Error en login"))
                        {
                            MessageBox.Show("Error en el login del API de Nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            string[] codigos_marcas = codigos.Split(',');
                            string[] desc_marcas = nombre.Split(',');
                            foreach (string item in codigos_marcas)
                            {
                                //Empezamos a Eliminar en Nidux
                                var client = new RestClient("https://api.nidux.dev/v3/brands/" + item);
                                client.Timeout = -1;
                                var request = new RestRequest(Method.DELETE);
                                request.AddHeader("Authorization", "Bearer " + token);
                                request.AddHeader("Content-Type", "application/json");
                                IRestResponse response = client.Execute(request);

                                if ((int)response.StatusCode == 200)
                                {
                                    //Respuesta_Categoria lista_respuesta = JsonConvert.DeserializeObject<Respuesta_Categoria>(response.Content);
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.Connection = cnn.AbrirConexion();
                                    cmd.CommandText = "" + com + ".ELIMINAR_MARCAS_ARTICULO_APP";
                                    cmd.CommandTimeout = 0;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@CODIGO", Convert.ToInt32(item));
                                    cmd.ExecuteNonQuery();
                                    cnn.CerrarConexion();
                                    n = 1;
                                }
                                else
                                {
                                    MessageBox.Show("Error a la hora de eliminar una marca en Nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            //eliminar en codisa
                            foreach (string item in desc_marcas)
                            {
                                OracleCommand oracmd = new OracleCommand();
                                oracmd.Connection = cnn.open();
                                oracmd.CommandText = "NAF5M.PKG_NIDUX_WEB.update_marcas";
                                oracmd.CommandTimeout = 0;
                                oracmd.CommandType = CommandType.StoredProcedure;
                                oracmd.Parameters.Add("PDESCRIPCION", OracleType.VarChar, 30).Value = item;
                                oracmd.Parameters.Add("PCOD_MARCA_NIUDX", OracleType.Number, 5).Value = DBNull.Value;
                                oracmd.Parameters.Add("pMENSAJE", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;
                                oracmd.ExecuteNonQuery();
                                oracmd.Parameters.Clear();
                                cnn.close();
                            }
                            if (n == 1)
                            {
                                MessageBox.Show("Marcas Elimanas con éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lista_marcas_editados.Clear();
                                dgvMarcas.Rows.Clear();
                                CargaDatos_Marcas();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Buscar_Marcas(string marcas)
        {
            try
            {
                dgvMarcas.Rows.Clear();
                if (cbbMarcas.Text.Equals("Nombre"))
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "SELECT DESCRIPCION_NIDUX, CODIGO_NIDUX FROM " + com + ".MARCAS WHERE DESCRIPCION_NIDUX LIKE '%" + marcas + "%'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dgvMarcas.Rows.Add();

                        Clases.Marca product = lista_marcas_editados.FirstOrDefault(x => x.id == Convert.ToInt32(dt.Rows[n]["CODIGO_NIDUX"].ToString()));

                        if (product == null)
                        {
                            dgvMarcas.Rows[n].Cells[0].Value = item["DESCRIPCION_NIDUX"].ToString();
                            dgvMarcas.Rows[n].Cells[1].Value = item["CODIGO_NIDUX"].ToString();
                            dgvMarcas.Rows[n].Cells[2].Value = false;
                        }
                        else
                        {
                            dgvMarcas.Rows[n].Cells[0].Value = product.nombre;
                            dgvMarcas.Rows[n].Cells[1].Value = product.id;
                            dgvMarcas.Rows[n].Cells[2].Value = product.activo;
                        }
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "SELECT DESCRIPCION_NIDUX, CODIGO_NIDUX FROM " + com + ".MARCAS WHERE CODIGO_NIDUX LIKE '%" + marcas + "%'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dgvMarcas.Rows.Add();

                        Clases.Marca product = lista_marcas_editados.FirstOrDefault(x => x.id == Convert.ToInt32(dt.Rows[n]["CODIGO_NIDUX"].ToString()));

                        if (product == null)
                        {
                            dgvMarcas.Rows[n].Cells[0].Value = item["DESCRIPCION_NIDUX"].ToString();
                            dgvMarcas.Rows[n].Cells[1].Value = item["CODIGO_NIDUX"].ToString();
                            dgvMarcas.Rows[n].Cells[2].Value = false;
                        }
                        else
                        {
                            dgvMarcas.Rows[n].Cells[0].Value = product.nombre;
                            dgvMarcas.Rows[n].Cells[1].Value = product.id;
                            dgvMarcas.Rows[n].Cells[2].Value = product.activo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void txtBuscarMarcas_TextChanged(object sender, EventArgs e)
        {
            Buscar_Marcas(txtBuscarMarcas.Text.ToString());
        }

        private void dgvMarcas_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Clases.Marca marca = new Clases.Marca();

            marca.nombre = this.dgvMarcas.CurrentRow.Cells[0].Value.ToString();
            marca.id = Convert.ToInt32(this.dgvMarcas.CurrentRow.Cells[1].Value.ToString());
            marca.activo = this.dgvMarcas.CurrentRow.Cells[2].Value.ToString();

            Clases.Marca product = lista_marcas_editados.FirstOrDefault(x => x.id == Convert.ToInt32(this.dgvMarcas.CurrentRow.Cells[1].Value.ToString()));

            if (product == null)
            {
                lista_marcas_editados.Add(marca);
            }
            else
            {
                lista_marcas_editados.RemoveAll(x => x.id == (marca.id));
                lista_marcas_editados.Add(marca);
            }
        }

        private void dgvMarcas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvMarcas.EndEdit();
            if (e.ColumnIndex == 2)
            {
                Clases.Marca marca = new Clases.Marca();

                marca.nombre = this.dgvMarcas.CurrentRow.Cells[0].Value.ToString();
                marca.id = Convert.ToInt32(this.dgvMarcas.CurrentRow.Cells[1].Value.ToString());
                marca.activo = this.dgvMarcas.CurrentRow.Cells[2].Value.ToString();

                Clases.Marca product = lista_marcas_editados.FirstOrDefault(x => x.id == Convert.ToInt32(this.dgvMarcas.CurrentRow.Cells[1].Value.ToString()));

                if (product == null)
                {
                    lista_marcas_editados.Add(marca);
                }
                else
                {
                    lista_marcas_editados.RemoveAll(x => x.id == (marca.id));
                    lista_marcas_editados.Add(marca);
                }
                dgvMarcas.EndEdit();
            }
        }

        private void btnOracleMarcas_Click(object sender, EventArgs e)
        {
            //CEMACO
            try
            {
                //obtener valores de oracle

                string query = "SELECT DISTINCT(A.cod_marca_codisa), A.cod_marca_niudx, A.descripcion FROM naf5m.nidux_marcas A, naf5m.arinda b WHERE A.cod_marca_codisa = b.codigo_marca AND b.grupo LIKE 'SUKASA%' AND A.cod_marca_niudx IS NULL";

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = cnn.open();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                OracleDataAdapter da = new OracleDataAdapter();
                da.SelectCommand = cmd;

                DataTable dt = new DataTable();
                da.Fill(dt);

                cnn.close();

                if (dt.Rows.Count > 0)//tiene datos
                {
                    //agregamos la categoria a Nidux
                    Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
                    string token = obj_metodos.Obtener_Token();
                    if (token.Equals("Error en login"))
                    {
                        MessageBox.Show("Error en el login", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            var client = new RestClient("https://api.nidux.dev/v3/brands/");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("Authorization", "Bearer " + token);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddParameter("application/json", "{\r\n   \"brand_name\":\"" + item["descripcion"].ToString() + "\"\r\n}", ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);

                            if ((int)response.StatusCode == 200)
                            {
                                Clases.Respuesta_Marcas lista_respuesta = JsonConvert.DeserializeObject<Clases.Respuesta_Marcas>(response.Content);
                                int id = lista_respuesta.id;

                                cmd.Connection = cnn.open();
                                cmd.CommandText = "NAF5M.PKG_NIDUX_WEB.update_marcas";
                                cmd.CommandTimeout = 0;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("PDESCRIPCION", OracleType.VarChar, 30).Value = item["descripcion"].ToString();
                                cmd.Parameters.Add("PCOD_MARCA_NIUDX", OracleType.Number, 5).Value = id;
                                cmd.Parameters.Add("pMENSAJE", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                cnn.close();
                            }
                            else
                            {
                                MessageBox.Show("Error al ingresar marca: " + item["descripcion"].ToString() + " en Nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        //me traigo esas marcas para que queden en las tablas intermedias
                        btnObtenerMarcas_Click(sender, e);
                        MessageBox.Show("Marca agregada con éxito", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    //no marcas que sincronizar
                    MessageBox.Show("No hay marcas nuevas para sincronizar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*------------------------ ATRIBUTOS -----------------------------------*/

        public void CargaDatos_Atributos()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT VA.ID, VA.DESCRIPCION, AT.DESCRIPCION AS DESCRIP_ATRIBUTO FROM " + com + ".VALORES_ATRIBUTOS AS VA" +
                    " INNER JOIN " + com + ".ATRIBUTOS AS AT ON VA.ID_ATRIBUTO = AT.ID";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvAtributos.Rows.Add();

                    dgvAtributos.Rows[n].Cells[0].Value = item["ID"].ToString();
                    dgvAtributos.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                    dgvAtributos.Rows[n].Cells[2].Value = item["DESCRIP_ATRIBUTO"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnObtenerAtributos_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            try
            {
                Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
                string token = obj_metodos.Obtener_Token();
                //empezamos la sincronizacion de las categorias desde el api de aplix a Nidux
                if (token.Equals("Error en login"))
                {
                    MessageBox.Show("Error en el login", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //Nos traemos los atributos
                    var client = new RestClient("https://api.nidux.dev/v3/attributes/");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddHeader("Content-Type", "application/json");
                    IRestResponse response = client.Execute(request);

                    if ((int)response.StatusCode == 200)
                    {
                        //Insertamos los atributos en la tabla
                        var client2 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/insertar_atributos");
                        client2.Timeout = -1;
                        var request2 = new RestRequest(Method.POST);
                        request2.AddParameter("application/json", response.Content, ParameterType.RequestBody);
                        IRestResponse response2 = client2.Execute(request2);

                        if ((int)response2.StatusCode == 200)
                        {
                            //Eliminamos los valores que ya exiten 
                            var client5 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/eliminar_valores_atributos");
                            client5.Timeout = -1;
                            var request5 = new RestRequest(Method.DELETE);
                            IRestResponse response5 = client5.Execute(request5);

                            //obtenemos los valores del Atributos
                            int n = 0;
                            Clases.Atributos_Nidux atributos = JsonConvert.DeserializeObject<Clases.Atributos_Nidux>(response.Content);
                            while (n < atributos.atributos.Count)
                            {
                                var client3 = new RestClient("https://api.nidux.dev/v3/attributes/" + atributos.atributos[n].id + "/values");
                                client3.Timeout = -1;
                                var request3 = new RestRequest(Method.GET);
                                request3.AddHeader("Authorization", "Bearer " + token);
                                request3.AddHeader("Content-Type", "application/json");
                                IRestResponse response3 = client3.Execute(request3);

                                if ((int)response3.StatusCode == 200)
                                {
                                    // deserializamos los valores del atributos
                                    Clases.Atributo_Valores atributos_valores = JsonConvert.DeserializeObject<Clases.Atributo_Valores>(response3.Content);

                                    //llenamos el nuevo Json
                                    var listavalores = new List<Clases.Valores>();
                                    Clases.Atributo atributo = new Clases.Atributo();

                                    int i = 0;
                                    while (i < atributos_valores.atributo_valores.Count)
                                    {
                                        Clases.Valores valores_atributo = new Clases.Valores();
                                        valores_atributo.id_atributo = atributos.atributos[n].id;
                                        valores_atributo.id_valor_atributo = atributos_valores.atributo_valores[i].id;
                                        valores_atributo.nombre_valor_atributo = atributos_valores.atributo_valores[i].nombre;

                                        listavalores.Add(valores_atributo);
                                        atributo.atributos = listavalores;
                                        i++;
                                    }

                                    //serializamos el json para pasarlo en string
                                    string json = JsonConvert.SerializeObject(atributo);

                                    //insertamos en tablas propias los valores del atributo
                                    var client4 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/insertar_valores_atributos");
                                    client4.Timeout = -1;
                                    var request4 = new RestRequest(Method.POST);
                                    request4.AddParameter("application/json", json, ParameterType.RequestBody);
                                    IRestResponse response4 = client4.Execute(request4);

                                    if ((int)response4.StatusCode == 200)
                                    {
                                        mensaje = atributos.atributos[n].nombre + "," + mensaje;
                                    }
                                }
                                n++;
                            }

                            MessageBox.Show("Atributos y valores Agregados con éxito: " + mensaje, "Mensaje de Confirmació", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvAtributos.Rows.Clear();
                            CargaDatos_Atributos();
                        }
                        else
                        {
                            MessageBox.Show("Error en el Api Propio Método insertar Atributos a tablas", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error en el Api nidux", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Buscar_Atributos(string atributos)
        {
            try
            {
                dgvAtributos.Rows.Clear();
                if (cbbAtributos.Text.Equals("Nombre"))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "SELECT VA.ID, VA.DESCRIPCION, AT.DESCRIPCION AS DESCRIP_ATRIBUTO FROM " + com + ".VALORES_ATRIBUTOS AS VA" +
                        " INNER JOIN " + com + ".ATRIBUTOS AS AT ON VA.ID_ATRIBUTO = AT.ID WHERE VA.DESCRIPCION LIKE '%" + atributos + "%'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cnn.CerrarConexion();
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dgvAtributos.Rows.Add();

                        dgvAtributos.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvAtributos.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                        dgvAtributos.Rows[n].Cells[2].Value = item["DESCRIP_ATRIBUTO"].ToString();
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "SELECT VA.ID, VA.DESCRIPCION, AT.DESCRIPCION AS DESCRIP_ATRIBUTO FROM " + com + ".VALORES_ATRIBUTOS AS VA" +
                        " INNER JOIN " + com + ".ATRIBUTOS AS AT ON VA.ID_ATRIBUTO = AT.ID WHERE VA.ID LIKE '%" + atributos + "%'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cnn.CerrarConexion();
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dgvAtributos.Rows.Add();

                        dgvAtributos.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvAtributos.Rows[n].Cells[1].Value = item["DESCRIPCION"].ToString();
                        dgvAtributos.Rows[n].Cells[2].Value = item["DESCRIP_ATRIBUTO"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void txtBuscarAtributo_TextChanged(object sender, EventArgs e)
        {
            Buscar_Atributos(txtBuscarAtributo.Text.ToString());
        }

        /*------------------------ PEDIDOS -----------------------------------*/

        public void CargaDatos_Pedidos()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT * FROM " + com + ".PEDIDOS ";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvPedidos.Rows.Add();

                    dgvPedidos.Rows[n].Cells[0].Value = item["ORDERID"].ToString();
                    dgvPedidos.Rows[n].Cells[1].Value = item["CLIENTE"].ToString();
                    dgvPedidos.Rows[n].Cells[2].Value = item["IDENTIFICACION"].ToString();
                    dgvPedidos.Rows[n].Cells[3].Value = item["CORREO"].ToString();
                    dgvPedidos.Rows[n].Cells[4].Value = item["TELEFONO_FIJO"].ToString();
                    dgvPedidos.Rows[n].Cells[5].Value = item["TELEFONO_MOVIL"].ToString();
                    dgvPedidos.Rows[n].Cells[6].Value = item["ES_ANONIMO"].ToString();
                    dgvPedidos.Rows[n].Cells[7].Value = item["MONTO_IMPUESTOS"].ToString();
                    dgvPedidos.Rows[n].Cells[8].Value = item["WISH_ID"].ToString();
                    dgvPedidos.Rows[n].Cells[9].Value = item["FECHA_ORDEN"].ToString();
                    dgvPedidos.Rows[n].Cells[10].Value = item["ORDERGIFTPOINTSUSED"].ToString();
                    dgvPedidos.Rows[n].Cells[11].Value = item["ESTADO_ORDEN"].ToString();
                    dgvPedidos.Rows[n].Cells[12].Value = item["MONEDA"].ToString();
                    dgvPedidos.Rows[n].Cells[13].Value = item["OBSERVACIONES"].ToString();
                    dgvPedidos.Rows[n].Cells[14].Value = item["CODIGO_AUTORIZACION"].ToString();
                    dgvPedidos.Rows[n].Cells[15].Value = item["IP_ORIGEN"].ToString();
                    dgvPedidos.Rows[n].Cells[16].Value = item["ESTADO_PAGO"].ToString();
                    dgvPedidos.Rows[n].Cells[17].Value = item["MEDIO_PAGO"].ToString();
                    dgvPedidos.Rows[n].Cells[18].Value = item["TOTAL"].ToString();
                    dgvPedidos.Rows[n].Cells[19].Value = item["CUPONUSADO"].ToString();
                    dgvPedidos.Rows[n].Cells[20].Value = item["CUPONTIPO"].ToString();
                    dgvPedidos.Rows[n].Cells[21].Value = item["SUCURSAL"].ToString();
                    dgvPedidos.Rows[n].Cells[22].Value = item["RECOGER_SUCURSAL"].ToString();
                    dgvPedidos.Rows[n].Cells[23].Value = item["CODIGO_METODO_PAGO"].ToString();
                    dgvPedidos.Rows[n].Cells[24].Value = item["MONEDA_FE"].ToString();
                    //NUEVOS
                    dgvPedidos.Rows[n].Cells[25].Value = item["TIPO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[26].Value = item["COSTO_TOTAL_SHIPPING"].ToString();
                    dgvPedidos.Rows[n].Cells[27].Value = item["TASA_IMPUESTO_SHIPPING"].ToString();

                    //campos de envio
                    dgvPedidos.Rows[n].Cells[28].Value = item["NOMBRE_DESTINATARIO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[29].Value = item["IDENTIFICACION_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[30].Value = item["TIPO_IDENTIFICACION_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[31].Value = item["CORREO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[32].Value = item["TELEFONO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[33].Value = item["MOVIL_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[34].Value = item["PAIS_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[35].Value = item["PROVINCIA_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[36].Value = item["CANTON_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[37].Value = item["DISTRITO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[38].Value = item["DETALLE_DIRECCION_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[39].Value = item["CIUDAD_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[40].Value = item["ZIP_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[41].Value = item["GEO_LATITUD_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[42].Value = item["GEO_LONGITUD_ENVIO"].ToString();
                    //campos de facturacion
                    dgvPedidos.Rows[n].Cells[43].Value = item["NOMBRE_DESTINATARIO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[44].Value = item["IDENTIFICACION_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[45].Value = item["TIPO_IDENTIFICACION_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[46].Value = item["CORREO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[47].Value = item["TELEFONO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[48].Value = item["MOVIL_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[49].Value = item["PAIS_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[50].Value = item["PROVINCIA_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[51].Value = item["CANTON_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[52].Value = item["DISTRITO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[53].Value = item["DETALLE_DIRECCION_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[54].Value = item["CIUDAD_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[55].Value = item["ZIP_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[56].Value = item["GEO_LATITUD_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[57].Value = item["GEO_LONGITUD_FAC"].ToString();

                    dgvPedidos.Rows[n].Cells[58].Value = item["CONSECUTIVO"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnObtenerPedidos_Click(object sender, EventArgs e)
        {
            dgvPedidos.Rows.Clear();
            CargaDatos_Pedidos();
            MessageBox.Show("Datos de Pedidos refrescados", "Mensaje de Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Buscar_Pedidos(string pedido)
        {
            try
            {
                dgvPedidos.Rows.Clear();
                if (cbbBuscarPedidos.Text.Equals("Orden"))
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "SELECT * FROM " + com + ".PEDIDOS WHERE ORDERID LIKE '%" + pedido + "%'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    cnn.CerrarConexion();
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dgvPedidos.Rows.Add();

                        dgvPedidos.Rows[n].Cells[0].Value = item["ORDERID"].ToString();
                        dgvPedidos.Rows[n].Cells[1].Value = item["CLIENTE"].ToString();
                        dgvPedidos.Rows[n].Cells[2].Value = item["IDENTIFICACION"].ToString();
                        dgvPedidos.Rows[n].Cells[3].Value = item["CORREO"].ToString();
                        dgvPedidos.Rows[n].Cells[4].Value = item["TELEFONO_FIJO"].ToString();
                        dgvPedidos.Rows[n].Cells[5].Value = item["TELEFONO_MOVIL"].ToString();
                        dgvPedidos.Rows[n].Cells[6].Value = item["ES_ANONIMO"].ToString();
                        dgvPedidos.Rows[n].Cells[7].Value = item["MONTO_IMPUESTOS"].ToString();
                        dgvPedidos.Rows[n].Cells[8].Value = item["FECHA_ORDEN"].ToString();
                        dgvPedidos.Rows[n].Cells[9].Value = item["ORDERGIFTPOINTSUSED"].ToString();
                        dgvPedidos.Rows[n].Cells[10].Value = item["ESTADO_ORDEN"].ToString();
                        dgvPedidos.Rows[n].Cells[11].Value = item["MONEDA"].ToString();
                        dgvPedidos.Rows[n].Cells[12].Value = item["OBSERVACIONES"].ToString();
                        dgvPedidos.Rows[n].Cells[13].Value = item["CODIGO_AUTORIZACION"].ToString();
                        dgvPedidos.Rows[n].Cells[14].Value = item["IP_ORIGEN"].ToString();
                        dgvPedidos.Rows[n].Cells[15].Value = item["ESTADO_PAGO"].ToString();
                        dgvPedidos.Rows[n].Cells[16].Value = item["MEDIO_PAGO"].ToString();
                        dgvPedidos.Rows[n].Cells[17].Value = item["TOTAL"].ToString();
                        dgvPedidos.Rows[n].Cells[18].Value = item["CUPONUSADO"].ToString();
                        dgvPedidos.Rows[n].Cells[19].Value = item["CUPONTIPO"].ToString();
                        dgvPedidos.Rows[n].Cells[20].Value = item["SUCURSAL"].ToString();
                        dgvPedidos.Rows[n].Cells[21].Value = item["RECOGER_SUCURSAL"].ToString();
                        dgvPedidos.Rows[n].Cells[22].Value = item["CODIGO_METODO_PAGO"].ToString();
                        dgvPedidos.Rows[n].Cells[23].Value = item["MONEDA_FE"].ToString();
                        //NUEVOS
                        dgvPedidos.Rows[n].Cells[24].Value = item["TIPO_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[25].Value = item["COSTO_TOTAL_SHIPPING"].ToString();
                        dgvPedidos.Rows[n].Cells[26].Value = item["TASA_IMPUESTO_SHIPPING"].ToString();

                        //campos de envio
                        dgvPedidos.Rows[n].Cells[27].Value = item["NOMBRE_DESTINATARIO_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[28].Value = item["IDENTIFICACION_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[29].Value = item["TIPO_IDENTIFICACION_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[30].Value = item["CORREO_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[31].Value = item["TELEFONO_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[32].Value = item["MOVIL_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[33].Value = item["PAIS_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[34].Value = item["PROVINCIA_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[35].Value = item["CANTON_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[36].Value = item["DISTRITO_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[37].Value = item["DETALLE_DIRECCION_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[38].Value = item["CIUDAD_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[39].Value = item["ZIP_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[40].Value = item["GEO_LATITUD_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[41].Value = item["GEO_LONGITUD_ENVIO"].ToString();
                        //campos de facturacion
                        dgvPedidos.Rows[n].Cells[42].Value = item["NOMBRE_DESTINATARIO_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[43].Value = item["IDENTIFICACION_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[44].Value = item["TIPO_IDENTIFICACION_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[45].Value = item["CORREO_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[46].Value = item["TELEFONO_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[47].Value = item["MOVIL_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[48].Value = item["PAIS_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[49].Value = item["PROVINCIA_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[50].Value = item["CANTON_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[51].Value = item["DISTRITO_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[52].Value = item["DETALLE_DIRECCION_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[53].Value = item["CIUDAD_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[54].Value = item["ZIP_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[55].Value = item["GEO_LATITUD_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[56].Value = item["GEO_LONGITUD_FAC"].ToString();

                        dgvPedidos.Rows[n].Cells[57].Value = item["CONSECUTIVO"].ToString();
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn.AbrirConexion();
                    cmd.CommandText = "SELECT * FROM " + com + ".PEDIDOS WHERE CLIENTE LIKE '%" + pedido + "%'";
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    cnn.CerrarConexion();
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dgvPedidos.Rows.Add();

                        dgvPedidos.Rows[n].Cells[0].Value = item["ORDERID"].ToString();
                        dgvPedidos.Rows[n].Cells[1].Value = item["CLIENTE"].ToString();
                        dgvPedidos.Rows[n].Cells[2].Value = item["IDENTIFICACION"].ToString();
                        dgvPedidos.Rows[n].Cells[3].Value = item["CORREO"].ToString();
                        dgvPedidos.Rows[n].Cells[4].Value = item["TELEFONO_FIJO"].ToString();
                        dgvPedidos.Rows[n].Cells[5].Value = item["TELEFONO_MOVIL"].ToString();
                        dgvPedidos.Rows[n].Cells[6].Value = item["ES_ANONIMO"].ToString();
                        dgvPedidos.Rows[n].Cells[7].Value = item["MONTO_IMPUESTOS"].ToString();
                        dgvPedidos.Rows[n].Cells[8].Value = item["FECHA_ORDEN"].ToString();
                        dgvPedidos.Rows[n].Cells[9].Value = item["ORDERGIFTPOINTSUSED"].ToString();
                        dgvPedidos.Rows[n].Cells[10].Value = item["ESTADO_ORDEN"].ToString();
                        dgvPedidos.Rows[n].Cells[11].Value = item["MONEDA"].ToString();
                        dgvPedidos.Rows[n].Cells[12].Value = item["OBSERVACIONES"].ToString();
                        dgvPedidos.Rows[n].Cells[13].Value = item["CODIGO_AUTORIZACION"].ToString();
                        dgvPedidos.Rows[n].Cells[14].Value = item["IP_ORIGEN"].ToString();
                        dgvPedidos.Rows[n].Cells[15].Value = item["ESTADO_PAGO"].ToString();
                        dgvPedidos.Rows[n].Cells[16].Value = item["MEDIO_PAGO"].ToString();
                        dgvPedidos.Rows[n].Cells[17].Value = item["TOTAL"].ToString();
                        dgvPedidos.Rows[n].Cells[18].Value = item["CUPONUSADO"].ToString();
                        dgvPedidos.Rows[n].Cells[19].Value = item["CUPONTIPO"].ToString();
                        dgvPedidos.Rows[n].Cells[20].Value = item["SUCURSAL"].ToString();
                        dgvPedidos.Rows[n].Cells[21].Value = item["RECOGER_SUCURSAL"].ToString();
                        dgvPedidos.Rows[n].Cells[22].Value = item["CODIGO_METODO_PAGO"].ToString();
                        dgvPedidos.Rows[n].Cells[23].Value = item["MONEDA_FE"].ToString();
                        //NUEVOS
                        dgvPedidos.Rows[n].Cells[24].Value = item["TIPO_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[25].Value = item["COSTO_TOTAL_SHIPPING"].ToString();
                        dgvPedidos.Rows[n].Cells[26].Value = item["TASA_IMPUESTO_SHIPPING"].ToString();

                        //campos de envio
                        dgvPedidos.Rows[n].Cells[27].Value = item["NOMBRE_DESTINATARIO_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[28].Value = item["IDENTIFICACION_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[29].Value = item["TIPO_IDENTIFICACION_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[30].Value = item["CORREO_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[31].Value = item["TELEFONO_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[32].Value = item["MOVIL_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[33].Value = item["PAIS_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[34].Value = item["PROVINCIA_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[35].Value = item["CANTON_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[36].Value = item["DISTRITO_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[37].Value = item["DETALLE_DIRECCION_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[38].Value = item["CIUDAD_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[39].Value = item["ZIP_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[40].Value = item["GEO_LATITUD_ENVIO"].ToString();
                        dgvPedidos.Rows[n].Cells[41].Value = item["GEO_LONGITUD_ENVIO"].ToString();
                        //campos de facturacion
                        dgvPedidos.Rows[n].Cells[42].Value = item["NOMBRE_DESTINATARIO_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[43].Value = item["IDENTIFICACION_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[44].Value = item["TIPO_IDENTIFICACION_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[45].Value = item["CORREO_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[46].Value = item["TELEFONO_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[47].Value = item["MOVIL_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[48].Value = item["PAIS_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[49].Value = item["PROVINCIA_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[50].Value = item["CANTON_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[51].Value = item["DISTRITO_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[52].Value = item["DETALLE_DIRECCION_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[53].Value = item["CIUDAD_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[54].Value = item["ZIP_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[55].Value = item["GEO_LATITUD_FAC"].ToString();
                        dgvPedidos.Rows[n].Cells[56].Value = item["GEO_LONGITUD_FAC"].ToString();

                        dgvPedidos.Rows[n].Cells[57].Value = item["CONSECUTIVO"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void txtBuscarPedidos_TextChanged(object sender, EventArgs e)
        {
            Buscar_Pedidos(txtBuscarPedidos.Text.ToString());
        }

        public void CargaDatos_Pedidos_Fechas(string fecha_ini, string fecha_fin)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                //cmd.CommandText = "SELECT * FROM " + com + ".PEDIDOS WHERE CONVERT(DATE,FECHA_ORDEN) " +
                //"BETWEEN CONVERT(DATE, '" + fecha_ini + "') AND CONVERT(DATE, '" + fecha_fin + "')";
                cmd.CommandText = "SELECT * FROM " + com + ".PEDIDOS WHERE FECHA_ORDEN BETWEEN '" + fecha_ini + "' AND '" + fecha_fin + "'";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvPedidos.Rows.Add();

                    dgvPedidos.Rows[n].Cells[0].Value = item["ORDERID"].ToString();
                    dgvPedidos.Rows[n].Cells[1].Value = item["CLIENTE"].ToString();
                    dgvPedidos.Rows[n].Cells[2].Value = item["IDENTIFICACION"].ToString();
                    dgvPedidos.Rows[n].Cells[3].Value = item["CORREO"].ToString();
                    dgvPedidos.Rows[n].Cells[4].Value = item["TELEFONO_FIJO"].ToString();
                    dgvPedidos.Rows[n].Cells[5].Value = item["TELEFONO_MOVIL"].ToString();
                    dgvPedidos.Rows[n].Cells[6].Value = item["ES_ANONIMO"].ToString();
                    dgvPedidos.Rows[n].Cells[7].Value = item["MONTO_IMPUESTOS"].ToString();
                    dgvPedidos.Rows[n].Cells[8].Value = item["FECHA_ORDEN"].ToString();
                    dgvPedidos.Rows[n].Cells[9].Value = item["ORDERGIFTPOINTSUSED"].ToString();
                    dgvPedidos.Rows[n].Cells[10].Value = item["ESTADO_ORDEN"].ToString();
                    dgvPedidos.Rows[n].Cells[11].Value = item["MONEDA"].ToString();
                    dgvPedidos.Rows[n].Cells[12].Value = item["OBSERVACIONES"].ToString();
                    dgvPedidos.Rows[n].Cells[13].Value = item["CODIGO_AUTORIZACION"].ToString();
                    dgvPedidos.Rows[n].Cells[14].Value = item["IP_ORIGEN"].ToString();
                    dgvPedidos.Rows[n].Cells[15].Value = item["ESTADO_PAGO"].ToString();
                    dgvPedidos.Rows[n].Cells[16].Value = item["MEDIO_PAGO"].ToString();
                    dgvPedidos.Rows[n].Cells[17].Value = item["TOTAL"].ToString();
                    dgvPedidos.Rows[n].Cells[18].Value = item["CUPONUSADO"].ToString();
                    dgvPedidos.Rows[n].Cells[19].Value = item["CUPONTIPO"].ToString();
                    dgvPedidos.Rows[n].Cells[20].Value = item["SUCURSAL"].ToString();
                    dgvPedidos.Rows[n].Cells[21].Value = item["RECOGER_SUCURSAL"].ToString();
                    dgvPedidos.Rows[n].Cells[22].Value = item["CODIGO_METODO_PAGO"].ToString();
                    dgvPedidos.Rows[n].Cells[23].Value = item["MONEDA_FE"].ToString();
                    //NUEVOS
                    dgvPedidos.Rows[n].Cells[24].Value = item["TIPO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[25].Value = item["COSTO_TOTAL_SHIPPING"].ToString();
                    dgvPedidos.Rows[n].Cells[26].Value = item["TASA_IMPUESTO_SHIPPING"].ToString();

                    //campos de envio
                    dgvPedidos.Rows[n].Cells[27].Value = item["NOMBRE_DESTINATARIO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[28].Value = item["IDENTIFICACION_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[29].Value = item["TIPO_IDENTIFICACION_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[30].Value = item["CORREO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[31].Value = item["TELEFONO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[32].Value = item["MOVIL_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[33].Value = item["PAIS_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[34].Value = item["PROVINCIA_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[35].Value = item["CANTON_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[36].Value = item["DISTRITO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[37].Value = item["DETALLE_DIRECCION_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[38].Value = item["CIUDAD_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[39].Value = item["ZIP_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[40].Value = item["GEO_LATITUD_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[41].Value = item["GEO_LONGITUD_ENVIO"].ToString();
                    //campos de facturacion
                    dgvPedidos.Rows[n].Cells[42].Value = item["NOMBRE_DESTINATARIO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[43].Value = item["IDENTIFICACION_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[44].Value = item["TIPO_IDENTIFICACION_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[45].Value = item["CORREO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[46].Value = item["TELEFONO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[47].Value = item["MOVIL_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[48].Value = item["PAIS_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[49].Value = item["PROVINCIA_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[50].Value = item["CANTON_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[51].Value = item["DISTRITO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[52].Value = item["DETALLE_DIRECCION_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[53].Value = item["CIUDAD_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[54].Value = item["ZIP_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[55].Value = item["GEO_LATITUD_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[56].Value = item["GEO_LONGITUD_FAC"].ToString();

                    dgvPedidos.Rows[n].Cells[57].Value = item["CONSECUTIVO"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void dtFechaInicio_ValueChanged(object sender, EventArgs e)
        {
            dgvPedidos.Rows.Clear();
            string fecha_ini = dtFechaInicio.Value.ToString("yyyy/MM/dd");
            string fecha_fin = dtFechaFinal.Value.ToString("yyyy/MM/dd");
            CargaDatos_Pedidos_Fechas(fecha_ini, fecha_fin);
        }

        private void dtFechaFinal_ValueChanged(object sender, EventArgs e)
        {
            dgvPedidos.Rows.Clear();
            string fecha_ini = dtFechaInicio.Value.ToString("yyyy/MM/dd");
            string fecha_fin = dtFechaFinal.Value.ToString("yyyy/MM/dd");
            CargaDatos_Pedidos_Fechas(fecha_ini, fecha_fin);
        }

        private void cbbEstadoPedido_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvPedidos.Rows.Clear();
            CargaDatos_Pedidos_Estados(cbbEstadoPedido.Text);
        }

        public void CargaDatos_Pedidos_Estados(string estado)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT * FROM " + com + ".PEDIDOS WHERE ESTADO_ORDEN = '" + estado + "'";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvPedidos.Rows.Add();

                    dgvPedidos.Rows[n].Cells[0].Value = item["ORDERID"].ToString();
                    dgvPedidos.Rows[n].Cells[1].Value = item["CLIENTE"].ToString();
                    dgvPedidos.Rows[n].Cells[2].Value = item["IDENTIFICACION"].ToString();
                    dgvPedidos.Rows[n].Cells[3].Value = item["CORREO"].ToString();
                    dgvPedidos.Rows[n].Cells[4].Value = item["TELEFONO_FIJO"].ToString();
                    dgvPedidos.Rows[n].Cells[5].Value = item["TELEFONO_MOVIL"].ToString();
                    dgvPedidos.Rows[n].Cells[6].Value = item["ES_ANONIMO"].ToString();
                    dgvPedidos.Rows[n].Cells[7].Value = item["MONTO_IMPUESTOS"].ToString();
                    dgvPedidos.Rows[n].Cells[8].Value = item["FECHA_ORDEN"].ToString();
                    dgvPedidos.Rows[n].Cells[9].Value = item["ORDERGIFTPOINTSUSED"].ToString();
                    dgvPedidos.Rows[n].Cells[10].Value = item["ESTADO_ORDEN"].ToString();
                    dgvPedidos.Rows[n].Cells[11].Value = item["MONEDA"].ToString();
                    dgvPedidos.Rows[n].Cells[12].Value = item["OBSERVACIONES"].ToString();
                    dgvPedidos.Rows[n].Cells[13].Value = item["CODIGO_AUTORIZACION"].ToString();
                    dgvPedidos.Rows[n].Cells[14].Value = item["IP_ORIGEN"].ToString();
                    dgvPedidos.Rows[n].Cells[15].Value = item["ESTADO_PAGO"].ToString();
                    dgvPedidos.Rows[n].Cells[16].Value = item["MEDIO_PAGO"].ToString();
                    dgvPedidos.Rows[n].Cells[17].Value = item["TOTAL"].ToString();
                    dgvPedidos.Rows[n].Cells[18].Value = item["CUPONUSADO"].ToString();
                    dgvPedidos.Rows[n].Cells[19].Value = item["CUPONTIPO"].ToString();
                    dgvPedidos.Rows[n].Cells[20].Value = item["SUCURSAL"].ToString();
                    dgvPedidos.Rows[n].Cells[21].Value = item["RECOGER_SUCURSAL"].ToString();
                    dgvPedidos.Rows[n].Cells[22].Value = item["CODIGO_METODO_PAGO"].ToString();
                    dgvPedidos.Rows[n].Cells[23].Value = item["MONEDA_FE"].ToString();
                    //NUEVOS
                    dgvPedidos.Rows[n].Cells[24].Value = item["TIPO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[25].Value = item["COSTO_TOTAL_SHIPPING"].ToString();
                    dgvPedidos.Rows[n].Cells[26].Value = item["TASA_IMPUESTO_SHIPPING"].ToString();

                    //campos de envio
                    dgvPedidos.Rows[n].Cells[27].Value = item["NOMBRE_DESTINATARIO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[28].Value = item["IDENTIFICACION_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[29].Value = item["TIPO_IDENTIFICACION_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[30].Value = item["CORREO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[31].Value = item["TELEFONO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[32].Value = item["MOVIL_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[33].Value = item["PAIS_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[34].Value = item["PROVINCIA_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[35].Value = item["CANTON_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[36].Value = item["DISTRITO_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[37].Value = item["DETALLE_DIRECCION_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[38].Value = item["CIUDAD_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[39].Value = item["ZIP_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[40].Value = item["GEO_LATITUD_ENVIO"].ToString();
                    dgvPedidos.Rows[n].Cells[41].Value = item["GEO_LONGITUD_ENVIO"].ToString();
                    //campos de facturacion
                    dgvPedidos.Rows[n].Cells[42].Value = item["NOMBRE_DESTINATARIO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[43].Value = item["IDENTIFICACION_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[44].Value = item["TIPO_IDENTIFICACION_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[45].Value = item["CORREO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[46].Value = item["TELEFONO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[47].Value = item["MOVIL_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[48].Value = item["PAIS_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[49].Value = item["PROVINCIA_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[50].Value = item["CANTON_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[51].Value = item["DISTRITO_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[52].Value = item["DETALLE_DIRECCION_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[53].Value = item["CIUDAD_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[54].Value = item["ZIP_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[55].Value = item["GEO_LATITUD_FAC"].ToString();
                    dgvPedidos.Rows[n].Cells[56].Value = item["GEO_LONGITUD_FAC"].ToString();

                    dgvPedidos.Rows[n].Cells[57].Value = item["CONSECUTIVO"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void dgvPedidos_DoubleClick(object sender, EventArgs e)
        {
            string orden = dgvPedidos.SelectedCells[0].Value.ToString();
            Pedido_Linea pl = new Pedido_Linea(orden);
            try
            {
                pl.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnFiltrarPedidos_Click(object sender, EventArgs e)
        {
            frm_filtro_pedidos pedidos = new frm_filtro_pedidos();
            pedidos.ShowDialog();
            if (pedidos.n == 1)
            {
                filtrarPedidos();
            }
        }

        public void filtrarPedidos()
        {
            try
            {
                List<Clases.filtro_pedidos> filtroTabla = new List<Clases.filtro_pedidos>();
                Metodos.met_filtro_pedidos met_pedidos = new Metodos.met_filtro_pedidos();

                // Guarda en el objeto filtroTabla los datos traidos desde BD
                filtroTabla = met_pedidos.obtenerFiltroPedidos();

                // Obtiene la cantidad de columnas que contiene el datagridview (dgvPedidos)
                var numColumns = dgvPedidos.Columns.Count;

                // Muestra todas las columnas del datagridview (dgvPedidos)
                for (int i = 0; i < numColumns; i++)
                {
                    dgvPedidos.Columns[i].Visible = true;
                }


                #region OCULTA COLUMNAS

                // Consulta si el valor traido de BD es 0 (falso) y siendo asi oculta la columna
                if (filtroTabla[0].numero_orden == 0) { dgvPedidos.Columns[0].Visible = false; }
                if (filtroTabla[0].nombre_cliente == 0) { dgvPedidos.Columns[1].Visible = false; }
                if (filtroTabla[0].identificacion == 0) { dgvPedidos.Columns[2].Visible = false; }
                if (filtroTabla[0].correo_cliente == 0) { dgvPedidos.Columns[3].Visible = false; }
                if (filtroTabla[0].tel_fijo == 0) { dgvPedidos.Columns[4].Visible = false; }
                if (filtroTabla[0].tel_movil == 0) { dgvPedidos.Columns[5].Visible = false; }
                if (filtroTabla[0].comprador_anonimo == 0) { dgvPedidos.Columns[6].Visible = false; }
                if (filtroTabla[0].monto_impuesto == 0) { dgvPedidos.Columns[7].Visible = false; }
                if (filtroTabla[0].fecha == 0) { dgvPedidos.Columns[8].Visible = false; }
                if (filtroTabla[0].order_gif == 0) { dgvPedidos.Columns[9].Visible = false; }

                if (filtroTabla[0].estado_orden == 0) { dgvPedidos.Columns[10].Visible = false; }
                if (filtroTabla[0].moneda == 0) { dgvPedidos.Columns[11].Visible = false; }
                if (filtroTabla[0].observaciones == 0) { dgvPedidos.Columns[12].Visible = false; }
                if (filtroTabla[0].cod_autorizacion == 0) { dgvPedidos.Columns[13].Visible = false; }
                if (filtroTabla[0].ip_origen == 0) { dgvPedidos.Columns[14].Visible = false; }
                if (filtroTabla[0].estado_pago == 0) { dgvPedidos.Columns[15].Visible = false; }
                if (filtroTabla[0].medio_pago == 0) { dgvPedidos.Columns[16].Visible = false; }
                if (filtroTabla[0].total_orden == 0) { dgvPedidos.Columns[17].Visible = false; }
                if (filtroTabla[0].uso_cupon == 0) { dgvPedidos.Columns[18].Visible = false; }
                if (filtroTabla[0].tipo_cupon == 0) { dgvPedidos.Columns[19].Visible = false; }

                if (filtroTabla[0].sucursal == 0) { dgvPedidos.Columns[20].Visible = false; }
                if (filtroTabla[0].recoger_sucursal == 0) { dgvPedidos.Columns[21].Visible = false; }
                if (filtroTabla[0].metodo_pago == 0) { dgvPedidos.Columns[22].Visible = false; }
                if (filtroTabla[0].moneda_facturacion == 0) { dgvPedidos.Columns[23].Visible = false; }
                if (filtroTabla[0].tipo_envio == 0) { dgvPedidos.Columns[24].Visible = false; }
                if (filtroTabla[0].costo_envio == 0) { dgvPedidos.Columns[25].Visible = false; }
                if (filtroTabla[0].costo_impuesto_envio == 0) { dgvPedidos.Columns[26].Visible = false; }
                if (filtroTabla[0].nombre_destinatario == 0) { dgvPedidos.Columns[27].Visible = false; }
                if (filtroTabla[0].identificacion_envio == 0) { dgvPedidos.Columns[28].Visible = false; }
                if (filtroTabla[0].tipo_identi_envio == 0) { dgvPedidos.Columns[29].Visible = false; }

                if (filtroTabla[0].correo_envio == 0) { dgvPedidos.Columns[30].Visible = false; }
                if (filtroTabla[0].telefono_envio == 0) { dgvPedidos.Columns[31].Visible = false; }
                if (filtroTabla[0].tel_movil_envio == 0) { dgvPedidos.Columns[32].Visible = false; }
                if (filtroTabla[0].pais_envio == 0) { dgvPedidos.Columns[33].Visible = false; }
                if (filtroTabla[0].provincia_envio == 0) { dgvPedidos.Columns[34].Visible = false; }
                if (filtroTabla[0].canton_envio == 0) { dgvPedidos.Columns[35].Visible = false; }
                if (filtroTabla[0].distrito_envio == 0) { dgvPedidos.Columns[36].Visible = false; }
                if (filtroTabla[0].detalle_direccion_envio == 0) { dgvPedidos.Columns[37].Visible = false; }
                if (filtroTabla[0].ciudad_envio == 0) { dgvPedidos.Columns[38].Visible = false; }
                if (filtroTabla[0].codigo_zip_envio == 0) { dgvPedidos.Columns[39].Visible = false; }

                if (filtroTabla[0].posicion_latitud == 0) { dgvPedidos.Columns[40].Visible = false; }
                if (filtroTabla[0].posicion_longitud == 0) { dgvPedidos.Columns[41].Visible = false; }
                if (filtroTabla[0].nombre_destinatario_fac == 0) { dgvPedidos.Columns[42].Visible = false; }
                if (filtroTabla[0].identificacion_fac == 0) { dgvPedidos.Columns[43].Visible = false; }
                if (filtroTabla[0].tipo_id_fac == 0) { dgvPedidos.Columns[44].Visible = false; }
                if (filtroTabla[0].correo_fac == 0) { dgvPedidos.Columns[45].Visible = false; }
                if (filtroTabla[0].telefono_fac == 0) { dgvPedidos.Columns[46].Visible = false; }
                if (filtroTabla[0].tel_movil_fac == 0) { dgvPedidos.Columns[47].Visible = false; }
                if (filtroTabla[0].pais_fac == 0) { dgvPedidos.Columns[48].Visible = false; }
                if (filtroTabla[0].provincia_fac == 0) { dgvPedidos.Columns[49].Visible = false; }

                if (filtroTabla[0].canton_fac == 0) { dgvPedidos.Columns[50].Visible = false; }
                if (filtroTabla[0].distrito_fac == 0) { dgvPedidos.Columns[51].Visible = false; }
                if (filtroTabla[0].detalle_dir_fac == 0) { dgvPedidos.Columns[52].Visible = false; }
                if (filtroTabla[0].ciudad_fac == 0) { dgvPedidos.Columns[53].Visible = false; }
                if (filtroTabla[0].codigo_zip_fac == 0) { dgvPedidos.Columns[54].Visible = false; }
                if (filtroTabla[0].posicion_latitud_fac == 0) { dgvPedidos.Columns[55].Visible = false; }
                if (filtroTabla[0].posicion_longitud_fac == 0) { dgvPedidos.Columns[56].Visible = false; }

                #endregion OCULTA COLUMNAS

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /*BEBEMUNDO*/
        private void btnPedidos_Oracle_Click(object sender, EventArgs e)
        {
            Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
            string token = obj_metodos.Obtener_Token();

            //primero insertamos los pedidos de nidux en tablas propias
            insertar_pedidos_app(token);

            //entonces leemos los pedidos que no tenga el consecutivo lleno para insertarlos
            DataTable dtPedidos = GetData("SELECT ORDERID, CLIENTE, IDENTIFICACION, TELEFONO_MOVIL, CORREO, FECHA_ORDEN, CODIGO_AUTORIZACION, WISH_ID, CASE WHEN RECOGER_SUCURSAL IS NULL THEN CONCAT(DETALLE_DIRECCION_ENVIO, ',', PROVINCIA_ENVIO) ELSE RECOGER_SUCURSAL END AS DIRECCION  FROM " + com + ".PEDIDOS WHERE CONSECUTIVO IS NULL");

            //revisamos si hay pedidos
            if (dtPedidos.Rows.Count > 0)
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cnn.open();
                OracleTransaction myTrans;
                myTrans = cnn.o_Conexion.BeginTransaction();
                cmd.Transaction = myTrans;

                try
                {
                    foreach (DataRow dtrEncabezado in dtPedidos.Rows)
                    {
                        string ID_WISH = "";
                        ID_WISH = dtrEncabezado["WISH_ID"].ToString();
                        #region "Encabezado"
                        //insertamos el encabezado
                        string query_enca = "NAF5M.nidux_PVECOM.REG_PVENC_ECOM";
                        cmd.CommandText = query_enca;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("pNO_TRANSA_MOV", OracleType.VarChar, 12).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("pNOMBRE_CLIENTE", OracleType.VarChar, 60).Value = dtrEncabezado["CLIENTE"].ToString();
                        cmd.Parameters.Add("pTELEFONO", OracleType.VarChar, 20).Value = dtrEncabezado["TELEFONO_MOVIL"].ToString();
                        cmd.Parameters.Add("pEMAIL", OracleType.VarChar, 60).Value = dtrEncabezado["CORREO"].ToString();
                        cmd.Parameters.Add("pCEDULA", OracleType.VarChar, 20).Value = dtrEncabezado["IDENTIFICACION"].ToString();

                        DateTime fecha = Convert.ToDateTime(dtrEncabezado["FECHA_ORDEN"]);
                        string fecha_slash = fecha.ToString("yyyy/MM/dd");
                        string fecha_final = fecha_slash.Replace("/", "");

                        cmd.Parameters.Add("pFECHA_REG", OracleType.VarChar, 10).Value = fecha_final;//yyyymmdd
                        cmd.Parameters.Add("pREFERENCIA", OracleType.VarChar, 15).Value = dtrEncabezado["ORDERID"].ToString();//numero de pedido en nidux
                        cmd.Parameters.Add("pLISTAREG", OracleType.VarChar, 15).Value = dtrEncabezado["WISH_ID"].ToString();//se pasa cero si no hay lista asociada
                        cmd.Parameters.Add("pcodigo_web", OracleType.VarChar, 15).Value = 1;  //SI ES CEMACO 1 Y SI ES BEBEMUNDO ENVIO 2
                        cmd.Parameters.Add("pMENSAJE", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        string no_transa = cmd.Parameters["pNO_TRANSA_MOV"].Value.ToString();
                        string respuesta = cmd.Parameters["pMENSAJE"].Value.ToString();

                        cmd.Parameters.Clear();

                        if (respuesta == "")
                        {
                            DataTable dtPedido_linea = GetData("SELECT SKU, CANTIDAD, PRECIO FROM " + com + ".PEDIDOS_LINEA WHERE ORDERID = " + dtrEncabezado["ORDERID"].ToString());
                            //insertamos las lineas
                            foreach (DataRow dtrLinea in dtPedido_linea.Rows)
                            {
                                string query_linea = "NAF5M.nidux_PVECOM.REG_PVDET_ECOM";
                                cmd.CommandText = query_linea;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("pNO_TRANSA_MOV", OracleType.VarChar, 12).Value = no_transa;
                                cmd.Parameters.Add("pNO_ARTI", OracleType.VarChar, 60).Value = dtrLinea["SKU"].ToString();
                                cmd.Parameters.Add("pCANTIDAD", OracleType.Number, 10).Value = Convert.ToInt32(dtrLinea["CANTIDAD"].ToString());
                                string precio = dtrLinea["PRECIO"].ToString().Replace('.', ',');
                                cmd.Parameters.Add("pPRECIO", OracleType.Number, 20).Value = Convert.ToDecimal(precio);
                                cmd.Parameters.Add("pMENSAJE", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }

                            //insertamos en la nota final
                            string query = "NAF5M.nidux_PVECOM.FIN_PEDIDO";
                            cmd.CommandText = query;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("pNO_TRANSA_MOV", OracleType.VarChar, 12).Value = no_transa;
                            cmd.Parameters.Add("pESTADO", OracleType.VarChar, 10).Value = "A";// para facturar
                            string codigo = dtrEncabezado["CODIGO_AUTORIZACION"].ToString();
                            cmd.Parameters.Add("PAUTORIZACION", OracleType.VarChar, 10).Value = codigo;//codigo de autorizacion nidux "codigo_autorizacion"
                            string direccion = dtrEncabezado["DIRECCION"].ToString();
                            cmd.Parameters.Add("PDIRECCION", OracleType.VarChar, 255).Value = direccion;
                            cmd.Parameters.Add("pMENSAJE", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            string respuesta2 = cmd.Parameters["pMENSAJE"].Value.ToString();
                            cmd.Parameters.Clear();

                            if (respuesta2 == "")
                            {
                                //completamos la inserccion del pedido en codisa
                                myTrans.Commit();

                                //si todo se completa tenemos que actualizar el estado en las tablas para que despues actualizarlo en Nidux
                                //actualizar el estado y el consecutivo
                                string query_pedido = "UPDATE " + com + ".PEDIDOS SET ESTADO_ORDEN = 'En Proceso', CONSECUTIVO = '" + no_transa + "' where ORDERID = " + dtrEncabezado["ORDERID"].ToString();
                                SqlCommand cmd_sql = new SqlCommand();
                                cmd_sql.Connection = cnn.AbrirConexion();
                                cmd_sql.CommandText = query_pedido;
                                cmd_sql.CommandTimeout = 0;
                                cmd_sql.CommandType = CommandType.Text;
                                cmd_sql.ExecuteNonQuery();
                                cnn.CerrarConexion();
                            }
                            else
                            {
                                MessageBox.Show("Ocurrio un error a la hora de insertar el pedido en detalle final: " + dtrEncabezado["ORDERID"].ToString() +
                                ", Mensaje: " + respuesta2, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                myTrans.Rollback();
                            }


                        }
                        else
                        {
                            //error en el pedido encabezado
                            MessageBox.Show("Ocurrio un error a la hora de insertar el pedido: " + dtrEncabezado["ORDERID"].ToString() +
                                ", Mensaje: " + respuesta, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            myTrans.Rollback();
                        }
                        #endregion

                        #region "Listas de regalo"

                        if (!string.IsNullOrEmpty(ID_WISH))
                        {

                            //post lISTA DE REGALOS

                            var client = new RestClient("https://api.nidux.dev/v3/giftlists/" + dtrEncabezado["WISH_ID"].ToString());
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            request.AddHeader("Authorization", "Bearer " + token);
                            request.AddHeader("Content-Type", "application/json");
                            IRestResponse responseListaRegalo = client.Execute(request);

                            //string urlBaseNidux = "https://api.nidux.dev/";
                            //var urlGetListaRegalo = new RestClient(urlBaseNidux);
                            //var requestGetListaRegalo = new RestRequest("v2/giftlists/" + dtrEncabezado["WISH_ID"].ToString()).AddHeader("Authorization", "Bearer " + token);
                            //var responseListaRegalo = await urlGetListaRegalo.GetAsync(requestGetListaRegalo); //aqui se me esta pegando

                            ListaRegalos respuestaListaRegalos = new ListaRegalos();

                            if (responseListaRegalo.StatusCode == System.Net.HttpStatusCode.OK)
                            {


                                respuestaListaRegalos = JsonConvert.DeserializeObject<ListaRegalos>(responseListaRegalo.Content);

                                int contProductos = 0;
                                int contadorLineaListaRegalo = 1;
                                foreach (var item in respuestaListaRegalos.Giftlist.Products.Available)
                                {
                                    //insersion a oracle de la lista
                                    string query = "NAF5M.NIDUX_LISTA_REGALOS.REG_LISTA_REGALOS";
                                    cmd.CommandText = query;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("pno_lista", OracleType.VarChar, 8).Value = Convert.ToString(respuestaListaRegalos.Giftlist.wish_id);
                                    cmd.Parameters.Add("pno_linea", OracleType.Number, 10).Value = contadorLineaListaRegalo;//Convert.ToInt32(respuestaListaRegalos.Giftlist.ListName);
                                    cmd.Parameters.Add("pno_arti", OracleType.VarChar, 15).Value = respuestaListaRegalos.Giftlist.Products.Available[contProductos].product_code;
                                    cmd.Parameters.Add("pcantidad", OracleType.Number, 10).Value = respuestaListaRegalos.Giftlist.Products.Available[contProductos].Stock;
                                    cmd.Parameters.Add("pmensaje", OracleType.VarChar, 255).Direction = ParameterDirection.Output;
                                    cmd.ExecuteNonQuery();
                                    string respuesta2 = cmd.Parameters["pmensaje"].Value.ToString();
                                    cmd.Parameters.Clear();

                                    //logsFile.WriteLogs($"LISTAS REGALO---- WISHID {respuestaListaRegalos.Giftlist.wish_id} ListName: {respuestaListaRegalos.Giftlist.ListName} ProductName: {respuestaListaRegalos.Giftlist.Products.Available[contProductos].ProductName} Stock: {respuestaListaRegalos.Giftlist.Products.Available[contProductos].Stock}");

                                    contProductos++;
                                    contadorLineaListaRegalo++;
                                }

                                #region "Consulto cliente"

                                token = obj_metodos.Obtener_Token();
                                //Consulto direccion cliente 
                                var client2 = new RestClient("https://api.nidux.dev/v3/customers/" + respuestaListaRegalos.Giftlist.OwnerId.ToString());
                                client2.Timeout = -1;
                                var request2 = new RestRequest(Method.GET);
                                request2.AddHeader("Authorization", "Bearer " + token);
                                request2.AddHeader("Content-Type", "application/json");
                                IRestResponse responseGetCliente = client2.Execute(request2);

                                //var urlGetCliente = new RestClient(urlBaseNidux);
                                //var requestGetCliente = new RestRequest("v2/customers/" + respuestaListaRegalos.Giftlist.OwnerId.ToString()).AddHeader("Authorization", "Bearer " + token);
                                //var responseGetCliente = await urlGetCliente.GetAsync(requestGetCliente);


                                if (responseGetCliente.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var direcciones = @"""direcciones"":[]";
                                    var direccionesNull = @"""direcciones"":null";

                                    var Json = responseGetCliente.Content;
                                    Json = Json.Replace(direcciones, direccionesNull);

                                    Cliente respuestaListaClientes = JsonConvert.DeserializeObject<Cliente>(Json);
                                    string telefono = string.Empty;
                                    DateTime dt = new DateTime();
                                    DateTime dt2 = new DateTime();
                                    dt = Convert.ToDateTime(respuestaListaClientes.Customer.creado);
                                    dt2 = Convert.ToDateTime(respuestaListaRegalos.Giftlist.eventDate);

                                    #region validamos si telefono es vacio 
                                    if (string.IsNullOrEmpty(respuestaListaClientes.Customer.Telefono1))
                                    {
                                        telefono = respuestaListaClientes.Customer.Telefono2;
                                    }
                                    else if (string.IsNullOrEmpty(respuestaListaClientes.Customer.Telefono1) && string.IsNullOrEmpty(respuestaListaClientes.Customer.Telefono2))
                                    {
                                        telefono = "0";
                                    }
                                    else
                                    {
                                        telefono = respuestaListaClientes.Customer.Telefono1;
                                    }
                                    #endregion

                                    //INSERTAR CLIENTE 
                                    string query3 = "NAF5M.NIDUX_LISTA_REGALOS.REG_CLIENTE";
                                    cmd.CommandText = query3;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("pid_identificacion", OracleType.VarChar, 20).Value = Convert.ToString(respuestaListaClientes.Customer.Identificacion);
                                    cmd.Parameters.Add("pnombre", OracleType.VarChar, 60).Value = respuestaListaClientes.Customer.Nombre;
                                    if (respuestaListaClientes.Customer.Direcciones == null)
                                    {
                                        cmd.Parameters.Add("pdireccion", OracleType.VarChar, 240).Value = "ND";
                                    }
                                    else
                                    {
                                        cmd.Parameters.Add("pdireccion", OracleType.VarChar, 240).Value = respuestaListaClientes.Customer.Direcciones[0].Detalle;
                                    }
                                    cmd.Parameters.Add("ptelefono", OracleType.VarChar, 20).Value = telefono;
                                    cmd.Parameters.Add("pemail", OracleType.VarChar, 120).Value = respuestaListaClientes.Customer.Correo;
                                    cmd.Parameters.Add("pgenero", OracleType.VarChar, 1).Value = respuestaListaClientes.Customer.Genero;
                                    cmd.Parameters.Add("pfnacimiento", OracleType.DateTime, 30).Value = Convert.ToDateTime(respuestaListaClientes.Customer.fecha_de_nacimiento);
                                    cmd.Parameters.Add("pmensaje", OracleType.VarChar, 255).Direction = ParameterDirection.Output;
                                    cmd.ExecuteNonQuery();
                                    string respuesta3 = cmd.Parameters["pmensaje"].Value.ToString();
                                    cmd.Parameters.Clear();

                                    //logsFile.WriteLogs($"REG CLIENTE-- Nombre: {respuestaListaClientes.Customer.Nombre} Detalle: {respuestaListaClientes.Customer.Direcciones} Telefono {telefono} Correo: {respuestaListaClientes.Customer.Correo} Genero: {respuestaListaClientes.Customer.Genero} FechaDeNacimiento: {respuestaListaClientes.Customer.fecha_de_nacimiento}");

                                    //INSERTAR EVENTO
                                    string query4 = "NAF5M.NIDUX_LISTA_REGALOS.REG_EVENTO";
                                    cmd.CommandText = query4;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("pno_lista", OracleType.VarChar, 8).Value = Convert.ToString(respuestaListaRegalos.Giftlist.wish_id);
                                    cmd.Parameters.Add("pcod_cliente1", OracleType.VarChar, 15).Value = Convert.ToString(respuestaListaClientes.Customer.id);
                                    cmd.Parameters.Add("pcod_cliente2", OracleType.VarChar, 15).Value = Convert.ToString(respuestaListaRegalos.Giftlist.extraOwnerId);
                                    cmd.Parameters.Add("pfecha_crea", OracleType.DateTime, 30).Value = respuestaListaClientes.Customer.creado;
                                    cmd.Parameters.Add("ptipo_lista", OracleType.VarChar, 2).Value = Convert.ToString(respuestaListaRegalos.Giftlist.modeList);
                                    cmd.Parameters.Add("pfecha_evento", OracleType.DateTime, 30).Value = respuestaListaRegalos.Giftlist.eventDate;
                                    cmd.Parameters.Add("ptipo_envoltura", OracleType.VarChar, 1).Value = Convert.ToString(respuestaListaRegalos.Giftlist.EcoFriendly.id);
                                    cmd.Parameters.Add("pmensaje", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
                                    cmd.ExecuteNonQuery();
                                    string respuesta4 = cmd.Parameters["pmensaje"].Value.ToString();
                                    cmd.Parameters.Clear();

                                    //logsFile.WriteLogs($"REG_EVENTO-- WishId: {respuestaListaRegalos.Giftlist.wish_id} ID: {respuestaListaClientes.Customer.id} ExtraOwnerId: {respuestaListaRegalos.Giftlist.extraOwnerId} Creado: {respuestaListaClientes.Customer.creado} ListType: {respuestaListaRegalos.Giftlist.ListType} EventDate: {respuestaListaRegalos.Giftlist.eventDate}");

                                }
                                else
                                {
                                    MessageBox.Show("Error al consultar Cliente:" + respuestaListaRegalos.Giftlist.OwnerId + ", Status Code: " + responseGetCliente.StatusCode.ToString() + ", Mensaje Error: " + responseGetCliente.ErrorMessage);
                                }
                                #endregion


                            }
                            else
                            {
                                MessageBox.Show("Error al consultar lista de regalo: " + respuestaListaRegalos.Giftlist.wish_id + ", Status Code: " + responseListaRegalo.StatusCode.ToString() + ", Mensaje Error: " + responseListaRegalo.ErrorMessage);
                            }
                        }
                        #endregion
                    }

                    //actualizamos el estado en Nidux de los pedidos
                    actualizar_pedidos_app();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    myTrans.Rollback();
                }
            }
            else
            {
                MessageBox.Show("No hay pedidos por sincronizar", "Mensaje de Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void insertar_pedidos_app(string token)
        {
            try
            {

                //empezamos la sincronizacion de las categorias desde el api de aplix a Nidux
                if (token.Equals("Error en login"))
                {
                    MessageBox.Show("Error en el login", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var client = new RestClient("https://api.nidux.dev/v3/orders/");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", "{\r\n   \"pagina\":1,\r\n   \"cantidad_ordenes\":100,\r\n   \"estado_orden\" : 0\r\n}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

                    if ((int)response.StatusCode == 200)
                    {
                        string json_ped_eliminados = "";

                        var Location = AppDomain.CurrentDomain.BaseDirectory;
                        var envio = @"""envio"":[]";
                        var envioNUll = @"""envio"":null";

                        var facturacion = @"""facturacion"":[]";
                        var facturacionNull = @"""facturacion"":null";

                        var Json = response.Content;
                        Json = Json.Replace(envio, envioNUll).Replace(facturacion, facturacionNull);

                        var fact = JsonConvert.DeserializeObject<Clases.Pedidos>(Json);

                        //var a = fact.ordenes.data.Where(x => x.orderId == 5898).ToList();
                        //fact.ordenes.data = a;

                        int contador_ped = 0;
                        int cantidad_pedidos = fact.ordenes.data.Count;

                        if (fact.ordenes.data.Count > 0)
                        {
                            while (contador_ped < cantidad_pedidos)
                            {
                                if (fact.ordenes.data[contador_ped].estado_pago != "Pagado")
                                {

                                    fact.ordenes.data.RemoveAt(contador_ped);
                                    cantidad_pedidos--;
                                }
                                else
                                {

                                    contador_ped++;
                                }
                            }

                            int count_data = 0;
                            Conexion con = new Conexion();
                            while (count_data < fact.ordenes.data.Count)
                            {
                                try
                                {
                                    //id_error = pedido.ordenes.data[count_data].orderId;
                                    int count_detalle_sp = 0;
                                    //insertamos el encabezado del pedido
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.Connection = con.AbrirConexion();
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandText = "" + com + ".AGREGAR_PEDIDOS";
                                    cmd.Parameters.AddWithValue("@orderId", ((object)fact.ordenes.data[count_data].orderId) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@wish_id", ((object)fact.ordenes.data[count_data].wish_id) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@cliente", ((object)fact.ordenes.data[count_data].cliente) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@identificacion", ((object)fact.ordenes.data[count_data].identificacion) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@correo", ((object)fact.ordenes.data[count_data].correo) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@telefono_fijo", ((object)fact.ordenes.data[count_data].telefono_fijo) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@telefono_movil", ((object)fact.ordenes.data[count_data].telefono_movil) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@es_anonimo", ((object)fact.ordenes.data[count_data].es_anonimo) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@monto_impuestos", ((object)fact.ordenes.data[count_data].monto_impuestos) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@fecha_orden", ((object)fact.ordenes.data[count_data].fecha_orden) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@orderGiftpointsUsed", ((object)fact.ordenes.data[count_data].orderGiftpointsUsed) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@estado_orden", ((object)fact.ordenes.data[count_data].estado_orden) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@moneda", ((object)fact.ordenes.data[count_data].moneda) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@observaciones", ((object)fact.ordenes.data[count_data].observaciones) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@codigo_autorizacion", ((object)fact.ordenes.data[count_data].codigo_autorizacion) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@ip_origen", ((object)fact.ordenes.data[count_data].ip_origen) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@estado_pago", ((object)fact.ordenes.data[count_data].estado_pago) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@medio_pago", ((object)fact.ordenes.data[count_data].medio_pago) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@total", ((object)fact.ordenes.data[count_data].total) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@cuponUsado", ((object)fact.ordenes.data[count_data].cuponUsado) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@cuponTipo", ((object)fact.ordenes.data[count_data].cuponTipo) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@sucursal", ((object)fact.ordenes.data[count_data].sucursal) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@recoger_sucursal", ((object)fact.ordenes.data[count_data].recoger_sucursal) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@codigo_metodo_pago", ((object)fact.ordenes.data[count_data].codigo_metodo_pago) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@moneda_fe", ((object)fact.ordenes.data[count_data].moneda_fe) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@tipo_envio", ((object)fact.ordenes.data[count_data].tipo_envio) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@costo_total_shipping", ((object)fact.ordenes.data[count_data].costo_total_shipping) ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@tasa_impuesto_shipping", ((object)fact.ordenes.data[count_data].tasa_impuesto_shipping) ?? DBNull.Value);
                                    //datos del envio
                                    if (fact.ordenes.data[count_data].direcciones.envio == null)
                                    {
                                        cmd.Parameters.AddWithValue("@nombre_destinatario_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@identificacion_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@tipo_identificacion_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@correo_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@telefono_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@movil_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@pais_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@provincia_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@canton_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@distrito_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@detalle_direccion_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@ciudad_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@zip_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@geo_latitud_envio", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@geo_longitud_envio", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@nombre_destinatario_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.nombre_destinatario) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@identificacion_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.identificacion) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@tipo_identificacion_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.tipo_identificacion) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@correo_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.correo) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@telefono_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.telefono) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@movil_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.movil) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@pais_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.pais) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@provincia_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.provincia) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@canton_envio", DBNull.Value);//nidux no vanda estos valores
                                        cmd.Parameters.AddWithValue("@distrito_envio", DBNull.Value);//nidux no vanda estos valores
                                        cmd.Parameters.AddWithValue("@detalle_direccion_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.detalle_direccion) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@ciudad_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.ciudad) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@zip_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.zip) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@geo_latitud_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.geo_latitud) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@geo_longitud_envio", ((object)fact.ordenes.data[count_data].direcciones.envio.geo_longitud) ?? DBNull.Value);
                                    }
                                    //datos facturacion
                                    if (fact.ordenes.data[count_data].direcciones.facturacion == null)
                                    {
                                        cmd.Parameters.AddWithValue("@nombre_destinatario_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@identificacion_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@tipo_identificacion_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@correo_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@telefono_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@movil_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@pais_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@provincia_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@canton_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@distrito_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@detalle_direccion_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@ciudad_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@zip_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@geo_latitud_fac", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@geo_longitud_fac", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@nombre_destinatario_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.nombre_destinatario) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@identificacion_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.identificacion) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@tipo_identificacion_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.tipo_identificacion) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@correo_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.correo) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@telefono_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.telefono) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@movil_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.movil) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@pais_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.pais) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@provincia_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.provincia_fe) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@canton_fac", DBNull.Value);//nidux no vanda estos valores
                                        cmd.Parameters.AddWithValue("@distrito_fac", DBNull.Value);//nidux no vanda estos valores
                                        cmd.Parameters.AddWithValue("@detalle_direccion_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.detalle_direccion) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@ciudad_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.ciudad) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@zip_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.zip) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@geo_latitud_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.geo_latitud) ?? DBNull.Value);
                                        cmd.Parameters.AddWithValue("@geo_longitud_fac", ((object)fact.ordenes.data[count_data].direcciones.facturacion.geo_longitud) ?? DBNull.Value);
                                    }
                                    //con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.CerrarConexion();

                                    while (count_detalle_sp < fact.ordenes.data[count_data].detalles.Count)
                                    {
                                        /*llamo al sp de lineas pedido*/
                                        SqlCommand cmd2 = new SqlCommand();
                                        cmd2.Connection = con.AbrirConexion();
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.CommandText = "" + com + ".AGREGAR_PEDIDOS_LINEA";
                                        cmd2.Parameters.AddWithValue("@orderId", ((object)fact.ordenes.data[count_data].orderId) ?? DBNull.Value);
                                        cmd2.Parameters.AddWithValue("@id_producto", ((object)fact.ordenes.data[count_data].detalles[count_detalle_sp].id_producto) ?? DBNull.Value);
                                        cmd2.Parameters.AddWithValue("@id_variacion", ((object)fact.ordenes.data[count_data].detalles[count_detalle_sp].id_variacion) ?? DBNull.Value);
                                        cmd2.Parameters.AddWithValue("@sku", ((object)fact.ordenes.data[count_data].detalles[count_detalle_sp].sku) ?? DBNull.Value);
                                        cmd2.Parameters.AddWithValue("@nombre_producto", ((object)fact.ordenes.data[count_data].detalles[count_detalle_sp].nombre_producto) ?? DBNull.Value);
                                        cmd2.Parameters.AddWithValue("@precio", ((object)fact.ordenes.data[count_data].detalles[count_detalle_sp].precio) ?? DBNull.Value);
                                        cmd2.Parameters.AddWithValue("@cantidad", ((object)fact.ordenes.data[count_data].detalles[count_detalle_sp].cantidad) ?? DBNull.Value);
                                        cmd2.Parameters.AddWithValue("@porcentaje_descuento", ((object)fact.ordenes.data[count_data].detalles[count_detalle_sp].porcentaje_descuento) ?? DBNull.Value);
                                        cmd2.Parameters.AddWithValue("@subtotal_descuento", ((object)fact.ordenes.data[count_data].detalles[count_detalle_sp].subtotal_descuento) ?? DBNull.Value);
                                        cmd2.Parameters.AddWithValue("@subtotal_linea", ((object)fact.ordenes.data[count_data].detalles[count_detalle_sp].subtotal_linea) ?? DBNull.Value);
                                        cmd2.Parameters.AddWithValue("@impuesto", ((object)fact.ordenes.data[count_data].detalles[count_detalle_sp].impuesto) ?? DBNull.Value);
                                        cmd2.Parameters.AddWithValue("@subtotal_impuestos", ((object)fact.ordenes.data[count_data].detalles[count_detalle_sp].subtotal_impuestos) ?? DBNull.Value);
                                        //con.Open();
                                        cmd2.ExecuteNonQuery();
                                        con.CerrarConexion();
                                        count_detalle_sp++;
                                    }
                                    //id_last = fact.ordenes.data[count_data].orderId;
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show($"Error: {e.Message}");
                                    con.CerrarConexion();
                                    //lista_error.Add("Fallo en el pedido numero: " + id_error.ToString() + ", Fecha error: " + day.ToString("MM/dd/yy HH:mm:ss"));
                                }
                                finally
                                {
                                    con.CerrarConexion();
                                }
                                count_data++;
                            }


                            #region Insertar pedidos por api propia
                            //json_ped_eliminados = JsonConvert.SerializeObject(fact, Formatting.Indented);

                            //var client1 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/insertar_pedidos");
                            //client1.Timeout = -1;
                            //var request1 = new RestRequest(Method.POST);
                            //request1.AddParameter("application/json", json_ped_eliminados, ParameterType.RequestBody);
                            //IRestResponse response1 = client1.Execute(request1);

                            //if ((int)response1.StatusCode == 200)
                            //{
                            //    Clases.Respuesta m = JsonConvert.DeserializeObject<Clases.Respuesta>(response1.Content);
                            //    if (m.error.Count > 0)
                            //    {
                            //        int contador = 0;
                            //        while (contador < m.error.Count)
                            //        {
                            //            MessageBox.Show("Metodo de Obtener ordenes por filtro de Nidux: " + m.error[contador].ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //            contador++;
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    MessageBox.Show("Error al obtener los pedidos de Nidux: " + response1.Content, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //}
                            #endregion
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al obtener los pedidos de Nidux: " + response.Content, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los pedidos de Nidux: " + ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //CAMBIA EL ESTADO EN NIDUX DEL PEDIDO
        public void actualizar_pedidos_app()
        {
            Metodos.LoginNidux obj_metodos = new Metodos.LoginNidux();
            string token = obj_metodos.Obtener_Token();
            if (token.Equals("Error en login"))
            {
                MessageBox.Show("Error en el login", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                List<Clases.Estados> lista = new List<Clases.Estados>();
                int contador1 = 0;
                try
                {
                    var client1 = new RestClient("http://" + ConfigurationManager.AppSettings["Ip"].ToString() + "/api/actualizar_estado_pedido");//revisar esto para ver si tengo que hacer algo antes
                    client1.Timeout = -1;
                    var request1 = new RestRequest(Method.GET);
                    IRestResponse response1 = client1.Execute(request1);

                    if ((int)response1.StatusCode == 200)
                    {
                        lista = JsonConvert.DeserializeObject<List<Clases.Estados>>(response1.Content);

                        if (lista.Count > 0)
                        {

                            while (contador1 < lista.Count)
                            {
                                var client = new RestClient("https://api.nidux.dev/v3/orders/" + lista[contador1].orderId.ToString() + "/orderStatus");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.PUT);
                                request.AddHeader("Authorization", "Bearer " + token);
                                request.AddHeader("Content-Type", "application/json");
                                request.AddParameter("application/json", "{\r\n        \"nuevo_estado\": " + lista[contador1].nuevo_estado.ToString() + "\r\n}", ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);

                                if ((int)response.StatusCode == 200)
                                {

                                }
                                else
                                {
                                    MessageBox.Show("Error en actualizar estado de Pedido: " + lista[contador1].orderId.ToString() +
                                        ", Error: " + response.ErrorMessage, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                contador1++;
                            }
                        }
                    }
                    else
                    {
                        //tsw5.WriteLine("\n" + "Error al actualizar el estado del pedido: " + lista[contador1].orderId + " " + response1.Content + " " + thisDay.ToString("MM / dd / yy H: mm:ss"));
                        MessageBox.Show("Error en obtener pedidos para actualizar estado en Nidux, " + response1.ErrorMessage, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en Actualizar Estado de Pedidos: " + ex.Message.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
        /*------------------------ EXCEL -----------------------------------*/
        //bebemundo

        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            string ruta = "";

            DialogResult result =
            MessageBox.Show("¿Desea generar una plantilla con su cátalogo de articulos? ",
            "◄ Generar plantilla de catalogo en Excel ►", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                using (ExcelPackage excelPackage = new ExcelPackage())
                {

                    try
                    {

                        saveFileDialog2.Filter = "Execl files (*.xlsx)|*.xlsx";
                        saveFileDialog2.FilterIndex = 0;
                        saveFileDialog2.RestoreDirectory = true;


                        if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                        {

                            ruta = saveFileDialog2.FileNames[0];


                            //Le añadimos los 'worksheets' que necesitemos son como la cantidad de hojas
                            //estan definirse al inicio para que luego no den error al seleccionar con cual trabajar
                            excelPackage.Workbook.Worksheets.Add("Articulos");
                            //excelPackage.Workbook.Worksheets.Add("Variaciones");
                            excelPackage.Workbook.Worksheets.Add("Categorías");
                            excelPackage.Workbook.Worksheets.Add("Marcas");
                            excelPackage.Workbook.Worksheets.Add("Atributos");
                            //excelPackage.Workbook.Worksheets.Add("Estados");



                            //Agregamos los articulos
                            ExcelWorksheet ew1 = excelPackage.Workbook.Worksheets[1];
                            fun_agregar_articulos_plantilla_excel(ew1);


                            ////Agregamos las variaciones
                            //ExcelWorksheet ew2 = excelPackage.Workbook.Worksheets[2];
                            //fun_agregar_variaciones_plantilla_excel(ew2);


                            //Consumimos el api para obtener las categorias generados en el excel
                            Metodos.Metodos_de_Excel metodos_categorias = new Metodos.Metodos_de_Excel();
                            List<Clases.CategoriasExcel> lista_categorias_excel = new List<Clases.CategoriasExcel>();
                            lista_categorias_excel = metodos_categorias.obtener_datos_de_categorias_excel();

                            if (lista_categorias_excel.Count > 0)
                            {
                                ExcelWorksheet ew3 = excelPackage.Workbook.Worksheets[2];
                                fun_agregar_categorias_excel(ew3, lista_categorias_excel);
                            }



                            Metodos.Metodos_de_Excel metodos_marcas = new Metodos.Metodos_de_Excel();
                            List<Clases.MarcasExcel> lista_marcas_excel = new List<Clases.MarcasExcel>();
                            lista_marcas_excel = metodos_marcas.obtener_datos_de_marcas_excel();


                            if (lista_marcas_excel.Count > 0)
                            {
                                ExcelWorksheet ew4 = excelPackage.Workbook.Worksheets[3];
                                fun_agregar_marcas_excel(ew4, lista_marcas_excel);
                            }


                            Metodos.Metodos_de_Excel metodos_atributos = new Metodos.Metodos_de_Excel();
                            List<Clases.AtributosExcel> lista_atributos = new List<Clases.AtributosExcel>();
                            lista_atributos = metodos_atributos.obtener_datos_de_atributos_excel();


                            if (lista_atributos.Count > 0)
                            {
                                ExcelWorksheet ew4 = excelPackage.Workbook.Worksheets[4];
                                fun_agregar_atributos_excel(ew4, lista_atributos);
                            }


                            //Metodos.Metodos_de_Excel metodos_estados = new Metodos.Metodos_de_Excel();
                            //List<Clases.EstadosExcel> lista_estados = new List<Clases.EstadosExcel>();
                            //lista_estados = metodos_estados.obtener_datos_de_estados_excel();


                            //if (lista_estados.Count > 0)
                            //{
                            //    ExcelWorksheet ew5 = excelPackage.Workbook.Worksheets[6];
                            //    fun_agregar_estados_excel(ew5, lista_estados);
                            //}

                            //Save your file
                            FileInfo fi = new FileInfo(@ruta);
                            excelPackage.SaveAs(fi);
                            MessageBox.Show("El arvhivo se guardó en la ruta:" + "\n"
                            + ruta, "Generación se archivo", MessageBoxButtons.OK, MessageBoxIcon.Information);



                        } // cierra el try

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message.ToString());
                    }

                }

            }
        }

        int GetLastUsedRow(ExcelWorksheet sheet)
        {
            var row = sheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }

        public void fun_agregar_articulos_plantilla_excel(ExcelWorksheet ew1)
        {


            int filas = 2;


            try
            {

                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#F19800");
                //Se agregan primero los encabezados
                ew1.Cells[1, 1].Value = "Sku Articulo";
                // Aplicar estilo al tipo de letra
                ew1.Cells[1, 1].Style.Font.Bold = true;
                ew1.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 2].Value = "Nombre ERP";
                ew1.Cells[1, 2].Style.Font.Bold = true;
                ew1.Cells[1, 2].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 3].Value = "Nombre Nidux";
                ew1.Cells[1, 3].Style.Font.Bold = true;
                ew1.Cells[1, 3].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 4].Value = "Descripcion Nidux";
                ew1.Cells[1, 4].Style.Font.Bold = true;
                ew1.Cells[1, 4].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 5].Value = "Sku Padre";
                ew1.Cells[1, 5].Style.Font.Bold = true;
                ew1.Cells[1, 5].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 5].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 6].Value = "Atributos";
                ew1.Cells[1, 6].Style.Font.Bold = true;
                ew1.Cells[1, 6].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 6].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 7].Value = "Marca";
                ew1.Cells[1, 7].Style.Font.Bold = true;
                ew1.Cells[1, 7].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 7].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 8].Value = "Categorías";
                ew1.Cells[1, 8].Style.Font.Bold = true;
                ew1.Cells[1, 8].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 8].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 9].Value = "Estado";
                ew1.Cells[1, 9].Style.Font.Bold = true;
                ew1.Cells[1, 9].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 9].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 10].Value = "Tags";
                ew1.Cells[1, 10].Style.Font.Bold = true;
                ew1.Cells[1, 10].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 10].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 11].Value = "Seo Tags";
                ew1.Cells[1, 11].Style.Font.Bold = true;
                ew1.Cells[1, 11].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 11].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 12].Value = "Sincroniza";
                ew1.Cells[1, 12].Style.Font.Bold = true;
                ew1.Cells[1, 12].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 12].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 13].Value = "Indicador de Stock";
                ew1.Cells[1, 13].Style.Font.Bold = true;
                ew1.Cells[1, 13].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 13].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 14].Value = "Destacar Articulo";
                ew1.Cells[1, 14].Style.Font.Bold = true;
                ew1.Cells[1, 14].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 14].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 15].Value = "Costo Shipping";
                ew1.Cells[1, 15].Style.Font.Bold = true;
                ew1.Cells[1, 15].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 15].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 15].Style.Fill.BackgroundColor.SetColor(colFromHex);


                ew1.Cells[1, 16].Value = "Permite Reserva";
                ew1.Cells[1, 16].Style.Font.Bold = true;
                ew1.Cells[1, 16].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 16].Style.Fill.BackgroundColor.SetColor(colFromHex);


                ew1.Cells[1, 17].Value = "Porcentaje Reserva";
                ew1.Cells[1, 17].Style.Font.Bold = true;
                ew1.Cells[1, 17].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 17].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 18].Value = "Límite de Carrito";
                ew1.Cells[1, 18].Style.Font.Bold = true;
                ew1.Cells[1, 18].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 18].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 19].Value = "Usar Gif";
                ew1.Cells[1, 19].Style.Font.Bold = true;
                ew1.Cells[1, 19].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 19].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 19].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 20].Value = "Tiempo Gif";
                ew1.Cells[1, 20].Style.Font.Bold = true;
                ew1.Cells[1, 20].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 20].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 21].Value = "Video YouTube";
                ew1.Cells[1, 21].Style.Font.Bold = true;
                ew1.Cells[1, 21].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 21].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 21].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 22].Value = "Nombre de Traducción";
                ew1.Cells[1, 22].Style.Font.Bold = true;
                ew1.Cells[1, 22].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 22].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 22].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 23].Value = "Descripción de Traducción";
                ew1.Cells[1, 23].Style.Font.Bold = true;
                ew1.Cells[1, 23].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 23].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 23].Style.Fill.BackgroundColor.SetColor(colFromHex);

                foreach (DataGridViewRow row in dgvArticulos.SelectedRows)
                {

                    string sku = "";
                    string nombre_erp = "";
                    string nombre_nidux = "";
                    string descripcion_nidux = "";
                    string sku_padre = "";
                    string atributos = "";
                    string id_marca = "";
                    string categorias = "";
                    string estado = "";
                    string tags = "";
                    string seo_tags = "";
                    string sincroniza = "";
                    string indicador_stock = "";
                    string destacar_articulo = "";
                    string costo_shipping = "";
                    string permite_reserva = "";
                    string porcentaje_reserva = "";
                    string limite_carrito = "";
                    string usar_gif = "";
                    string tiempo_gif = "";
                    string video = "";
                    string nombre_traduccion = "";
                    string descripcion_traduccion = "";



                    if (row.Cells["articulo"].Value != null)
                    {
                        sku = row.Cells["articulo"].Value.ToString();
                    }

                    if (row.Cells["nombre"].Value != null)
                    {
                        nombre_erp = row.Cells["nombre"].Value.ToString();
                    }

                    if (row.Cells["NOMBRE_NIDUX"].Value != null)
                    {
                        nombre_nidux = row.Cells["NOMBRE_NIDUX"].Value.ToString();
                    }

                    if (row.Cells["DESCRIPCION_NIDUX"].Value != null)
                    {
                        descripcion_nidux = row.Cells["DESCRIPCION_NIDUX"].Value.ToString();
                    }

                    if (row.Cells["padre"].Value != null)
                    {
                        sku_padre = row.Cells["padre"].Value.ToString();
                    }

                    if (row.Cells["atributos"].Value != null)
                    {
                        atributos = row.Cells["atributos"].Value.ToString();
                    }

                    if (row.Cells["id_marca"].Value != null)
                    {
                        id_marca = row.Cells["id_marca"].Value.ToString();
                    }

                    if (row.Cells["categorias"].Value != null)
                    {
                        categorias = row.Cells["categorias"].Value.ToString();
                    }

                    if (row.Cells["estado"].Value != null)
                    {
                        estado = row.Cells["estado"].Value.ToString();
                    }

                    if (row.Cells["tags"].Value != null)
                    {
                        tags = row.Cells["tags"].Value.ToString();
                    }

                    if (row.Cells["seotags"].Value != null)
                    {
                        seo_tags = row.Cells["seotags"].Value.ToString();
                    }

                    if (row.Cells["activo"].Value != null)
                    {
                        sincroniza = row.Cells["activo"].Value.ToString();
                    }

                    if (row.Cells["indicador"].Value != null)
                    {
                        indicador_stock = row.Cells["indicador"].Value.ToString();
                    }

                    if (row.Cells["destacado"].Value != null)
                    {
                        destacar_articulo = row.Cells["destacado"].Value.ToString();
                    }

                    if (row.Cells["shipping"].Value != null)
                    {
                        costo_shipping = row.Cells["shipping"].Value.ToString();
                    }

                    if (row.Cells["reserva"].Value != null)
                    {
                        permite_reserva = row.Cells["reserva"].Value.ToString();
                    }

                    if (row.Cells["porcentaje"].Value != null)
                    {
                        porcentaje_reserva = row.Cells["porcentaje"].Value.ToString();
                    }

                    if (row.Cells["carrito"].Value != null)
                    {
                        limite_carrito = row.Cells["carrito"].Value.ToString();
                    }

                    if (row.Cells["gif"].Value != null)
                    {
                        usar_gif = row.Cells["gif"].Value.ToString();
                    }

                    if (row.Cells["tiempo_gif"].Value != null)
                    {
                        tiempo_gif = row.Cells["tiempo_gif"].Value.ToString();
                    }

                    if (row.Cells["video"].Value != null)
                    {
                        video = row.Cells["video"].Value.ToString();
                    }

                    if (row.Cells["nombre_tra"].Value != null)
                    {
                        nombre_traduccion = row.Cells["nombre_tra"].Value.ToString();
                    }

                    if (row.Cells["descrip_traduc"].Value != null)
                    {
                        descripcion_traduccion = row.Cells["descrip_traduc"].Value.ToString();
                    }


                    Metodos.Metodos_de_Excel met_esta = new Metodos.Metodos_de_Excel();

                    List<Clases.EstadosExcel> lis = new List<Clases.EstadosExcel>();
                    lis = met_esta.obtener_datos_de_estados_excel();
                    int estados = 0;

                    var unitmeasure = ew1.DataValidations.AddListValidation("I" + filas.ToString());

                    while (estados < lis.Count)
                    {

                        unitmeasure.Formula.Values.Add(lis[estados].descripcion);
                        estados++;

                    }

                    if (sincroniza == "True") { sincroniza = "S"; } else { sincroniza = "N"; }
                    if (indicador_stock == "True") { indicador_stock = "S"; } else { indicador_stock = "N"; }
                    if (destacar_articulo == "True") { destacar_articulo = "S"; } else { destacar_articulo = "N"; }
                    if (permite_reserva == "True") { permite_reserva = "S"; } else { permite_reserva = "N"; }
                    if (usar_gif == "True") { usar_gif = "S"; } else { usar_gif = "N"; }
                    if (estado == "" || estado == " ") { estado = "En Stock"; }

                    ew1.Cells[filas, 1].Value = sku;
                    ew1.Cells[filas, 2].Value = nombre_erp;
                    ew1.Cells[filas, 3].Value = nombre_nidux;
                    ew1.Cells[filas, 4].Value = descripcion_nidux;
                    ew1.Cells[filas, 5].Value = sku_padre;
                    ew1.Cells[filas, 6].Value = atributos;
                    ew1.Cells[filas, 7].Value = id_marca;
                    ew1.Cells[filas, 8].Value = categorias;
                    ew1.Cells[filas, 9].Value = estado;
                    ew1.Cells[filas, 10].Value = tags;
                    ew1.Cells[filas, 11].Value = seo_tags;
                    ew1.Cells[filas, 12].Value = sincroniza;
                    ew1.Cells[filas, 13].Value = indicador_stock;
                    ew1.Cells[filas, 14].Value = destacar_articulo;
                    ew1.Cells[filas, 15].Value = costo_shipping;
                    ew1.Cells[filas, 16].Value = permite_reserva;
                    ew1.Cells[filas, 17].Value = porcentaje_reserva;
                    ew1.Cells[filas, 18].Value = limite_carrito;
                    ew1.Cells[filas, 19].Value = usar_gif;
                    ew1.Cells[filas, 20].Value = tiempo_gif;
                    ew1.Cells[filas, 21].Value = video;
                    ew1.Cells[filas, 22].Value = nombre_traduccion;
                    ew1.Cells[filas, 23].Value = descripcion_traduccion;



                    filas++;
                }


                //while (cont_articulos < 1)
                //{

                //    ew1.Cells[filas, 6].Value = "0";
                //    ew1.Cells[filas, 7].Value = "S";
                //    ew1.Cells[filas, 8].Value = "N";
                //    ew1.Cells[filas, 9].Value = "N";



                //    ew1.Cells[filas, 10].Value = "En Stock";
                //    ew1.Cells[filas, 11].Value = "0";
                //    ew1.Cells[filas, 12].Value = "1";
                //    ew1.Cells[filas, 13].Value = "N";
                //    ew1.Cells[filas, 14].Value = "120";
                //    ew1.Cells[filas, 18].Value = "S";
                //    ew1.Cells[filas, 19].Value = " ";

                //    cont_articulos++;
                //    filas++;

                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                //cont_articulos++;
                //filas++;
            }

        }

        public void fun_agregar_variaciones_plantilla_excel(ExcelWorksheet ew1)
        {


            int filas = 2;
            int cont_categorias = 0;

            try
            {

                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#F19800");
                //Se agregan primero los encabezados
                ew1.Cells[1, 1].Value = "Sku principal";
                // Aplicar estilo al tipo de letra
                ew1.Cells[1, 1].Style.Font.Bold = true;
                ew1.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 2].Value = "Sku Variacion";
                ew1.Cells[1, 2].Style.Font.Bold = true;
                ew1.Cells[1, 2].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 3].Value = "Atributos";
                ew1.Cells[1, 3].Style.Font.Bold = true;
                ew1.Cells[1, 3].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 4].Value = "Sincroniza";
                ew1.Cells[1, 4].Style.Font.Bold = true;
                ew1.Cells[1, 4].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(colFromHex);


                while (cont_categorias < 1)
                {



                    ew1.Cells[filas, 4].Value = "S";
                    ew1.Cells[filas, 12].Value = " ";

                    cont_categorias++;
                    filas++;

                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                cont_categorias++;
                filas++;
            }


        }

        public void fun_agregar_categorias_excel(ExcelWorksheet ew1, List<Clases.CategoriasExcel> lista)
        {


            int filas = 2;
            int cont_categorias = 0;

            try
            {

                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#F19800");
                //Se agregan primero los encabezados
                ew1.Cells[1, 1].Value = "Id Categoria";
                // Aplicar estilo al tipo de letra
                ew1.Cells[1, 1].Style.Font.Bold = true;
                ew1.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(colFromHex);


                ew1.Cells[1, 2].Value = "Nombre";
                ew1.Cells[1, 2].Style.Font.Bold = true;
                ew1.Cells[1, 2].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 3].Value = "Padre";
                ew1.Cells[1, 3].Style.Font.Bold = true;
                ew1.Cells[1, 3].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(colFromHex);


                while (cont_categorias < lista.Count)
                {

                    ew1.Cells[filas, 1].Value = lista[cont_categorias].codigo;
                    ew1.Cells[filas, 2].Value = lista[cont_categorias].nombre;
                    ew1.Cells[filas, 3].Value = lista[cont_categorias].padre;

                    ew1.Cells[filas, 4].Value = " ";

                    cont_categorias++;
                    filas++;

                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                cont_categorias++;
                filas++;
            }


        }

        public void fun_agregar_marcas_excel(ExcelWorksheet ew1, List<Clases.MarcasExcel> lista)
        {


            int filas = 2;
            int cont_categorias = 0;

            try
            {

                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#F19800");
                //Se agregan primero los encabezados
                ew1.Cells[1, 1].Value = "Id Marca";
                // Aplicar estilo al tipo de letra
                ew1.Cells[1, 1].Style.Font.Bold = true;
                ew1.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(colFromHex);


                ew1.Cells[1, 2].Value = "Nombre";
                ew1.Cells[1, 2].Style.Font.Bold = true;
                ew1.Cells[1, 2].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(colFromHex);



                while (cont_categorias < lista.Count)
                {

                    ew1.Cells[filas, 1].Value = lista[cont_categorias].codigo;
                    ew1.Cells[filas, 2].Value = lista[cont_categorias].nombre;
                    ew1.Cells[filas, 3].Value = " ";

                    cont_categorias++;
                    filas++;

                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                cont_categorias++;
                filas++;
            }


        }

        public void fun_agregar_atributos_excel(ExcelWorksheet ew1, List<Clases.AtributosExcel> lista)
        {


            int filas = 2;
            int cont_categorias = 0;

            try
            {

                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#F19800");
                //Se agregan primero los encabezados
                ew1.Cells[1, 1].Value = "Id Atributo";
                // Aplicar estilo al tipo de letra
                ew1.Cells[1, 1].Style.Font.Bold = true;
                ew1.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(colFromHex);


                ew1.Cells[1, 2].Value = "Nombre";
                ew1.Cells[1, 2].Style.Font.Bold = true;
                ew1.Cells[1, 2].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(colFromHex);

                ew1.Cells[1, 3].Value = "Tipo Atributo";
                ew1.Cells[1, 3].Style.Font.Bold = true;
                ew1.Cells[1, 3].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(colFromHex);


                while (cont_categorias < lista.Count)
                {

                    ew1.Cells[filas, 1].Value = lista[cont_categorias].codigo;
                    ew1.Cells[filas, 2].Value = lista[cont_categorias].nombre;
                    ew1.Cells[filas, 3].Value = lista[cont_categorias].atributo;

                    ew1.Cells[filas, 4].Value = " ";

                    cont_categorias++;
                    filas++;

                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                cont_categorias++;
                filas++;
            }


        }

        public void fun_agregar_estados_excel(ExcelWorksheet ew1, List<Clases.EstadosExcel> lista)
        {


            int filas = 2;
            int cont_categorias = 0;

            try
            {

                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#F19800");
                //Se agregan primero los encabezados
                ew1.Cells[1, 1].Value = "Id Estado";
                // Aplicar estilo al tipo de letra
                ew1.Cells[1, 1].Style.Font.Bold = true;
                ew1.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(colFromHex);


                ew1.Cells[1, 2].Value = "Nombre";
                ew1.Cells[1, 2].Style.Font.Bold = true;
                ew1.Cells[1, 2].Style.Font.Color.SetColor(Color.White);
                ew1.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ew1.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(colFromHex);

                while (cont_categorias < lista.Count)
                {

                    ew1.Cells[filas, 1].Value = lista[cont_categorias].id;
                    ew1.Cells[filas, 2].Value = lista[cont_categorias].descripcion;

                    ew1.Cells[filas, 3].Value = " ";

                    cont_categorias++;
                    filas++;

                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                cont_categorias++;
                filas++;
            }

        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            string ruta = "";

            List<string> lista_carga_errores_variaciones = new List<string>();
            List<string> lista_carga_errores_articulos = new List<string>();

            //try
            //{
            //    Metodos.Metodos_de_Excel met_carga = new Metodos.Metodos_de_Excel();
            //    met_carga.emparejar_tablas_erp_articulos();
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message.ToString());
            //}




            using (ExcelPackage paquete = new ExcelPackage())
            {

                bool bandera_padres = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    ruta = openFileDialog1.FileName.ToString();


                    // Creamos un flujo a partir del archivo de Excel, y lo cargamos en el paquete
                    using (FileStream flujo = File.OpenRead(ruta))
                    {
                        paquete.Load(flujo);
                    }

                    // Obtenemos la primera hoja del documento
                    ExcelWorksheet hoja1 = paquete.Workbook.Worksheets[1];

                    //MessageBox.Show(GetLastUsedRow(hoja1).ToString());
                    int rows = (GetLastUsedRow(hoja1) + 1);

                    // Empezamos a leer a partir de la segunda fila
                    for (int numFila = 2; numFila < rows; numFila++)
                    {
                        // Obtenemos el valor de la celda de la primera columna, como texto

                        try
                        {
                            Clases.ArticulosExcel obj = new Clases.ArticulosExcel();
                            Metodos.Metodos_de_Excel met = new Metodos.Metodos_de_Excel();

                            string sku = hoja1.Cells[numFila, 1].Text;
                            //limpiamos de comas finales el campo de categorias
                            string categorias = hoja1.Cells[numFila, 8].Text.ToString().TrimEnd(',');
                            //;ipia,os de comas finales el campo de marcas
                            string marcas = hoja1.Cells[numFila, 7].Text.ToString().TrimEnd(',');


                            if (sku == "" || sku.Length <= 0 || sku == "NULL" || sku == "null" || sku == "Null")
                            {

                                lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " el  campo articulo se encuentra vacío");

                            }
                            else
                            {


                                //Elimina cualquier valor que no sea de tipo numerico o de una ,
                                categorias = Regex.Replace(categorias, "[^0-9 + ,]", "", RegexOptions.None);

                                //Elimina cualquier valor que no sea de tipo numerico
                                marcas = Regex.Replace(marcas, "[^0-9]", "", RegexOptions.None);

                                bool bandera = true;
                                bool bandera_marcas = true;
                                bandera_padres = true;

                                Metodos.Metodos_de_Excel met_articulos = new Metodos.Metodos_de_Excel();

                                obj.sku = hoja1.Cells[numFila, 1].Text;
                                obj.nombre_nidux = hoja1.Cells[numFila, 3].Text;
                                obj.descripcion_nidux = hoja1.Cells[numFila, 4].Text;
                                obj.marca = marcas;
                                obj.categoria = categorias;

                                obj.porcentaje_reserva = Regex.Replace(hoja1.Cells[numFila, 17].Text, "[^0-9]", "", RegexOptions.None);

                                if (obj.porcentaje_reserva == "" || obj.porcentaje_reserva == null)
                                {

                                    lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " el  campo porcentaje de reserva se encuentra vacío por lo tanto su valor por defecto fue ingresado en 0");
                                    obj.porcentaje_reserva = "0";
                                }


                                if (hoja1.Cells[numFila, 16].Text == "S" || hoja1.Cells[numFila, 16].Text == "N")
                                {

                                    obj.permite_reserva = hoja1.Cells[numFila, 16].Text;

                                }
                                else
                                {

                                    lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " el  campo permite reserva tiene un valor diferente de N o S por lo tanto su valor por defecto fue ingresado en S");
                                    obj.permite_reserva = "N";
                                }


                                if (hoja1.Cells[numFila, 14].Text == "S" || hoja1.Cells[numFila, 14].Text == "N")
                                {
                                    obj.destacado = hoja1.Cells[numFila, 14].Text;

                                }
                                else
                                {
                                    lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " el  campo destacado tiene un valor diferente de N o S por lo tanto su valor por defecto fue ingresado en N");
                                    obj.destacado = "N";
                                }


                                if (hoja1.Cells[numFila, 13].Text == "S" || hoja1.Cells[numFila, 13].Text == "N")
                                {
                                    obj.indicador_stock = hoja1.Cells[numFila, 13].Text;

                                }
                                else
                                {
                                    lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " el campo indicador de stock tiene un valor diferente de N o S por lo tanto su valor por defecto fue ingresado en N");
                                    obj.indicador_stock = "N";
                                }


                                if (hoja1.Cells[numFila, 9].Text == "Oculto" || hoja1.Cells[numFila, 9].Text == "Sin Stock" || hoja1.Cells[numFila, 9].Text == "En Stock" || hoja1.Cells[numFila, 9].Text == "Contrapedido" || hoja1.Cells[numFila, 9].Text == "Preventa" || hoja1.Cells[numFila, 9].Text == "Servicio")
                                {
                                    if (hoja1.Cells[numFila, 9].Text == "Oculto") { obj.estado = "1"; }
                                    if (hoja1.Cells[numFila, 9].Text == "Sin Stock") { obj.estado = "2"; }
                                    if (hoja1.Cells[numFila, 9].Text == "En Stock") { obj.estado = "3"; }
                                    if (hoja1.Cells[numFila, 9].Text == "Contrapedido") { obj.estado = "4"; }
                                    if (hoja1.Cells[numFila, 9].Text == "Preventa") { obj.estado = "5"; }
                                    if (hoja1.Cells[numFila, 9].Text == "Servicio") { obj.estado = "6"; }

                                }
                                else
                                {
                                    lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " el  campo estado tiene un valor diferente que no coincide con los codigos de estados por lo tanto su valor por defecto fue ingresado en 3 - 'En Stock'");
                                    obj.estado = "3";
                                }


                                obj.costo_shipping = Regex.Replace(hoja1.Cells[numFila, 15].Text, "[^0-9]", "", RegexOptions.None);

                                if (obj.costo_shipping == "" || obj.costo_shipping == null)
                                {
                                    lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " el  campo de costo shipping tiene un valor vacío por lo tanto su valor por defecto fue ingresado en 0");
                                    obj.costo_shipping = "0";
                                }


                                obj.limite_carrito = Regex.Replace(hoja1.Cells[numFila, 18].Text, "[^0-9]", "", RegexOptions.None);

                                if (obj.limite_carrito == "" || obj.limite_carrito == null)
                                {
                                    lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " el campo limite carrito tiene un valor vacío por lo tanto su valor por defecto fue ingresado en 1");
                                    obj.limite_carrito = "1";
                                }


                                if (hoja1.Cells[numFila, 19].Text == "S" || hoja1.Cells[numFila, 19].Text == "N")
                                {
                                    obj.usar_gif = hoja1.Cells[numFila, 19].Text;

                                }
                                else
                                {
                                    lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " el  campo usar gif tiene un valor diferente de N o S por lo tanto su valor por defecto fue ingresado en N");
                                    obj.usar_gif = "N";
                                }


                                obj.tiempo_gif = Regex.Replace(hoja1.Cells[numFila, 20].Text, "[^0-9]", "", RegexOptions.None);

                                if (obj.tiempo_gif == "" || obj.tiempo_gif == null)
                                {
                                    lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " el  campo tiempo gif tiene un valor vacío por lo tanto su valor por defecto fue ingresado en 120");
                                    obj.tiempo_gif = "120";
                                }

                                obj.video_youtube = hoja1.Cells[numFila, 21].Text;
                                obj.nombre_traduccion = hoja1.Cells[numFila, 22].Text;
                                obj.descripcion_traduccion = hoja1.Cells[numFila, 23].Text;

                                if (hoja1.Cells[numFila, 12].Text == "S" || hoja1.Cells[numFila, 12].Text == "N")
                                {
                                    obj.sincroniza = hoja1.Cells[numFila, 12].Text;

                                }
                                else
                                {
                                    lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " el  campo sincroniza tiene un valor diferente de N o S por lo tanto su valor por defecto fue ingresado en S");
                                    obj.sincroniza = "N";
                                }

                                obj.tags = hoja1.Cells[numFila, 10].Text;

                                obj.seo_tags = hoja1.Cells[numFila, 11].Text;

                                string atributos = hoja1.Cells[numFila, 6].Text;

                                atributos = Regex.Replace(atributos, "[^0-9 + ,]", "", RegexOptions.None);

                                obj.padre = hoja1.Cells[numFila, 5].Text;

                                obj.atributos = atributos;

                                string skusp = obj.sku;
                                int filap = numFila;

                                if (obj.padre != "" && obj.atributos == "")
                                {
                                    bandera_padres = false;
                                }


                                Metodos.Metodos_de_Excel met_validar_categorias = new Metodos.Metodos_de_Excel();
                                bandera = met_validar_categorias.validar_categorias_excel(obj);

                                Metodos.Metodos_de_Excel met_validar_marcas = new Metodos.Metodos_de_Excel();
                                bandera_marcas = met_validar_marcas.validar_marcas_excel(obj);

                                if (bandera == false)
                                {

                                    lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " algún código de categoria en esta fila no existe, por favor verifique que los códigos de categorias en el documento de Excel hoja de Articulos se encuentren separados por una coma (,)");

                                }
                                else
                                {

                                    if (bandera_marcas == false)
                                    {

                                        lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " algún código de marca en esta fila no existe, por favor verifique que los códigos de marcas en el documento de Excel hoja de Articulos");

                                    }
                                    else
                                    {
                                        if (bandera_padres == false)
                                        {

                                            lista_carga_errores_articulos.Add("En articulos padres la fila: " + numFila.ToString() + " Al elegir un articulo como variación deben de asignarsele atributos");

                                        }
                                        else
                                        {

                                            met.agregar_articulos_excel(obj);

                                        }

                                    }


                                }

                            }


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }


                    }



                    //// Obtenemos la segunda hoja del documento
                    //ExcelWorksheet hoja2 = paquete.Workbook.Worksheets[2];

                    //int rows2 = (GetLastUsedRow(hoja2) + 1);

                    //// Empezamos a leer a partir de la segunda fila
                    //for (int numFila2 = 2; numFila2 < rows2; numFila2++)
                    //{
                    //    // Obtenemos el valor de la celda de la primera columna, como texto

                    //    try
                    //    {
                    //        Clases.VariacionesExcel obj_atributos = new Clases.VariacionesExcel();
                    //        Metodos.Metodos_de_Excel met_atributos = new Metodos.Metodos_de_Excel();

                    //        string sku = hoja2.Cells[numFila2, 2].Text;
                    //        string padre = hoja2.Cells[numFila2, 1].Text;
                    //        string atributos = hoja2.Cells[numFila2, 3].Text.ToString().TrimEnd(',');

                    //        if (sku == "" || sku.Length <= 0 || sku == "NULL" || sku == "null" || sku == "Null")
                    //        {
                    //            lista_carga_errores_variaciones.Add("En variaciones la fila: " + numFila2.ToString() + " el campo articulo para variacion viene vacío");
                    //        }
                    //        else
                    //        {
                    //            if (padre == "" || padre.Length <= 0 || padre == "NULL" || padre == "null" || padre == "Null")
                    //            {
                    //                lista_carga_errores_variaciones.Add("En variaciones la fila: " + numFila2.ToString() + "el  campo articulo principal se encuentra vacío");
                    //            }
                    //            else
                    //            {

                    //                if (atributos == "" || atributos.Length <= 0 || atributos == "NULL" || atributos == "null" || atributos == "Null")
                    //                {
                    //                    lista_carga_errores_variaciones.Add("En variaciones la fila: " + numFila2.ToString() + "el campo atributo  se encuentra vacío");
                    //                }
                    //                else
                    //                {


                    //                    atributos = Regex.Replace(atributos, "[^0-9 + ,]", "", RegexOptions.None);


                    //                    bool bandera = true;

                    //                    Metodos.Metodos_de_Excel met_variaciones3 = new Metodos.Metodos_de_Excel();

                    //                    obj_atributos.sku_padre = hoja2.Cells[numFila2, 1].Text;
                    //                    obj_atributos.sku_variacion = hoja2.Cells[numFila2, 2].Text;
                    //                    obj_atributos.atributos = atributos;

                    //                    if (hoja2.Cells[numFila2, 4].Text == "S" || hoja2.Cells[numFila2, 4].Text == "N")
                    //                    {
                    //                        obj_atributos.sincroniza = hoja2.Cells[numFila2, 4].Text;

                    //                    }
                    //                    else
                    //                    {
                    //                        lista_carga_errores_variaciones.Add("En variaciones la fila: " + numFila2.ToString() + "el campo sincroniza tiene un valor diferente a N o S por lo tanto fue ingreado con el valor por defecto S");
                    //                        obj_atributos.sincroniza = "S";
                    //                    }


                    //                    bandera = met_variaciones3.validar_atributos_excel(obj_atributos);

                    //                    if (bandera == false)
                    //                    {

                    //                        lista_carga_errores_variaciones.Add("En variaciones la fila: " + numFila2.ToString() + " algún código de atributo en esta fila no existe, por favor verifique que los códigos de atributos en el documento de Excel hoja de Variaciones se encuentren separados por una coma (,)");

                    //                    }
                    //                    else
                    //                    {


                    //                        met_atributos.agregar_variaciones_excel(obj_atributos);

                    //                        //Al final del proceso ejecutar los procedimientos de emparejar articulos

                    //                    }



                    //                }

                    //            }
                    //        }
                    //    }
                    //    catch (Exception)
                    //    {

                    //    }


                    //}



                }

            }

            // Aqui llamamos al formulario de errores

            //try
            //{
            //    Metodos.Metodos_de_Excel met_carga_valores = new Metodos.Metodos_de_Excel();
            //    met_carga_valores.emparejar_tablas_erp_articulos_emparejar_valores();
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message.ToString());
            //}

            Errores_Excel frm = new Errores_Excel(lista_carga_errores_articulos);
            frm.ShowDialog();


        }

        private void toolStripArticulos_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //Clase para cargar el combobox de estados
        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        //Metodo que nos permite cargar las opciones del combobox de estados
        //public void cargar_estados()
        //{

        //    comboestados.Items.Clear();

        //    ComboboxItem item = new ComboboxItem();
        //    item.Text = "Sincronizados";
        //    item.Value = "1";
        //    comboestados.Items.Add(item);

        //    ComboboxItem item2 = new ComboboxItem();
        //    item2.Text = "No Sincronizados";
        //    item2.Value = "2";
        //    comboestados.Items.Add(item2);

        //    ComboboxItem item3 = new ComboboxItem();
        //    item3.Text = "3." + " Todos";
        //    item3.Value = "3";
        //    comboestados.Items.Add(item3);

        //    comboestados.SelectedIndex = 0;

        //}

        private void cbbArticulos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Buscar_Articulos(txtBuscarArticulos.Text);
        }

        private void comboestados_SelectedIndexChanged(object sender, EventArgs e)
        {
            Buscar_Articulos(txtBuscarArticulos.Text);
        }

        private void buttonBitacora_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de texto o log (*.txt;*.log)|*.txt;*.log|Todos los archivos (*.*)|*.*",
                Title = "Seleccionar archivo de bitácora",
                DefaultExt = "txt"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                rutaArchivoSeleccionado = openFileDialog.FileName;

                try
                {
                    textBitacora.Text = File.ReadAllText(rutaArchivoSeleccionado);

                    // Detener y eliminar el watcher anterior si existe
                    if (watcher != null)
                    {
                        watcher.EnableRaisingEvents = false;
                        watcher.Dispose();
                    }

                    // Crear un nuevo FileSystemWatcher
                    watcher = new FileSystemWatcher
                    {
                        Path = System.IO.Path.GetDirectoryName(rutaArchivoSeleccionado),
                        Filter = System.IO.Path.GetFileName(rutaArchivoSeleccionado),
                        NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
                    };

                    watcher.Changed += Watcher_Changed;
                    watcher.EnableRaisingEvents = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

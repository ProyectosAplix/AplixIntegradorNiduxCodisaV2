using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AplixEcommerceIntegration.Globales;
using System.Configuration;

namespace AplixEcommerceIntegration.Nidux
{
    public partial class Pedido_Linea : Form
    {
        Conexion cnn = new Conexion();
        static string numOrden;
        static string com = ConfigurationManager.AppSettings["Company_Nidux"];

        public Pedido_Linea(string orden)
        {
            InitializeComponent();
            numOrden = orden;
        }

        private void Pedido_Linea_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn.AbrirConexion();
                cmd.CommandText = "SELECT * FROM " + com + ".PEDIDOS_LINEA WHERE ORDERID = '" + numOrden + "'";
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.CerrarConexion();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvPedidoLinea.Rows.Add();

                    dgvPedidoLinea.Rows[n].Cells[0].Value = item["ORDERID"].ToString();
                    dgvPedidoLinea.Rows[n].Cells[1].Value = item["ID_PRODUCTO"].ToString();
                    dgvPedidoLinea.Rows[n].Cells[2].Value = item["ID_VARIACION"].ToString();
                    dgvPedidoLinea.Rows[n].Cells[3].Value = item["SKU"].ToString();
                    dgvPedidoLinea.Rows[n].Cells[4].Value = item["NOMBRE_PRODUCTO"].ToString();
                    dgvPedidoLinea.Rows[n].Cells[5].Value = item["PRECIO"].ToString();
                    dgvPedidoLinea.Rows[n].Cells[6].Value = item["CANTIDAD"].ToString();
                    dgvPedidoLinea.Rows[n].Cells[7].Value = item["PORCENTAJE_DESCUENTO"].ToString();
                    dgvPedidoLinea.Rows[n].Cells[8].Value = item["SUBTOTAL_DESCUENTO"].ToString();
                    dgvPedidoLinea.Rows[n].Cells[9].Value = item["SUBTOTAL_LINEA"].ToString();
                    dgvPedidoLinea.Rows[n].Cells[10].Value = item["IMPUESTO"].ToString();
                    dgvPedidoLinea.Rows[n].Cells[11].Value = item["SUBTOTAL_IMPUESTOS"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}

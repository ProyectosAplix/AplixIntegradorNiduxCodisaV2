using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AplixEcommerceIntegration.Shopify.Metodos;

namespace AplixEcommerceIntegration.Shopify
{
    public partial class MostrarPedidoLinea : Form
    {
        Metodos_Pedidos mtp = new Metodos_Pedidos();
        private string orden;
        DataTable dtPedidoLinea;
        public MostrarPedidoLinea(string orden_number)
        {
            InitializeComponent();
            orden = orden_number;
        }

        public void cargaPedidoLinea()
        {
            dtPedidoLinea = mtp.pedidoLinea(orden);
            if (dtPedidoLinea == null)
            {
                MessageBox.Show("Error en Procedimiento de Obtener Pedido Linea", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dtPedidoLinea.Rows.Count == 0)
                {
                    MessageBox.Show("No hay lineas para mostrar", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //datos normales de articulos
                    foreach (DataRow item in dtPedidoLinea.Rows)
                    {
                        int n = dgvPedidoLinea.Rows.Add();

                        dgvPedidoLinea.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[1].Value = item["ORDER_NUMBER"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[2].Value = item["PRODUCT_ID"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[3].Value = item["NAME"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[4].Value = item["PRICE"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[5].Value = item["PRODUCT_EXISTS"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[6].Value = item["QUANTITY"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[7].Value = item["REQUIRES_SHIPPING"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[8].Value = item["SKU"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[9].Value = item["TAXABLE"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[10].Value = item["TOTAL_DISCOUNT"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[11].Value = item["VARIANT_ID"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[12].Value = item["VARIANT_INVENTORY_MANAGEMENT"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[13].Value = item["VARIANT_TITLE"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[14].Value = item["VENDOR"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[15].Value = item["TAX_LINE_PRICE"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[16].Value = item["TAX_LINE_RATE"].ToString();
                        dgvPedidoLinea.Rows[n].Cells[17].Value = item["TAX_LINE_TITLE"].ToString();

                    }
                }
            }
        }

        private void MostrarPedidoLinea_Load(object sender, EventArgs e)
        {
            cargaPedidoLinea();
        }
    }
}

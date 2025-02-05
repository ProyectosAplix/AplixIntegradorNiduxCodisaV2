using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplixEcommerceIntegration.WooCommerce
{
    public partial class Pedido_Linea : Form
    {

        static int id_ped;
      
        public Pedido_Linea(int pedido)
        {
           
            InitializeComponent();
            id_ped = pedido;
            met_cargar_lineas_de_pedido();
            met_cargar_lineas_de_envio();
        }

        private void Pedido_Linea_Load(object sender, EventArgs e)
        {
        }


        public void met_cargar_lineas_de_pedido()
        {
            dt_lineas.Rows.Clear();
            Metodos.MetodosPedidosWoocomerce met_atributos = new Metodos.MetodosPedidosWoocomerce();
            DataTable v_tabla = new DataTable();
            Clases.PedidosWoocomerce obj_pedido = new Clases.PedidosWoocomerce();
            obj_pedido.id = id_ped;
            v_tabla = met_atributos.mostrar_lineas_pedidos(obj_pedido);

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
               
                string id_pedido = v_tabla.Rows[i]["ID_PEDIDO"].ToString();
                string id = v_tabla.Rows[i]["ID"].ToString();
                string name = v_tabla.Rows[i]["NAME"].ToString();
                string product_id = v_tabla.Rows[i]["PRODUCT_ID"].ToString();
                string variation_id = v_tabla.Rows[i]["VARIATION_ID"].ToString();
                string quantity = v_tabla.Rows[i]["QUANTITY"].ToString();
                string tax_class = v_tabla.Rows[i]["TAX_CLASS"].ToString();
                string subtotal = v_tabla.Rows[i]["SUBTOTAL"].ToString();
                string subtotal_tax = v_tabla.Rows[i]["SUBTOTAL_TAX"].ToString();
                string total = v_tabla.Rows[i]["TOTAL"].ToString();
                string total_tax = v_tabla.Rows[i]["TOTAL_TAX"].ToString();
                string shipping_tax = v_tabla.Rows[i]["SHIPPING_TAX"].ToString();
                string sku = v_tabla.Rows[i]["SKU"].ToString();
                string price = v_tabla.Rows[i]["PRICE"].ToString();
                string linea = v_tabla.Rows[i]["LINEA"].ToString();
                string createdate = v_tabla.Rows[i]["CREATEDATE"].ToString();
                string recordate = v_tabla.Rows[i]["RECORDDATE"].ToString();

                dt_lineas.Rows.Add(id_pedido, id, name, product_id, variation_id, quantity, tax_class, subtotal, subtotal_tax, total, total_tax, shipping_tax, 
                    sku, price, linea, createdate, recordate);
            }

        }

        public void met_cargar_lineas_de_envio()
        {
            dt_metodos.Rows.Clear();
            Metodos.MetodosPedidosWoocomerce met_atributos = new Metodos.MetodosPedidosWoocomerce();
            DataTable v_tabla = new DataTable();
            Clases.PedidosWoocomerce obj_pedido = new Clases.PedidosWoocomerce();
            obj_pedido.id = id_ped;
            v_tabla = met_atributos.mostrar_envios_pedidos(obj_pedido);

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {

                string id_pedido_metodo = v_tabla.Rows[i]["ID_PEDIDO"].ToString();
                string id_metodo = v_tabla.Rows[i]["ID"].ToString();
                string method_title = v_tabla.Rows[i]["METHOD_TITTLE"].ToString();
                string method_id = v_tabla.Rows[i]["METHOD_ID"].ToString();
                string instance_id = v_tabla.Rows[i]["INSTANCE_ID"].ToString();
                string total = v_tabla.Rows[i]["TOTAL"].ToString();
                string total_tax = v_tabla.Rows[i]["TOTAL_TAX"].ToString();
                string lineas = v_tabla.Rows[i]["LINEAS"].ToString();
                string createdate_metodo = v_tabla.Rows[i]["CREATEDATE"].ToString();
                string recordate_metodo = v_tabla.Rows[i]["RECORDDATE"].ToString();
              
                dt_metodos.Rows.Add(id_pedido_metodo, id_metodo, method_title, method_id, instance_id, total, total_tax, lineas, createdate_metodo, recordate_metodo);
            }

        }



    }
}

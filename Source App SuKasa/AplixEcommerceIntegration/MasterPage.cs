using AplixEcommerceIntegration.Nidux;
using AplixEcommerceIntegration.WooCommerce;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplixEcommerceIntegration
{
    public partial class MasterPage : Form
    {
        UC_Nidux_SUKASA nidux;
        UC_Woocommerce woo;
        Shopify.UC_Shopify shopify;
        public string tienda_select;
        public MasterPage(string tienda)
        {
            InitializeComponent();
            tienda_select = tienda;
        }

        private void MasterPage_Load(object sender, EventArgs e)
        {
            //Segun el login y la tienda que haya escogido se inicia el UserControl
            if (tienda_select.Equals("Nidux"))
            {
                nidux = new UC_Nidux_SUKASA();
                Controls.Add(nidux);
                nidux.BringToFront();
                nidux.Dock = DockStyle.Fill;
            }
            else if(tienda_select.Equals("WooCommerce"))
            {
                woo = new UC_Woocommerce();
                Controls.Add(woo);
                woo.BringToFront();
                woo.Dock = DockStyle.Fill;
            }
            else if (tienda_select.Equals("Shopify"))
            {
                shopify = new Shopify.UC_Shopify();
                Controls.Add(shopify);
                shopify.BringToFront();
                shopify.Dock = DockStyle.Fill;
            }
        }

        /*
          Comentarios para el formato de programacion
          Clases:  ejemplo: ArticulosNidux
          Metodos: ejemplo: agregar_articulos
         */
    }
}

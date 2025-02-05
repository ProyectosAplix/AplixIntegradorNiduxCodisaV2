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
    public partial class Mostrar_Articulos_Envio : Form
    {
        public Mostrar_Articulos_Envio()
        {
            InitializeComponent();
            met_cargar_articulos_envio();
        }

        private void Mostrar_Articulos_Envio_Load(object sender, EventArgs e)
        {

        }


        public void met_cargar_articulos_envio()
        {

            dt_articulos.Rows.Clear();
            Metodos.MetodosEnvioWoocomerce met_atributos = new Metodos.MetodosEnvioWoocomerce();
            DataTable v_tabla = new DataTable();
            v_tabla = met_atributos.mostrar_articulos_envio();

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
              
                string articulo = v_tabla.Rows[i]["ARTICULO"].ToString();
                string descripcion = v_tabla.Rows[i]["DESCRIPCION"].ToString();
                string precio = v_tabla.Rows[i]["PRECIO"].ToString();
                string impuesto = v_tabla.Rows[i]["IMPUESTO1"].ToString();
                string precio_impuesto = v_tabla.Rows[i]["PRECIO_IMP"].ToString();

                dt_articulos.Rows.Add(articulo, descripcion, precio, impuesto, precio_impuesto, false);

            }

        }

        public void met_cargar_envios_busqueda()
        {

            dt_articulos.Rows.Clear();
            Metodos.MetodosEnvioWoocomerce met_envios = new Metodos.MetodosEnvioWoocomerce();
            Clases.EnvioBusqueda obj_envio = new Clases.EnvioBusqueda();
            obj_envio.dato = txt_dato.Text;

            if (combo_opciones.Text == "Nombre") { obj_envio.parametro = "N"; } else { obj_envio.parametro = "C"; }

            DataTable v_tabla = new DataTable();
            v_tabla = met_envios.mostrar_articulos_envio_busqueda(obj_envio);


            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {

                string articulo = v_tabla.Rows[i]["ARTICULO"].ToString();
                string descripcion = v_tabla.Rows[i]["DESCRIPCION"].ToString();
                string precio = v_tabla.Rows[i]["PRECIO"].ToString();
                string impuesto = v_tabla.Rows[i]["IMPUESTO1"].ToString();
                string precio_impuesto = v_tabla.Rows[i]["PRECIO_IMP"].ToString();

                dt_articulos.Rows.Add(articulo, descripcion, precio, impuesto, precio_impuesto, false);

            }

        }


        private void dt_articulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Si selecciona la celda de padre que muestre los articulo padres
            if (e.ColumnIndex == 5)
            {

                Envios frm_envio = Owner as Envios;

                string articulo = this.dt_articulos.CurrentRow.Cells[0].Value.ToString();
                string precio = this.dt_articulos.CurrentRow.Cells[4].Value.ToString();
                string descripcion = this.dt_articulos.CurrentRow.Cells[1].Value.ToString();
                frm_envio.cambiar_articulo_precio( articulo, precio, descripcion);
                this.Close();

            }
        }

        private void txt_dato_TextChanged(object sender, EventArgs e)
        {
            met_cargar_envios_busqueda();
        }



    }
}

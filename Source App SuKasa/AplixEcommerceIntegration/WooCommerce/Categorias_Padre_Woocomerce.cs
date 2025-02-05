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
    public partial class Categorias_Padre_Woocomerce : Form
    {
        public Categorias_Padre_Woocomerce()
        {
            InitializeComponent();
            met_cargar_categorias_padre();
            
        }

        private void Categorias_Padre_Woocomerce_Load(object sender, EventArgs e)
        {

        }



        //Cargar las categorias padre
        public void met_cargar_categorias_padre()
        {
            dt_categorias.Rows.Clear();
            Metodos.MetodosCategoriasWoocomerce met_categorias = new Metodos.MetodosCategoriasWoocomerce();
            DataTable v_tabla = new DataTable();
            v_tabla = met_categorias.mostrar_categorias_padre();         

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                string id_atributo = v_tabla.Rows[i]["CODIGO_WOOCOMERCE"].ToString();
                string nombre_atributo = v_tabla.Rows[i]["NOMBRE"].ToString();
                dt_categorias.Rows.Add(id_atributo, nombre_atributo);
            }

        }

        private void dt_categorias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Si selecciona la celda de padre que muestre los articulo padres
            if (e.ColumnIndex == 2)
            {

                Agregar_Categoria_Woocomerce frm_principal = Owner as Agregar_Categoria_Woocomerce;
                frm_principal.met_seleccionar_padre( this.dt_categorias.CurrentRow.Cells[0].Value.ToString());
                this.Close();

            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            dt_categorias.Rows.Clear();

            Metodos.MetodosCategoriasWoocomerce met_categorias = new Metodos.MetodosCategoriasWoocomerce();
            WooCommerce.Clases.BusquedaCategoriasWoocomerce obj_categoria = new WooCommerce.Clases.BusquedaCategoriasWoocomerce();

            obj_categoria.dato = txtdato.Text;

            if (combo_opciones.Text == "Nombre")
            { obj_categoria.parametro = "N"; }
            else { obj_categoria.parametro = "C"; }

            DataTable v_tabla = new DataTable();
            v_tabla = met_categorias.buscar_categorias_padre(obj_categoria);

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                string codigo_categoria = v_tabla.Rows[i]["CODIGO_WOOCOMERCE"].ToString();
                string nombre = v_tabla.Rows[i]["NOMBRE"].ToString();              
                dt_categorias.Rows.Add(codigo_categoria, nombre);
            }

        }

        private void txtdato_Click(object sender, EventArgs e)
        {

        }
    }
}

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
    public partial class Agregar_Categoria_Woocomerce : Form
    {
        public Agregar_Categoria_Woocomerce()
        {
            InitializeComponent();
         
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {


            if (txtnombre.Text == "")
            {

                MessageBox.Show("Debe de llenar el campo de nombre de categoría", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtnombre.Focus();

            }
            else {

                //llenamos todos los datos obtenidos del formulario
                WooCommerce.Clases.CategoriasWoocomerce obj_categorias = new Clases.CategoriasWoocomerce();
                obj_categorias.name = txtnombre.Text;
                obj_categorias.description = txtdescripcion.Text;
                obj_categorias.slug = txtslug.Text;
                obj_categorias.estado = "S";

                if (txtpadre.Text == "")
                {
                    obj_categorias.parent = 0;
                }
                else {

                    obj_categorias.parent = Convert.ToInt32(txtpadre.Text);
                }

                //Validamos que ese codigo de categoria de padre exista
                WooCommerce.Metodos.MetodosCategoriasWoocomerce met_categorias = new WooCommerce.Metodos.MetodosCategoriasWoocomerce();

                if (met_categorias.validar_codigo_de_categoria(obj_categorias) == "NO")
                {
                    MessageBox.Show("Debe de seleccionar un código de categoria padre válido", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else {

                    ////Validamos que ese nombre de categoria de padre exista
                    //WooCommerce.Metodos.MetodosCategoriasWoocomerce met_categorias_nom = new WooCommerce.Metodos.MetodosCategoriasWoocomerce();

                    //if (met_categorias_nom.validar_nombre_de_categoria(obj_categorias) == "SI")
                    //{
                    //    MessageBox.Show("El nombre de esta categoría ya se encuentra registrado por favor ingrese otro nombre", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //}
                    //else {

                        WooCommerce.Metodos.MetodosCategoriasWoocomerce met_categoria = new WooCommerce.Metodos.MetodosCategoriasWoocomerce();
                        MessageBox.Show(met_categoria.agregar_nueva_categoria_woocomerce(obj_categorias), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar_campos();
                    //}

                }
            
            }

        }

        public void limpiar_campos() {

            txtnombre.Text = "";
            txtdescripcion.Text = "";
            txtpadre.Text = "";
            txtslug.Text = "";

        }

        public void met_seleccionar_padre(string categoria_padre) {

            txtpadre.Text = categoria_padre;
        }

        private void txtpadre_Click(object sender, EventArgs e)
        {
            Categorias_Padre_Woocomerce frm_categoria_padre = new Categorias_Padre_Woocomerce();
            AddOwnedForm(frm_categoria_padre);
            frm_categoria_padre.ShowDialog();
        }





    }
}

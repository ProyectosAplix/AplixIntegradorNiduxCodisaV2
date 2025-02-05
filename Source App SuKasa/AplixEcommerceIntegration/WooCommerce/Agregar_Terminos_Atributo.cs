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
    public partial class Agregar_Terminos_Atributo : Form
    {
        public Agregar_Terminos_Atributo()
        {
            InitializeComponent();
            cargar_atributos();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void Agregar_Terminos_Atributo_Load(object sender, EventArgs e)
        {

        }


        public void cargar_atributos() {

            combo_atributos.Items.Clear();
            Metodos.MetodosAtributosWoocomerce met_categorias = new Metodos.MetodosAtributosWoocomerce();

            DataTable v_tabla = new DataTable();
            v_tabla = met_categorias.mostrar_atributos();

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                ComboboxItem item2 = new ComboboxItem();
                item2.Text = v_tabla.Rows[i]["ID"].ToString() + " - " + v_tabla.Rows[i]["DESCRIPCION"].ToString();
                item2.Value = v_tabla.Rows[i]["ID"].ToString();
                combo_atributos.Items.Add(item2);
            }

            combo_atributos.SelectedIndex = 0;
        }

    private void combo_atributos_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void toolStripButton6_Click(object sender, EventArgs e)
    {

            if (combo_atributos.Text == "")
            {

                MessageBox.Show("Debe de seleccionar un atributo", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                combo_atributos.Focus();

            }
            else {

                if (txt_nombre.Text == "")
                {

                    MessageBox.Show("Debe de completar el campo de nombre de atributo", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_nombre.Focus();

                }
                else {

                    Metodos.MetodosTerminoAtributosWoocomerce met_terminos = new Metodos.MetodosTerminoAtributosWoocomerce();
                    Clases.TerminosAtributoWoocomerce obj_termino = new Clases.TerminosAtributoWoocomerce();
                    obj_termino.codigo_atributo = Convert.ToInt32((combo_atributos.SelectedItem as ComboboxItem).Value.ToString());
                    obj_termino.name = txt_nombre.Text;
                    obj_termino.description = txt_descripcion.Text;
                    obj_termino.slug = txtslug.Text;
                    MessageBox.Show(met_terminos.agregar_nuevo_atributo(obj_termino), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }

            }
         
    }


        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }


        public void limpiar() {

            txt_nombre.Text = "";
            txt_descripcion.Text = "";
            txtslug.Text = "";

        }





    }

}

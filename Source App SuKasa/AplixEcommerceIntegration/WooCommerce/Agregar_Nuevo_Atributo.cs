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
    public partial class Agregar_Nuevo_Atributo : Form
    {
        public Agregar_Nuevo_Atributo()
        {
            InitializeComponent();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Metodos.MetodosAtributosWoocomerce met_Atributos = new Metodos.MetodosAtributosWoocomerce();

            if (txt_nombre_atributo.Text == "")
            {
                MessageBox.Show("Debe de completar el campo de nombre de atributo", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_nombre_atributo.Focus();
            }
            else
            {

                Clases.AtributosWoocomerce obj_atributo = new Clases.AtributosWoocomerce();
                obj_atributo.name = txt_nombre_atributo.Text;
                obj_atributo.estado = "S";

                if (met_Atributos.validar_nombre_atributo(obj_atributo) == "NO")
                {
                    MessageBox.Show(met_Atributos.agregar_nuevo_atributo(obj_atributo), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_nombre_atributo.Text = "";
                }
                else
                {
                    MessageBox.Show("El nombre del atributo ya se encuentra registrado", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }



    }
}

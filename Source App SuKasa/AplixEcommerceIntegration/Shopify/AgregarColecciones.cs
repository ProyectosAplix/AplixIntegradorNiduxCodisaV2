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
    public partial class AgregarColecciones : Form
    {
        Metodos_Colecciones mtc = new Metodos_Colecciones();
        public int indicador = 0;
        public AgregarColecciones()
        {
            InitializeComponent();
        }

        private void btnGuardarColeccion_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text == "")
            {
                MessageBox.Show("El campo de Nombre es de uso Obligario", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                int resultado = mtc.agregar_colecciones(nombre, descripcion);
                if (resultado == 1)
                {
                    MessageBox.Show("Colección agregada con éxito", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    indicador = 1;
                    txtDescripcion.Clear();
                    txtNombre.Clear();
                }
                else if(resultado == 0)
                {
                    MessageBox.Show("Error a la hora de ingresar la Colección", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    indicador = 0;
                }
                else
                {
                    MessageBox.Show("Error a la hora de ingresar la Colección en la tienda de Shopify", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    indicador = 0;
                }
            }
        }
    }
}

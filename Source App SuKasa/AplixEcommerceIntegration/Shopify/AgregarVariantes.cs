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
    public partial class AgregarVariantes : Form
    {
        /*------------------------- INSTANCIAS DE METODOS --------------------------*/

        Metodos_Opciones mto = new Metodos_Opciones();
        public int indicador = 0;
        public AgregarVariantes()
        {
            InitializeComponent();
        }

        private void AgregarVariantes_Load(object sender, EventArgs e)
        {
            CargarCombobox();
        }

        //Llenado del ComboBox
        public void CargarCombobox()
        {
            DataTable dt = new DataTable();
            dt = mto.opciones_ComboBox();

            if (dt == null)
            {
                MessageBox.Show("Error en Procedimiento de Obtener Opciones", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos de Opciones para mostrar", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        cbbOpciones.Items.Add(dr["NOMBRE"].ToString());
                    }
                }
            }

        }

        private void btnGuardarOpciones_Click(object sender, EventArgs e)
        {
            if (txtNombreOpcion.Text == "")
            {
                MessageBox.Show("El campo de Nombre de opción es de uso Obligario", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int resultado = mto.agregar_opciones(txtNombreOpcion.Text);
                if (resultado == 1)
                {
                    MessageBox.Show("La Opción ha sido agregada con éxito", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbbOpciones.Items.Clear();
                    CargarCombobox();
                    txtNombreOpcion.Clear();
                }
                else if(resultado == 2)
                {
                    MessageBox.Show("La Opción ya existe, por favor ingresa otra", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (resultado == 0)
                {
                    MessageBox.Show("Error en el procedimiento de Insertar una opción", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGuardarValores_Click(object sender, EventArgs e)
        {
            if (cbbOpciones.Text.Equals(""))
            {
                MessageBox.Show("El campo de opción no puede estar vacío, por favor seleccione una de las opciones disponibles", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (txtNombreValor.Text.Equals(""))
                {
                    MessageBox.Show("El campo de nombre del Valore no puede estar vacío, por favor ingrese uno", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    int resultado = mto.agregar_opciones_valores(txtNombreValor.Text, cbbOpciones.Text);
                    if (resultado == 1)
                    {
                        MessageBox.Show("El valor de la opción ha sido agregada con éxito", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtNombreValor.Clear();
                        cbbOpciones.Text = "";
                        indicador = 1;
                    }
                    else if (resultado == 2)
                    {
                        MessageBox.Show("El valor de la opción ya existe, por favor ingresa otra", "Mensaje de Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (resultado == 0)
                    {
                        MessageBox.Show("Error en el procedimiento de Insertar un valor de opción", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}

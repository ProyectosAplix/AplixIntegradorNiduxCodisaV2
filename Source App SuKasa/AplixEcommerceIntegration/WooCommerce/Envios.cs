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
    public partial class Envios : Form
    {
        public Envios()
        {
            InitializeComponent();
            cargar_combo_zonas();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Metodos.MetodosEnvioWoocomerce met = new Metodos.MetodosEnvioWoocomerce();

            try
            {
                if (combo_zonas.Text == "")
                {
                    MessageBox.Show("Debe de seleccionar una zona de envío", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    if (combo_metodos.Text == "")
                    {
                        MessageBox.Show("Debe de seleccionar un método de envío", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {

                        if (txt_articulos.Text == "")
                        {
                            MessageBox.Show("Debe de seleccionar un articulo de envío", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {

                            if (txt_precio.Text == "")
                            {
                                MessageBox.Show("Debe de seleccionar un precio de envío", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {


                                Clases.EnvioWoocomerce obj_envio = new Clases.EnvioWoocomerce();

                                obj_envio.zona = Convert.ToInt32((combo_zonas.SelectedItem as ComboboxItem).Value.ToString());
                                obj_envio.nombre_zona = combo_zonas.Text;
                                obj_envio.id_metodo_pago = Convert.ToInt32((combo_metodos.SelectedItem as ComboboxItem).Value.ToString());
                                obj_envio.metodo_pago = combo_metodos.Text;
                                obj_envio.sku = txt_articulos.Text;
                                obj_envio.precio = txt_precio.Text;

                                met.registrar_metodo_envio(obj_envio);

                                //MessageBox.Show("Método de envío agregado con éxito", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                this.Close();

                            }

                        }

                    }

                }
            }
            catch (Exception ex )
            {

                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


           

    


        }
        public void cargar_combo_zonas() {

            combo_zonas.Items.Clear();
            Metodos.MetodosEnvioWoocomerce met_categorias = new Metodos.MetodosEnvioWoocomerce();
            List<Clases.ZonasWoocoemrce> lista_zonas = met_categorias.obtener_zonas_de_woocomerce();

            //validamos que la lista venga llena
            if (lista_zonas.Count > 0) {

                int i = 0;

                while ( i < lista_zonas.Count) {

                    ComboboxItem item2 = new ComboboxItem();
                    item2.Text =  lista_zonas[i].name.ToString();
                    item2.Value = lista_zonas[i].id.ToString().ToString();
                    combo_zonas.Items.Add(item2);

                    i++;
                }


            }

            if (lista_zonas.Count > 0) {

                combo_zonas.SelectedIndex = 0;

            }


        }
        public void cargar_combo_metodos_zonas()
        {

            combo_metodos.Items.Clear();
            Metodos.MetodosEnvioWoocomerce met_categorias = new Metodos.MetodosEnvioWoocomerce();
            List<Clases.MetodosEnvioWoocoemrce> 
            lista_zonas = met_categorias.obtener_metodos_envio_de_woocomerce(
                (combo_zonas.SelectedItem as ComboboxItem).Value.ToString()              
                );

            //validamos que la lista venga llena
            if (lista_zonas.Count > 0)
            {

                int i = 0;

                while (i < lista_zonas.Count)
                {

                    ComboboxItem item2 = new ComboboxItem();
                    item2.Text = lista_zonas[i].title.ToString();
                    item2.Value = lista_zonas[i].instance_id.ToString().ToString();
                    combo_metodos.Items.Add(item2);

                    i++;
                }


            }
            else {

                combo_metodos.Items.Clear();
                combo_metodos.Text = "";
            }

            if (lista_zonas.Count > 0)
            {
                combo_metodos.SelectedIndex = 0;
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
        private void Envios_Load(object sender, EventArgs e)
        {

        }
        private void combo_zonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_combo_metodos_zonas();
        }

        private void txt_articulos_MouseClick(object sender, MouseEventArgs e)
        {
            Mostrar_Articulos_Envio frm_articulos = new Mostrar_Articulos_Envio();
            AddOwnedForm(frm_articulos);
            frm_articulos.ShowDialog();
        }

        public void cambiar_articulo_precio(string articulo, string precio, string descripcion) {

            txt_articulos.Text = articulo;
            txt_precio.Text = precio;
            lb_descri.Text = descripcion;

        }


    }
}

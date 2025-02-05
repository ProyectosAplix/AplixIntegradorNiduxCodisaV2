using Newtonsoft.Json;
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
    public partial class AsignarCategoriasArticulos : Form
    {
        static List<Clases.CategoriasWoocomerce> lista_editados_recientemente = new List<Clases.CategoriasWoocomerce>();
        static List<Clases.CategoriasWoocomerce> lista_categorias_seleccionados = new List<Clases.CategoriasWoocomerce>();
        static int fila = 0;

        public AsignarCategoriasArticulos(int i, string categorias)
        {
            InitializeComponent();
            fila = i;
            pasar_cadena_lista(categorias);
            met_cargar_categorias();
        }

        private void AsignarCategoriasArticulos_Load(object sender, EventArgs e)
        {

        }
        public void met_cargar_categorias()
        {


            Metodos.MetodosCategoriasWoocomerce met_categorias = new Metodos.MetodosCategoriasWoocomerce();

            DataTable v_tabla = new DataTable();
            v_tabla = met_categorias.mostrar_categorias_para_asignar();

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                bool est = false;
                string codigo_woocomerce = v_tabla.Rows[i]["CODIGO_WOOCOMERCE"].ToString();
                string nombre = v_tabla.Rows[i]["NOMBRE"].ToString();
                string categoria_pad = v_tabla.Rows[i]["PADRE"].ToString();

                Clases.CategoriasWoocomerce product = lista_categorias_seleccionados.FirstOrDefault(x => x.id == Convert.ToInt32(v_tabla.Rows[i]["CODIGO_WOOCOMERCE"].ToString()));
                if (product == null) { est = false; } else { est = true; }


                dt_categorias.Rows.Add(codigo_woocomerce, nombre, categoria_pad, est);


            }

        }
        public void pasar_cadena_lista(string cadena)
        {

            if (cadena == "" || cadena == null || cadena == " ") { }
            else
            {

                char[] delimiterChars = { ',' };
                string[] words = cadena.Split(delimiterChars);
                foreach (var word in words)
                {
                    Clases.CategoriasWoocomerce obj = new Clases.CategoriasWoocomerce();
                    obj.id = Convert.ToInt32(word);                  
                    obj.estado = "true";
                    lista_categorias_seleccionados.Add(obj);
                    lista_editados_recientemente.Add(obj);
                }
            }


        }
        private void AsignarCategoriasArticulos_FormClosing(object sender, FormClosingEventArgs e)
        {
            lista_categorias_seleccionados.Clear();
            lista_editados_recientemente.Clear();
        }
        private void dt_categorias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {

                dt_categorias.EndEdit();

                Clases.CategoriasWoocomerce obj_categorias = new Clases.CategoriasWoocomerce();
                obj_categorias.id = Convert.ToInt32(this.dt_categorias.CurrentRow.Cells[0].Value.ToString());

                //En este caso usamos este valor para guardar el estado de la categoria
                obj_categorias.estado = this.dt_categorias.CurrentRow.Cells[3].Value.ToString();
               

                Clases.CategoriasWoocomerce product = lista_editados_recientemente.FirstOrDefault(x => x.id == Convert.ToInt32(this.dt_categorias.CurrentRow.Cells[0].Value.ToString()));

                if (product == null)
                {
                    lista_editados_recientemente.Add(obj_categorias);
                }
                else
                {

                    lista_editados_recientemente.RemoveAll(x => x.id == (obj_categorias.id));
                    lista_editados_recientemente.Add(obj_categorias);

                }

                dt_categorias.EndEdit();

            }

            btn_art.Enabled = true;
        }
        private void btn_art_Click(object sender, EventArgs e)
        {
            //string jsonString = JsonConvert.SerializeObject(lista_editados_recientemente, Formatting.Indented);

            //MessageBox.Show(jsonString);

            string cadena_categorias = "";

            for (int i = 0; i < lista_editados_recientemente.Count; i++)
            {
                if (lista_editados_recientemente[i].estado.Equals("true"))
                {
                    cadena_categorias = lista_editados_recientemente[i].id + "," + cadena_categorias;
                }
            }

            if (cadena_categorias != "")
            {
                cadena_categorias = cadena_categorias.Remove(cadena_categorias.Length - 1);
            }

            MessageBox.Show(cadena_categorias);

            WooCommerce.UC_Woocommerce fr = new WooCommerce.UC_Woocommerce();
            fr.met_asignar_categorias_greed(cadena_categorias, fila);

            this.Close();
        }



    }
}

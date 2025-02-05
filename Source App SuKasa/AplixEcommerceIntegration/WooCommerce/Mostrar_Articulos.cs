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
    public partial class Mostrar_Articulos : Form
    {

        //En esta lista guardamos los articulos que se van a poner en estado de activo
        static List<Clases.ArticulosBusquedaWoocomerce> lista_art = new List<Clases.ArticulosBusquedaWoocomerce>();

        public Mostrar_Articulos()
        {
            InitializeComponent();
            met_cargar_atributos_agregados();
        }
        private void Mostrar_Articulos_Load(object sender, EventArgs e)
        {

        }

        //Cargar el datagrid de atributos
        public void met_cargar_atributos_agregados()
        {

            dt_atributos.Rows.Clear();
            Metodos.MetodosArticulosWoocomerce met_atributos = new Metodos.MetodosArticulosWoocomerce();
            DataTable v_tabla = new DataTable();
            v_tabla = met_atributos.mostrar_articulos_para_activar_sincronizador();
            //dt_atributos.AutoSizeColumnsMode =  DataGridViewAutoSizeColumnsMode.Fill;

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                string id_atributo = v_tabla.Rows[i]["ARTICULO"].ToString();
                string nombre_atributo = v_tabla.Rows[i]["NOMBRE"].ToString();

                dt_atributos.Rows.Add(id_atributo, nombre_atributo);
            }

        }
        public void met_cargar_terminos_atributos()
        {

            dt_atributos.Rows.Clear();
            Metodos.MetodosArticulosWoocomerce met_atributos = new Metodos.MetodosArticulosWoocomerce();
            Clases.ArticulosBusquedaWoocomerce obj_atributos = new Clases.ArticulosBusquedaWoocomerce();
            obj_atributos.dato = txt_dato.Text;

            if (combo_opciones.Text == "Nombre") { obj_atributos.parametro = "N"; } else { obj_atributos.parametro = "C"; }

            DataTable v_tabla = new DataTable();
            v_tabla = met_atributos.mostrar_buscar_articulos(obj_atributos);

         
            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                bool valor_check = false;
                string sku;
                string descripcion;
                string estado;
           

                Clases.ArticulosBusquedaWoocomerce product = lista_art.FirstOrDefault(x => x.sku == (sku = v_tabla.Rows[i]["ARTICULO"].ToString()));

                if (product == null)
                {
                    sku = v_tabla.Rows[i]["ARTICULO"].ToString();
                    descripcion = v_tabla.Rows[i]["NOMBRE"].ToString();
                  
                }
                else
                {

                    sku = product.sku.ToString();
                    descripcion = product.description;
                    estado = product.sincroniza;

                    if (estado == "true") { valor_check = true; } else { valor_check = false; }

                }

                dt_atributos.Rows.Add(sku, descripcion, valor_check);


            }

        }
        private void dt_atributos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dt_atributos.EndEdit();

            if (e.ColumnIndex == 2)
            {

                Clases.ArticulosBusquedaWoocomerce obj_termino = new Clases.ArticulosBusquedaWoocomerce();

                obj_termino.sku =(this.dt_atributos.CurrentRow.Cells[0].Value.ToString());
                obj_termino.description = this.dt_atributos.CurrentRow.Cells[1].Value.ToString();
                obj_termino.sincroniza = this.dt_atributos.CurrentRow.Cells[2].Value.ToString();
               
                Clases.ArticulosBusquedaWoocomerce product = lista_art.FirstOrDefault(x => x.sku == (this.dt_atributos.CurrentRow.Cells[0].Value.ToString()));

                if (product == null)
                {
                    lista_art.Add(obj_termino);
                }
                else
                {

                    lista_art.RemoveAll(x => x.sku == (obj_termino.sku));
                    lista_art.Add(obj_termino);

                }

                dt_atributos.EndEdit();

                btn_art.Enabled = true;


            }
        }
        private void btn_art_Click(object sender, EventArgs e)
        {
            if (lista_art.Count > 0) {

                int i = 0;

                while (i < lista_art.Count) {

                    Metodos.MetodosArticulosWoocomerce met = new Metodos.MetodosArticulosWoocomerce();

                    if (lista_art[i].sincroniza == "true") {

                        met.actualizar_articulos_estados(lista_art[i]);

                    }

                    i++;
                }

            }

            this.Close();

        }
        private void txt_dato_TextChanged(object sender, EventArgs e)
        {
            met_cargar_terminos_atributos();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Metodos.MetodosArticulosWoocomerce met = new Metodos.MetodosArticulosWoocomerce();

            try
            {
                met.sincronizador_de_nuevos_articulos_precios_cantidades();
                MessageBox.Show("Articulos cargados con éxito", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                met_cargar_atributos_agregados();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

           

          
        }
    }
}

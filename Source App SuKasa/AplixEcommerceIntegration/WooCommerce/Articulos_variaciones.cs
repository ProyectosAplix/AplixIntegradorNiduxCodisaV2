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
    public partial class Articulos_variaciones : Form
    {
        //static int linea_greed = 0;

        static List<Clases.ArticulosVariaciones> lista_seleccion = new List<Clases.ArticulosVariaciones>();

        public Articulos_variaciones()
        {
            InitializeComponent();
            met_cargar_atributos_agregados();
           
            //linea_greed = linea;
        }

        private void Articulos_variaciones_Load(object sender, EventArgs e)
        {

        }


        public void met_cargar_atributos_agregados()
        {

            dt_atributos.Rows.Clear();
            Metodos.MetodosArticulosWoocomerce met_atributos = new Metodos.MetodosArticulosWoocomerce();
            DataTable v_tabla = new DataTable();
            v_tabla = met_atributos.mostrar_articulos_para_variacion();
            //dt_atributos.AutoSizeColumnsMode =  DataGridViewAutoSizeColumnsMode.Fill;

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                string id_atributo = v_tabla.Rows[i]["ARTICULO"].ToString();
                string nombre_atributo = v_tabla.Rows[i]["NOMBRE"].ToString();
                string stock = v_tabla.Rows[i]["STOCK"].ToString();
                string precio = v_tabla.Rows[i]["PRECIO"].ToString();
                string peso = v_tabla.Rows[i]["PESO"].ToString();

                dt_atributos.Rows.Add(id_atributo, nombre_atributo, stock, precio, peso ,false);
            }

        }

        private void dt_atributos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dt_atributos.EndEdit();

            if (e.ColumnIndex == 5)
            {
                //Variaciones frm_principal = Owner as Variaciones;

              
                 Clases.ArticulosVariaciones obj = new Clases.ArticulosVariaciones();

                 obj.articulo = (this.dt_atributos.CurrentRow.Cells[0].Value.ToString());
                 obj.nombre = (this.dt_atributos.CurrentRow.Cells[1].Value.ToString());
                 obj.cantidad_en_greed = (this.dt_atributos.CurrentRow.Cells[2].Value.ToString());
                 obj.precio = (this.dt_atributos.CurrentRow.Cells[3].Value.ToString());              
                 obj.peso = (this.dt_atributos.CurrentRow.Cells[4].Value.ToString());
                 obj.estado = (this.dt_atributos.CurrentRow.Cells[5].Value.ToString());

                Clases.ArticulosVariaciones product = lista_seleccion.FirstOrDefault(x => x.articulo == this.dt_atributos.CurrentRow.Cells[0].Value.ToString());

                if (product == null)
                {
                    lista_seleccion.Add(obj);
                }
                else
                {

                    lista_seleccion.RemoveAll(x => x.articulo == (obj.articulo));
                    lista_seleccion.Add(obj);

                }
           

                btn_art.Enabled = true;

                //frm_principal.met_asignar_valores_greed(articulo, nombre, cantidad, precio, linea_greed);

                //this.Close();
            }


        }

        private void Articulos_variaciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            lista_seleccion.Clear();
        }

        private void btn_art_Click(object sender, EventArgs e)
        {
            //string jsonData = JsonConvert.SerializeObject(lista_seleccion, Formatting.Indented);
            //MessageBox.Show(jsonData);

            Variaciones frm_principal = Owner as Variaciones;

            int i = 0;

            while (i < lista_seleccion.Count) {

                if (lista_seleccion[i].estado == "true") {

                    frm_principal.met_asignar_valores_greed(lista_seleccion[i].articulo, lista_seleccion[i].nombre, lista_seleccion[i].cantidad_en_greed.ToString(), lista_seleccion[i].precio , lista_seleccion[i].peso);

                }

                i++;

            }
       
            this.Close();


        }


        //public void met_cargar_terminos_atributos()
        //{

        //    dt_atributos.Rows.Clear();
        //    Metodos.MetodosArticulosWoocomerce met_atributos = new Metodos.MetodosArticulosWoocomerce();
        //    Clases.ArticulosBusquedaWoocomerce obj_atributos = new Clases.ArticulosBusquedaWoocomerce();
        //    obj_atributos.dato = txt_dato.Text;

        //    if (combo_opciones.Text == "Nombre") { obj_atributos.parametro = "N"; } else { obj_atributos.parametro = "C"; }

        //    DataTable v_tabla = new DataTable();
        //    v_tabla = met_atributos.mostrar_buscar_articulos(obj_atributos);


        //    for (int i = 0; i < v_tabla.Rows.Count; i++)
        //    {
        //        bool valor_check = false;
        //        string sku;
        //        string descripcion;
        //        string estado;


        //        Clases.ArticulosBusquedaWoocomerce product = lista_art.FirstOrDefault(x => x.sku == (sku = v_tabla.Rows[i]["ARTICULO"].ToString()));

        //        if (product == null)
        //        {
        //            sku = v_tabla.Rows[i]["ARTICULO"].ToString();
        //            descripcion = v_tabla.Rows[i]["NOMBRE"].ToString();

        //        }
        //        else
        //        {

        //            sku = product.sku.ToString();
        //            descripcion = product.description;
        //            estado = product.sincroniza;

        //            if (estado == "true") { valor_check = true; } else { valor_check = false; }

        //        }

        //        dt_atributos.Rows.Add(sku, descripcion, valor_check);


        //    }

        //}


       


    }
}

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
    public partial class Asignar_Terminos_Variaciones : Form
    {

        static string articulo_padre = "";
        static int fila = 0;
        static List<Clases.ArtributosArticulos> lista = new List<Clases.ArtributosArticulos>();
        static List<Clases.BusquedaAtributos> lista_atributos = new List<Clases.BusquedaAtributos>();
        static List<Clases.ArticulosVariaciones> lista_atributos_seleccionados = new List<Clases.ArticulosVariaciones>();

        public Asignar_Terminos_Variaciones(int i, string articulo, string atributos)
        {
            InitializeComponent();
            articulo_padre = articulo;
            fila = i;
            pasar_cadena_lista(atributos);
            met_mostrar_valores_padre();
           

        }

        private void Asignar_Terminos_Variaciones_Load(object sender, EventArgs e)
        { 
        }

        public void met_mostrar_valores_padre()
        {

            dt_variaciones.Rows.Clear();
            Metodos.MetodosVariacionesWoocomerce met_atributos = new Metodos.MetodosVariacionesWoocomerce();
            DataTable v_tabla = new DataTable();

            Clases.ArticulosVariaciones obj = new Clases.ArticulosVariaciones();
            obj.articulo_padre = articulo_padre;

            v_tabla = met_atributos.mostrar_variaciones_padres(obj);


            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                bool est = false;

                string id_atributo = v_tabla.Rows[i]["ID_ATRIBUTO"].ToString();
                string id_termino_atributo = v_tabla.Rows[i]["ID_TERMINO_ATRIBUTO"].ToString();
                string nombre = v_tabla.Rows[i]["NOMBRE"].ToString();
                string estado = v_tabla.Rows[i]["ESTADO"].ToString();

                Clases.ArticulosVariaciones product = lista_atributos_seleccionados.FirstOrDefault(x => x.id_termino == Convert.ToInt32(v_tabla.Rows[i]["ID_TERMINO_ATRIBUTO"].ToString()));
                if (product == null) { est = false; }else {

                    Clases.ArtributosArticulos obj_atri = new Clases.ArtributosArticulos();
                    obj_atri.id_termino_atributo = Convert.ToInt32(id_termino_atributo);
                    obj_atri.estado = "True";
                    lista.Add(obj_atri);

                    est = true; }

                dt_variaciones.Rows.Add(id_atributo, id_termino_atributo, nombre, est);

            }

        }

        private void Asignar_Terminos_Variaciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            lista.Clear();
            lista_atributos_seleccionados.Clear();
            lista_atributos.Clear();
        }

        private void dt_variaciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (e.ColumnIndex == 3)
            {

                dt_variaciones.EndEdit();

                Clases.ArtributosArticulos obj_atributos = new Clases.ArtributosArticulos();
                obj_atributos.id_atributo = Convert.ToInt32(this.dt_variaciones.CurrentRow.Cells[0].Value.ToString());
                obj_atributos.id_termino_atributo = Convert.ToInt32(this.dt_variaciones.CurrentRow.Cells[1].Value.ToString());              
                obj_atributos.estado = this.dt_variaciones.CurrentRow.Cells[3].Value.ToString();

                Clases.ArtributosArticulos product = lista.FirstOrDefault(x => x.id_termino_atributo == Convert.ToInt32(this.dt_variaciones.CurrentRow.Cells[1].Value.ToString()));

                if (product == null)
                {
                    lista.Add(obj_atributos);
                }
                else
                {

                    lista.RemoveAll(x => x.id_termino_atributo == (obj_atributos.id_termino_atributo));
                    lista.Add(obj_atributos);

                }

                dt_variaciones.EndEdit();

            }

            btn_art.Enabled = true;

        }

        private void btn_art_Click(object sender, EventArgs e)
        {
            //string jsonString = JsonConvert.SerializeObject(lista, Formatting.Indented);

            //MessageBox.Show(jsonString);


            string cadena_atributos = "";

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].estado.Equals("True"))
                {
                    cadena_atributos = lista[i].id_termino_atributo + "," + cadena_atributos;
                }
            }

            if (cadena_atributos != "")
            {
                cadena_atributos = cadena_atributos.Remove(cadena_atributos.Length - 1);
            }

            Variaciones frm_principal = Owner as Variaciones;
            frm_principal.met_asignar_valores_atributos_greed(cadena_atributos, fila);

            this.Close();


        }

        public void pasar_cadena_lista( string cadena) {

            if (cadena == "" ||  cadena == null || cadena == " ") {}
            else {

                char[] delimiterChars = { ',' };
                string[] words = cadena.Split(delimiterChars);
                foreach (var word in words)
                {
                    Clases.ArticulosVariaciones obj = new Clases.ArticulosVariaciones();
                    obj.id_termino = Convert.ToInt32(word);

                    lista_atributos_seleccionados.Add(obj);
                }
            }


        }

        
    }
}

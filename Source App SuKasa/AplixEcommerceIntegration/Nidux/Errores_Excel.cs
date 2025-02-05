using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplixEcommerceIntegration.Nidux
{
    public partial class Errores_Excel : Form
    {

        static List<string> lista_articulos = new List<string>();
        static List<string> lista_variaciones = new List<string>();

        public Errores_Excel(List<string> articulos)//, List<string> variaciones)
        {
            InitializeComponent();
            lista_articulos = articulos;
            //lista_variaciones = variaciones;
        }

        private void Errores_Excel_Load(object sender, EventArgs e)
        {
            richTextBox1.AppendText("Errores de carga de articulos : \n\n");

            if (lista_articulos.Count <= 0)
            {

                richTextBox1.AppendText("Ningun error detectado \n\n");
            }
            else
            {

                int i = 0;

                while (i < lista_articulos.Count)
                {

                    richTextBox1.AppendText(lista_articulos[i].ToString() + "\n\n");

                    i++;
                }


            }

            //richTextBox1.AppendText("Errores de carga de variaciones : \n\n");


            //if (lista_variaciones.Count <= 0)
            //{

            //    richTextBox1.AppendText("Ningun error detectado \n\n");
            //}
            //else
            //{

            //    int j = 0;

            //    while (j < lista_variaciones.Count)
            //    {

            //        richTextBox1.AppendText(lista_variaciones[j].ToString() + "\n\n");

            //        j++;
            //    }


            //}

        }

        private void Errores_Excel_FormClosing(object sender, FormClosingEventArgs e)
        {
            lista_articulos.Clear();
            lista_variaciones.Clear();
        }


    }
}

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
    public partial class frm_filtro_articulos : Form
    {
        public int n = 0;
        public frm_filtro_articulos()
        {
            InitializeComponent();
        }
        private void actualizarFiltrosArticulo_BD()
        {
            Clases.filtro_articulos filtro_Articulos = new Clases.filtro_articulos();
            Metodos.met_filtro_articulos met_Filtro_Articulos = new Metodos.met_filtro_articulos();

            filtro_Articulos.codigo_articulo = Convert.ToInt32(chk_codigo_articulo.Checked);
            filtro_Articulos.nombre = Convert.ToInt32(chk_nombre.Checked);
            filtro_Articulos.nombre_nidux = Convert.ToInt32(chk_nombre_nidux.Checked);
            filtro_Articulos.descripcion = Convert.ToInt32(chk_descripcion.Checked);
            filtro_Articulos.descripcion_nidux = Convert.ToInt32(chk_descripcion_nidux.Checked);
            filtro_Articulos.peso = Convert.ToInt32(chk_peso.Checked);
            filtro_Articulos.cantidad = Convert.ToInt32(chk_cantidad.Checked);
            filtro_Articulos.precio = Convert.ToInt32(chk_precio.Checked);
            filtro_Articulos.porcentaje_descuento = Convert.ToInt32(chk_porcentaje_descuento.Checked);
            filtro_Articulos.impuesto_articulo = Convert.ToInt32(chk_impuesto_articulo.Checked);
            filtro_Articulos.sincroniza = Convert.ToInt32(chk_sincroniza.Checked);
            filtro_Articulos.estado = Convert.ToInt32(chk_estado.Checked);
            filtro_Articulos.id_nidux = Convert.ToInt32(chk_id_nidux.Checked);
            filtro_Articulos.marca_nidux = Convert.ToInt32(chk_marca_nidux.Checked);
            filtro_Articulos.categorias = Convert.ToInt32(chk_categorias.Checked);
            filtro_Articulos.valores_atributos = Convert.ToInt32(chk_valores_atributos.Checked);
            filtro_Articulos.id_padre = Convert.ToInt32(chk_id_padre.Checked);
            filtro_Articulos.id_hijo = Convert.ToInt32(chk_id_hijo.Checked);
            filtro_Articulos.indicador_stock = Convert.ToInt32(chk_indicador_stock.Checked);
            filtro_Articulos.destacar_articulo = Convert.ToInt32(chk_destacar_articulo.Checked);
            filtro_Articulos.costo_shipping = Convert.ToInt32(chk_costo_shipping.Checked);
            filtro_Articulos.permite_reserva = Convert.ToInt32(chk_permite_reserva.Checked);
            filtro_Articulos.porcentaje_reserva = Convert.ToInt32(chk_porcentaje_reserva.Checked);
            filtro_Articulos.limite_carrito = Convert.ToInt32(chk_limite_carrito.Checked);
            filtro_Articulos.usa_gif = Convert.ToInt32(chk_usa_gif.Checked);
            filtro_Articulos.tiempo_gif = Convert.ToInt32(chk_tiempo_gif.Checked);
            filtro_Articulos.video_youtube = Convert.ToInt32(chk_video_youtube.Checked);
            filtro_Articulos.nombre_ingles = Convert.ToInt32(chk_nombre_ingles.Checked);
            filtro_Articulos.descripcion_ingles = Convert.ToInt32(chk_descripcion_ingles.Checked);


            met_Filtro_Articulos.actualizarFiltroArticulos(filtro_Articulos);
        }

        private void cargarCheckBox_FiltroArticulos()
        {
            try
            {
                List<Clases.filtro_articulos> filtro_Articulos = new List<Clases.filtro_articulos>();
                Metodos.met_filtro_articulos met_Filtro_Articulos = new Metodos.met_filtro_articulos();

                filtro_Articulos = met_Filtro_Articulos.obtenerFiltroArticulos();

                // LOS CHECKBOX POR DEFECTO APARECEN ACTIVOS
                // ACA PREGUNTA SI EL VALOR EN BD ES 0 Y DESACTIVA LOS CHECKBOXS
                if (filtro_Articulos[0].codigo_articulo == 0) { chk_codigo_articulo.Checked = false; }
                if (filtro_Articulos[0].nombre == 0) { chk_nombre.Checked = false; }
                if (filtro_Articulos[0].nombre_nidux == 0) { chk_nombre_nidux.Checked = false; }
                if (filtro_Articulos[0].descripcion == 0) { chk_descripcion.Checked = false; }
                if (filtro_Articulos[0].descripcion_nidux == 0) { chk_descripcion_nidux.Checked = false; }
                if (filtro_Articulos[0].peso == 0) { chk_peso.Checked = false; }
                if (filtro_Articulos[0].cantidad == 0) { chk_cantidad.Checked = false; }
                if (filtro_Articulos[0].precio == 0) { chk_precio.Checked = false; }
                if (filtro_Articulos[0].porcentaje_descuento == 0) { chk_porcentaje_descuento.Checked = false; }
                if (filtro_Articulos[0].impuesto_articulo == 0) { chk_impuesto_articulo.Checked = false; }
                if (filtro_Articulos[0].sincroniza == 0) { chk_sincroniza.Checked = false; }
                if (filtro_Articulos[0].estado == 0) { chk_estado.Checked = false; }
                if (filtro_Articulos[0].id_nidux == 0) { chk_id_nidux.Checked = false; }
                if (filtro_Articulos[0].marca_nidux == 0) { chk_marca_nidux.Checked = false; }
                if (filtro_Articulos[0].categorias == 0) { chk_categorias.Checked = false; }
                if (filtro_Articulos[0].valores_atributos == 0) { chk_valores_atributos.Checked = false; }
                if (filtro_Articulos[0].id_padre == 0) { chk_id_padre.Checked = false; }
                if (filtro_Articulos[0].id_hijo == 0) { chk_id_hijo.Checked = false; }
                if (filtro_Articulos[0].indicador_stock == 0) { chk_indicador_stock.Checked = false; }
                if (filtro_Articulos[0].destacar_articulo == 0) { chk_destacar_articulo.Checked = false; }
                if (filtro_Articulos[0].costo_shipping == 0) { chk_costo_shipping.Checked = false; }
                if (filtro_Articulos[0].permite_reserva == 0) { chk_permite_reserva.Checked = false; }
                if (filtro_Articulos[0].porcentaje_reserva == 0) { chk_porcentaje_reserva.Checked = false; }
                if (filtro_Articulos[0].limite_carrito == 0) { chk_limite_carrito.Checked = false; }
                if (filtro_Articulos[0].usa_gif == 0) { chk_usa_gif.Checked = false; }
                if (filtro_Articulos[0].tiempo_gif == 0) { chk_tiempo_gif.Checked = false; }
                if (filtro_Articulos[0].video_youtube == 0) { chk_video_youtube.Checked = false; }
                if (filtro_Articulos[0].nombre_ingles == 0) { chk_nombre_ingles.Checked = false; }
                if (filtro_Articulos[0].descripcion_ingles == 0) { chk_descripcion_ingles.Checked = false; }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnFiltro_Articulos_Click(object sender, EventArgs e)
        {
            try
            {
                actualizarFiltrosArticulo_BD();
                n = 1;
                this.Close();
            }
            catch (Exception)
            {
                n = 0;
                throw;
            }
        }

        private void frm_filtro_articulos_Load(object sender, EventArgs e)
        {
            cargarCheckBox_FiltroArticulos();
        }
    }
}

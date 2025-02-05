using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace AplixEcommerceIntegration.WooCommerce
{
    public partial class UC_Woocommerce : UserControl
    {

        static string tienda = "";


        //***************************************************************************************************
        //ESTAS VARIABLES SE USAN PARA LOS COMBOBOX DE ARTICULOS SON MUY MUY IMPORTANTES
        //esta variable nos indica cual numero de columna fue la que se selcciono en el combobox
        static int columna_combo = 0;

        //static string valor enviado por combo_box de datagreed
        static string valor_enviado_por_combo_box = "";
        //***************************************************************************************************



        //*************LISTAS STATICAS QUE GUARDAN VALORES EDITADOS*********************
        static List<Clases.CategoriasWoocomerce> lista_editada_categorias = new List<Clases.CategoriasWoocomerce>();
        static List<Clases.CategoriasWoocomerce> lista_eliminar_categorias = new List<Clases.CategoriasWoocomerce>();
        static List<Clases.AtributosWoocomerce> lista_editada_atributos = new List<Clases.AtributosWoocomerce>();
        static List<Clases.AtributosWoocomerce> lista_eliminar_atributos = new List<Clases.AtributosWoocomerce>();
        static List<Clases.TerminosAtributosCompletoWoocomerce> lista_editada_terminos = new List<Clases.TerminosAtributosCompletoWoocomerce>();
        static List<Clases.ArticulosWoocomerce> lista_articulos_editados = new List<Clases.ArticulosWoocomerce>();
        //*************LISTAS STATICAS QUE GUARDAN VALORES EDITADOS*********************

        public UC_Woocommerce()
        {
            InitializeComponent();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Agregar_Categoria_Woocomerce frm_categorias = new Agregar_Categoria_Woocomerce();
                frm_categorias.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }

        //*********************** CARGAR DE GRIDS DE FORMULARIO PRINCIPAL *******************************************//
        public void met_cargar_categorias() {

            dt_categorias.Rows.Clear();
            Metodos.MetodosCategoriasWoocomerce met_categorias = new Metodos.MetodosCategoriasWoocomerce();
        
            DataTable v_tabla = new DataTable();
            v_tabla = met_categorias.mostrar_categorias();

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                bool valor_check = true;
                string codigo_woocomerce = v_tabla.Rows[i]["CODIGO_WOOCOMERCE"].ToString();
                string nombre = v_tabla.Rows[i]["NOMBRE"].ToString();
                string descripcion = v_tabla.Rows[i]["DESCRIPCION"].ToString();
                string slug = v_tabla.Rows[i]["SLUG"].ToString();
                string categoria_pad = v_tabla.Rows[i]["PADRE"].ToString();
                string estado = v_tabla.Rows[i]["ESTADO"].ToString();

                if (estado == "S")
                {

                    valor_check = true;

                }
                else {

                    valor_check = false;

                }
               
                dt_categorias.Rows.Add(codigo_woocomerce, nombre, descripcion, slug, categoria_pad, valor_check);
            }

        }
        public void met_cargar_categorias_busqueda() {

            dt_categorias.Rows.Clear();

            Metodos.MetodosCategoriasWoocomerce met_categorias = new Metodos.MetodosCategoriasWoocomerce();
            WooCommerce.Clases.BusquedaCategoriasWoocomerce obj_categoria = new WooCommerce.Clases.BusquedaCategoriasWoocomerce();

            obj_categoria.dato = txt_categorias.Text;

            if (combo_categorias.Text == "Nombre")
            { obj_categoria.parametro = "N"; }
            else { obj_categoria.parametro = "C"; }

            DataTable v_tabla = new DataTable();
            v_tabla = met_categorias.buscar_categorias(obj_categoria);

            string codigo_woocomerce;
            bool valor_check = true;
            string nombre;
            string descripcion;
            string slug;
            string categoria_pad;
            string estado;


            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {


                Clases.CategoriasWoocomerce product = lista_editada_categorias.FirstOrDefault(x => x.id == Convert.ToInt32(v_tabla.Rows[i]["CODIGO_WOOCOMERCE"].ToString()));

                if (product == null)
                {
                    codigo_woocomerce = v_tabla.Rows[i]["CODIGO_WOOCOMERCE"].ToString();
                    nombre = v_tabla.Rows[i]["NOMBRE"].ToString();
                    descripcion = v_tabla.Rows[i]["DESCRIPCION"].ToString();
                    slug = v_tabla.Rows[i]["SLUG"].ToString();
                    categoria_pad = v_tabla.Rows[i]["PADRE"].ToString();
                    estado = v_tabla.Rows[i]["ESTADO"].ToString();

                    if (estado == "S")
                    {

                        valor_check = true;

                    }
                    else
                    {

                        valor_check = false;

                    }
                }
                else {

                    codigo_woocomerce = product.id.ToString();
                    nombre = product.name.ToString();
                    descripcion = product.description.ToString();
                    slug = product.slug.ToString();
                    categoria_pad = product.parent.ToString();
                    estado = product.estado.ToString();

                    if (estado == "S")
                    {

                        valor_check = true;

                    }
                    else
                    {

                        valor_check = false;

                    }

                }

               

                dt_categorias.Rows.Add(codigo_woocomerce, nombre, descripcion, slug, categoria_pad, valor_check);
            }

        }      
        public void met_cargar_atributos()
        {
            dt_atributos.Rows.Clear();
            Metodos.MetodosAtributosWoocomerce met_atributos = new Metodos.MetodosAtributosWoocomerce();
            DataTable v_tabla = new DataTable();
            v_tabla = met_atributos.mostrar_atributos();


            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                bool est = true;
                string id_atributo = v_tabla.Rows[i]["ID"].ToString();
                string nombre_atributo = v_tabla.Rows[i]["DESCRIPCION"].ToString();
                string estado = v_tabla.Rows[i]["ESTADO"].ToString();

                if (estado == "S") { est = true; } else { est = false; }

                dt_atributos.Rows.Add(id_atributo, nombre_atributo, est);
            }

        }
        public void met_cargar_atributos_busqueda()
        {

            dt_atributos.Rows.Clear();

            Metodos.MetodosAtributosWoocomerce met_atributos = new Metodos.MetodosAtributosWoocomerce();
            WooCommerce.Clases.BusquedaAtributos obj_atributos = new WooCommerce.Clases.BusquedaAtributos();

            obj_atributos.dato = txt_atributo.Text;

            if (combo_atributo.Text == "Nombre")
            { obj_atributos.parametro = "N"; }
            else { obj_atributos.parametro = "C"; }

            DataTable v_tabla = new DataTable();
            v_tabla = met_atributos.buscar_atributos(obj_atributos);

           

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                bool valor_check = true;
                string codigo_woocomerce;
                string descripcion;
                string estado;

                Clases.AtributosWoocomerce product = lista_editada_atributos.FirstOrDefault(x => x.id == Convert.ToInt32(codigo_woocomerce = v_tabla.Rows[i]["ID"].ToString()));

                if (product == null)
                {
                    codigo_woocomerce = v_tabla.Rows[i]["ID"].ToString();
                    descripcion = v_tabla.Rows[i]["DESCRIPCION"].ToString();
                    estado = v_tabla.Rows[i]["ESTADO"].ToString();
                    if (estado == "S") {valor_check = true;} else {valor_check = false;}
                }
                else {

                    codigo_woocomerce = product.id.ToString();
                    descripcion = product.name;
                    estado = product.estado;
                    if (estado == "S") {valor_check = true;} else{ valor_check = false; }

                }


                dt_atributos.Rows.Add(codigo_woocomerce, descripcion, valor_check);
            }

        }
        public void met_cargar_terminos_atributos()
        {
            dt_terninos_atributo.Rows.Clear();
            Metodos.MetodosTerminoAtributosWoocomerce met_atributos = new Metodos.MetodosTerminoAtributosWoocomerce();
            DataTable v_tabla = new DataTable();
            v_tabla = met_atributos.mostrar_terminos_atributos();


            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                bool est = true;
                string id_termino = v_tabla.Rows[i]["ID"].ToString();
                string nombre_termino = v_tabla.Rows[i]["NOMBRE"].ToString();
                string descripcion = v_tabla.Rows[i]["DESCRIPCION"].ToString();
                string slug = v_tabla.Rows[i]["SLUG"].ToString();
                string estado = v_tabla.Rows[i]["ESTADO"].ToString();
                string id_atributo = v_tabla.Rows[i]["ID_ATRIBUTO"].ToString();
                string nombre_atributo = v_tabla.Rows[i]["ATRIBUTO"].ToString();

                if (estado == "S") { est = true; } else { est = false; }

                dt_terninos_atributo.Rows.Add(id_termino, nombre_termino, descripcion, slug, est, id_atributo, nombre_atributo);
            }

        }
        public void met_cargar_terminos_atributos_busqueda()
        {

            dt_terninos_atributo.Rows.Clear();

            Metodos.MetodosTerminoAtributosWoocomerce met_atributos = new Metodos.MetodosTerminoAtributosWoocomerce();
            WooCommerce.Clases.BusquedaTerminoAtributosWoocomerce obj_atributos = new WooCommerce.Clases.BusquedaTerminoAtributosWoocomerce();

            obj_atributos.dato = txtnombre_termino.Text;

            if (combo_terminos.Text == "Nombre")
            { obj_atributos.parametro = "N"; }
            else { obj_atributos.parametro = "C"; }

            DataTable v_tabla = new DataTable();
            v_tabla = met_atributos.buscar_terminos_atributos(obj_atributos);

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {

                bool valor_check = true;
                string id_termino;
                string nombre;
                string descripcion;
                string slug;
                string id_atributo;
                string nombre_atributo;
                string estado;

                Clases.TerminosAtributosCompletoWoocomerce product = lista_editada_terminos.FirstOrDefault(x => x.id_termino_atributo == Convert.ToInt32(id_termino = v_tabla.Rows[i]["ID"].ToString()));

                if (product == null)
                {
                    id_termino = v_tabla.Rows[i]["ID"].ToString();
                    nombre = v_tabla.Rows[i]["NOMBRE"].ToString();
                    descripcion = v_tabla.Rows[i]["DESCRIPCION"].ToString();
                    slug = v_tabla.Rows[i]["SLUG"].ToString();
                    id_atributo = v_tabla.Rows[i]["ID_ATRIBUTO"].ToString();
                    nombre_atributo = v_tabla.Rows[i]["ATRIBUTO"].ToString();
                    estado = v_tabla.Rows[i]["ESTADO"].ToString();
                    if (estado == "S") { valor_check = true; } else { valor_check = false; }
                }
                else
                {

                    id_termino = product.id_termino_atributo.ToString();
                    nombre = product.name_termino_atributo;
                    descripcion = product.description_termino_atributo;
                    slug = product.slug.ToString();
                    id_atributo = product.id_atributo.ToString();
                    nombre_atributo = product.name_atributo;
                    estado = product.estado.ToString();         
                    if (estado == "True") { valor_check = true; } else { valor_check = false; }

                }

                dt_terninos_atributo.Rows.Add(id_termino, nombre, descripcion, slug, valor_check, id_atributo, nombre_atributo);
            }

        }
        public void met_cargar_terminos_atributos_busqueda_atributo()
        {

            dt_terninos_atributo.Rows.Clear();

            Metodos.MetodosTerminoAtributosWoocomerce met_atributos = new Metodos.MetodosTerminoAtributosWoocomerce();
            WooCommerce.Clases.BusquedaTerminoAtributosWoocomerce obj_atributos = new WooCommerce.Clases.BusquedaTerminoAtributosWoocomerce();

            obj_atributos.dato = (comobo_opcion_terminos.SelectedItem as ComboboxItem).Value.ToString(); ;

            DataTable v_tabla = new DataTable();
            v_tabla = met_atributos.buscar_terminos_atributos_atributo(obj_atributos);

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {

                bool valor_check = true;
                string id_termino;
                string nombre;
                string descripcion;
                string slug;
                string id_atributo;
                string nombre_atributo;
                string estado;

                Clases.TerminosAtributosCompletoWoocomerce product = lista_editada_terminos.FirstOrDefault(x => x.id_termino_atributo == Convert.ToInt32(id_termino = v_tabla.Rows[i]["ID"].ToString()));

                if (product == null)
                {
                    id_termino = v_tabla.Rows[i]["ID"].ToString();
                    nombre = v_tabla.Rows[i]["NOMBRE"].ToString();
                    descripcion = v_tabla.Rows[i]["DESCRIPCION"].ToString();
                    slug = v_tabla.Rows[i]["SLUG"].ToString();
                    id_atributo = v_tabla.Rows[i]["ID_ATRIBUTO"].ToString();
                    nombre_atributo = v_tabla.Rows[i]["ATRIBUTO"].ToString();
                    estado = v_tabla.Rows[i]["ESTADO"].ToString();
                    if (estado == "S") { valor_check = true; } else { valor_check = false; }
                }
                else
                {

                    id_termino = product.id_termino_atributo.ToString();
                    nombre = product.name_termino_atributo;
                    descripcion = product.description_termino_atributo;
                    slug = product.slug.ToString();
                    id_atributo = product.id_atributo.ToString();
                    nombre_atributo = product.name_atributo;
                    estado = product.estado.ToString();
                    if (estado == "True") { valor_check = true; } else { valor_check = false; }

                }

                dt_terninos_atributo.Rows.Add(id_termino, nombre, descripcion, slug, valor_check, id_atributo, nombre_atributo);
            }

        }
        public void met_cargar_articulos_sincronizados()
        {

            dt_articulos.Rows.Clear();
            Metodos.MetodosArticulosWoocomerce met_articulos = new Metodos.MetodosArticulosWoocomerce();
            DataTable v_tabla = new DataTable();
            v_tabla = met_articulos.mostrar_articulos_sincronizados();

            cargar_combo_estados_articulos();
            cargar_combo_tipos_articulo();
            cargar_combo_estados_stock();

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                string articulo = v_tabla.Rows[i]["ARTICULO"].ToString();
                string estado = v_tabla.Rows[i]["ESTADO_ARTICULO"].ToString();
                string id_woocomerce = v_tabla.Rows[i]["ID_WOOCOMERCE"].ToString();
                string nombre = v_tabla.Rows[i]["NOMBRE"].ToString();
                string nombre_woocomerce = v_tabla.Rows[i]["NOMBRE_WOOCOMERCE"].ToString();
                string descripcion = v_tabla.Rows[i]["DESCRIPCION"].ToString();
                string descripcion_woocomerce = v_tabla.Rows[i]["DESCRIPCION_WOOCOMERCE"].ToString();
                string descripcion_corta = v_tabla.Rows[i]["DESCRIPCION_CORTA_WOOCOMERCE"].ToString();
                string tipo_articulo = v_tabla.Rows[i]["TIPO_ARTICULO"].ToString();
                string stock = v_tabla.Rows[i]["STOCK"].ToString();
                string estado_stock = v_tabla.Rows[i]["ESTADO_STOCK"].ToString();
                string peso = v_tabla.Rows[i]["PESO"].ToString();
                string impuesto = v_tabla.Rows[i]["IMPUESTO"].ToString();
                string categorias = v_tabla.Rows[i]["ID_CATEGORIAS"].ToString();               
                string id_padre = v_tabla.Rows[i]["VARIACIONES"].ToString();
                string precio = v_tabla.Rows[i]["PRECIO"].ToString();
                string precio_descuento = v_tabla.Rows[i]["DESCUENTO"].ToString();

                bool maneja_stock = true;
                if (v_tabla.Rows[i]["MANEJA_STOCK"].ToString() == "S") { maneja_stock = true; } else { maneja_stock = false;  }

                bool destacado = true;
                if (v_tabla.Rows[i]["DESTACADO"].ToString() == "S") { destacado = true; } else { destacado = false; }
        
                string recor_date = v_tabla.Rows[i]["RECORD_DATE"].ToString();
                //string sincroniza = v_tabla.Rows[i]["SINCRONIZA"].ToString();

                dt_articulos.Rows.Add(

                    articulo,
                    nombre,
                    descripcion,
                    descripcion_corta,
                    estado,
                    id_woocomerce,
                    precio,
                    stock,
                    estado_stock,
                    impuesto,
                    categorias,
                    descripcion_woocomerce,
                    nombre_woocomerce,
                    precio_descuento,
                    peso,
                    tipo_articulo,
                    true,
                    null,
                    maneja_stock,
                    destacado,
                    recor_date


                    );
            }

        }
        public void met_cargar_metodos_envio()
        {

            data_envios.Rows.Clear();
            Metodos.MetodosEnvioWoocomerce met_atributos = new Metodos.MetodosEnvioWoocomerce();
            DataTable v_tabla = new DataTable();
            v_tabla = met_atributos.mostrar_metodos_envio();

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {

                bool esta = true;
                string id_metodo_pago = v_tabla.Rows[i]["ID_METODO_PAGO"].ToString();
                string descripcion_metodo = v_tabla.Rows[i]["DESCRIPCION_METODO"].ToString();
                string articulo = v_tabla.Rows[i]["ARTICULO"].ToString();
                string id_zona = v_tabla.Rows[i]["ID_ZONA"].ToString();
                string descripcion_zona = v_tabla.Rows[i]["DESCRIPCION_ZONA"].ToString();
                string precio = v_tabla.Rows[i]["PRECIO"].ToString();
                string estado = v_tabla.Rows[i]["ESTADO"].ToString();

                if (estado == "S") { esta = true; } else { esta = false; }

                data_envios.Rows.Add( articulo, precio, id_metodo_pago, descripcion_metodo, id_zona, descripcion_zona, esta);



            }

        }
        public void met_cargar_pedidos()
        {

            dt_pedidos.Rows.Clear();
            Metodos.MetodosPedidosWoocomerce met_pedidos = new Metodos.MetodosPedidosWoocomerce();
            DataTable v_tabla = new DataTable();
            v_tabla = met_pedidos.mostrar_pedidos();

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {

                
                string id_pedido = v_tabla.Rows[i]["ID_PEDIDO"].ToString();
                string number = v_tabla.Rows[i]["ID_PEDIDO"].ToString();
                string status = v_tabla.Rows[i]["STATUS"].ToString();
                string date_created = v_tabla.Rows[i]["DATE_CREATED"].ToString();
                string date_created_gtm = v_tabla.Rows[i]["DATE_CREATED_GTM"].ToString();
                string currency = v_tabla.Rows[i]["CURRENCY"].ToString();
                string discount_total = v_tabla.Rows[i]["DISCOUNT_TOTAL"].ToString();
                string discount_tax = v_tabla.Rows[i]["DISCOUNT_TAX"].ToString();
                string shipping_total = v_tabla.Rows[i]["SHIPPING_TOTAL"].ToString();
                string shipping_tax = v_tabla.Rows[i]["SHIPPING_TAX"].ToString();
                string card_tax = v_tabla.Rows[i]["CARD_TAX"].ToString();
                string total = v_tabla.Rows[i]["TOTAL"].ToString();
                string total_tax = v_tabla.Rows[i]["TOTAL_TAX"].ToString();
                string customer_id = v_tabla.Rows[i]["CUSTOMER_ID"].ToString();
                string first_name_billing = v_tabla.Rows[i]["FIRST_NAME_BILLING"].ToString();
                string last_name_billing = v_tabla.Rows[i]["LAST_NAME_BILLING"].ToString();
                string company_billing = v_tabla.Rows[i]["COMPANY_BILLING"].ToString();
                string address_1_billing = v_tabla.Rows[i]["ADDRESS_1_BILLING"].ToString();
                string address_2_billing = v_tabla.Rows[i]["ADDRESS_2_BILLING"].ToString();
                string city_billing = v_tabla.Rows[i]["CITY_BILLING"].ToString();
                string state_billing = v_tabla.Rows[i]["STATE_BILLING"].ToString();
                string post_code_billing = v_tabla.Rows[i]["POSTCODE_BILLING"].ToString();
                string country_billing = v_tabla.Rows[i]["COUNTRY_BILLING"].ToString();
                string email_billing = v_tabla.Rows[i]["EMAIL_BILLING"].ToString();
                string phone_billing = v_tabla.Rows[i]["PHONE_BILLING"].ToString();
                string first_name_shipping = v_tabla.Rows[i]["FIRST_NAME_BILLING"].ToString();
                string last_name_shipping = v_tabla.Rows[i]["LAST_NAME_SHIPPING"].ToString();
                string company_shipping = v_tabla.Rows[i]["COMPANY_SHIPPING"].ToString();
                string addres_1_shipping = v_tabla.Rows[i]["ADDRESS_1_SHIPPING"].ToString();
                string addres_2_shipping = v_tabla.Rows[i]["ADDRESS_2_SHIPPING"].ToString();
                string city_shipping = v_tabla.Rows[i]["CITY_SHIPPING"].ToString();
                string state_shipping = v_tabla.Rows[i]["STATE_SHIPPING"].ToString();
                string postcode_shipping = v_tabla.Rows[i]["POSTCODE_SHIPPING"].ToString();
                string country_shipping = v_tabla.Rows[i]["COUNTRY_SHIPPING"].ToString();
                string payment_method = v_tabla.Rows[i]["PAYMENT_METHOD"].ToString();
                string payment_method_titllee = v_tabla.Rows[i]["PAYMENT_METHOD_TITLE"].ToString();
                string transaction_idd = v_tabla.Rows[i]["TRANSACTION_ID"].ToString();
                string customer_note = v_tabla.Rows[i]["CUSTOMER_NOTE"].ToString();
                string date_paidment = v_tabla.Rows[i]["DATE_PAID"].ToString();
                string date_paid_gtm = v_tabla.Rows[i]["DATE_PAID_GMT"].ToString();
                string consecutivo = v_tabla.Rows[i]["CONSECUTIVO"].ToString();
                string created_date = v_tabla.Rows[i]["CREATEDATE"].ToString();
                string record_date = v_tabla.Rows[i]["RECORDDATE"].ToString();

                dt_pedidos.Rows.Add( id_pedido, number, consecutivo, status, date_created, date_created_gtm, currency, discount_total , discount_tax, 
                shipping_total, shipping_tax, card_tax, total, total_tax, customer_id, first_name_billing, last_name_billing, company_billing, address_1_billing,
                address_2_billing, city_billing, state_billing, post_code_billing, country_billing, email_billing, phone_billing, first_name_shipping,
                last_name_shipping, company_shipping, addres_1_shipping, addres_2_shipping, city_shipping, state_shipping, postcode_shipping,
                country_shipping, payment_method, payment_method_titllee, transaction_idd, customer_note, date_paidment, date_paid_gtm,
                created_date, record_date
                    
                );



            }

        }
        public void met_cargar_pedidos_busqueda()
        {

            dt_pedidos.Rows.Clear();

            Metodos.MetodosPedidosWoocomerce met_pedidos = new Metodos.MetodosPedidosWoocomerce();
            WooCommerce.Clases.PedidosBusquedaWoocomerce obj_pedidos = new WooCommerce.Clases.PedidosBusquedaWoocomerce();

            obj_pedidos.dato = txtpedidos.Text;

            if (combo_pedidos.Text == "Consecutivo")
            { obj_pedidos.parametro = "N"; }
            else { obj_pedidos.parametro = "C"; }

            DataTable v_tabla = new DataTable();
            v_tabla = met_pedidos.buscar_pedidos(obj_pedidos);

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                string id_pedido = v_tabla.Rows[i]["ID_PEDIDO"].ToString();
                string number = v_tabla.Rows[i]["ID_PEDIDO"].ToString();
                string status = v_tabla.Rows[i]["STATUS"].ToString();
                string date_created = v_tabla.Rows[i]["DATE_CREATED"].ToString();
                string date_created_gtm = v_tabla.Rows[i]["DATE_CREATED_GTM"].ToString();
                string currency = v_tabla.Rows[i]["CURRENCY"].ToString();
                string discount_total = v_tabla.Rows[i]["DISCOUNT_TOTAL"].ToString();
                string discount_tax = v_tabla.Rows[i]["DISCOUNT_TAX"].ToString();
                string shipping_total = v_tabla.Rows[i]["SHIPPING_TOTAL"].ToString();
                string shipping_tax = v_tabla.Rows[i]["SHIPPING_TAX"].ToString();
                string card_tax = v_tabla.Rows[i]["CARD_TAX"].ToString();
                string total = v_tabla.Rows[i]["TOTAL"].ToString();
                string total_tax = v_tabla.Rows[i]["TOTAL_TAX"].ToString();
                string customer_id = v_tabla.Rows[i]["CUSTOMER_ID"].ToString();
                string first_name_billing = v_tabla.Rows[i]["FIRST_NAME_BILLING"].ToString();
                string last_name_billing = v_tabla.Rows[i]["LAST_NAME_BILLING"].ToString();
                string company_billing = v_tabla.Rows[i]["COMPANY_BILLING"].ToString();
                string address_1_billing = v_tabla.Rows[i]["ADDRESS_1_BILLING"].ToString();
                string address_2_billing = v_tabla.Rows[i]["ADDRESS_2_BILLING"].ToString();
                string city_billing = v_tabla.Rows[i]["CITY_BILLING"].ToString();
                string state_billing = v_tabla.Rows[i]["STATE_BILLING"].ToString();
                string post_code_billing = v_tabla.Rows[i]["POSTCODE_BILLING"].ToString();
                string country_billing = v_tabla.Rows[i]["COUNTRY_BILLING"].ToString();
                string email_billing = v_tabla.Rows[i]["EMAIL_BILLING"].ToString();
                string phone_billing = v_tabla.Rows[i]["PHONE_BILLING"].ToString();
                string first_name_shipping = v_tabla.Rows[i]["FIRST_NAME_BILLING"].ToString();
                string last_name_shipping = v_tabla.Rows[i]["LAST_NAME_SHIPPING"].ToString();
                string company_shipping = v_tabla.Rows[i]["COMPANY_SHIPPING"].ToString();
                string addres_1_shipping = v_tabla.Rows[i]["ADDRESS_1_SHIPPING"].ToString();
                string addres_2_shipping = v_tabla.Rows[i]["ADDRESS_2_SHIPPING"].ToString();
                string city_shipping = v_tabla.Rows[i]["CITY_SHIPPING"].ToString();
                string state_shipping = v_tabla.Rows[i]["STATE_SHIPPING"].ToString();
                string postcode_shipping = v_tabla.Rows[i]["POSTCODE_SHIPPING"].ToString();
                string country_shipping = v_tabla.Rows[i]["COUNTRY_SHIPPING"].ToString();
                string payment_method = v_tabla.Rows[i]["PAYMENT_METHOD"].ToString();
                string payment_method_titllee = v_tabla.Rows[i]["PAYMENT_METHOD_TITLE"].ToString();
                string transaction_idd = v_tabla.Rows[i]["TRANSACTION_ID"].ToString();
                string customer_note = v_tabla.Rows[i]["CUSTOMER_NOTE"].ToString();
                string date_paidment = v_tabla.Rows[i]["DATE_PAID"].ToString();
                string date_paid_gtm = v_tabla.Rows[i]["DATE_PAID_GMT"].ToString();
                string consecutivo = v_tabla.Rows[i]["CONSECUTIVO"].ToString();
                string created_date = v_tabla.Rows[i]["CREATEDATE"].ToString();
                string record_date = v_tabla.Rows[i]["RECORDDATE"].ToString();

                dt_pedidos.Rows.Add(id_pedido, number, consecutivo, status, date_created, date_created_gtm, currency, discount_total, discount_tax,
                shipping_total, shipping_tax, card_tax, total, total_tax, customer_id, first_name_billing, last_name_billing, company_billing, address_1_billing,
                address_2_billing, city_billing, state_billing, post_code_billing, country_billing, email_billing, phone_billing, first_name_shipping,
                last_name_shipping, company_shipping, addres_1_shipping, addres_2_shipping, city_shipping, state_shipping, postcode_shipping,
                country_shipping, payment_method, payment_method_titllee, transaction_idd, customer_note, date_paidment, date_paid_gtm,
                created_date, record_date

                );

            }

        }
        public void met_cargar_pedidos_busqueda_fecha()
        {

            dt_pedidos.Rows.Clear();

            Metodos.MetodosPedidosWoocomerce met_pedidos = new Metodos.MetodosPedidosWoocomerce();
            WooCommerce.Clases.PedidosBusquedaFechaWoocomerce obj_pedidos = new WooCommerce.Clases.PedidosBusquedaFechaWoocomerce();

            obj_pedidos.fecha_ini = dt_fecha_ini.Text;
            obj_pedidos.fecha_fin = dt_fecha_fin.Text;

            DataTable v_tabla = new DataTable();
            v_tabla = met_pedidos.buscar_pedidos_fecha(obj_pedidos);

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                string id_pedido = v_tabla.Rows[i]["ID_PEDIDO"].ToString();
                string number = v_tabla.Rows[i]["ID_PEDIDO"].ToString();
                string status = v_tabla.Rows[i]["STATUS"].ToString();
                string date_created = v_tabla.Rows[i]["DATE_CREATED"].ToString();
                string date_created_gtm = v_tabla.Rows[i]["DATE_CREATED_GTM"].ToString();
                string currency = v_tabla.Rows[i]["CURRENCY"].ToString();
                string discount_total = v_tabla.Rows[i]["DISCOUNT_TOTAL"].ToString();
                string discount_tax = v_tabla.Rows[i]["DISCOUNT_TAX"].ToString();
                string shipping_total = v_tabla.Rows[i]["SHIPPING_TOTAL"].ToString();
                string shipping_tax = v_tabla.Rows[i]["SHIPPING_TAX"].ToString();
                string card_tax = v_tabla.Rows[i]["CARD_TAX"].ToString();
                string total = v_tabla.Rows[i]["TOTAL"].ToString();
                string total_tax = v_tabla.Rows[i]["TOTAL_TAX"].ToString();
                string customer_id = v_tabla.Rows[i]["CUSTOMER_ID"].ToString();
                string first_name_billing = v_tabla.Rows[i]["FIRST_NAME_BILLING"].ToString();
                string last_name_billing = v_tabla.Rows[i]["LAST_NAME_BILLING"].ToString();
                string company_billing = v_tabla.Rows[i]["COMPANY_BILLING"].ToString();
                string address_1_billing = v_tabla.Rows[i]["ADDRESS_1_BILLING"].ToString();
                string address_2_billing = v_tabla.Rows[i]["ADDRESS_2_BILLING"].ToString();
                string city_billing = v_tabla.Rows[i]["CITY_BILLING"].ToString();
                string state_billing = v_tabla.Rows[i]["STATE_BILLING"].ToString();
                string post_code_billing = v_tabla.Rows[i]["POSTCODE_BILLING"].ToString();
                string country_billing = v_tabla.Rows[i]["COUNTRY_BILLING"].ToString();
                string email_billing = v_tabla.Rows[i]["EMAIL_BILLING"].ToString();
                string phone_billing = v_tabla.Rows[i]["PHONE_BILLING"].ToString();
                string first_name_shipping = v_tabla.Rows[i]["FIRST_NAME_BILLING"].ToString();
                string last_name_shipping = v_tabla.Rows[i]["LAST_NAME_SHIPPING"].ToString();
                string company_shipping = v_tabla.Rows[i]["COMPANY_SHIPPING"].ToString();
                string addres_1_shipping = v_tabla.Rows[i]["ADDRESS_1_SHIPPING"].ToString();
                string addres_2_shipping = v_tabla.Rows[i]["ADDRESS_2_SHIPPING"].ToString();
                string city_shipping = v_tabla.Rows[i]["CITY_SHIPPING"].ToString();
                string state_shipping = v_tabla.Rows[i]["STATE_SHIPPING"].ToString();
                string postcode_shipping = v_tabla.Rows[i]["POSTCODE_SHIPPING"].ToString();
                string country_shipping = v_tabla.Rows[i]["COUNTRY_SHIPPING"].ToString();
                string payment_method = v_tabla.Rows[i]["PAYMENT_METHOD"].ToString();
                string payment_method_titllee = v_tabla.Rows[i]["PAYMENT_METHOD_TITLE"].ToString();
                string transaction_idd = v_tabla.Rows[i]["TRANSACTION_ID"].ToString();
                string customer_note = v_tabla.Rows[i]["CUSTOMER_NOTE"].ToString();
                string date_paidment = v_tabla.Rows[i]["DATE_PAID"].ToString();
                string date_paid_gtm = v_tabla.Rows[i]["DATE_PAID_GMT"].ToString();
                string consecutivo = v_tabla.Rows[i]["CONSECUTIVO"].ToString();
                string created_date = v_tabla.Rows[i]["CREATEDATE"].ToString();
                string record_date = v_tabla.Rows[i]["RECORDDATE"].ToString();

                dt_pedidos.Rows.Add(id_pedido, number, consecutivo, status, date_created, date_created_gtm, currency, discount_total, discount_tax,
                shipping_total, shipping_tax, card_tax, total, total_tax, customer_id, first_name_billing, last_name_billing, company_billing, address_1_billing,
                address_2_billing, city_billing, state_billing, post_code_billing, country_billing, email_billing, phone_billing, first_name_shipping,
                last_name_shipping, company_shipping, addres_1_shipping, addres_2_shipping, city_shipping, state_shipping, postcode_shipping,
                country_shipping, payment_method, payment_method_titllee, transaction_idd, customer_note, date_paidment, date_paid_gtm,
                created_date, record_date

                );

            }

        }
        public void met_cargar_bitacora_errores()
        {
            dt_bitacora_errores.Rows.Clear();
            Metodos.MetodosBitacorasWoocomerce met_bitacoras = new Metodos.MetodosBitacorasWoocomerce();
            DataTable v_tabla = new DataTable();
            v_tabla = met_bitacoras.mostrar_bitacoras();


            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
            
                string id_bitacora = v_tabla.Rows[i]["IDBITACORA"].ToString();
                string fecha = v_tabla.Rows[i]["FECHA"].ToString();
                string procedimiento = v_tabla.Rows[i]["PROCEDIMIENTO"].ToString();
                string error = v_tabla.Rows[i]["ERROR"].ToString();
                string modulo = v_tabla.Rows[i]["MODULO"].ToString();

                dt_bitacora_errores.Rows.Add(id_bitacora, fecha, procedimiento, error, modulo);
            }

        }
        public void met_cargar_bitacora_por_modulo()
        {

            dt_bitacora_errores.Rows.Clear();

            Metodos.MetodosBitacorasWoocomerce met_bitacora = new Metodos.MetodosBitacorasWoocomerce();
            WooCommerce.Clases.BitacorasWoocomerce obj_bitacora = new WooCommerce.Clases.BitacorasWoocomerce();

            obj_bitacora.modulo = combo_modulos.Text;

            DataTable v_tabla = new DataTable();
            v_tabla = met_bitacora.buscar_bitacora_modulo(obj_bitacora);

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {

                string id_bitacora = v_tabla.Rows[i]["IDBITACORA"].ToString();
                string fecha = v_tabla.Rows[i]["FECHA"].ToString();
                string procedimiento = v_tabla.Rows[i]["PROCEDIMIENTO"].ToString();
                string error = v_tabla.Rows[i]["ERROR"].ToString();
                string modulo = v_tabla.Rows[i]["MODULO"].ToString();

                dt_bitacora_errores.Rows.Add(id_bitacora, fecha, procedimiento, error, modulo);
            }

        }
        public void met_cargar_bitacora_por_fechas() {

            dt_bitacora_errores.Rows.Clear();

            Metodos.MetodosBitacorasWoocomerce met_bitacoras = new Metodos.MetodosBitacorasWoocomerce();
            WooCommerce.Clases.BitacorasFechasWoocomerce obj_bitacoras = new WooCommerce.Clases.BitacorasFechasWoocomerce();

            obj_bitacoras.fecha_ini = date_bi_fecha_ini.Text;
            obj_bitacoras.fecha_fin = date_bi_fecha_fin.Text;

            DataTable v_tabla = new DataTable();
            v_tabla = met_bitacoras.buscar_bitacora_por_fechas(obj_bitacoras);

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {

                string id_bitacora = v_tabla.Rows[i]["IDBITACORA"].ToString();
                string fecha = v_tabla.Rows[i]["FECHA"].ToString();
                string procedimiento = v_tabla.Rows[i]["PROCEDIMIENTO"].ToString();
                string error = v_tabla.Rows[i]["ERROR"].ToString();
                string modulo = v_tabla.Rows[i]["MODULO"].ToString();

                dt_bitacora_errores.Rows.Add(id_bitacora, fecha, procedimiento, error, modulo);
            }


        }

        //*********************** CARGAR DE GRIDS DE FORMULARIO PRINCIPAL *******************************************//





        //*********************** CARGAR DE COMBO BOX *******************************************//

        //Matodo que carga el combo box de atributos
        public void cargar_atributos()
        {

            comobo_opcion_terminos.Items.Clear();
            Metodos.MetodosAtributosWoocomerce met_categorias = new Metodos.MetodosAtributosWoocomerce();

            DataTable v_tabla = new DataTable();
            v_tabla = met_categorias.mostrar_atributos();

            ComboboxItem item = new ComboboxItem();
            item.Text = "  " + "TD" + " - " + "Todos";
            item.Value = "ND";
            comobo_opcion_terminos.Items.Add(item);

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {
                ComboboxItem item2 = new ComboboxItem();
                item2.Text =  "  " + v_tabla.Rows[i]["ID"].ToString() + " - " + v_tabla.Rows[i]["DESCRIPCION"].ToString();
                item2.Value = v_tabla.Rows[i]["ID"].ToString();
                comobo_opcion_terminos.Items.Add(item2);
            }

            comobo_opcion_terminos.SelectedIndex = 0;
        }
        public void cargar_combo_estados_articulos()
        {

            Metodos.MetodosArticulosWoocomerce met_cargar_estados = new Metodos.MetodosArticulosWoocomerce();
            List<Clases.EstadosArticuloWoocomerce> lista_estados = new List<Clases.EstadosArticuloWoocomerce>();
            lista_estados = met_cargar_estados.obtener_estados_articulos();
            ComboBox CB = new ComboBox();

            int i = 0;

            while (i < lista_estados.Count)
            {
                CB.Items.Add(lista_estados[i].descripcion);
                i++;
            }

           ((DataGridViewComboBoxColumn)dt_articulos.Columns["status_producto"]).DataSource = CB.Items;

        }
        //Metodo de tipos de articulos
        public void cargar_combo_tipos_articulo()
        {

            Metodos.MetodosArticulosWoocomerce met_cargar_estados = new Metodos.MetodosArticulosWoocomerce();
            List<Clases.TipoArticuloWoocomerce> lista_estados = new List<Clases.TipoArticuloWoocomerce>();
            lista_estados = met_cargar_estados.obtener_tipos_articulo();
            ComboBox CB = new ComboBox();

            int i = 0;

            while (i < lista_estados.Count)
            {
                CB.Items.Add(lista_estados[i].descripcion);
                i++;
            }

            ((DataGridViewComboBoxColumn)dt_articulos.Columns["tipo_articulo"]).DataSource = CB.Items;

        }
        //Metodo de tipos de articulos
        public void cargar_combo_estados_stock()
        {

            Metodos.MetodosArticulosWoocomerce met_cargar_estados = new Metodos.MetodosArticulosWoocomerce();
            List<Clases.EstadosStockWoocomerce> lista_estados = new List<Clases.EstadosStockWoocomerce>();
            lista_estados = met_cargar_estados.obtener_estados_stock();
            ComboBox CB = new ComboBox();

            int i = 0;

            while (i < lista_estados.Count)
            {
                CB.Items.Add(lista_estados[i].descripcion);
                i++;
            }

            ((DataGridViewComboBoxColumn)dt_articulos.Columns["estado_stock"]).DataSource = CB.Items;

        }
        //Cargar combo de modulos de pedidos
        public void cargar_combo_modulos_bitacora()
        {

            combo_modulos.Items.Clear();
            Metodos.MetodosBitacorasWoocomerce met_bitacora = new Metodos.MetodosBitacorasWoocomerce();

            DataTable v_tabla = new DataTable();
            v_tabla = met_bitacora.mostrar_modulos_bitacora();

          
            string ini =  "Todos";
            combo_modulos.Items.Add(ini);

            for (int i = 0; i < v_tabla.Rows.Count; i++)
            {

                string item2 = v_tabla.Rows[i]["MODULO"].ToString();
                combo_modulos.Items.Add(item2);
            }

            combo_modulos.Text = "Todos";

        }

        //*********************** CARGAR DE COMBO BOX  *******************************************//




        //*********************** CATEGORIAS *******************************************//
        //Refrescar categorias
        private void toolStripButton10_Click(object sender, EventArgs e)
        {

            try
            {
                met_cargar_categorias();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }

        //Sincronizar categorias actualizadas
        private void toolStripButton11_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult result =
            MessageBox.Show("Al ejecutar esta acción los cambios de categorías se verán reflejados en su tienda Woocomerce:" + "\n" + tienda,
            "◄ ¿Está seguro que desea ejecutar esta acción? ►", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosCategoriasWoocomerce met_categorias = new Metodos.MetodosCategoriasWoocomerce();
                    List<Clases.CategoriasWoocomerce> list_cate = new List<Clases.CategoriasWoocomerce>();

                    //devuelve si algun termino entro con error
                    string json_categorias = met_categorias.obtener_categorias_tablas_propias_editadas();


                    //Deserealizamos la respuesta
                    list_cate = JsonConvert.DeserializeObject<List<Clases.CategoriasWoocomerce>>(json_categorias);

                    if (list_cate.Count > 0)
                    {
                        int i = 0;

                        while (i < list_cate.Count)
                        {

                            Metodos.MetodosCategoriasWoocomerce met_cat = new Metodos.MetodosCategoriasWoocomerce();

                            met_cat.actualizar_categorias_woocomerce(list_cate[i]);

                            i++;

                        }

                        MessageBox.Show("Se finalizó el proceso de sincronización de categorías: ", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No hay categorías por actualizar", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    lista_editada_categorias.Clear();
                    met_cargar_categorias();
                    btn_guardar_categorias.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        //Sincronizar categorias del woocomerce a nuestras tablas
        private void toolStripButton12_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult result =
                MessageBox.Show("Al ejecutar esta acción las categorías de su tienda Woocomerce:" + " " + tienda + " se sincronizán con el sistema" + "\n\n ◄ El proceso puede demorar unos minutos \n por favor espere ► ",
                "◄ ¿Está seguro que desea ejecutar esta acción? ► ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosCategoriasWoocomerce met_categorias_borrar = new Metodos.MetodosCategoriasWoocomerce();
                    met_categorias_borrar.limpiar_tabla_categorias();

                    Metodos.MetodosCategoriasWoocomerce met_categorias = new Metodos.MetodosCategoriasWoocomerce();
                    //Sincronizar los atributos a nuestro sistema          
                    MessageBox.Show(met_categorias.insertar_categorias_tablas_propias(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    met_cargar_categorias();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }



           

        }
        private void btn_guardar_categorias_Click(object sender, EventArgs e)
        {
            try
            {
                if (lista_editada_categorias.Count > 0)
                {

                    int i = 0;

                    while (i < lista_editada_categorias.Count)
                    {

                        Metodos.MetodosCategoriasWoocomerce met_categorias = new Metodos.MetodosCategoriasWoocomerce();

                        met_categorias.insertar_categorias(lista_editada_categorias[i]);

                        i++;

                    }

                    MessageBox.Show("Categorías actualizadas exitosamente", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    lista_editada_categorias.Clear();
                    met_cargar_categorias();
                    btn_guardar_categorias.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


           

        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult result =
                    MessageBox.Show("Al ejecutar esta acción las categorías en estado de inactividad se eliminarán de su tienda Woocomerce:" + " " + tienda + "" + "\n\n ◄ El proceso puede demorar unos minutos \n por favor espere ► ",
                    "◄ ¿Está seguro que desea ejecutar esta acción? ► ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosCategoriasWoocomerce met_categorias_borrar = new Metodos.MetodosCategoriasWoocomerce();
                    met_categorias_borrar.eliminarr_categorias_woocomerce();
                    MessageBox.Show("Categorías eliminadas con éxito", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    met_cargar_categorias();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void txt_categorias_Click(object sender, EventArgs e)
        {

        }
        private void txt_categorias_TextChanged(object sender, EventArgs e)
        {
            met_cargar_categorias_busqueda();
        }

        //************************METODOS DEL GREED PARA ACTUALIZAR CATEGORIAS *********************************************
        private void dt_categorias_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Clases.CategoriasWoocomerce obj_categoria = new Clases.CategoriasWoocomerce();

            obj_categoria.id = Convert.ToInt32(this.dt_categorias.CurrentRow.Cells[0].Value.ToString());
            obj_categoria.name = this.dt_categorias.CurrentRow.Cells[1].Value.ToString();
            obj_categoria.description = this.dt_categorias.CurrentRow.Cells[2].Value.ToString();
            obj_categoria.slug = this.dt_categorias.CurrentRow.Cells[3].Value.ToString();
            obj_categoria.parent = Convert.ToInt32(this.dt_categorias.CurrentRow.Cells[4].Value.ToString());
            obj_categoria.estado = this.dt_categorias.CurrentRow.Cells[5].Value.ToString();


            Clases.CategoriasWoocomerce product = lista_editada_categorias.FirstOrDefault(x => x.id == Convert.ToInt32(this.dt_categorias.CurrentRow.Cells[0].Value.ToString()));

            if (product == null)
            {
                lista_editada_categorias.Add(obj_categoria);
            }
            else
            {

                lista_editada_categorias.RemoveAll(x => x.id == (obj_categoria.id));
                lista_editada_categorias.Add(obj_categoria);

            }

            btn_guardar_categorias.Enabled = true;

        }
        private void dt_categorias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            dt_categorias.EndEdit();

            //16 columna de sincro

            if (e.ColumnIndex == 5)
            {

                Clases.CategoriasWoocomerce obj_categprias = new Clases.CategoriasWoocomerce();
                obj_categprias.id = Convert.ToInt32(this.dt_categorias.CurrentRow.Cells[0].Value.ToString());
                obj_categprias.estado = this.dt_categorias.CurrentRow.Cells[5].Value.ToString();


                Clases.CategoriasWoocomerce product = lista_eliminar_categorias.FirstOrDefault(x => x.id == Convert.ToInt32(this.dt_categorias.CurrentRow.Cells[0].Value.ToString()));

                if (product == null)
                {
                    lista_eliminar_categorias.Add(obj_categprias);
                }
                else
                {

                    lista_eliminar_categorias.RemoveAll(x => x.id == (obj_categprias.id));
                    lista_eliminar_categorias.Add(obj_categprias);

                }

                dt_categorias.EndEdit();

            }

        }

        //************************METODOS DEL GREED PARA ACTUALIZAR CATEGORIAS *********************************************
        //*********************** CATEGORIAS *******************************************//




        //*********************** ATRIBUTOS *******************************************//
        private void btn_refrescar_atributos_Click(object sender, EventArgs e)
        {

            try
            {
                met_cargar_atributos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
          
        }
        private void btn_agregar_atributo_Click(object sender, EventArgs e)
        {

            try
            {
                Agregar_Nuevo_Atributo frm_atributo = new Agregar_Nuevo_Atributo();
                frm_atributo.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {

            try
            {
               DialogResult result =
              MessageBox.Show("Al ejecutar esta acción los atributos de su tienda Woocomerce:" + " " + tienda + " se sincronizán con el sistema \nEl proceso puede demorar unos minutos por favor espere ",
             "◄ ¿Está seguro que desea ejecutar esta acción? ►", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosAtributosWoocomerce met_atributos = new Metodos.MetodosAtributosWoocomerce();
                    //Sincronizar los atributos a nuestro sistema
                    Metodos.MetodosAtributosWoocomerce met_atributos_eliminar = new Metodos.MetodosAtributosWoocomerce();
                    met_atributos_eliminar.limpiar_atributos();
                    MessageBox.Show(met_atributos.insertar_atributos(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    met_cargar_atributos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


          
        }
        private void txt_atributo_TextChanged(object sender, EventArgs e)
        {
            met_cargar_atributos_busqueda();
        }
        private void dt_atributos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Clases.AtributosWoocomerce obj_atributo = new Clases.AtributosWoocomerce();

            obj_atributo.id = Convert.ToInt32(this.dt_atributos.CurrentRow.Cells[0].Value.ToString());
            obj_atributo.name = this.dt_atributos.CurrentRow.Cells[1].Value.ToString();
            obj_atributo.estado = this.dt_atributos.CurrentRow.Cells[2].Value.ToString();


            Clases.AtributosWoocomerce product = lista_editada_atributos.FirstOrDefault(x => x.id == Convert.ToInt32(this.dt_atributos.CurrentRow.Cells[0].Value.ToString()));

            if (product == null)
            {
                lista_editada_atributos.Add(obj_atributo);
            }
            else
            {

                lista_editada_atributos.RemoveAll(x => x.id == (obj_atributo.id));
                lista_editada_atributos.Add(obj_atributo);

            }

            btn_guardar_atributos.Enabled = true;
        }
        private void dt_atributos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dt_atributos.EndEdit();

            if (e.ColumnIndex == 2)
            {

                Clases.AtributosWoocomerce obj_atributos = new Clases.AtributosWoocomerce();
                obj_atributos.id = Convert.ToInt32(this.dt_atributos.CurrentRow.Cells[0].Value.ToString());
                obj_atributos.estado = this.dt_atributos.CurrentRow.Cells[2].Value.ToString();


                Clases.AtributosWoocomerce product = lista_eliminar_atributos.FirstOrDefault(x => x.id == Convert.ToInt32(this.dt_atributos.CurrentRow.Cells[0].Value.ToString()));

                if (product == null)
                {
                    lista_eliminar_atributos.Add(obj_atributos);
                }
                else
                {

                    lista_eliminar_atributos.RemoveAll(x => x.id == (obj_atributos.id));
                    lista_eliminar_atributos.Add(obj_atributos);

                }

                dt_atributos.EndEdit();

            }
        }
        private void btn_guardar_atributos_Click(object sender, EventArgs e)
        {

            try
            {
                if (lista_editada_atributos.Count > 0)
                {
                    Metodos.MetodosAtributosWoocomerce met_atributos = new Metodos.MetodosAtributosWoocomerce();

                    //Convertimos las lista de atributos a editar para enviarlos al API
                    string json_obj_atributos = JsonConvert.SerializeObject(lista_editada_atributos);

                    met_atributos.actualizar_atributos(json_obj_atributos);

                    MessageBox.Show("Atributos actualizadas exitosamente", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    lista_editada_atributos.Clear();
                    met_cargar_atributos();
                    btn_guardar_atributos.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult result =
                MessageBox.Show("Al ejecutar esta acción los cambios de atributos se verán reflejados en su tienda Woocomerce:" + "\n" + tienda,
                "◄ ¿Está seguro que desea ejecutar esta acción? ►", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {

                    Metodos.MetodosAtributosWoocomerce met_atributo = new Metodos.MetodosAtributosWoocomerce();
                    met_atributo.actualizar_un_atributo_woocomerce();

                    lista_editada_atributos.Clear();
                    met_cargar_atributos();
                    btn_guardar_atributos.Enabled = false;

                    MessageBox.Show("Se finalizó el proceso de sincronización de atributos: ", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
            //else
            //{
            //    MessageBox.Show("No hay atributos por actualizar", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}    
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult result =
                    MessageBox.Show("Al ejecutar esta acción los atributos en estado de inactividad se eliminarán de su tienda Woocomerce:" + " " + tienda + "" + "\n\n *Esto también eliminará todos los términos del atributo asociados* \n\n ◄ El proceso puede demorar unos minutos \n por favor espere ► ",
                    "◄ ¿Está seguro que desea ejecutar esta acción? ► ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosAtributosWoocomerce met_atributos = new Metodos.MetodosAtributosWoocomerce();             
                    MessageBox.Show(met_atributos.eliminar_atributos_de_nuestras_tablas(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    met_cargar_atributos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

          
            
        }
        private void txt_atributo_Click(object sender, EventArgs e)
        {

        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Agregar_Terminos_Atributo frm_termino_atributo = new Agregar_Terminos_Atributo();
            frm_termino_atributo.ShowDialog();
        }      
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            met_cargar_terminos_atributos();
            cargar_atributos();
        }

        //*********************** ATRIBUTOS *******************************************//



        //*********************** TERMINOS ATRIBUTOS *******************************************//
        private void txtnombre_termino_TextChanged(object sender, EventArgs e)
        {
            met_cargar_terminos_atributos_busqueda();
        }
        private void txtnombre_termino_Click(object sender, EventArgs e)
        {

        }
        private void dt_terninos_atributo_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {

            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {

            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {

            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {

            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";
                anError.ThrowException = false;
            }
        }
        private void toolStripButton15_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult result =
               MessageBox.Show("Al ejecutar esta acción los términos de atributos de su tienda Woocomerce:" + " " + tienda + " se sincronizán con el sistema \nEl proceso puede demorar unos minutos por favor espere ",
              "◄ ¿Está seguro que desea ejecutar esta acción? ►", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosTerminoAtributosWoocomerce met_atributos = new Metodos.MetodosTerminoAtributosWoocomerce();
                    //Sincronizar los atributos a nuestro sistema
                    Metodos.MetodosTerminoAtributosWoocomerce met_atributos_eliminar = new Metodos.MetodosTerminoAtributosWoocomerce();
                    met_atributos_eliminar.limpiar_terminos_atributos();
                    MessageBox.Show(met_atributos.insertar_terminos_atributo(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    met_cargar_terminos_atributos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
        private void comobo_opcion_terminos_SelectedIndexChanged(object sender, EventArgs e)
        {
            met_cargar_terminos_atributos_busqueda_atributo();
        }
        private void dt_terninos_atributo_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Clases.TerminosAtributosCompletoWoocomerce obj_termino = new Clases.TerminosAtributosCompletoWoocomerce();

            obj_termino.id_termino_atributo = Convert.ToInt32(this.dt_terninos_atributo.CurrentRow.Cells[0].Value.ToString());
            obj_termino.name_termino_atributo = this.dt_terninos_atributo.CurrentRow.Cells[1].Value.ToString();
            obj_termino.description_termino_atributo = this.dt_terninos_atributo.CurrentRow.Cells[2].Value.ToString();
            obj_termino.slug = this.dt_terninos_atributo.CurrentRow.Cells[3].Value.ToString();
            obj_termino.estado = this.dt_terninos_atributo.CurrentRow.Cells[4].Value.ToString();
            obj_termino.id_atributo = Convert.ToInt32(this.dt_terninos_atributo.CurrentRow.Cells[5].Value.ToString());
            obj_termino.name_atributo = this.dt_terninos_atributo.CurrentRow.Cells[6].Value.ToString();

            Clases.TerminosAtributosCompletoWoocomerce product = lista_editada_terminos.FirstOrDefault(x => x.id_termino_atributo == Convert.ToInt32(this.dt_terninos_atributo.CurrentRow.Cells[0].Value.ToString()));

            if (product == null)
            {
                lista_editada_terminos.Add(obj_termino);
            }
            else
            {

                lista_editada_terminos.RemoveAll(x => x.id_termino_atributo == (obj_termino.id_termino_atributo));
                lista_editada_terminos.Add(obj_termino);

            }

            btn_guardar_terminos.Enabled = true;
        }
        private void btn_guardar_terminos_Click(object sender, EventArgs e)
        {

            if (lista_editada_terminos.Count > 0)
            {

                int cont = 0;

                while (cont < lista_editada_terminos.Count) {

                    Metodos.MetodosTerminoAtributosWoocomerce met_terminos = new Metodos.MetodosTerminoAtributosWoocomerce();
                    met_terminos.actualizar_terminos_atributos_tablas(lista_editada_terminos[cont]);
                    cont++;
                }

                MessageBox.Show("Términos de atributos actualizadas exitosamente", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }
        private void dt_terninos_atributo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dt_terninos_atributo.EndEdit();

            if (e.ColumnIndex == 4)
            {

                Clases.TerminosAtributosCompletoWoocomerce obj_termino = new Clases.TerminosAtributosCompletoWoocomerce();

                obj_termino.id_termino_atributo = Convert.ToInt32(this.dt_terninos_atributo.CurrentRow.Cells[0].Value.ToString());
                obj_termino.name_termino_atributo = this.dt_terninos_atributo.CurrentRow.Cells[1].Value.ToString();
                obj_termino.description_termino_atributo = this.dt_terninos_atributo.CurrentRow.Cells[2].Value.ToString();
                obj_termino.slug = this.dt_terninos_atributo.CurrentRow.Cells[3].Value.ToString();
                obj_termino.estado = this.dt_terninos_atributo.CurrentRow.Cells[4].Value.ToString();
                obj_termino.id_atributo = Convert.ToInt32(this.dt_terninos_atributo.CurrentRow.Cells[5].Value.ToString());
                obj_termino.name_atributo = this.dt_terninos_atributo.CurrentRow.Cells[6].Value.ToString();

                Clases.TerminosAtributosCompletoWoocomerce product = lista_editada_terminos.FirstOrDefault(x => x.id_termino_atributo == Convert.ToInt32(this.dt_terninos_atributo.CurrentRow.Cells[0].Value.ToString()));

                if (product == null)
                {
                    lista_editada_terminos.Add(obj_termino);
                }
                else
                {

                    lista_editada_terminos.RemoveAll(x => x.id_termino_atributo == (obj_termino.id_termino_atributo));
                    lista_editada_terminos.Add(obj_termino);

                }

                dt_terninos_atributo.EndEdit();

                btn_guardar_terminos.Enabled = true;


            }
        

        }
        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result =
                MessageBox.Show("Al ejecutar esta acción los cambios de términos de atributos se verán reflejados en su tienda Woocomerce:" + "\n" + tienda,
                "◄ ¿Está seguro que desea ejecutar esta acción? ►", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {

                    Metodos.MetodosTerminoAtributosWoocomerce met_atributo = new Metodos.MetodosTerminoAtributosWoocomerce();
                    met_atributo.actualizar_terminos_atributo_woocomerce();

                    lista_editada_terminos.Clear();
                    met_cargar_terminos_atributos();
                    btn_guardar_terminos.Enabled = false;

                    MessageBox.Show("Se finalizó el proceso de sincronización de términos de atributos: ", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult result =
                    MessageBox.Show("Al ejecutar esta acción los términos de atributos en estado de inactividad se eliminarán de su tienda Woocomerce:" + " " + tienda + "" + "\n\n ◄ El proceso puede demorar unos minutos \n por favor espere ► ",
                    "◄ ¿Está seguro que desea ejecutar esta acción? ► ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosTerminoAtributosWoocomerce met = new Metodos.MetodosTerminoAtributosWoocomerce();
                    MessageBox.Show(met.eliminar_atributos_de_nuestras_tablas(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    met_cargar_terminos_atributos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //*********************** TERMINOS ATRIBUTOS *******************************************//



        //************************ ARTICULOS ****************************************************//

        //Para captura de errores
        private void dt_articulos_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {

            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {

            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {

            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {

            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";
                anError.ThrowException = false;
            }
        }

        //Para boton de sincronizacion
        private void dt_articulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dt_articulos.EndEdit();

            //16 columna de sincro

            if (e.ColumnIndex == 16)
            {

                Clases.ArticulosWoocomerce obj_articulos = new Clases.ArticulosWoocomerce();
                obj_articulos.sku = this.dt_articulos.CurrentRow.Cells[0].Value.ToString();
                obj_articulos.name = this.dt_articulos.CurrentRow.Cells[1].Value.ToString();
                obj_articulos.description = this.dt_articulos.CurrentRow.Cells[2].Value.ToString();
                obj_articulos.short_description = this.dt_articulos.CurrentRow.Cells[3].Value.ToString();
                obj_articulos.status = this.dt_articulos.CurrentRow.Cells[4].Value.ToString();
                if ((this.dt_articulos.CurrentRow.Cells[5].Value.ToString()) == null || (this.dt_articulos.CurrentRow.Cells[5].Value.ToString()) == "")
                {

                }
                else
                {

                    obj_articulos.id = Convert.ToInt32(this.dt_articulos.CurrentRow.Cells[5].Value.ToString());
                }
                obj_articulos.regular_price = this.dt_articulos.CurrentRow.Cells[6].Value.ToString();
                obj_articulos.stock_quantity = this.dt_articulos.CurrentRow.Cells[7].Value.ToString();
                obj_articulos.stock_status = this.dt_articulos.CurrentRow.Cells[8].Value.ToString();
                obj_articulos.tax_class = this.dt_articulos.CurrentRow.Cells[9].Value.ToString();
                //obj_articulos.categories = this.dt_articulos.CurrentRow.Cells[10].Value.ToString();
                obj_articulos.description = this.dt_articulos.CurrentRow.Cells[11].Value.ToString();
                obj_articulos.name = this.dt_articulos.CurrentRow.Cells[12].Value.ToString();
                obj_articulos.regular_price = this.dt_articulos.CurrentRow.Cells[13].Value.ToString();
                obj_articulos.weight = this.dt_articulos.CurrentRow.Cells[14].Value.ToString();
                obj_articulos.type = this.dt_articulos.CurrentRow.Cells[15].Value.ToString();
                obj_articulos.sincroniza = this.dt_articulos.CurrentRow.Cells[16].Value.ToString();

                Clases.ArticulosWoocomerce product = lista_articulos_editados.FirstOrDefault(x => x.sku == this.dt_articulos.CurrentRow.Cells[0].Value.ToString());

                if (product == null)
                {
                    lista_articulos_editados.Add(obj_articulos);
                }
                else
                {

                    lista_articulos_editados.RemoveAll(x => x.sku == (obj_articulos.sku));
                    lista_articulos_editados.Add(obj_articulos);

                }

                dt_articulos.EndEdit();
            }


        }

        //Para cuando editan un valor en algun txt del 
        private void dt_articulos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Clases.ArticulosWoocomerce obj_articulos = new Clases.ArticulosWoocomerce();


            obj_articulos.status = this.dt_articulos.CurrentRow.Cells[4].Value.ToString();
            obj_articulos.stock_status = this.dt_articulos.CurrentRow.Cells[8].Value.ToString();
            obj_articulos.type = this.dt_articulos.CurrentRow.Cells[15].Value.ToString();
            obj_articulos.sku = this.dt_articulos.CurrentRow.Cells[0].Value.ToString();
            obj_articulos.name = this.dt_articulos.CurrentRow.Cells[1].Value.ToString();
            obj_articulos.description = this.dt_articulos.CurrentRow.Cells[2].Value.ToString();
            obj_articulos.short_description = this.dt_articulos.CurrentRow.Cells[3].Value.ToString();
            if ((this.dt_articulos.CurrentRow.Cells[5].Value.ToString()) == null || (this.dt_articulos.CurrentRow.Cells[5].Value.ToString()) == "")
            {

            }
            else
            {

                obj_articulos.id = Convert.ToInt32(this.dt_articulos.CurrentRow.Cells[5].Value.ToString());
            }
            obj_articulos.regular_price = this.dt_articulos.CurrentRow.Cells[6].Value.ToString();
            obj_articulos.stock_quantity = this.dt_articulos.CurrentRow.Cells[7].Value.ToString();
            obj_articulos.tax_class = this.dt_articulos.CurrentRow.Cells[9].Value.ToString();
            //obj_articulos.categories = this.dt_articulos.CurrentRow.Cells[10].Value.ToString();
            obj_articulos.description = this.dt_articulos.CurrentRow.Cells[11].Value.ToString();
            obj_articulos.name = this.dt_articulos.CurrentRow.Cells[12].Value.ToString();
            obj_articulos.regular_price = this.dt_articulos.CurrentRow.Cells[13].Value.ToString();
            obj_articulos.weight = this.dt_articulos.CurrentRow.Cells[14].Value.ToString();
            obj_articulos.sincroniza = this.dt_articulos.CurrentRow.Cells[16].Value.ToString();

            Clases.ArticulosWoocomerce product = lista_articulos_editados.FirstOrDefault(x => x.sku == this.dt_articulos.CurrentRow.Cells[0].Value.ToString());

            if (product == null)
            {
                lista_articulos_editados.Add(obj_articulos);
            }
            else
            {

                lista_articulos_editados.RemoveAll(x => x.sku == (obj_articulos.sku));
                lista_articulos_editados.Add(obj_articulos);

            }

            btn_guardar_articulos.Enabled = true;
        }
        //Para los combo box
        private void dt_articulos_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewComboBoxEditingControl dgvCombo = e.Control as DataGridViewComboBoxEditingControl;
            columna_combo = this.dt_articulos.CurrentCell.ColumnIndex;

            if (dgvCombo != null)
            {
                //
                // se remueve el handler previo que pudiera tener asociado, a causa ediciones previas de la celda
                // evitando asi que se ejecuten varias veces el evento
                //
                dgvCombo.SelectedIndexChanged -= new EventHandler(dvgCombo_SelectedIndexChanged);

                dgvCombo.SelectedIndexChanged += new EventHandler(dvgCombo_SelectedIndexChanged);
            }
        }
        //Para los combo box
        private void dvgCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

            //se recupera el valor del combo
            //a modo de ejemplo se escribe en consola el valor seleccionado
            ComboBox combo = sender as ComboBox;
            if (combo.SelectedValue == null) { } else { valor_enviado_por_combo_box = combo.SelectedItem.ToString(); }

            Clases.ArticulosWoocomerce obj_articulos = new Clases.ArticulosWoocomerce();

            if (columna_combo == 4)
            {
                obj_articulos.status = valor_enviado_por_combo_box;
                obj_articulos.stock_status = this.dt_articulos.CurrentRow.Cells[8].Value.ToString();
                obj_articulos.type = this.dt_articulos.CurrentRow.Cells[15].Value.ToString();
            }

            if (columna_combo == 8)
            {
                obj_articulos.status = this.dt_articulos.CurrentRow.Cells[4].Value.ToString();
                obj_articulos.stock_status = valor_enviado_por_combo_box;
                obj_articulos.type = this.dt_articulos.CurrentRow.Cells[15].Value.ToString();
            }

            if (columna_combo == 15)
            {
                obj_articulos.status = this.dt_articulos.CurrentRow.Cells[4].Value.ToString();
                obj_articulos.stock_status = this.dt_articulos.CurrentRow.Cells[8].Value.ToString();
                obj_articulos.type = valor_enviado_por_combo_box;
            }


            obj_articulos.sku = this.dt_articulos.CurrentRow.Cells[0].Value.ToString();
            obj_articulos.name = this.dt_articulos.CurrentRow.Cells[1].Value.ToString();
            obj_articulos.description = this.dt_articulos.CurrentRow.Cells[2].Value.ToString();
            obj_articulos.short_description = this.dt_articulos.CurrentRow.Cells[3].Value.ToString();

            if ((this.dt_articulos.CurrentRow.Cells[5].Value.ToString()) == null || (this.dt_articulos.CurrentRow.Cells[5].Value.ToString()) == "")
            {
                
            }
            else {

                obj_articulos.id = Convert.ToInt32(this.dt_articulos.CurrentRow.Cells[5].Value.ToString());
            }
           
            obj_articulos.regular_price = this.dt_articulos.CurrentRow.Cells[6].Value.ToString();
            obj_articulos.stock_quantity = this.dt_articulos.CurrentRow.Cells[7].Value.ToString();
            obj_articulos.tax_class = this.dt_articulos.CurrentRow.Cells[9].Value.ToString();
            //obj_articulos.categories = this.dt_articulos.CurrentRow.Cells[10].Value.ToString();
            obj_articulos.description = this.dt_articulos.CurrentRow.Cells[11].Value.ToString();
            obj_articulos.name = this.dt_articulos.CurrentRow.Cells[12].Value.ToString();
            obj_articulos.regular_price = this.dt_articulos.CurrentRow.Cells[13].Value.ToString();
            obj_articulos.weight = this.dt_articulos.CurrentRow.Cells[14].Value.ToString();

            obj_articulos.sincroniza = this.dt_articulos.CurrentRow.Cells[16].Value.ToString();

            Clases.ArticulosWoocomerce product = lista_articulos_editados.FirstOrDefault(x => x.sku == this.dt_articulos.CurrentRow.Cells[0].Value.ToString());

            if (product == null)
            {
                lista_articulos_editados.Add(obj_articulos);
            }
            else
            {

                lista_articulos_editados.RemoveAll(x => x.sku == (obj_articulos.sku));
                lista_articulos_editados.Add(obj_articulos);

            }


            btn_guardar_articulos.Enabled = true;

        }

        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            met_cargar_articulos_sincronizados();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Mostrar_Articulos frm_articulos = new Mostrar_Articulos();
            frm_articulos.ShowDialog();
        }

        private void dt_articulos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 17)
            {
                string articulo = this.dt_articulos.CurrentRow.Cells[0].Value.ToString();
                string descripcion = this.dt_articulos.CurrentRow.Cells[1].Value.ToString();
                string variaciones = "";

                if (this.dt_articulos.CurrentRow.Cells[18].Value != null) {
                variaciones  = this.dt_articulos.CurrentRow.Cells[18].Value.ToString();
                }
           
                Variaciones frm_variacion = new Variaciones(articulo, descripcion, variaciones);
                frm_variacion.ShowDialog();

            }

            if (e.ColumnIndex == 10)
            {
                
                string categorias = "";
                int fila = this.dt_articulos.CurrentRow.Index;

                if (this.dt_articulos.CurrentRow.Cells[10].Value != null)
                {
                    categorias = this.dt_articulos.CurrentRow.Cells[10].Value.ToString();
                }

                AsignarCategoriasArticulos frm = new AsignarCategoriasArticulos(fila, categorias);
                frm.Show();

            }

        }

      

        public void met_asignar_categorias_greed(string categorias, int fila)
        {
            this.dt_articulos.EndEdit();
            int a = this.dt_articulos.RowCount;
            //dt_articulos.Rows[1].Cells[10].Value = "1";

            //MessageBox.Show(categorias + "fila: " + fila.ToString());

        }

        //************************ ARTICULOS ****************************************************//


        //************************ ENVIOS ****************************************************//
        private void toolStripButton23_Click(object sender, EventArgs e)
        {
            met_cargar_metodos_envio();
        }
        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            Envios frm_envios = new Envios();
            frm_envios.ShowDialog();
        }
        private void data_envios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                data_envios.EndEdit();

                //Si selecciona la celda de padre que muestre los articulo padres
                if (e.ColumnIndex == 6)
                {

                    Clases.EnvioWoocomerce obj_envio = new Clases.EnvioWoocomerce();
                    Metodos.MetodosEnvioWoocomerce met_envio = new Metodos.MetodosEnvioWoocomerce();

                    obj_envio.sku = this.data_envios.CurrentRow.Cells[0].Value.ToString();
                    obj_envio.id_metodo_pago = Convert.ToInt32(this.data_envios.CurrentRow.Cells[2].Value.ToString());
                    obj_envio.zona = Convert.ToInt32(this.data_envios.CurrentRow.Cells[4].Value.ToString());
                    obj_envio.estado = this.data_envios.CurrentRow.Cells[6].Value.ToString();

                    met_envio.actualizar_metodo_envio(obj_envio);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

          



        }
        private void toolStripButton24_Click(object sender, EventArgs e)
        {
          
            try
            {
                DialogResult result =
                   MessageBox.Show("Al ejecutar esta acción los métodos de envío en estado de inactividad se eliminarán, tenga en cuenta que estos son importantes para sincronizar el costo de envío  en su tienda::" + " " + tienda + "" + "\n\n ◄ El proceso puede demorar unos minutos \n por favor espere ► ",
                   "◄ ¿Está seguro que desea ejecutar esta acción? ► ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosEnvioWoocomerce met_envios_eliminar = new Metodos.MetodosEnvioWoocomerce();
                    met_envios_eliminar.eliminar_metodo_envio();
                    MessageBox.Show("Métodos de envío eliminados", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    met_cargar_metodos_envio();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
        private void toolStripButton25_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result =
                   MessageBox.Show("Al ejecutar esta acción los precios de métodos de envío se sincronizarán en su tienda:" + " " + tienda + "" + "\n\n ◄ El proceso puede demorar unos minutos \n por favor espere ► ",
                   "◄ ¿Está seguro que desea ejecutar esta acción? ► ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosEnvioWoocomerce met_envios_eliminar = new Metodos.MetodosEnvioWoocomerce();
                    met_envios_eliminar.eliminar_metodo_envio();
                    MessageBox.Show(met_envios_eliminar.actualizar_precio_envios_woocomerce(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    met_cargar_metodos_envio();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //************************ ENVIOS ****************************************************//



        //**************************** PEDIDOS **********************************************//

        //Obtener pedidos de woocomerce
        public string obtener_pedidos_woocoemrce()
        {

            string json = "";

            try
            {
                Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                string tienda = met_configuracion.obtener_datos_configuracion().TIENDA; //obtenermos el nombre de la tienda woocomerce guardada en base de datos
                string clave = met_configuracion.obtener_datos_configuracion().CLAVE_API; //obtenemos la clave del api woocomerce guardada en base de datos
                string password = met_configuracion.obtener_datos_configuracion().PASS_API; // obtenemos el password del api woocomerce guardada en base de datos
                                                                                            //Hacemos una lista de terminos de clase de atributos con sus terminos atributos
                List<Clases.CategoriasWoocomerce> lista_obj_atributos_con_terminos_atributo = new List<Clases.CategoriasWoocomerce>();

                RestClient restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://" + tienda + "/wp-json/wc/v3/orders");
                RestRequest restRequest = new RestRequest(Method.GET);
                restClient.Authenticator = new HttpBasicAuthenticator(clave, password);
                //Esta linea se agrega cuando la tienda no esta hosteada quita el error de certificacion de SSL/TLS
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                IRestResponse restResponse = restClient.Execute(restRequest);


                //Si Woocomerce responde entoces que continue el proceso
                if ((int)restResponse.StatusCode == 200)
                {
                    json = restResponse.Content;
                }

                return json;
            }
            catch (Exception)
            {

                throw;
            }

        }


        //**************************** PEDIDOS **********************************************//
        private void toolStripButton30_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult result =
                   MessageBox.Show("Al ejecutar esta acción los pedidos no tengan un consecutivo asignado en la aplicación intermedia se ingresarán el su sistema ERP" + "\n\n ◄ El proceso puede demorar unos minutos \n por favor espere ► ",
                   "◄ ¿Está seguro que desea ejecutar esta acción? ► ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosPedidosWoocomerce met = new Metodos.MetodosPedidosWoocomerce();
                    met.sincronizador_pedidos_al_ERP();                
                    Metodos.MetodosPedidosWoocomerce met2 = new Metodos.MetodosPedidosWoocomerce();
                    met2.actualizar_pedidos_woocoemrce();
                    MessageBox.Show("Pedidos ingresados con éxito", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    met_cargar_pedidos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void toolStripButton31_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult result =
                   MessageBox.Show("Al ejecutar esta acción los pedidos en su tienda:" + " " + tienda + "" + " se sincronizán con el sistema de tablas intermedias" + "\n\n ◄ El proceso puede demorar unos minutos \n por favor espere ► ",
                   "◄ ¿Está seguro que desea ejecutar esta acción? ► ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosPedidosWoocomerce met = new Metodos.MetodosPedidosWoocomerce();
                    met.insertar_pedidos_woocomerce();
                    MessageBox.Show(met.insertar_pedidos_woocomerce(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    met_cargar_pedidos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void dt_pedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        private void txtpedidos_TextChanged(object sender, EventArgs e)
        {
            met_cargar_pedidos_busqueda();
        }
        private void toolStripButton29_Click(object sender, EventArgs e)
        {
            met_cargar_pedidos();
        }
        private void dt_pedidos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id_pedido = Convert.ToInt32(this.dt_pedidos.CurrentRow.Cells[1].Value.ToString());
            Pedido_Linea frm_pedidos = new Pedido_Linea(id_pedido);
            frm_pedidos.ShowDialog();
        }
        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            met_cargar_pedidos_busqueda_fecha();
        }
        private void dt_fecha_fin_ValueChanged(object sender, EventArgs e)
        {
            met_cargar_pedidos_busqueda_fecha();
        }

        //**************************** PEDIDOS **********************************************//

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }     
        private void toolStrip3_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }      
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }
        private void dt_pedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }        
        private void dt_pedidos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void toolStripButton34_Click(object sender, EventArgs e)
        {

        }
        private void UC_Woocommerce_Load(object sender, EventArgs e)
        {

            if (!this.DesignMode)
            {
                Metodos.MetodosConfiguracionWoocomerce met_configuracion = new Metodos.MetodosConfiguracionWoocomerce();
                tienda = met_configuracion.obtener_datos_configuracion().TIENDA;

                //******Carga de greed*****
                met_cargar_categorias();
                met_cargar_atributos();
                met_cargar_terminos_atributos();
                met_cargar_articulos_sincronizados();
                met_cargar_metodos_envio();
                met_cargar_pedidos();
                met_cargar_bitacora_errores();
                //******Carga de greed*****

                //******Carga de combobox*****
                cargar_atributos();
                cargar_combo_modulos_bitacora();
                met_asignar_categorias_greed("123", 1);
                //******Carga de combobox*****
            }


        }
        private void comobo_opcion_terminos_Click(object sender, EventArgs e)
        {

        }
        private void combo_modulos_DropDown(object sender, EventArgs e)
        {

        }

        //**************************** BITACORAS DE EROORES **********************************************//

        private void combo_modulos_SelectedIndexChanged(object sender, EventArgs e)
        {
            met_cargar_bitacora_por_modulo();
        }

        private void toolStripButton27_Click(object sender, EventArgs e)
        {
            met_cargar_bitacora_errores();
            cargar_combo_modulos_bitacora();
        }

        private void date_bi_fecha_ini_ValueChanged(object sender, EventArgs e)
        {
            met_cargar_bitacora_por_fechas();
        }

        private void date_bi_fecha_fin_ValueChanged(object sender, EventArgs e)
        {
            met_cargar_bitacora_por_fechas();
        }

        private void toolStripButton26_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result =
                   MessageBox.Show("Al ejecutar esta acción los datos de bitacora se eliminarán" + "\n\n ◄ El proceso puede demorar unos minutos \n por favor espere ► ",
                   "◄ ¿Está seguro que desea ejecutar esta acción? ► ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Metodos.MetodosBitacorasWoocomerce met = new Metodos.MetodosBitacorasWoocomerce();
                    met.eliminar_bitacora_errores();
                    MessageBox.Show("Bitacora eliminada con éxito", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   met_cargar_bitacora_errores();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_guardar_articulos_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton20_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result =
            MessageBox.Show("Al ejecutar esta acción los cambios de articulos se verán reflejados en su tienda Woocomerce:" + "\n" + tienda,
            "◄ ¿Está seguro que desea ejecutar esta acción? ►", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {

                    try
                    {
                        Metodos.MetodosArticulosWoocomerce met_articulos = new Metodos.MetodosArticulosWoocomerce();
                        met_articulos.actualizar_articulos_o_insertarlos_en_woocomerce();
                        
                    }
                    catch (Exception ex )
                    {

                        MessageBox.Show("Sincronizacion de articulos: " + ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    try
                    {
                        Metodos.MetodosArticulosWoocomerce met = new Metodos.MetodosArticulosWoocomerce();
                        met.obtener_atributos_del_articulo_padre();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Sincronizacion de atributos: " + ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    try
                    {
                        Metodos.MetodosVariacionesWoocomerce met_var = new Metodos.MetodosVariacionesWoocomerce();
                        met_var.actualizar_variaciones_o_insertarlos_en_woocomerce();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Sincronizacion de variaciones: " + ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    MessageBox.Show("Sincronizacion de articulos con éxito", "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "◄ AVISO ►", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {

            Metodos.MetodosVariacionesWoocomerce met_var = new Metodos.MetodosVariacionesWoocomerce();
            met_var.actualizar_variaciones_o_insertarlos_en_woocomerce();


            //Metodos.MetodosArticulosWoocomerce met = new Metodos.MetodosArticulosWoocomerce();
            //met.obtener_atributos_del_articulo_padre();
        }

        private void toolStripButton21_Click(object sender, EventArgs e)
        {

        }

        //**************************** BITACORAS DE EROORES **********************************************//
    }
}

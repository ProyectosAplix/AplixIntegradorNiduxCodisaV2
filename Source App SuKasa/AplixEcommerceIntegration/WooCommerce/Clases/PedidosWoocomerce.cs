using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.WooCommerce.Clases
{
   public class PedidosWoocomerce
    {
        public int id { get; set; }
        public string number { get; set; }
        public string status { get; set; }
        public string date_created { get; set; }
        public string date_created_gmt { get; set; }
        public string currency { get; set; }
        public string discount_total { get; set; }
        public string discount_tax { get; set; }
        public string shipping_total { get; set; }
        public string shipping_tax { get; set; }
        public string cart_tax { get; set; }
        public string total { get; set; }
        public string total_tax { get; set; }
        public int customer_id { get; set; }
        public Billing billing { get; set; }
        public Shipping shipping { get; set; }
        public string payment_method { get; set; }
        public string payment_method_title { get; set; }
        public string transaction_id { get; set; }
        public string customer_note { get; set; }
        public string date_paid { get; set; }
        public string date_paid_gmt { get; set; }
        public IList<Line_items> line_items { get; set; }
        public IList<Shipping_Lines> shipping_lines { get; set; }


    }


    public class PedidosBusquedaWoocomerce
    {

        public string dato { get; set; }
        public string parametro { get; set; }

    }


    public class PedidosBusquedaFechaWoocomerce
    {

        public string fecha_ini { get; set; }
        public string fecha_fin { get; set; }

    }

    public class PedidosEstado
    {
        public int id { get; set; }
        public string status { get; set; }

    }

}


public class Billing
{
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string company { get; set; }
    public string address_1 { get; set; }
    public string address_2 { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string postcode { get; set; }
    public string country { get; set; }
    public string email { get; set; }
    public string phone { get; set; }

}
public class Shipping
{
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string company { get; set; }
    public string address_1 { get; set; }
    public string address_2 { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string postcode { get; set; }
    public string country { get; set; }

}

public class Line_items
{
    public int id { get; set; }
    public string name { get; set; }
    public int product_id { get; set; }
    public int variation_id { get; set; }
    public int quantity { get; set; }
    public string tax_class { get; set; }
    public string subtotal { get; set; }
    public string subtotal_tax { get; set; }
    public string total { get; set; }
    public string total_tax { get; set; }
    public string sku { get; set; }
    public int price { get; set; }

}

public class Shipping_Lines {
    public int id { get; set; }
    public string method_title { get; set; }
    public int method_id { get; set; }
    public int instance_id { get; set; }
    public int total { get; set; }
    public string total_tax { get; set; }

}




            

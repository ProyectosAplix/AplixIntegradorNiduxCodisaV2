using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.WooCommerce.Clases
{
    public class EnvioWoocomerce
    {

        public string sku { get; set; }
        public string descripcion { get; set; }
        public string precio { get; set; }
        public string metodo_pago { get; set; }
        public int id_metodo_pago { get; set; }
        public int zona { get; set; }
        public string nombre_zona { get; set; }
        public string estado { get; set; }


    }

    public class ZonasWoocoemrce {

        public int id { get; set; }
        public string name { get; set; }

    }


    public class MetodosEnvioWoocoemrce
    {

        public int instance_id { get; set; }
        public string title { get; set; }
        //public SettingsWoocomerce settings { get; set; }
         

    }


    public class MetodosEnvioActWoocoemrce
    {

     
        public SettingsWoocomerce settings { get; set; }


    }

    public class EnvioBusqueda {
        public string dato { get; set; }
        public string parametro { get; set; }

    }


    public class SettingsWoocomerce {

        public string cost { get; set; }

    }


}


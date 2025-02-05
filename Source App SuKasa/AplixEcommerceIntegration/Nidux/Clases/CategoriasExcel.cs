using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    public class CategoriasExcel
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string padre { get; set; }

    }

    public class MarcasExcel
    {
        public string codigo { get; set; }
        public string nombre { get; set; }

    }


    public class AtributosExcel
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string atributo { get; set; }

    }

    public class ArticulosExcel
    {
        public string sku { get; set; }
        public string nombre_nidux { get; set; }
        public string descripcion_nidux { get; set; }
        public string marca { get; set; }
        public string categoria { get; set; }
        public string porcentaje_reserva { get; set; }
        public string permite_reserva { get; set; }
        public string destacado { get; set; }
        public string indicador_stock { get; set; }
        public string estado { get; set; }
        public string costo_shipping { get; set; }
        public string limite_carrito { get; set; }
        public string usar_gif { get; set; }
        public string tiempo_gif { get; set; }
        public string video_youtube { get; set; }
        public string nombre_traduccion { get; set; }
        public string descripcion_traduccion { get; set; }
        public string sincroniza { get; set; }

        public string tags { get; set; }

        public string seo_tags { get; set; }

        public string padre { get; set; }
        public string atributos { get; set; }


    }


    public class VariacionesExcel
    {
        public string sku_padre { get; set; }
        public string sku_variacion { get; set; }
        public string atributos { get; set; }
        public string sincroniza { get; set; }

    }

    public class EstadosExcel
    {
        public string id { get; set; }
        public string descripcion { get; set; }


    }

}

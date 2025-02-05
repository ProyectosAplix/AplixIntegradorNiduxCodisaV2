using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Shopify.Clases
{
    //Clases con los objetos que nos ayudan a llenar la lista para cuando modifican algun valor en la tabla y para las busquedas
    public class Variantes
    {
        public string opcion { get; set; }
        public string descripcion { get; set; }
        public string codigo { get; set; }
        public string elimanado { get; set; }
    }
}

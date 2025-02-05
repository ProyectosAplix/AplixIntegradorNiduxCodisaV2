using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    public class MarcasNidux
    {
        public IList<Marca> marcas { get; set; }
    }

    public class Marca
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string codigo_marca { get; set; }
        public string activo { get; set; }


    }
    public class Respuesta_Marcas
    {

        public int id { get; set; }
        public string codigo_marca { get; set; }

    }
}

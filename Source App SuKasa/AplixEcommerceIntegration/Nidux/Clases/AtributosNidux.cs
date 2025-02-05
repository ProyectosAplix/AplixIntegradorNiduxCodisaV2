using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    public class Atributos_Nidux
    {
        public IList<Atributo_Nidux> atributos { get; set; }
    }
    public class Atributo_Nidux
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string estilo { get; set; }
    }

    public class Atributo
    {
        public IList<Valores> atributos { get; set; }
    }
    public class Valores
    {
        public int id_atributo { get; set; }
        public string nombre_atributo { get; set; }
        public int id_valor_atributo { get; set; }
        public string nombre_valor_atributo { get; set; }
    }

    public class Atributo_Valores
    {
        public IList<Valores_Atributos> atributo_valores { get; set; }
    }

    public class Valores_Atributos
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string activo { get; set; }
        public string atributo { get; set; }

    }

    public class Respuesta_Variaciones
    {
        public string estado { get; set; }

        public string comentarios { get; set; }
    }
}

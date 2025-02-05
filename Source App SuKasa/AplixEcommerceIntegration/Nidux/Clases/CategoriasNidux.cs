using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    public class Cats
    {
        public IList<Categorias> cats { get; set; }
    }

    public class Categorias
    {
        public string nombre { get; set; }

        public string codigo_categoria { get; set; }

        public string categoria_padre { get; set; }

        public string descripcion { get; set; }

        public int estado { get; set; }

        public int peso_precedencia { get; set; }

        public int categoria_en_malls { get; set; }

        public string cantidad { get; set; }

        public string activo { get; set; }

        public IList<Traducciones_Cat> traducciones { get; set; }

    }

    public class Traducciones_Cat
    {
        public int idioma { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }
    }

    public class Respuesta_Categoria
    {
        public string estado { get; set; }
        public int id { get; set; }
        public string comentarios { get; set; }
        public string cod_categoria { get; set; }
    }
}

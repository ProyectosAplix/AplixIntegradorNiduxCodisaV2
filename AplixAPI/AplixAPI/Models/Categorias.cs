using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplixAPI.Models
{
    public class Categorias
    {
        public Dictionary<string, Cats> cats { get; set; }
    }
    public class Cats
    {
        public int id { get; set; }
        public string category_name { get; set; }
        public int category_father { get; set; }
        public string category_description { get; set; }

    }

    public class Categorias_Padre
    {
        public string nombre { get; set; }
        public string codigo_categoria { get; set; }
        public string categoria_padre { get; set; }
        public string cantidad { get; set; }
        public string descripcion { get; set; }
        public int estado { get; set; }
        public int peso_precedencia { get; set; }
        public int categoria_en_malls { get; set; }
        public IList<Traducciones_Cat> traducciones { get; set; }

    }

    public class Traducciones_Cat
    {
        public int lang_id { get; set; }
        public string category_name { get; set; }
        public string category_description { get; set; }
    }

    public class Respuesta_Categoria
    {
        public int id { get; set; }
        public string cod_categoria { get; set; }

    }
}
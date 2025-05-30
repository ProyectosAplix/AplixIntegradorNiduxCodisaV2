using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplixAPI.Models
{
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

    public class Valores_hijo
    {
        public string[] id_valores_atributos { get; set; }
        public string sku_variacion { get; set; }
        public decimal peso { get; set; }
        public int stock { get; set; }
        public decimal precio { get; set; }

    }

    public class Valores_hijo_atributos
    {
        public int[] id_atributos { get; set; }
    }

    public class Atributo_Valores
    {
        public Dictionary<string, Valores_Atributos> atributos { get; set; }
    }

    public class Valores_Atributos
    {
        public int id { get; set; }
        public int attribute_id { get; set; }      
        public string attribute_name { get; set; }
    }

    public class Variaciones_Padre
    {
        public string padre { get; set; }
        public string ID { get; set; }
        //public string sku_padre { get; set; }
    }

}
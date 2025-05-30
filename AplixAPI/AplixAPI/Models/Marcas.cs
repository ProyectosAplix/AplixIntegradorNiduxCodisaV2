using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplixAPI.Models
{
    public class Marca
    {
        public int id { get; set; }
        public string brand_name { get; set; }
        public string nombre { get; set; }
        public string codigo_marca { get; set; }

    }
    public class Marcas
    {
        public Dictionary<string, Marca> marcas { get; set; }

    }

    public class Respuesta_Marcas
    {
        public int id { get; set; }
        public string codigo_marca { get; set; }
    }

}
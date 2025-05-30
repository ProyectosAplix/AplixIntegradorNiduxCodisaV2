using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplixAPI.Models
{
    public class Credencial
    {
        public IList<Valores_credencial> credenciales { get; set; }
    }
    public class Valores_credencial
    {
        //public string compania { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public int storeId { get; set; }

    }
}
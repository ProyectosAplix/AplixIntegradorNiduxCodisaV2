using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorAplix_Nidux.models
{
    public class Credencial
    {
        public IList<Valores_credencial> credenciales { get; set; }
    }
    public class Valores_credencial
    {
        public string compania { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public int storeId { get; set; }
        public string lista_precio { get; set; }
        public int versiones { get; set; }
        public string bodega { get; set; }
        public string mensaje { get; set; }
    }
}

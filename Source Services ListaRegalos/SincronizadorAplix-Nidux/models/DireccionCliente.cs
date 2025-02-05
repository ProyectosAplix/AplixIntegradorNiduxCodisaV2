using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorAplix_Nidux.models
{
    class DireccionCliente
    {
        public CustomerAddress CustomerAddress { get; set; }
    }

    public partial class CustomerAddress
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public long Zip { get; set; }
        public string Detalle { get; set; }
        public object Latitud { get; set; }
        public object Longitud { get; set; }
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Distrito { get; set; }
    }
}

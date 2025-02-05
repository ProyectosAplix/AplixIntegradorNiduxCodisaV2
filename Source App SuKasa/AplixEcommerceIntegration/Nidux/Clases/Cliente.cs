using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    class Cliente
    {
        public Customer Customer { get; set; }
    }

    public partial class Customer
    {
        public long id { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string fecha_de_nacimiento { get; set; }
        public string Correo { get; set; }
        public string Genero { get; set; }
        public long Estado { get; set; }
        public long CompradorAnonimo { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Idioma { get; set; }
        public string creado { get; set; }
        public string Ip { get; set; }
        public long Exonerado { get; set; }
        public long Saldo { get; set; }
        public System.Collections.Generic.Dictionary<int, Direccione> Direcciones { get; set; }
    }

    public partial class Direccione
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public long Zip { get; set; }
        public string Detalle { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Distrito { get; set; }
    }
}

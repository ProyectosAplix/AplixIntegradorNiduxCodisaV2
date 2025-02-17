using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorAplix_Nidux.models
{
    class Cliente
    {
        [JsonProperty("Cliente")]
        public Customer Customer { get; set; }
    }

    public partial class Customer
    {
        [JsonProperty("id")]
        public long id { get; set; }

        [JsonProperty("consumer_identification")]
        public string Identificacion { get; set; }

        [JsonProperty("consumer_name")]
        public string Nombre { get; set; }

        [JsonProperty("consumer_bday")]
        public string fecha_de_nacimiento { get; set; }

        [JsonProperty("consumer_email")]
        public string Correo { get; set; }

        [JsonProperty("consumer_gender")]
        public string Genero { get; set; }

        [JsonProperty("consumer_status")]
        public long Estado { get; set; }

        [JsonProperty("consumer_anon")]
        public long CompradorAnonimo { get; set; }

        [JsonProperty("consumer_tel")]
        public string Telefono1 { get; set; }

        [JsonProperty("consumer_cel")]
        public string Telefono2 { get; set; }

        [JsonProperty("consumer_language")]
        public string Idioma { get; set; }

        [JsonProperty("consumer_created")]
        public string creado { get; set; }

        [JsonProperty("consumer_created_ip")]
        public string Ip { get; set; }

        [JsonProperty("consumer_exonerate")]
        public long Exonerado { get; set; }

        [JsonProperty("saldo")]
        public long Saldo { get; set; }

        [JsonProperty("direcciones")]
        public Dictionary<int, Direccione> Direcciones { get; set; }
    }

    public partial class Direccione
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("address_name")]
        public string Nombre { get; set; }

        [JsonProperty("address_city")]
        public string Ciudad { get; set; }

        [JsonProperty("address_zip")]
        public string Zip { get; set; }

        [JsonProperty("address_description")]
        public string Detalle { get; set; }

        [JsonProperty("address_lat")]
        public string Latitud { get; set; }

        [JsonProperty("address_lon")]
        public string Longitud { get; set; }

        [JsonProperty("Pais")]
        public string Pais { get; set; }

        [JsonProperty("Provincia")]
        public string Provincia { get; set; }

        [JsonProperty("Canton")]
        public string Canton { get; set; }

        [JsonProperty("Distrito")]
        public string Distrito { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    public class filtro_articulos
    {
        public int id_filtro_articulo { get; set; }
        public int codigo_articulo { get; set; }
        public int nombre { get; set; }
        public int nombre_nidux { get; set; }
        public int descripcion { get; set; }
        public int descripcion_nidux { get; set; }
        public int peso { get; set; }
        public int cantidad { get; set; }
        public int precio { get; set; }
        public int porcentaje_descuento { get; set; }
        public int impuesto_articulo { get; set; }
        public int sincroniza { get; set; }
        public int estado { get; set; }
        public int id_nidux { get; set; }
        public int marca_nidux { get; set; }
        public int categorias { get; set; }
        public int valores_atributos { get; set; }
        public int id_padre { get; set; }
        public int id_hijo { get; set; }
        public int indicador_stock { get; set; }
        public int destacar_articulo { get; set; }
        public int costo_shipping { get; set; }
        public int permite_reserva { get; set; }
        public int porcentaje_reserva { get; set; }
        public int limite_carrito { get; set; }
        public int usa_gif { get; set; }
        public int tiempo_gif { get; set; }
        public int video_youtube { get; set; }
        public int nombre_ingles { get; set; }
        public int descripcion_ingles { get; set; }
    }
}

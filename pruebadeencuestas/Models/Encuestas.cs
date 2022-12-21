using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pruebadeencuestas.Models
{
    public class Encuestas
    {
        public int id { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }

        public List<Detalle> Detalles { get; set; }

        public Encuestas()
        {
            this.Detalles = new List<Detalle>();
        }





}

    public class Detalle
    {
        public string nombrecampo { get; set; }
        public string titulo_campo { get; set; }

        public string esrequerido { get; set; }

        public string tipo { get; set; }


    }


}

using System;
using System.Collections.Generic;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class Modelo
    {
        public Modelo()
        {
            Autos = new HashSet<Auto>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdMarca { get; set; }

        public virtual Marca IdMarcaNavigation { get; set; }
        public virtual ICollection<Auto> Autos { get; set; }
    }
}

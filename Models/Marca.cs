using System;
using System.Collections.Generic;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class Marca
    {
        public Marca()
        {
            Modelos = new HashSet<Modelo>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Modelo> Modelos { get; set; }
    }
}

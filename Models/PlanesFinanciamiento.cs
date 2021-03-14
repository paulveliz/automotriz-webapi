using System;
using System.Collections.Generic;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class PlanesFinanciamiento
    {
        public PlanesFinanciamiento()
        {
            Autos = new HashSet<Auto>();
            Solicitudes = new HashSet<Solicitude>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal? PrecioInicial { get; set; }
        public decimal? PrecioLimite { get; set; }

        public virtual ICollection<Auto> Autos { get; set; }
        public virtual ICollection<Solicitude> Solicitudes { get; set; }
    }
}

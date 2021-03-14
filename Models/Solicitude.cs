using System;
using System.Collections.Generic;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class Solicitude
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime? Fecha { get; set; }
        public bool Aprobado { get; set; }
        public int? IdPlanFinanciamiento { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual PlanesFinanciamiento IdPlanFinanciamientoNavigation { get; set; }
    }
}

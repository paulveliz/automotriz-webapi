using System;
using System.Collections.Generic;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class Deuda
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? UltimoAbono { get; set; }
        public int IdFinanciamiento { get; set; }
        public int IdSolicitud { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Financiamiento IdFinanciamientoNavigation { get; set; }
        public virtual Solicitude IdSolicitudNavigation { get; set; }
    }
}

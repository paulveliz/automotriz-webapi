using System;
using System.Collections.Generic;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class Auto
    {
        public Auto()
        {
            Financiamientos = new HashSet<Financiamiento>();
        }

        public int Id { get; set; }
        public decimal ValorComecial { get; set; }
        public string UrlImagen { get; set; }
        public int IdPlanFinanciamiento { get; set; }
        public int IdModelo { get; set; }

        public virtual Modelo IdModeloNavigation { get; set; }
        public virtual PlanesFinanciamiento IdPlanFinanciamientoNavigation { get; set; }
        public virtual ICollection<Financiamiento> Financiamientos { get; set; }
    }
}

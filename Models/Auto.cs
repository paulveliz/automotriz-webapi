using System;
using System.Collections.Generic;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class Auto
    {
        public byte Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public decimal ValorComercial { get; set; }
        public string UrlImagen { get; set; }
        public byte PlanFinanciamiento { get; set; }

        public virtual PlanesFinanciamiento PlanFinanciamientoNavigation { get; set; }
    }
}

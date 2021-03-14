using System;
using System.Collections.Generic;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class Cliente
    {
        public byte Id { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Curp { get; set; }
        public string Domicilio { get; set; }
        public byte IdEstadoCivil { get; set; }

        public virtual EstadoCivil IdEstadoCivilNavigation { get; set; }
    }
}

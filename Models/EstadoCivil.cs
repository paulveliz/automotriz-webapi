using System;
using System.Collections.Generic;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class EstadoCivil
    {
        public EstadoCivil()
        {
            Clientes = new HashSet<Cliente>();
        }

        public byte Id { get; set; }
        public string Descripcion { get; set; }
        public byte IdHijos { get; set; }
        public decimal IngresoAcumulable { get; set; }

        public virtual Hijo IdHijosNavigation { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}

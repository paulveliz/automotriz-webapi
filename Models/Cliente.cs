using System;
using System.Collections.Generic;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Deuda = new HashSet<Deuda>();
            Encriptaciones = new HashSet<Encriptacione>();
            Financiamientos = new HashSet<Financiamiento>();
            Hijos = new HashSet<Hijo>();
            Solicitudes = new HashSet<Solicitude>();
        }

        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Domicilio { get; set; }
        public string Curp { get; set; }
        public decimal? IngresosMensuales { get; set; }
        public string UrlImagen { get; set; }
        public byte Edad { get; set; }
        public int? IdEstadoCivil { get; set; }
        public string RealCurp { get; set; }

        public virtual EstadosCivile IdEstadoCivilNavigation { get; set; }
        public virtual ICollection<Deuda> Deuda { get; set; }
        public virtual ICollection<Encriptacione> Encriptaciones { get; set; }
        public virtual ICollection<Financiamiento> Financiamientos { get; set; }
        public virtual ICollection<Hijo> Hijos { get; set; }
        public virtual ICollection<Solicitude> Solicitudes { get; set; }
    }
}

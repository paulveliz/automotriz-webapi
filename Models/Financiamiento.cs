using System;
using System.Collections.Generic;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class Financiamiento
    {
        public Financiamiento()
        {
            Deuda = new HashSet<Deuda>();
        }

        public int Id { get; set; }
        public int IdCliente { get; set; }
        public decimal ValorDelAuto { get; set; }
        public decimal Enganche { get; set; }
        public decimal CantidadAFinanciar { get; set; }
        public int Meses { get; set; }
        public decimal Mensualidad { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual ICollection<Deuda> Deuda { get; set; }
    }
}

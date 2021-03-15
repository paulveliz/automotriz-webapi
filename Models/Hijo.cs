using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class Hijo
    {
        public int Id { get; set; }
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        public byte Edad { get; set; }
        [Required]
        public bool Trabaja { get; set; }
        [Required]
        public int IdCliente { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace automotriz_webapi.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Hijos = new HashSet<Hijo>();
            Solicitudes = new HashSet<Solicitude>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        [MinLength(5, ErrorMessage = "El campo {0} Debe tener al menos 5 caracteres de longitud.")]
        public string NombreCompleto { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string Domicilio { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        [MinLength(17, ErrorMessage = "La CURP Debe tener almenos 17 caracteres de longitud."), MaxLength(18, ErrorMessage = "La CURP Debe tener maximo 18 caracteres de longitud.")]
        public string Curp { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public decimal? IngresosMensuales { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string UrlImagen { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public byte Edad { get; set; }

        public virtual ICollection<Hijo> Hijos { get; set; }
        public virtual ICollection<Solicitude> Solicitudes { get; set; }
    }
}

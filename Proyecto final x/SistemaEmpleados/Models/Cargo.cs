using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaEmpleados.Models
{
    public class Cargo
    {
        [Key]
        public int CargoID { get; set; }

        [Required(ErrorMessage = "El nombre del cargo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Cargo")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El nivel de experiencia es obligatorio")]
        [Display(Name = "Nivel de Experiencia")]
        public string NivelExperiencia { get; set; }

        // Relación con Empleados
        public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaEmpleados.Models
{
    public class Departamento
    {
        [Key]
        public int DepartamentoID { get; set; }

        [Required(ErrorMessage = "El nombre del departamento es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Departamento")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        // Relación con Empleados
        public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}

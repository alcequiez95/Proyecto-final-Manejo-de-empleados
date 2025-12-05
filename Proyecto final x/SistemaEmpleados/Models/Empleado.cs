using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEmpleados.Models
{
    public class Empleado
    {
        [Key]
        public int EmpleadoID { get; set; }

        [Required(ErrorMessage = "El nombre del empleado es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Empleado")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El departamento es obligatorio")]
        [Display(Name = "Departamento")]
        public int DepartamentoID { get; set; }

        [ForeignKey("DepartamentoID")]
        public virtual Departamento? Departamento { get; set; }

        [Required(ErrorMessage = "El cargo es obligatorio")]
        [Display(Name = "Cargo")]
        public int CargoID { get; set; }

        [ForeignKey("CargoID")]
        public virtual Cargo? Cargo { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio")]
        [Display(Name = "Salario")]
        [Column(TypeName = "decimal(10, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser mayor a 0")]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Vigente"; // Vigente o No Vigente

        // Propiedades Calculadas
        [Display(Name = "Tiempo en la Empresa")]
        public string TiempoEmpresa { get; set; } = "";

        [Display(Name = "AFP (3%)")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal AFP { get; set; }

        [Display(Name = "ARS (3.04%)")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ARS { get; set; }

        [Display(Name = "ISR")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ISR { get; set; }

        [Display(Name = "Salario Neto")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal SalarioNeto { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Método para calcular el tiempo en la empresa
        public void CalcularTiempoEmpresa()
        {
            TimeSpan tiempo = DateTime.Now - FechaInicio;
            int años = tiempo.Days / 365;
            int meses = (tiempo.Days % 365) / 30;
            TiempoEmpresa = $"{años} años y {meses} meses";
        }

        // Método para calcular AFP (3%)
        public void CalcularAFP()
        {
            AFP = Salario * 0.03m;
        }

        // Método para calcular ARS (3.04%)
        public void CalcularARS()
        {
            ARS = Salario * 0.0304m;
        }

        // Método para calcular ISR (según escala de impuestos dominicana)
        public void CalcularISR()
        {
            decimal salarioMensual = Salario;
            decimal impuesto = 0;

            if (salarioMensual > 34875)
            {
                if (salarioMensual <= 61666.67m)
                {
                    impuesto = (salarioMensual - 34875) * 0.15m;
                }
                else if (salarioMensual <= 74250)
                {
                    impuesto = (61666.67m - 34875) * 0.15m + (salarioMensual - 61666.67m) * 0.20m;
                }
                else
                {
                    impuesto = (61666.67m - 34875) * 0.15m + (74250 - 61666.67m) * 0.20m + (salarioMensual - 74250) * 0.25m;
                }
            }

            ISR = impuesto;
        }

        // Método para calcular el salario neto
        public void CalcularSalarioNeto()
        {
            CalcularAFP();
            CalcularARS();
            CalcularISR();
            SalarioNeto = Salario - AFP - ARS - ISR;
        }

        // Método para recalcular todos los valores
        public void RecalcularValores()
        {
            CalcularTiempoEmpresa();
            CalcularSalarioNeto();
        }
    }
}

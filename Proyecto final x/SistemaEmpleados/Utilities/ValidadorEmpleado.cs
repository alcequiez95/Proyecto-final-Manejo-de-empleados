using System;
using System.Globalization;

namespace SistemaEmpleados.Utilities
{
    public class ValidadorEmpleado
    {
        /// <summary>
        /// Valida que la fecha sea válida y no sea en el futuro
        /// </summary>
        public static (bool valido, string mensaje) ValidarFecha(DateTime fecha)
        {
            if (fecha > DateTime.Now)
            {
                return (false, "La fecha no puede ser en el futuro");
            }

            if (fecha.Year < 1900)
            {
                return (false, "La fecha no es válida");
            }

            return (true, "");
        }

        /// <summary>
        /// Valida que el salario sea válido
        /// </summary>
        public static (bool valido, string mensaje) ValidarSalario(decimal salario)
        {
            if (salario <= 0)
            {
                return (false, "El salario debe ser mayor a 0");
            }

            if (salario > 999999999)
            {
                return (false, "El salario es demasiado grande");
            }

            return (true, "");
        }

        /// <summary>
        /// Valida que el nombre no esté vacío
        /// </summary>
        public static (bool valido, string mensaje) ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return (false, "El nombre es obligatorio");
            }

            if (nombre.Length > 100)
            {
                return (false, "El nombre no puede exceder 100 caracteres");
            }

            return (true, "");
        }

        /// <summary>
        /// Calcula los descuentos de un empleado
        /// </summary>
        public static (decimal AFP, decimal ARS, decimal ISR, decimal neto) CalcularDescuentos(decimal salario)
        {
            decimal afp = salario * 0.03m;
            decimal ars = salario * 0.0304m;
            decimal isr = CalcularISR(salario);
            decimal neto = salario - afp - ars - isr;

            return (afp, ars, isr, neto);
        }

        /// <summary>
        /// Calcula el ISR según la escala de impuestos dominicana
        /// </summary>
        private static decimal CalcularISR(decimal salarioMensual)
        {
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

            return impuesto;
        }

        /// <summary>
        /// Calcula el tiempo en la empresa
        /// </summary>
        public static string CalcularTiempoEmpresa(DateTime fechaInicio)
        {
            try
            {
                TimeSpan tiempo = DateTime.Now - fechaInicio;
                int años = tiempo.Days / 365;
                int meses = (tiempo.Days % 365) / 30;
                return $"{años} años y {meses} meses";
            }
            catch
            {
                return "N/A";
            }
        }
    }
}

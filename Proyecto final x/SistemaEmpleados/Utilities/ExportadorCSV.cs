using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SistemaEmpleados.Models;

namespace SistemaEmpleados.Utilities
{
    public class ExportadorCSV
    {
        // Exportar Empleados a CSV
        public static string ExportarEmpleados(List<Empleado> empleados)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // Encabezado
                sb.AppendLine("ID,Nombre,Departamento,Cargo,Fecha Inicio,Salario,Estado,Tiempo en Empresa,AFP,ARS,ISR,Salario Neto");

                // Datos
                foreach (var empleado in empleados)
                {
                    sb.AppendLine($"\"{empleado.EmpleadoID}\",\"{empleado.Nombre}\",\"{empleado.Departamento?.Nombre ?? "N/A"}\",\"{empleado.Cargo?.Nombre ?? "N/A"}\",\"{empleado.FechaInicio:yyyy-MM-dd}\",\"{empleado.Salario:F2}\",\"{empleado.Estado}\",\"{empleado.TiempoEmpresa}\",\"{empleado.AFP:F2}\",\"{empleado.ARS:F2}\",\"{empleado.ISR:F2}\",\"{empleado.SalarioNeto:F2}\"");
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al exportar empleados: " + ex.Message);
            }
        }

        // Exportar Departamentos a CSV
        public static string ExportarDepartamentos(List<Departamento> departamentos)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // Encabezado
                sb.AppendLine("ID,Nombre,Descripción,Fecha Creación");

                // Datos
                foreach (var departamento in departamentos)
                {
                    sb.AppendLine($"\"{departamento.DepartamentoID}\",\"{departamento.Nombre}\",\"{departamento.Descripcion ?? "N/A"}\",\"{departamento.FechaCreacion:yyyy-MM-dd}\"");
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al exportar departamentos: " + ex.Message);
            }
        }

        // Exportar Cargos a CSV
        public static string ExportarCargos(List<Cargo> cargos)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // Encabezado
                sb.AppendLine("ID,Nombre,Descripción,Nivel Experiencia,Fecha Creación");

                // Datos
                foreach (var cargo in cargos)
                {
                    sb.AppendLine($"\"{cargo.CargoID}\",\"{cargo.Nombre}\",\"{cargo.Descripcion ?? "N/A"}\",\"{cargo.NivelExperiencia}\",\"{cargo.FechaCreacion:yyyy-MM-dd}\"");
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al exportar cargos: " + ex.Message);
            }
        }

        // Guardar CSV a archivo
        public static bool GuardarArchivoCSV(string contenido, string nombreArchivo)
        {
            try
            {
                string rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exports", nombreArchivo);
                
                // Crear carpeta si no existe
                string directorio = Path.GetDirectoryName(rutaArchivo);
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                File.WriteAllText(rutaArchivo, contenido, Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar archivo CSV: " + ex.Message);
            }
        }
    }
}

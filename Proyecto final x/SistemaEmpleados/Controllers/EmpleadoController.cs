using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEmpleados.Data;
using SistemaEmpleados.Models;
using SistemaEmpleados.Utilities;

namespace SistemaEmpleados.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly EmpleadoContext _db;

        public EmpleadoController(EmpleadoContext db)
        {
            _db = db;
        }

        // GET: Empleado - Mostrar lista de empleados
        public async Task<IActionResult> Index()
        {
            try
            {
                var empleados = await _db.Empleados
                    .Include(e => e.Departamento)
                    .Include(e => e.Cargo)
                    .ToListAsync();

                // Recalcular valores en la vista
                foreach (var empleado in empleados)
                {
                    empleado.RecalcularValores();
                }

                return View(empleados);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar empleados: " + ex.Message;
                return View(new List<Empleado>());
            }
        }

        // GET: Empleado/Create - Mostrar formulario de crear
        public async Task<IActionResult> Create()
        {
            try
            {
                var departamentos = await _db.Departamentos.ToListAsync();
                var cargos = await _db.Cargos.ToListAsync();
                
                ViewBag.DepartamentoID = new SelectList(departamentos, "DepartamentoID", "Nombre");
                ViewBag.CargoID = new SelectList(cargos, "CargoID", "Nombre");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar formulario: " + ex.Message;
                return View();
            }
        }

        // POST: Empleado/Create - Guardar nuevo empleado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            try
            {
                // Validar fecha de inicio
                if (empleado.FechaInicio > DateTime.Now)
                {
                    ModelState.AddModelError("FechaInicio", "La fecha de inicio no puede ser en el futuro");
                    var departamentos = await _db.Departamentos.ToListAsync();
                    var cargos = await _db.Cargos.ToListAsync();
                    ViewBag.DepartamentoID = new SelectList(departamentos, "DepartamentoID", "Nombre");
                    ViewBag.CargoID = new SelectList(cargos, "CargoID", "Nombre");
                    return View(empleado);
                }

                // Validar salario
                if (empleado.Salario <= 0)
                {
                    ModelState.AddModelError("Salario", "El salario debe ser mayor a 0");
                    var departamentos = await _db.Departamentos.ToListAsync();
                    var cargos = await _db.Cargos.ToListAsync();
                    ViewBag.DepartamentoID = new SelectList(departamentos, "DepartamentoID", "Nombre");
                    ViewBag.CargoID = new SelectList(cargos, "CargoID", "Nombre");
                    return View(empleado);
                }

                // Inicializar valores por defecto si no están establecidos
                if (string.IsNullOrEmpty(empleado.TiempoEmpresa))
                {
                    empleado.TiempoEmpresa = "";
                }

                if (ModelState.IsValid)
                {
                    empleado.RecalcularValores();
                    empleado.FechaCreacion = DateTime.Now;
                    _db.Empleados.Add(empleado);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Empleado creado exitosamente";
                    return RedirectToAction("Index");
                }

                // Si ModelState no es válido, mostrar errores
                var departamentos2 = await _db.Departamentos.ToListAsync();
                var cargos2 = await _db.Cargos.ToListAsync();
                ViewBag.DepartamentoID = new SelectList(departamentos2, "DepartamentoID", "Nombre");
                ViewBag.CargoID = new SelectList(cargos2, "CargoID", "Nombre");
                
                // Log de errores para debugging
                var modelStateErrors = ModelState.Values.SelectMany(v => v.Errors);
                if (modelStateErrors.Any())
                {
                    ViewBag.Error = "Por favor corrige los siguientes errores: " + 
                        string.Join(", ", modelStateErrors.Select(e => e.ErrorMessage));
                }
                
                return View(empleado);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al crear empleado: " + ex.Message;
                var departamentos = await _db.Departamentos.ToListAsync();
                var cargos = await _db.Cargos.ToListAsync();
                ViewBag.DepartamentoID = new SelectList(departamentos, "DepartamentoID", "Nombre");
                ViewBag.CargoID = new SelectList(cargos, "CargoID", "Nombre");
                return View(empleado);
            }
        }

        // GET: Empleado/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                Empleado empleado = await _db.Empleados
                    .Include(e => e.Departamento)
                    .Include(e => e.Cargo)
                    .FirstOrDefaultAsync(e => e.EmpleadoID == id);

                if (empleado == null)
                {
                    return NotFound();
                }

                var departamentos = await _db.Departamentos.ToListAsync();
                var cargos = await _db.Cargos.ToListAsync();
                ViewBag.DepartamentoID = new SelectList(departamentos, "DepartamentoID", "Nombre");
                ViewBag.CargoID = new SelectList(cargos, "CargoID", "Nombre");
                return View(empleado);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Empleado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Empleado empleado)
        {
            try
            {
                // Validar que el empleado exista
                if (!await _db.Empleados.AnyAsync(e => e.EmpleadoID == empleado.EmpleadoID))
                {
                    ModelState.AddModelError("EmpleadoID", "El empleado no existe");
                    var departamentos = await _db.Departamentos.ToListAsync();
                    var cargos = await _db.Cargos.ToListAsync();
                    ViewBag.DepartamentoID = new SelectList(departamentos, "DepartamentoID", "Nombre");
                    ViewBag.CargoID = new SelectList(cargos, "CargoID", "Nombre");
                    return View(empleado);
                }

                // Validar fecha de inicio
                if (empleado.FechaInicio > DateTime.Now)
                {
                    ModelState.AddModelError("FechaInicio", "La fecha de inicio no puede ser en el futuro");
                    var departamentos = await _db.Departamentos.ToListAsync();
                    var cargos = await _db.Cargos.ToListAsync();
                    ViewBag.DepartamentoID = new SelectList(departamentos, "DepartamentoID", "Nombre");
                    ViewBag.CargoID = new SelectList(cargos, "CargoID", "Nombre");
                    return View(empleado);
                }

                // Validar salario
                if (empleado.Salario <= 0)
                {
                    ModelState.AddModelError("Salario", "El salario debe ser mayor a 0");
                    var departamentos = await _db.Departamentos.ToListAsync();
                    var cargos = await _db.Cargos.ToListAsync();
                    ViewBag.DepartamentoID = new SelectList(departamentos, "DepartamentoID", "Nombre");
                    ViewBag.CargoID = new SelectList(cargos, "CargoID", "Nombre");
                    return View(empleado);
                }

                if (ModelState.IsValid)
                {
                    empleado.RecalcularValores();
                    _db.Entry(empleado).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Empleado actualizado exitosamente";
                    return RedirectToAction("Index");
                }

                var departamentos2 = await _db.Departamentos.ToListAsync();
                var cargos2 = await _db.Cargos.ToListAsync();
                ViewBag.DepartamentoID = new SelectList(departamentos2, "DepartamentoID", "Nombre");
                ViewBag.CargoID = new SelectList(cargos2, "CargoID", "Nombre");
                return View(empleado);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al actualizar empleado: " + ex.Message;
                var departamentos = await _db.Departamentos.ToListAsync();
                var cargos = await _db.Cargos.ToListAsync();
                ViewBag.DepartamentoID = new SelectList(departamentos, "DepartamentoID", "Nombre");
                ViewBag.CargoID = new SelectList(cargos, "CargoID", "Nombre");
                return View(empleado);
            }
        }

        // GET: Empleado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                Empleado empleado = await _db.Empleados
                    .Include(e => e.Departamento)
                    .Include(e => e.Cargo)
                    .FirstOrDefaultAsync(e => e.EmpleadoID == id);

                if (empleado == null)
                {
                    ViewBag.Error = "El empleado no existe";
                    return RedirectToAction("Index");
                }

                return View(empleado);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Empleado empleado = await _db.Empleados.FindAsync(id);
                if (empleado == null)
                {
                    ViewBag.Error = "El empleado no existe";
                    return RedirectToAction("Index");
                }

                _db.Empleados.Remove(empleado);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Empleado eliminado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Empleado/Exportar
        public async Task<IActionResult> Exportar()
        {
            try
            {
                var empleados = await _db.Empleados
                    .Include(e => e.Departamento)
                    .Include(e => e.Cargo)
                    .ToListAsync();

                foreach (var empleado in empleados)
                {
                    empleado.RecalcularValores();
                }

                string contenido = ExportadorCSV.ExportarEmpleados(empleados);
                string nombreArchivo = $"Empleados_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                ExportadorCSV.GuardarArchivoCSV(contenido, nombreArchivo);

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(contenido);
                return File(buffer, "text/csv", nombreArchivo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al exportar empleados: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}

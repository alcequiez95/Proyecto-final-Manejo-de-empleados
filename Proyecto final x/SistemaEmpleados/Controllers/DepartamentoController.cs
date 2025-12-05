using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEmpleados.Data;
using SistemaEmpleados.Models;
using SistemaEmpleados.Utilities;

namespace SistemaEmpleados.Controllers
{
    public class DepartamentoController : Controller
    {
        private readonly EmpleadoContext _db;

        public DepartamentoController(EmpleadoContext db)
        {
            _db = db;
        }

        // GET: Departamento - Mostrar lista de departamentos
        public async Task<IActionResult> Index()
        {
            try
            {
                var departamentos = await _db.Departamentos.ToListAsync();
                return View(departamentos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar departamentos: " + ex.Message;
                return View(new List<Departamento>());
            }
        }

        // GET: Departamento/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar formulario: " + ex.Message;
                return View();
            }
        }

        // POST: Departamento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Departamento departamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Departamentos.Add(departamento);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Departamento creado exitosamente";
                    return RedirectToAction("Index");
                }

                return View(departamento);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al crear departamento: " + ex.Message;
                return View(departamento);
            }
        }

        // GET: Departamento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                Departamento departamento = await _db.Departamentos.FindAsync(id);
                if (departamento == null)
                {
                    return NotFound();
                }

                return View(departamento);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar departamento: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Departamento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Departamento departamento)
        {
            try
            {
                // Validar que el departamento exista
                if (!await _db.Departamentos.AnyAsync(d => d.DepartamentoID == departamento.DepartamentoID))
                {
                    ModelState.AddModelError("DepartamentoID", "El departamento no existe");
                    return View(departamento);
                }

                if (ModelState.IsValid)
                {
                    _db.Entry(departamento).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Departamento actualizado exitosamente";
                    return RedirectToAction("Index");
                }

                return View(departamento);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al actualizar departamento: " + ex.Message;
                return View(departamento);
            }
        }

        // GET: Departamento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                Departamento departamento = await _db.Departamentos.FindAsync(id);
                if (departamento == null)
                {
                    ViewBag.Error = "El departamento no existe";
                    return RedirectToAction("Index");
                }

                // Validar que no haya empleados asociados
                if (await _db.Empleados.AnyAsync(e => e.DepartamentoID == id))
                {
                    ViewBag.Error = "No se puede eliminar el departamento porque tiene empleados asociados";
                    return RedirectToAction("Index");
                }

                return View(departamento);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar departamento: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Departamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Departamento departamento = await _db.Departamentos.FindAsync(id);
                if (departamento == null)
                {
                    ViewBag.Error = "El departamento no existe";
                    return RedirectToAction("Index");
                }

                // Validar que no haya empleados asociados
                if (await _db.Empleados.AnyAsync(e => e.DepartamentoID == id))
                {
                    ViewBag.Error = "No se puede eliminar el departamento porque tiene empleados asociados";
                    return RedirectToAction("Index");
                }

                _db.Departamentos.Remove(departamento);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Departamento eliminado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar departamento: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Departamento/Exportar
        public async Task<IActionResult> Exportar()
        {
            try
            {
                var departamentos = await _db.Departamentos.ToListAsync();
                string contenido = ExportadorCSV.ExportarDepartamentos(departamentos);
                string nombreArchivo = $"Departamentos_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                ExportadorCSV.GuardarArchivoCSV(contenido, nombreArchivo);

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(contenido);
                return File(buffer, "text/csv", nombreArchivo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al exportar departamentos: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}

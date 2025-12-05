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
    public class CargoController : Controller
    {
        private readonly EmpleadoContext _db;

        public CargoController(EmpleadoContext db)
        {
            _db = db;
        }

        // GET: Cargo - Mostrar lista de cargos
        public async Task<IActionResult> Index()
        {
            try
            {
                var cargos = await _db.Cargos.ToListAsync();
                return View(cargos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar cargos: " + ex.Message;
                return View(new List<Cargo>());
            }
        }

        // GET: Cargo/Create
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

        // POST: Cargo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cargo cargo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Cargos.Add(cargo);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Cargo creado exitosamente";
                    return RedirectToAction("Index");
                }

                return View(cargo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al crear cargo: " + ex.Message;
                return View(cargo);
            }
        }

        // GET: Cargo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                Cargo cargo = await _db.Cargos.FindAsync(id);
                if (cargo == null)
                {
                    return NotFound();
                }

                return View(cargo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar cargo: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Cargo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cargo cargo)
        {
            try
            {
                // Validar que el cargo exista
                if (!await _db.Cargos.AnyAsync(c => c.CargoID == cargo.CargoID))
                {
                    ModelState.AddModelError("CargoID", "El cargo no existe");
                    return View(cargo);
                }

                if (ModelState.IsValid)
                {
                    _db.Entry(cargo).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Cargo actualizado exitosamente";
                    return RedirectToAction("Index");
                }

                return View(cargo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al actualizar cargo: " + ex.Message;
                return View(cargo);
            }
        }

        // GET: Cargo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                Cargo cargo = await _db.Cargos.FindAsync(id);
                if (cargo == null)
                {
                    ViewBag.Error = "El cargo no existe";
                    return RedirectToAction("Index");
                }

                // Validar que no haya empleados asociados
                if (await _db.Empleados.AnyAsync(e => e.CargoID == id))
                {
                    ViewBag.Error = "No se puede eliminar el cargo porque tiene empleados asociados";
                    return RedirectToAction("Index");
                }

                return View(cargo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar cargo: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Cargo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Cargo cargo = await _db.Cargos.FindAsync(id);
                if (cargo == null)
                {
                    ViewBag.Error = "El cargo no existe";
                    return RedirectToAction("Index");
                }

                // Validar que no haya empleados asociados
                if (await _db.Empleados.AnyAsync(e => e.CargoID == id))
                {
                    ViewBag.Error = "No se puede eliminar el cargo porque tiene empleados asociados";
                    return RedirectToAction("Index");
                }

                _db.Cargos.Remove(cargo);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Cargo eliminado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar cargo: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Cargo/Exportar
        public async Task<IActionResult> Exportar()
        {
            try
            {
                var cargos = await _db.Cargos.ToListAsync();
                string contenido = ExportadorCSV.ExportarCargos(cargos);
                string nombreArchivo = $"Cargos_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                ExportadorCSV.GuardarArchivoCSV(contenido, nombreArchivo);

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(contenido);
                return File(buffer, "text/csv", nombreArchivo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al exportar cargos: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}

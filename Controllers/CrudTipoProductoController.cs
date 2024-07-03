using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentasMVC.Data;
using ProyectoFinalVentasMVC.Models;
using ProyectoFinalVentasMVC.ViewsModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalVentasMVC.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CrudTipoProductoController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public CrudTipoProductoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        // GET: CrudTipoProducto
        // Acción para mostrar la lista de tipo de productos
        public IActionResult ListarTipoProducto()
        {
            IQueryable<TipoProducto> tipoProducto = _appDBContext.TiposProductos.AsQueryable();

            // Obtener la lista de tipo de productos
            var listarTipoProducto = tipoProducto.ToList();
            return View(listarTipoProducto);
        }

        // GET: CrudTipoProducto/Agregar
        public IActionResult AgregarTipoProducto()
        {
            return View();
        }

        // POST: CrudTipoProducto/AgregarTipoProducto
        [HttpPost]
        public IActionResult AgregarTipoProducto(ViewsModelTipoProducto tipoProductoViewModel)
        {
            if (ModelState.IsValid)
            {
                var tipoProducto = new TipoProducto
                {
                    Tipo = tipoProductoViewModel.Tipo
                };

                _appDBContext.TiposProductos.Add(tipoProducto);
                _appDBContext.SaveChanges();
                return RedirectToAction(nameof(ListarTipoProducto));
            }
            return View(tipoProductoViewModel);
        }

        // GET: CrudTipoProducto/ActualizarTipoProducto
        public IActionResult ActualizarTipoProducto(int idtipo)
        {
            var tipoProducto = _appDBContext.TiposProductos.FirstOrDefault(tp => tp.TipoId == idtipo);
            if (tipoProducto == null)
            {
                return NotFound();
            }

            // Aquí se mapea el modelo de dominio TipoProducto al ViewModel ViewsModelTipoProducto
            var tipoProductoViewModel = new ViewsModelTipoProducto
            {
                TipoId = tipoProducto.TipoId,
                Tipo = tipoProducto.Tipo
            };

            return View(tipoProductoViewModel);
        }

        // POST: CrudTipoProducto/ActualizarTipoProducto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarTipoProducto(int idtipo, ViewsModelTipoProducto tipoProductoViewModel)
        {
            if (idtipo != tipoProductoViewModel.TipoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Aquí se mapea el ViewModel ViewsModelTipoProducto al modelo de dominio TipoProducto
                    var tipoProducto = new TipoProducto
                    {
                        TipoId = tipoProductoViewModel.TipoId,
                        Tipo = tipoProductoViewModel.Tipo
                    };

                    _appDBContext.Update(tipoProducto);
                    _appDBContext.SaveChanges();
                    return RedirectToAction(nameof(ListarTipoProducto));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_appDBContext.TiposProductos.Any(tp => tp.TipoId == idtipo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Manejar otros errores de actualización aquí si es necesario
                    }
                }
            }

            // Si ModelState.IsValid es falso, significa que hay errores de validación. 
            // Devolver la vista con el ViewModel para que el usuario pueda corregir los errores.
            return View(tipoProductoViewModel);
        }


        // GET: CrudTipoProducto/EliminarTipoProducto/
        // Acción para mostrar la vista de eliminación (advertencia inicial)
        public IActionResult EliminarTipoProducto(int? idtipo)
        {
            if (idtipo == null)
            {
                return NotFound();
            }

            var tipoProducto = _appDBContext.TiposProductos
                                            .FirstOrDefault(tp => tp.TipoId == idtipo);

            if (tipoProducto == null)
            {
                return NotFound();
            }

            return View(tipoProducto);
        }

        // Acción para mostrar la vista de confirmación de eliminación
        public IActionResult ConfirmDelete(int id)
        {
            var tipoProducto = _appDBContext.TiposProductos
                                            .FirstOrDefault(tp => tp.TipoId == id);

            if (tipoProducto == null)
            {
                return NotFound();
            }

            return View(tipoProducto);
        }

        // POST: Confirmar la eliminación del tipo de producto y sus productos asociados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeleteConfirmed(int idtipo)
        {
            var tipoProducto = _appDBContext.TiposProductos
                                            .Include(tp => tp.Productos)
                                            .FirstOrDefault(tp => tp.TipoId == idtipo);

            if (tipoProducto == null)
            {
                return NotFound();
            }

            // Eliminar en cascada
            _appDBContext.TiposProductos.Remove(tipoProducto);
            _appDBContext.SaveChanges();

            // Redirigir a la lista de tipos de productos
            return RedirectToAction("ListarTipoProducto");
        }


        // Acción para cerrar sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentasMVC.Data;
using ProyectoFinalVentasMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ProyectoFinalVentasMVC.Controllers
{
    public class CrudProductoController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public CrudProductoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        // Acción para listar productos
        [Authorize(Roles = "Administrador,Vendedor")] // Acceso para roles Administrador y Vendedor
        public IActionResult ListarProductos()
        {
            var productos = _appDBContext.Productos.Include(p => p.TipoProducto).ToList();
            return View(productos);
        }
        [Authorize(Roles = "Administrador")]
        // Método GET para agregar producto
        public IActionResult AgregarProducto()
        {
            ViewBag.TiposProductos = _appDBContext.TiposProductos.ToList();
            return View();
        }

        // POST: /CrudProducto/AgregarProducto
        [Authorize(Roles = "Administrador")]
        // Método POST para agregar producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarProducto(Producto producto)
        {
            // Asignar el TipoProducto basado en IdTipo
            producto.TipoProducto = _appDBContext.TiposProductos
                .FirstOrDefault(tp => tp.TipoId == producto.IdTipo);

            if (producto.TipoProducto == null)
            {
                ModelState.AddModelError("IdTipo", "Seleccione un tipo de producto válido.");
                ViewBag.TiposProductos = _appDBContext.TiposProductos.ToList();
                return View(producto);
            }
                    _appDBContext.Productos.Add(producto);
                    _appDBContext.SaveChanges();
                    return RedirectToAction(nameof(ListarProductos));


            // Recargar tipos de productos para la lista desplegable en caso de error
            ViewBag.TiposProductos = _appDBContext.TiposProductos.ToList();
            return View(producto);
        }


        [Authorize(Roles = "Administrador")]
        // GET: /CrudProducto/ActualizarProducto/5
        public IActionResult ActualizarProducto(int id)
        {
            var producto = _appDBContext.Productos
                .Include(p => p.TipoProducto)
                .FirstOrDefault(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            ViewBag.TiposProductos = _appDBContext.TiposProductos.ToList(); 

            return View(producto);
        }

        // POST: /CrudProducto/ActualizarProducto/
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarProducto(int id, Producto producto)
        {

            producto.TipoProducto = _appDBContext.TiposProductos
            .FirstOrDefault(tp => tp.TipoId == producto.IdTipo);
            if (id != producto.Id)
            {
                return NotFound();
            }

            _appDBContext.Update(producto);
            _appDBContext.SaveChanges();
            return RedirectToAction(nameof(ListarProductos));


            if (ModelState.IsValid)
            {
                try
                {
                
                    _appDBContext.Update(producto);
                    _appDBContext.SaveChanges();
                    return RedirectToAction(nameof(ListarProductos));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.TiposProductos = _appDBContext.TiposProductos.ToList(); 

            return View(producto);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarProducto(int id)
        {
            var producto = _appDBContext.Productos
    .Include(p => p.TipoProducto)
    .FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto); // Pasando un solo objeto Cliente a la vista
        }


        // Acción para confirmar la eliminación de cliente
        [Authorize(Roles = "Administrador")] // Requiere el rol de Administrador para acceder
        public IActionResult ConfirmDelete(int id)
        {
            var producto = _appDBContext.Productos.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            return View("ConfirmDelete", producto); // Mostrar la vista de confirmación adaptada
        }

        
        // Acción para confirmar la eliminación de cliente
        [Authorize(Roles = "Administrador")] // Requiere el rol de Administrador para acceder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarEliminarProducto(int id)
        {
            var producto = _appDBContext.Productos.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            _appDBContext.Productos.Remove(producto);
            _appDBContext.SaveChanges();

            // Redirigir a la vista de listado de clientes
            return RedirectToAction(nameof(ListarProductos));
        }

        // Método para verificar la existencia de un producto por su ID
        private bool ProductoExists(int id)
        {
            return _appDBContext.Productos.Any(e => e.Id == id);
        }

        // Acción para cerrar sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}

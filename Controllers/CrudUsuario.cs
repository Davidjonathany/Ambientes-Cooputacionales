using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentasMVC.Data;
using ProyectoFinalVentasMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoFinalVentasMVC.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CrudUsuario : Controller
    {
        private readonly AppDBContext _appDBContext;

        public CrudUsuario(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        // Acción para mostrar el formulario de agregar Usuarios (GET)
        [HttpGet]
        public IActionResult AgregarUsuario()
        {
            return View(); // Devuelve la vista para agregar usuarios
        }

        //Accion para agregar un nuevo Usuario (POST)
        [HttpPost]
        public IActionResult AgregarUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Si el modelo es válido, agregar el usuario a la base de datos
                _appDBContext.Usuarios.Add(usuario);
                _appDBContext.SaveChanges(); // Guardar cambios en la base de datos
                return RedirectToAction(nameof(ListarUsuario)); // Redirigir a la lista de usuario
            }
            // Si el modelo no es válido, regresar a la vista de agregar usuario para corregir los errores
            return View(usuario);
        }


        // Acción para mostrar la lista de clientes
        public IActionResult ListarUsuario()
        {
            IQueryable<Usuario> usuarios = _appDBContext.Usuarios.AsQueryable();

            // Obtener la lista de clientes
            var ListarUsuario = usuarios.ToList();
            return View(ListarUsuario);
        }


        // Acción para mostrar la vista de actualizar información de un usuario
        public IActionResult ActualizarUsuario(int id)
        {
            var usuario = _appDBContext.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // Acción para actualizar la información de un usuario (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar usuario en la base de datos
                    _appDBContext.Update(usuario);
                    _appDBContext.SaveChanges();
                    return RedirectToAction(nameof(ListarUsuario));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_appDBContext.Usuarios.Any(u => u.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(usuario);
        }

        // Acción para mostrar la vista de eliminación de usuario (GET)
        [HttpGet]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = _appDBContext.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario); // Pasando un solo objeto Usuario a la vista
        }

        // Acción para confirmar la eliminación de usuario (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarEliminarUsuario(int id)
        {
            var usuario = _appDBContext.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            _appDBContext.Usuarios.Remove(usuario);
            _appDBContext.SaveChanges();

            // Redirigir a la vista de listado de usuarios
            return RedirectToAction(nameof(ListarUsuario));
        }

        // Acción para cerrar sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}

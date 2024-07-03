using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentasMVC.Data;
using ProyectoFinalVentasMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoFinalVentasMVC.Controllers
{
    public class CrudCliente : Controller
    {
        private readonly AppDBContext _appDBContext;

        public CrudCliente(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        // Acción para mostrar la lista de clientes
        [Authorize(Roles = "Administrador,Vendedor")] // Requiere el rol de Administrador o Vendedor para acceder

        public IActionResult ListarClientes(string searchCedula)
        {
            IQueryable<Cliente> clientes = _appDBContext.Clientes.AsQueryable();

            if (!string.IsNullOrEmpty(searchCedula))
            {
                // Filtrar por cédula si se proporciona en la búsqueda
                clientes = clientes.Where(c => c.Cedula.ToString() == searchCedula);
            }

            // Obtener la lista de clientes
            var listaClientes = clientes.ToList();

            // Verificar si se encontraron resultados
            if (listaClientes.Count == 0 && !string.IsNullOrEmpty(searchCedula))
            {
                // No se encontraron resultados para la búsqueda especificada
                TempData["Mensaje"] = "No se encontraron clientes con la cédula proporcionada.";
                // Volver a cargar todos los clientes
                listaClientes = _appDBContext.Clientes.ToList();
            }

            return View(listaClientes);
        }

        // Acción para mostrar el formulario de agregar cliente (GET)
        [Authorize(Roles = "Vendedor")] // Requiere el rol de Vendedor para acceder
        [HttpGet]
        public IActionResult AgregarCliente()
        {
            return View(); // Devuelve la vista para agregar cliente
        }

        // Acción para agregar un nuevo cliente (POST)
        [Authorize(Roles = "Vendedor")] // Requiere el rol de Vendedor para acceder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarCliente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                // Si el modelo es válido, agregar el cliente a la base de datos
                _appDBContext.Clientes.Add(cliente);
                _appDBContext.SaveChanges(); // Guardar cambios en la base de datos
                return RedirectToAction(nameof(ListarClientes)); // Redirigir a la lista de clientes
            }
            // Si el modelo no es válido, regresar a la vista de agregar cliente para corregir los errores
            return View(cliente);
        }


        // Acción para actualizar información de un cliente
        [Authorize(Roles = "Administrador,Vendedor")] // Requiere el rol de Administrador o Vendedor para acceder
        public IActionResult ActualizarCliente(int id)
        {
            var cliente = _appDBContext.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [Authorize(Roles = "Administrador,Vendedor")] // Requiere el rol de Administrador o Vendedor para acceder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar cliente en la base de datos
                    _appDBContext.Update(cliente);
                    _appDBContext.SaveChanges();
                    return RedirectToAction(nameof(ListarClientes));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_appDBContext.Clientes.Any(c => c.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(cliente);
        }


        // Acción para mostrar la vista de eliminación de cliente
        [Authorize(Roles = "Administrador")] // Requiere el rol de Administrador para acceder
        public IActionResult EliminarCliente(int id)
        {
            var cliente = _appDBContext.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente); // Pasando un solo objeto Cliente a la vista
        }

        // Acción para confirmar la eliminación de cliente
        [Authorize(Roles = "Administrador")] // Requiere el rol de Administrador para acceder
        public IActionResult ConfirmDelete(int id)
        {
            var cliente = _appDBContext.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente); // Mostrar la vista de confirmación adaptada
        }

        // Acción para confirmar la eliminación de cliente
        [Authorize(Roles = "Administrador")] // Requiere el rol de Administrador para acceder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarEliminarCliente(int id)
        {
            var cliente = _appDBContext.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            _appDBContext.Clientes.Remove(cliente);
            _appDBContext.SaveChanges();

            // Redirigir a la vista de listado de clientes
            return RedirectToAction(nameof(ListarClientes));
        }

        // Acción para cerrar sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}

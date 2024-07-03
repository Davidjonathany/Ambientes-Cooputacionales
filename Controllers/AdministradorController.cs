using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentasMVC.Data;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentasMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoFinalVentasMVC.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministradorController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public AdministradorController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Acción para redirigir a la lista de clientes
        public IActionResult Clientes()
        {
            return RedirectToAction("ListarClientes", "CrudCliente");
        }
        // Acción para redirigir a la lista de usuarios
        public IActionResult Usuarios()
        {
            return RedirectToAction("ListarUsuario", "CrudUsuario");
        }
        // Acción para redirigir a la lista de tipo de producto
        public IActionResult TipoProducto()
        {
            return RedirectToAction("ListarTipoProducto", "CrudTipoProducto");
        }
        // Acción para redirigir a la lista de productos
        public IActionResult Producto()
        {
            return RedirectToAction("ListarProductos", "CrudProducto");
        }
        public IActionResult Factura()
        {
            return RedirectToAction("Crear", "Facturacion");
        }
        // Acción para cerrar sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}

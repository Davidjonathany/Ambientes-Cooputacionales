using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalVentasMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ProyectoFinalVentasMVC.Data;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoFinalVentasMVC.Controllers
{
    [Authorize(Roles = "Vendedor")]
    public class VendedorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly AppDBContext _appDBContext;

        public VendedorController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public IActionResult Clientes()
        {
            return RedirectToAction("ListarClientes", "CrudCliente");
        }
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

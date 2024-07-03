using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentasMVC.Data;
using ProyectoFinalVentasMVC.Models;
using ProyectoFinalVentasMVC.ViewsModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ProyectoFinalVentasMVC.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public AccesoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        // GET: Acceso/Login
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return View();
            }
            return View();
        }

        // POST: Acceso/Login
        [HttpPost]
        public async Task<IActionResult> Login(Login modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            Usuario usuario_encontrado = await _appDBContext.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == modelo.Correo && u.Clave == modelo.Clave);

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "Credenciales no válidas. Verifica e intenta nuevamente";
                return View();
            }

            // Obtener el rol seleccionado por el usuario desde el modelo
            string rolSeleccionado = modelo.Rol;

            // Validar si el rol seleccionado coincide con el rol registrado en la base de datos
            if (rolSeleccionado != usuario_encontrado.Rol)
            {
                ViewData["Mensaje"] = "Credenciales no válidas. Verifica e intenta nuevamente";
                return View();
            }

            // Establecer el rol dinámicamente antes de autenticar al usuario
            usuario_encontrado.Rol = rolSeleccionado;

            // Autenticar al usuario y crear las reclamaciones
            List<Claim> claims = new List<Claim>
    {
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
                new Claim(ClaimTypes.Email, usuario_encontrado.Correo),
                new Claim(ClaimTypes.Role, usuario_encontrado.Rol)
    };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            // Redireccionar según el rol del usuario
            switch (usuario_encontrado.Rol)
            {
                case "Administrador":
                    return RedirectToAction("Index", "Administrador");

                case "Vendedor":
                    return RedirectToAction("Index", "Vendedor");

                default:
                    return View(); // Devuelve la vista del Login
            }
        }


        // Acción para cerrar sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

    }
}

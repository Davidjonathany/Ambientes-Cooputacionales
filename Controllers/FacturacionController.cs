using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalVentasMVC.Data;
using ProyectoFinalVentasMVC.Models;
using ProyectoFinalVentasMVC.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoFinalVentasMVC.Controllers
{
    public class FacturacionController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public FacturacionController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        // Acción para mostrar la vista de creación de factura
        public IActionResult Crear()
        {
            var productos = _appDBContext.Productos
                .Select(p => new ProductoFacturaViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    PrecioUnitario = p.Precio,
                    TieneIVA = p.Iva,
                    TipoProducto = p.TipoProducto
                })
                .ToList();

            var viewModel = new FacturacionViewModel
            {
                Cliente = new Cliente(),
                Productos = productos,
                NombresProductos = productos.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nombre
                }).ToList(),
                DetallesFactura = new List<FacturaDetalle>(),
                Subtotal = 0,
                Descuento = 0,
                Total = 0
            };

            return View(viewModel);
        }

        public IActionResult ObtenerDetallesProducto(int idProducto)
        {
            var producto = _appDBContext.Productos
                .Where(p => p.Id == idProducto)
                .Select(p => new
                {
                    Precio = p.Precio,
                    TieneIVA = p.Iva
                })
                .FirstOrDefault();

            if (producto == null)
            {
                return NotFound();
            }

            return Json(producto);
        }

        [HttpPost]
        public IActionResult BuscarClientePorCedula(string searchCedula)
        {
            var viewModel = new FacturacionViewModel
            {
                Cliente = new Cliente(),
                Productos = new List<ProductoFacturaViewModel>(),
                NombresProductos = _appDBContext.Productos
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Nombre
                    }).ToList(), // Asegúrate de que esta línea esté correcta
                FacturaDetalle = new FacturaDetalle(),
                DetallesFactura = new List<FacturaDetalle>(),
                Subtotal = 0,
                Descuento = 0,
                Total = 0
            };

            if (!string.IsNullOrEmpty(searchCedula))
            {
                viewModel.Cliente = _appDBContext.Clientes.FirstOrDefault(c => c.Cedula == searchCedula);

                if (viewModel.Cliente == null)
                {
                    TempData["Mensaje"] = "No se encontró ningún cliente con la cédula proporcionada.";
                }
            }

            return View("Crear", viewModel);
        }

        // Nueva acción para agregar productos a la factura
        [HttpPost]
        public IActionResult AgregarProducto(FacturacionViewModel viewModel)
        {
            var producto = _appDBContext.Productos.FirstOrDefault(p => p.Id == viewModel.FacturaDetalle.IdProducto);
            if (producto != null)
            {
                viewModel.FacturaDetalle.Producto = producto;
                viewModel.FacturaDetalle.Total = (producto.Precio * viewModel.FacturaDetalle.Cantidad) - viewModel.FacturaDetalle.Descuento;

                if (viewModel.DetallesFactura == null)
                {
                    viewModel.DetallesFactura = new List<FacturaDetalle>();
                }

                viewModel.DetallesFactura.Add(viewModel.FacturaDetalle);
                viewModel.Subtotal += viewModel.FacturaDetalle.Total;
                viewModel.Descuento += viewModel.FacturaDetalle.Descuento;
                viewModel.Total = viewModel.Subtotal - viewModel.Descuento;
            }

            // Recargar la lista de productos para el dropdown
            viewModel.NombresProductos = _appDBContext.Productos.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();

            return View("Crear", viewModel);
        }
    }
}

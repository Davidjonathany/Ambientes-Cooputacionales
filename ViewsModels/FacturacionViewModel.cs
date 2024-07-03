using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFinalVentasMVC.Models;

namespace ProyectoFinalVentasMVC.ViewModels
{
    public class FacturacionViewModel
    {
        public Cliente Cliente { get; set; }
        public List<ProductoFacturaViewModel> Productos { get; set; }
        public List<SelectListItem> NombresProductos { get; set; } // Cambia el tipo a List<SelectListItem>
        public FacturaDetalle FacturaDetalle { get; set; }
        public List<FacturaDetalle> DetallesFactura { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
    }
}
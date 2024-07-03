using ProyectoFinalVentasMVC.Models;
using System.Collections.Generic;

namespace ProyectoFinalVentasMVC.ViewModels
{
    public class FacturaViewModel
    {
        public Cliente Cliente { get; set; }
        public List<FacturaDetalleViewModel> DetallesFactura { get; set; }
        public decimal Iva { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public FacturaDetalle FacturaDetalle { get; set; }
        public FacturaViewModel()
        {
            DetallesFactura = new List<FacturaDetalleViewModel>();
        }
    }
}

using ProyectoFinalVentasMVC.Models;

namespace ProyectoFinalVentasMVC.ViewModels
{
    public class FacturaDetalleViewModel
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public bool TieneIVA { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
    }
}

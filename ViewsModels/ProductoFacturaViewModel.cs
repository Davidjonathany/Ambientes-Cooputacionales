using ProyectoFinalVentasMVC.Models;

namespace ProyectoFinalVentasMVC.ViewModels
{
    public class ProductoFacturaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioUnitario { get; set; }
        public bool TieneIVA { get; set; }
        public TipoProducto TipoProducto { get; set; }
    }
}

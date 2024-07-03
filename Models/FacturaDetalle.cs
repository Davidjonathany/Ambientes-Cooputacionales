using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalVentasMVC.Models
{
    public class FacturaDetalle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID de la factura es obligatorio")]
        public int IdFactura { get; set; }

        [Required(ErrorMessage = "El ID del producto es obligatorio")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El descuento es obligatorio")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El descuento debe tener hasta dos decimales.")]
        public decimal Descuento { get; set; }

        [Required(ErrorMessage = "El total es obligatorio")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El total debe tener hasta dos decimales.")]
        public decimal Total { get; set; }

        [ForeignKey("IdFactura")]
        public virtual Factura Factura { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Producto Producto { get; set; }
    }
}

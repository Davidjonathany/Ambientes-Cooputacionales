using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalVentasMVC.Models
{
    public class Factura
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El ID del cliente es obligatorio")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El total es obligatorio")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El total debe tener hasta dos decimales.")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El IVA es obligatorio")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El IVA debe tener hasta dos decimales.")]
        public decimal Iva { get; set; }

        [Required(ErrorMessage = "El subtotal es obligatorio")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El subtotal debe tener hasta dos decimales.")]
        public decimal SubTotal { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        // Relación uno a muchos con FacturaDetalle
        public virtual ICollection<FacturaDetalle> Detalles { get; set; }
    }
}

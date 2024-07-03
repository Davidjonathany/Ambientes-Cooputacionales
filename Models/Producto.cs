using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalVentasMVC.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El ID del tipo de producto es obligatorio")]
        public int IdTipo { get; set; }

        [Required(ErrorMessage = "El código de barras es obligatorio")]
        public string CodigoBarras { get; set; }

        [Required(ErrorMessage = "El IVA es obligatorio")]
        public bool Iva { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El precio debe tener hasta dos decimales.")]
        public decimal Precio { get; set; }

        [ForeignKey("IdTipo")]
        public virtual TipoProducto TipoProducto { get; set; }
    }
}

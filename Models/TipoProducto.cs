using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalVentasMVC.Models
{
    public class TipoProducto
    {
        public int TipoId { get; set; }

        [Required(ErrorMessage = "El tipo de producto es obligatorio")]
        public string Tipo { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}

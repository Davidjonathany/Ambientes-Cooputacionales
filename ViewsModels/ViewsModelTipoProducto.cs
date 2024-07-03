using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalVentasMVC.ViewsModels
{
    public class ViewsModelTipoProducto
    {
        public int TipoId { get; set; }

        [Required(ErrorMessage = "El tipo de producto es obligatorio")]
        public string Tipo { get; set; }
    }
}

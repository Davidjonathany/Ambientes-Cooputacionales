using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalVentasMVC.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es una dirección válida")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        public string Clave { get; set; }
        [Required(ErrorMessage = "El Rol es necesario")]
        public string Rol { get; set; }
    }
}

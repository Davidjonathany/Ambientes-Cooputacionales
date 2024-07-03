using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalVentasMVC.ViewsModels
{
    public class Login
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es una dirección válida")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        public string Clave { get; set; }

        [Required(ErrorMessage = "El Rol es obligatorio")]
        public string Rol { get; set; }
    }
}


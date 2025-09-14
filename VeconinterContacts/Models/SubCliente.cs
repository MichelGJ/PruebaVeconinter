using System.ComponentModel.DataAnnotations;
using VeconinterContacts.Models;

namespace VeconinterContacts.Models;
public class SubCliente
{
    public int Id { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder {1} caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un email válido.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Teléfono")]
    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [StringLength(11, ErrorMessage = "El teléfono no puede exceder {1} caracteres.")]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
}

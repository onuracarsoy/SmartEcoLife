using System.ComponentModel.DataAnnotations;

namespace SmartEcoLife.Shared.Dtos.UsersDtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "E-posta alanı boş bırakılamaz"), EmailAddress(ErrorMessage = "Geçerli bir email giriniz")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        public string Password { get; set; } 
    }
}

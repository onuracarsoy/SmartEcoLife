using System.ComponentModel.DataAnnotations;

namespace SmartEcoLife.Shared.Dtos.UsersDtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "E-posta alanı boş bırakılamaz"), EmailAddress(ErrorMessage = "Geçerli bir email giriniz")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz"), MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tekrar şifrenizi giriniz"), Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ad Soyad alanı boş bırakılamaz")]
        public string DisplayName { get; set; } = string.Empty;
    }
}

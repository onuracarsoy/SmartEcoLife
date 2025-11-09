using System.ComponentModel.DataAnnotations;

namespace SmartEcoLife.Shared.Dtos.UsersDtos
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Mevcut şifre zorunlu.")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni şifre zorunlu.")]
        [MinLength(6, ErrorMessage = "En az 6 karakter olmalı.")]
        public string NewPassword { get; set; } = string.Empty;

        [Compare(nameof(NewPassword), ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

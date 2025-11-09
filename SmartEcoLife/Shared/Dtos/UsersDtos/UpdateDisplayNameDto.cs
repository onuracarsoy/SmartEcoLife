using System.ComponentModel.DataAnnotations;

namespace SmartEcoLife.Shared.Dtos.UsersDtos
{
    public class UpdateDisplayNameDto
    {
      
            [Required(ErrorMessage = "Ad Soyad boş bırakılamaz")]
            [MinLength(3, ErrorMessage = "En az 3 karakter olmalı")]
            public string NewDisplayName { get; set; } = string.Empty;
        
    }
}

using SmartEcoLife.Features.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartEcoLife.Shared.Dtos.GoalDtos
{
    public class GoalDto
    {

        public Guid Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı bilgisi zorunludur.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [MaxLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Hedef miktarı 0'dan büyük olmalıdır.")]
        public decimal? TargetAmount { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Geçerli bir tarih formatı giriniz.")]
        public DateTimeOffset? DueDate { get; set; }

        public bool Achieved { get; set; } = false;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}

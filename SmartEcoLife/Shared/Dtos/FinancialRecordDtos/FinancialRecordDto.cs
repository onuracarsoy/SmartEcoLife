using System.ComponentModel.DataAnnotations;

namespace SmartEcoLife.Shared.Dtos.FinancialRecordDtos
{
    public class FinancialRecordDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Tutar alanı boş bırakılamaz.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Başlık alanı boş bırakılamaz.")]
        [MaxLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Kayıt türü seçilmelidir.")]
        [RegularExpression("^(Income|Expense)$", ErrorMessage = "Geçerli bir kayıt türü seçiniz (Gelir veya Gider).")]
        public string Type { get; set; } = "Expense"; // "Income" veya "Expense"

        [Required(ErrorMessage = "Tarih alanı boş bırakılamaz.")]
        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;

        [Required(ErrorMessage = "Bir kategori seçilmelidir.")]
        public Guid? CategoryId { get; set; }

        public string? CategoryName { get; set; } 
    }
}

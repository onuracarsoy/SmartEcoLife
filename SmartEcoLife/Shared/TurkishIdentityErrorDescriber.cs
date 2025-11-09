using Microsoft.AspNetCore.Identity;

namespace SmartEcoLife.Shared
{
    public class TurkishIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
            => new() { Code = nameof(DefaultError), Description = "Bilinmeyen bir hata oluştu." };

        public override IdentityError DuplicateEmail(string email)
            => new() { Code = nameof(DuplicateEmail), Description = $"{email} adresi zaten kullanımda." };

        public override IdentityError DuplicateUserName(string userName)
            => new() { Code = nameof(DuplicateUserName), Description = $"{userName} kullanıcı adı zaten alınmış." };

        public override IdentityError InvalidEmail(string email)
            => new() { Code = nameof(InvalidEmail), Description = $"{email} geçerli bir e-posta adresi değil." };

        public override IdentityError InvalidUserName(string userName)
            => new() { Code = nameof(InvalidUserName), Description = $"{userName} geçerli bir kullanıcı adı değil." };

        public override IdentityError PasswordTooShort(int length)
            => new() { Code = nameof(PasswordTooShort), Description = $"Şifre en az {length} karakter olmalıdır." };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Şifre en az bir özel karakter içermelidir." };

        public override IdentityError PasswordRequiresDigit()
            => new() { Code = nameof(PasswordRequiresDigit), Description = "Şifre en az bir rakam içermelidir." };

        public override IdentityError PasswordRequiresLower()
            => new() { Code = nameof(PasswordRequiresLower), Description = "Şifre en az bir küçük harf içermelidir." };

        public override IdentityError PasswordRequiresUpper()
            => new() { Code = nameof(PasswordRequiresUpper), Description = "Şifre en az bir büyük harf içermelidir." };

        public override IdentityError PasswordMismatch()
            => new() { Code = nameof(PasswordMismatch), Description = "Girilen şifreler eşleşmiyor." };

        public override IdentityError InvalidToken()
            => new() { Code = nameof(InvalidToken), Description = "Geçersiz veya süresi dolmuş doğrulama kodu." };

        public override IdentityError UserAlreadyHasPassword()
            => new() { Code = nameof(UserAlreadyHasPassword), Description = "Kullanıcının zaten bir şifresi var." };
    }
}

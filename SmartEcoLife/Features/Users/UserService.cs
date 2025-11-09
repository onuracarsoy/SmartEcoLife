using AutoMapper;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using SmartEcoLife.Shared.Dtos.UsersDtos;
using System.Security.Claims;

namespace SmartEcoLife.Features.Users
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AuthenticationStateProvider authenticationStateProvider, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.authenticationStateProvider = authenticationStateProvider;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var user = _mapper.Map<ApplicationUser>(dto);
            return await _userManager.CreateAsync(user, dto.Password);
        }

        public async Task<SignInResult> LoginAsync(LoginDto dto)
        {
            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, isPersistent: false, lockoutOnFailure: false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string?> GetDisplayNameAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var principalUser = authState.User;

 
            if (principalUser?.Identity == null || !principalUser.Identity.IsAuthenticated)
                return null;

            var userId = principalUser.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return null;

         
            var appUser = await _userManager.FindByIdAsync(userId);
            return appUser?.DisplayName;
        }

        public async Task<IdentityResult> UpdateDisplayNameAsync(Guid userId, string newDisplayName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı bulunamadı." });

            user.DisplayName = newDisplayName;
            return await _userManager.UpdateAsync(user);
        }


        public async Task<IdentityResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı bulunamadı." });

            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IdentityResult> DeleteAccountAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user?.Identity is null || !user.Identity.IsAuthenticated)
                return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı oturumu bulunamadı." });

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı ID'si alınamadı." });

            var appUser = await _userManager.FindByIdAsync(userId);

            if (appUser == null)
                return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı bulunamadı." });

            return await _userManager.DeleteAsync(appUser);
        }
    }
}

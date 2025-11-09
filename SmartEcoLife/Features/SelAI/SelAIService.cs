using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.SemanticKernel;
using SmartEcoLife.Data;
using SmartEcoLife.Features.Categories;
using SmartEcoLife.Shared.Dtos.FinancialRecordDtos;
using SmartEcoLife.Shared.Dtos.SelAIDtos;
using System.Security.Claims;

namespace SmartEcoLife.Features.SelAI
{
    public class SelAIService
    {
        private readonly SmartEcoLifeDbContext _context;
        private readonly Kernel _recommendationKernel;
        private readonly Kernel _chatKernel;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SelAIService> _logger;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly CategoryService _categoryService;

        public SelAIService(
            SmartEcoLifeDbContext context,
            Kernel recommendationKernel,
            [FromKeyedServices("ChatKernel")] Kernel chatKernel,
            IMemoryCache cache,
            ILogger<SelAIService> logger,
            AuthenticationStateProvider authenticationStateProvider,
            CategoryService categoryService)
        {
            _context = context;
            _recommendationKernel = recommendationKernel;
            _chatKernel = chatKernel;
            _cache = cache;
            _logger = logger;
            _authenticationStateProvider = authenticationStateProvider;
            _categoryService = categoryService;
        }


        private async Task<Guid?> GetCurrentUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
                return userId;

            return null;
        }


        public async Task<string?> GenerateRecommendationAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return "Kullanıcı oturumu bulunamadı.";

            var cacheKey = $"ai_recommendation_{userId}";
            if (_cache.TryGetValue(cacheKey, out string? cached))
                return cached;

            var records = await _context.FinancialRecords
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .Take(10)
                .ToListAsync();

            if (!records.Any())
                return "Henüz yeterli veri yok. Birkaç işlem ekledikten sonra seni analiz edebilirim.";

            var goal = await _context.Goals
                  .Where(r => r.UserId == userId)
                  .OrderByDescending(r => r.CreatedAt)
                  .Take(10)
                  .ToListAsync();

            var goalDataSummary = string.Join("\n", goal.Select(r =>
    $"{r.Title} ({r.Description}): {r.TargetAmount} TL - {r.Achieved} Durum - {r.CreatedAt:dd.MM.yyyy} Oluşturulma tarihi - {r.DueDate:dd.MM.yyyy} Son tarih - {DateTime.UtcNow} Bugünün tarihi"
));

            var financialRecordDataSummary = string.Join("\n", records.Select(r =>
                 $"{r.Title} ({r.Type}): {r.Amount} TL - {r.Description} - {r.Date:dd.MM.yyyy}"
             ));

            var prompt = $"""
Kullanıcının finansal hedefi: {goalDataSummary}

Son finansal kayıtları:
{financialRecordDataSummary}

Kullanıcının hedefi doğrultusunda 1-2 cümlelik motive edici bir finansal tavsiye ver.
Kısa, samimi ve Türkçe yaz.
""";

            try
            {
                var result = await _recommendationKernel.InvokePromptAsync(prompt);
                var text = result.GetValue<string>()?.Trim();

                if (!string.IsNullOrEmpty(text))
                {
                    _cache.Set(cacheKey, text, TimeSpan.FromHours(1));
                    return text;
                }

                return "Şu anda öneri oluşturamıyorum, lütfen daha sonra tekrar dene.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AI önerisi oluşturulamadı.");
                return "AI tavsiyesi oluşturulurken bir hata oluştu.";
            }
        }

        public async Task<string> ChatAsync(string userMessage)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return "Kullanıcı oturumu bulunamadı.";

            if (string.IsNullOrWhiteSpace(userMessage))
                return "Mesaj boş olamaz.";

            var cacheKey = $"ai_chat_history_{userId}";

          
            if (!_cache.TryGetValue(cacheKey, out List<(string Role, string Message)> chatHistory))
                chatHistory = new List<(string Role, string Message)>();

           
            chatHistory.Add(("User", userMessage));

            var records = await _context.FinancialRecords
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .Take(10)
                .ToListAsync();

            var goal = await _context.Goals
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Take(10)
                .ToListAsync();

            if (!records.Any())
                return "Henüz yeterli veri yok. Birkaç işlem ekledikten sonra seni analiz edebilirim.";

            var goalDataSummary = string.Join("\n", goal.Select(r =>
                $"{r.Title} ({r.Description}): {r.TargetAmount} TL - {r.Achieved} Durum - {r.CreatedAt:dd.MM.yyyy} Tamamlanma tarihi - {r.DueDate:dd.MM.yyyy} Son tarih"
            ));

            var financialRecordDataSummary = string.Join("\n", records.Select(r =>
                $"{r.Title} ({r.Type}): {r.Amount} TL - {r.Description} - {r.Date:dd.MM.yyyy}"
            ));

            // 🔹 4️⃣ Sohbet geçmişini dahil eden prompt hazırla
            var conversationContext = string.Join("\n", chatHistory.Select(h => $"{h.Role}: {h.Message}"));

            var prompt = $"""
Sen SmartEcoLife isimli bir finansal danışman yapay zekasısın.
Aşağıda kullanıcının verileri ve hedefi bulunuyor:

🧭 Hedefi: {goalDataSummary}

💰 Son finansal kayıtları:
{financialRecordDataSummary}

Kullanıcıyla yaptığın geçmiş konuşmalar:
{conversationContext}

Yeni kullanıcı mesajı:
{userMessage}

Cevaplarını kısa, anlaşılır ve motive edici tut.
""";

            try
            {
                var result = await _chatKernel.InvokePromptAsync(prompt);
                var reply = result.GetValue<string>()?.Trim() ?? "Şu anda cevap veremiyorum, lütfen tekrar dene.";

       
                chatHistory.Add(("AI", reply));

         
                _cache.Set(cacheKey, chatHistory, TimeSpan.FromDays(1));

                return reply;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Chat AI hatası oluştu.");
                return "AI sohbetinde bir hata oluştu.";
            }
        }


        public async Task<List<SelAIChatMessageDto>> GetCachedChatHistoryAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return new List<SelAIChatMessageDto>();

            var cacheKey = $"ai_chat_history_{userId}";
            if (_cache.TryGetValue(cacheKey, out List<(string Role, string Message)> chatHistory))
            {
                return chatHistory
                    .Select(h => new SelAIChatMessageDto
                    {
                        Text = h.Message,
                        IsUser = h.Role == "User"
                    })
                    .ToList();
            }

            return new List<SelAIChatMessageDto>();
        }
    }
}


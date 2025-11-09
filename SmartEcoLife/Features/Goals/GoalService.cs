using AutoMapper;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using SmartEcoLife.Data;
using SmartEcoLife.Features.Categories;
using SmartEcoLife.Features.Goals;
using SmartEcoLife.Shared.Dtos.GoalDtos;
using System.Security.Claims;

namespace SmartEcoLife.Features.Goals
{
    public class GoalService
    {
        private readonly SmartEcoLifeDbContext _context;
        private readonly IMapper _mapper;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public GoalService(
            SmartEcoLifeDbContext context,
            IMapper mapper,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _context = context;
            _mapper = mapper;
            _authenticationStateProvider = authenticationStateProvider;
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

        public async Task<List<GoalDto>> GetAllAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return new List<GoalDto>();

            var goals = await _context.Goals
                .Where(g => g.UserId == userId)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<GoalDto>>(goals);
        }

        public async Task<GoalDto?> GetByIdAsync(Guid id)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return null;

            var goal = await _context.Goals
                .FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

            return _mapper.Map<GoalDto?>(goal);
        }

        public async Task AddAsync(GoalDto dto)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return;

            var entity = _mapper.Map<Goal>(dto);
            entity.UserId = userId.Value;
            entity.CreatedAt = DateTimeOffset.UtcNow;
            if (dto.DueDate.HasValue)
                entity.DueDate = new DateTimeOffset(dto.DueDate.Value.DateTime, TimeSpan.Zero);
            //entity.Achieved = false;
            _context.Goals.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GoalDto dto)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return;

            var existing = await _context.Goals
                .FirstOrDefaultAsync(g => g.Id == dto.Id && g.UserId == userId);

            if (existing is null)
                throw new InvalidOperationException("Hedef bulunamadı veya kullanıcıya ait değil.");

            if (dto.DueDate.HasValue)
                dto.DueDate = new DateTimeOffset(dto.DueDate.Value.DateTime, TimeSpan.Zero);
            _mapper.Map(dto, existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return;

            var goal = await _context.Goals
                .FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

            if (goal is not null)
            {
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsAchievedAsync(Guid id, bool value)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return;

            var goal = await _context.Goals
                .FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

            if (goal is not null)
            {
                goal.Achieved = value;
                await _context.SaveChangesAsync();
            }
        }
    }
}

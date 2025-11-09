using AutoMapper;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using SmartEcoLife.Data;
using SmartEcoLife.Features.Categories;
using SmartEcoLife.Shared.Dtos.FinancialRecordDtos;
using System.Security.Claims;

namespace SmartEcoLife.Features.FinancialRecords
{
    public class FinancialRecordService
    {
        private readonly SmartEcoLifeDbContext _context;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public FinancialRecordService(
            SmartEcoLifeDbContext context,
            IMapper mapper,
            CategoryService categoryService,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _context = context;
            _mapper = mapper;
            _categoryService = categoryService;
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

        public async Task<List<FinancialRecordDto>> GetAllAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return new List<FinancialRecordDto>();

            var records = await _context.FinancialRecords
                .Include(r => r.Category)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            return _mapper.Map<List<FinancialRecordDto>>(records);
        }

        public async Task<FinancialRecordDto?> GetByIdAsync(Guid id)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return null;

            var record = await _context.FinancialRecords
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            return _mapper.Map<FinancialRecordDto?>(record);
        }

        public async Task AddAsync(FinancialRecordDto dto)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return;

            var entity = _mapper.Map<FinancialRecord>(dto);
            entity.UserId = userId.Value;

            if (dto.CategoryId.HasValue)
            {
                entity.Category = await _context.Categories.FindAsync(dto.CategoryId.Value);
                entity.CategoryId = dto.CategoryId.Value;
                dto.CategoryName = entity.Category?.Name;
            }

            _context.FinancialRecords.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FinancialRecordDto dto)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return;

            var existing = await _context.FinancialRecords
                .FirstOrDefaultAsync(r => r.Id == dto.Id && r.UserId == userId);

            if (existing is null)
                throw new InvalidOperationException("Kayıt bulunamadı veya kullanıcıya ait değil.");

            _mapper.Map(dto, existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return;

            var record = await _context.FinancialRecords
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (record is not null)
            {
                _context.FinancialRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(decimal income, decimal expense)> GetTotalsAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId is null)
                return (0, 0);

            var records = await _context.FinancialRecords
                .Where(r => r.UserId == userId)
                .ToListAsync();

            var income = records.Where(r => r.Type == RecordType.Income).Sum(r => r.Amount);
            var expense = records.Where(r => r.Type == RecordType.Expense).Sum(r => r.Amount);

            return (income, expense);
        }
    }
}


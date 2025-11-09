using Microsoft.EntityFrameworkCore;
using SmartEcoLife.Data;
using SmartEcoLife.Features.Dashboards;
using System;

namespace SmartEcoLife.Features.Categories
{
    public class CategoryService
    {
        private readonly SmartEcoLifeDbContext _context;

        public CategoryService(SmartEcoLifeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<string?> GetCategoryNameByIdAsync(Guid? categoryId)
        {
            if (categoryId == null)
                return null;

            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            return category?.Name;
        }
    }
}

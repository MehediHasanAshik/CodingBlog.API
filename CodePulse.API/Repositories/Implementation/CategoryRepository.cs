using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return category;
        }


        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
           return await context.Categories.FirstOrDefaultAsync(x => x.Id == id);  
        }

        public async Task<Category?> UpdateCategoryAsync(Category category)
        {
            var exCategory = await context.Categories.FindAsync(category.Id);

            if(exCategory != null)
            {
                context.Entry(exCategory).CurrentValues.SetValues(category);

                /*exCategory.Id = category.Id;
                exCategory.Name = category.Name;
                exCategory.UrlHandle = category.UrlHandle;*/

                await context.SaveChangesAsync();
                return category;
            }

            return null;
        }

        public async Task<Category?> DeleteCategoryAsync(Guid id)
        {
            var category = await context.Categories.FindAsync(id);

            if(category != null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return category;
            }
            return null;
        }
    }
}

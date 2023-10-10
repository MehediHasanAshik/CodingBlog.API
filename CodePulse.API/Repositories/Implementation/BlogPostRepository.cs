using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var blogPost = await dbContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return null;
            }
            dbContext.BlogPosts.Remove(blogPost);
            await dbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAsync()
        {
           return await dbContext.BlogPosts.Include(x=> x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await dbContext.BlogPosts.Include(x=> x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlAsync(string urlHandle)
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existedBlogPost = await dbContext.BlogPosts.Include(x => x.Categories)
                  .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if(existedBlogPost == null)
            {
                return null;
            }

            //update BlogPost
            dbContext.Entry(existedBlogPost).CurrentValues.SetValues(blogPost);

            //Update Categories
            existedBlogPost.Categories = blogPost.Categories;
            await dbContext.SaveChangesAsync();

            return blogPost;
        }
    }
}

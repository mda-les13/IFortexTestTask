using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;

        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Author> GetAuthor()
        {
            var authorWithLongestBookTitle = await _context.Books
                .IgnoreAutoIncludes()
                .Include(a => a.Author)
                .OrderByDescending(x => x.Title.Length)
                .ThenBy(x => x.Author.Id)
                .Select(x => x.Author)
                .FirstOrDefaultAsync();

            return authorWithLongestBookTitle;
        }

        public async Task<List<Author>> GetAuthors()
        {
            var authorsWithEvenBooksAfter2015 = await _context.Books
                .Where(b => b.PublishDate.Year > 2015)
                .GroupBy(b => b.Author)
                .Where(b => b.Count() > 0 && b.Count() % 2 == 0)
                .Select(b => b.Key)
                .ToListAsync();

            return authorsWithEvenBooksAfter2015;
        }
    }
}

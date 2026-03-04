using AnagramSolver.Contracts;
using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class DbWordRepository : IWordRepository
    {
        private AnagramDbContext _context;
        public DbWordRepository(AnagramDbContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<string>> GetDictionary(CancellationToken ct)
        {
            var dictionary = await _context.Words.Select(w => w.Value).ToListAsync(ct);
            return dictionary;
        }

        public async Task SaveDictionary(IEnumerable<string> words, CancellationToken ct)
        {
            var existing = await _context.Words.ToListAsync(ct);

            _context.Words.RemoveRange(existing);
            await _context.SaveChangesAsync();

            var newEntries = words.Select(w => new Word
            {
                Value = w.ToLower(),
                CreatedAt = DateTime.Now,
            });

            await _context.Words.AddRangeAsync(newEntries, ct);
            await _context.SaveChangesAsync();

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.BusinessLogic
{
    public class EFcoreQuereis
    {
        private AnagramDbContext _context;
        private List<string> categories = new List<string> { "Daiktavardis", "Veiksmažodis", "Būdvardis" };
        private List<string> daikt = new List<string> {"alus", "sula", "vanduo", "katinas", "medis", "programavimas", "saulė"};
        private List<string> veiks = new List<string> { "bėgti", "mąstyti", "koduoti" };
        private List<string> budv = new List<string> { "gražus", "greitas", "stiprus" };
        public EFcoreQuereis(AnagramDbContext context) 
        { 
            _context = context;
        }

        public void AddToCategories()
        {
            foreach (var category in categories)
            {
                var c = new Category { Name = category };
                _context.Categories.Add(c);
            }
            _context.SaveChanges();
        }

        public void AddToWords()
        {
            var daiktCat = _context.Categories.First(c => c.Name == "Daiktavardis");
            var veiksCat = _context.Categories.First(c => c.Name == "Veiksmažodis");
            var budvCat = _context.Categories.First(c => c.Name == "Būdvardis");

            foreach (var word in daikt)
            {
                var w = new Word { Value = word, CategoryId = daiktCat.Id, CreatedAt = DateTime.Now};
                _context.Words.Add(w);
            }
            foreach (var word in veiks)
            {
                var w = new Word { Value = word, CategoryId = veiksCat.Id, CreatedAt = DateTime.Now };
                _context.Words.Add(w);
            }
            foreach(var word in budv)
            {
                var w = new Word { Value = word, CategoryId = budvCat.Id, CreatedAt = DateTime.Now };
                _context.Words.Add(w);
            };
            _context.SaveChanges();
        }

        public void AddNewWord(string word)
        {
            var newWord = new Word { Value = word };
            _context.Words.Add(newWord);
            _context.SaveChanges();
        }

        public void WordsWithCategories()
        {
            var words = _context.Words.Include(w => w.Category).ToList();
            foreach (var word in words)
            {
                Console.WriteLine($"word: {word.Value}, category: {word.Category?.Name}");
            }
        }

        public void FilterByCategory(string category)
        {
            var words = _context.Words.Include(w => w.Category).Where(w => w.Category!= null && w.Category.Name == category);
            foreach (var word in words)
            {
                Console.WriteLine(word.Value);
            }
        }

        public void CountByCategory()
        {
            var counts = _context.Words.GroupBy(w => w.Category.Name).ToList();
            var cleanCounts = counts.Select(n => new 
            {category = n.Key ?? "No Category", categoryCount = n.Count()});

            foreach (var category in cleanCounts)
            {
                Console.WriteLine($"{category.category} category has {category.categoryCount} words.");
            }
        }

        public void NoCategory()
        {
            var noCat = _context.Words.Where(w => w.Category == null).ToList();
            Console.WriteLine("Words with no category: ");
            foreach(var w in noCat)
            {
                Console.WriteLine(w.Value);
            }
        }
    }
}

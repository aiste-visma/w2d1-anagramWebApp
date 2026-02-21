using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.EF.CodeFirst.Models
{
    public class Word
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Category? Category { get; set; }
    }
}

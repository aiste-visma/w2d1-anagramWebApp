using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.EF.CodeFirst.Models
{
    public class SearchLog
    {
        public int Id { get; set; }
        public string SearchText {  get; set; } = string.Empty;
        public DateTime SearchedAt { get; set; } = DateTime.Now;
    }
}

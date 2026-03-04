using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class WordsDatabaseInitializer
    {
        private readonly IWordRepository _wordRepository;

        public WordsDatabaseInitializer(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        /// <summary>
        /// Reads words from a file and inserts them into the database.
        /// This should be called once during application initialization.
        /// </summary>
        /// <param name="filePath">Path to the file containing words (one word per line)</param>
        /// <param name="ct">Cancellation token</param>
        public async Task InitializeDatabaseWithWordsAsync(string filePath, CancellationToken ct = default)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Words file not found at path: {filePath}");
                }

                // Read all words from file
                var words = await File.ReadAllLinesAsync(filePath, Encoding.UTF8, ct);

                // Filter out empty lines and trim whitespace
                var validWords = words
                    .Where(w => !string.IsNullOrWhiteSpace(w))
                    .Select(w => w.Trim())
                    .Distinct()
                    .ToList();

                if (validWords.Count == 0)
                {
                    Console.WriteLine("No valid words found in the file.");
                    return;
                }

                // Save to database
                await _wordRepository.SaveDictionary(validWords, ct);

                Console.WriteLine($"Successfully inserted {validWords.Count} words into the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database with words: {ex.Message}");
                throw;
            }
        }
    }
}

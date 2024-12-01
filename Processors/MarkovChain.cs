using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Concurrent;
using static System.Net.Mime.MediaTypeNames;
using System;

using System.Collections.Concurrent;

namespace AmazonReviews.Processors
{
    public class WordPair
    {
        public int Count { get; set; }
        public decimal Probability { get; set; }
    }

    public class MarkovChain
    {
        //use the GUID hashcode to improve the default seed
        private readonly Random rnd = new Random(Guid.NewGuid().GetHashCode());
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, int>> _chain = new();

        public async Task TrainAsync(IEnumerable<string> lines)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(lines, line =>
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

                        words.Insert(0, "<START>");
                        words.Add("<END>");

                        for (int i = 0; i < words.Count - 1; i++)
                        {
                            var currentWord = words[i];
                            var nextWord = words[i + 1];

                            _chain.AddOrUpdate(currentWord,
                                _ => new ConcurrentDictionary<string, int> { [nextWord] = 1 },
                                (_, existing) =>
                                {
                                    existing.AddOrUpdate(nextWord, 1, (_, count) => count + 1);
                                    return existing;
                                });
                        }
                    }
                });
            });
        }

        public async Task<string> GenerateAsync(int maxLength = 50)
        {
            return await Task.Run(() =>
            {
                var currentWord = "<START>";
                var result = new List<string>();

                for (int i = 0; i < maxLength; i++)
                {
                    if (!_chain.ContainsKey(currentWord) || !_chain[currentWord].Any())
                        break;

                    currentWord = GetNextWord(currentWord);

                    if (currentWord == "<END>")
                        break;

                    result.Add(currentWord);
                }

                return string.Join(" ", result);
            });
        }

        private string GetNextWord(string currentWord)
        {
            var options = _chain[currentWord];
            var totalWeight = options.Values.Sum();

            var randomValue = rnd.Next(totalWeight);
            foreach (var kvp in options)
            {
                randomValue -= kvp.Value;
                if (randomValue < 0)
                {
                    return kvp.Key;
                }
            }

            return "<END>";
        }
    }
}

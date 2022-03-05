using System.Collections.Concurrent;
using System.Diagnostics;

namespace TextFrequencyAnalysis
{
    public class Program
    {
        private const int TripletsLength = 3;
        private const int LimitOfTriplets = 10;

        static void Main(string[] args)
        {
            var filePath = Console.ReadLine();
            var text = File.ReadAllText(filePath);

            var stopWatch = Stopwatch.StartNew();

            var mostFrequentTriplets = GetMostFrequentTripletsUsingDictionary(text);
            Console.WriteLine(mostFrequentTriplets);

            stopWatch.Stop();
            Console.WriteLine("Total Number of Milliseconds: " + stopWatch.ElapsedMilliseconds);
        }

        private static string GetMostFrequentTripletsUsingDictionary(string text)
        {
            var tripletsFrequencies = new ConcurrentDictionary<string, int>();

            PopulateTripletsFromTextUsingTasks(tripletsFrequencies, text);

            return string.Join(',', MostFrequentTriplets(tripletsFrequencies));
        }

        private static IEnumerable<string> MostFrequentTriplets(ConcurrentDictionary<string, int> tripletsFrequencies)
        {
            return tripletsFrequencies
                .OrderByDescending(x => x.Value)
                .Take(LimitOfTriplets)
                .Select(x => x.Key);
        }

        private static void PopulateTripletsFromTextUsingTasks(ConcurrentDictionary<string, int> triplets, string text)
        {
            Parallel.For(0, text.Length - TripletsLength + 1, i =>
            {
                if (char.IsLetter(text[i]) && char.IsLetter(text[i + 1]) && char.IsLetter(text[i + 2]))
                {
                    var triplet = new string(new[] { text[i], text[i + 1], text[i + 2] });
                    AddOrUpdateTripletsFrequencies(triplets, triplet);
                }
            });
        }

        private static void AddOrUpdateTripletsFrequencies(ConcurrentDictionary<string, int> triplets, string triplet)
        {
            triplets.AddOrUpdate(
                triplet,
                triplet => 1,
                (triplet, counter) => counter + 1
            );
        }
    }
}
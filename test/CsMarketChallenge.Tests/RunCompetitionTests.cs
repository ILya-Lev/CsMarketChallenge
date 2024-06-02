using MathNet.Numerics.Statistics;
using Xunit.Abstractions;

namespace CsMarketChallenge.Tests;

public class RunCompetitionTests(ITestOutputHelper output)
{
    private readonly Competitor[] competitors = new IPlayer[]
        {
            new RandomPlayer(),
            new OkoZaOko(),
            new ForgivingOkoZaOko(2),
            new ForgivingOkoZaOko(3),
            new ForgivingOkoZaOko(4),
            new ForgivingOkoZaOko(5),
            new ForgivingOkoZaOko(6),
            new Opposite(),
            new RandomOkoZaOko(),
            new ConstantTrue(),
            new ConstantFalse(),
        }
        .OrderBy(x => Random.Shared.NextDouble())
        .Select(x => new Competitor(x))
        .ToArray();

    [Fact]
    public void RunOnce_All_Observe()
    {
        var result = Program.RunCompetitions(competitors);

        foreach (var competitor in result)
            output.WriteLine($"{competitor.Score} {competitor.Player.Title}");
    }

    [Fact]
    public void Run1000Times_All_FindTheBest()
    {
        var runs = 1_000;
        var results = new Dictionary<string, List<int>>();
        for (int i = 0; i < runs; i++)
        {
            var r = Program.RunCompetitions(competitors);
            foreach (var c in r)
            {
                results.TryAdd(c.Player.Title, new List<int>());
                results[c.Player.Title].Add(c.Score);
            }

            foreach (var c in competitors)
                c.Score = 0;
        }

        var sorted = results.Select(p => (TotalScore: p.Value.Sum(s => (long)s), Pair: p))
            .OrderByDescending(p => p.TotalScore);

        foreach (var (totalScore, pair)  in sorted)
        {
            var (mean, stdDev) = pair.Value.Select(s => s*1.0).MeanStandardDeviation();
            var min = pair.Value.Min();
            var max = pair.Value.Max();
            output.WriteLine($"{pair.Key} avg {mean:N0} std dev {stdDev:N0} min {min} max {max}");
        }
    }

}
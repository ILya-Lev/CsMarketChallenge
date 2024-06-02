namespace CsMarketChallenge;

public static class Program
{
    public static void Main()
    {
        var competitors = new IPlayer[]
            {
                // Here will be all the strategies of all participants
                new RandomPlayer(),
                new OkoZaOko(),
                new ForgivingOkoZaOko(2),
                new ForgivingOkoZaOko(3),
                new ForgivingOkoZaOko(4),
                new Opposite(),
                new RandomOkoZaOko(),
                new ConstantTrue(),
                new ConstantFalse(),
            }
            .OrderBy(x => Random.Shared.NextDouble())
            .Select(x => new Competitor(x))
            .ToArray();

        var orderedCompetitors = RunCompetitions(competitors);

        foreach (var competitor in orderedCompetitors)
        {
            Console.WriteLine($"{competitor.Score} - {competitor.Player.Title} - {competitor.Player.Author}");
        }
    }

    internal static Competitor[] RunCompetitions(Competitor[] competitors)
    {
        for (var i = 0; i < competitors.Length; i++)
        {
            for (var j = i; j < competitors.Length; j++)
            {
                var game = new GameLoop(competitors[i].Player, competitors[j].Player);

                while (game.Step()) { }

                competitors[i].Score += game.score1;
                competitors[j].Score += game.score2;
            }
        }

        return competitors.OrderByDescending(x => x.Score).ToArray();
    }
}

internal class Competitor(IPlayer player)
{
    public readonly IPlayer Player = player;
    public int Score;
}

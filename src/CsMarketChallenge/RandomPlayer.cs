using System.Security.Cryptography;

namespace CsMarketChallenge;

public class RandomPlayer : IPlayer
{
    public override string Author => "John Smith";

    public override string Title => "Random Strategy";

    public override bool NextDecision(State state) => RandomNumberGenerator.GetInt32(0,100) < 50;
}

using System.Security.Cryptography;

namespace CsMarketChallenge.SelectedPlayers;

public abstract class BasePlayer : IPlayer
{
    public override string Author => "Illia Levandovskyi";
}

public class Opposite : BasePlayer
{
    public override string Title => "Opposite";

    public override bool NextDecision(State state)
    {
        if (state.OpponentDecisions.Count == 0) return true;

        return !state.OpponentDecisions.Last();
    }
}

public class RandomOkoZaOko : BasePlayer
{
    public override string Title => "Random Oko Za Oko";

    public override bool NextDecision(State state)
    {
        if (state.OpponentDecisions.Count == 0) return true;

        return state.OpponentDecisions.Last() && RandomNumberGenerator.GetInt32(0, 100) > 50;
    }
}

public class ConstantFalse : BasePlayer
{
    public override string Title => "Constant False";
    public override bool NextDecision(State state) => false;
}

public class Even : BasePlayer
{
    public override string Title => "Even";

    public override bool NextDecision(State state)
    {
        if (state.OpponentDecisions.Count == 0) return true;

        return state.OpponentDecisions.Sum(d => d ? 1 : 0) % 2 == 0;
    }
}

public class Odd : BasePlayer
{
    public override string Title => "Odd";

    public override bool NextDecision(State state)
    {
        if (state.OpponentDecisions.Count == 0) return true;

        return state.OpponentDecisions.Sum(d => d ? 1 : 0) % 2 == 1;
    }
}
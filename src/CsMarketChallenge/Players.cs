using System.Security.Cryptography;

namespace CsMarketChallenge;

public abstract class BasePlayer : IPlayer
{
    public override string Author => "Illia Levandovskyi";
}

public class OkoZaOko : BasePlayer
{
    public override string Title => "Oko Za Oko";

    public override bool NextDecision(State state)
    {
        if (state.OpponentDecisions.Count == 0) return true;

        return state.OpponentDecisions.Last();
    }
}

public class ForgivingOkoZaOko : BasePlayer
{
    private readonly int _n;

    public ForgivingOkoZaOko(int n) => _n = n;

    public override string Title => $"Forgiving {_n} Oko Za Oko";

    public override bool NextDecision(State state)
    {
        if (state.OpponentDecisions.Count == 0) return true;
        if (state.OpponentDecisions.Last()) return true;
        if (state.OpponentDecisions.Count < _n) return state.OpponentDecisions.Last();

        var lastOpponentDecisions = state.OpponentDecisions.TakeLast(_n).Select(v => v ? +1 : -1).Sum();

        return lastOpponentDecisions switch
        {
            > 0 => true,
            < 0 => false,
            0 => state.MyDecisions.Last()
        };
    }
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

public class ConstantTrue : BasePlayer
{
    public override string Title => "Constant True";
    public override bool NextDecision(State state) => true;
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

public class SerialPunisher : BasePlayer
{
    private readonly int _n;

    public SerialPunisher(int n) => _n = n;

    public override string Title => $"Serial {_n} Punisher";

    public override bool NextDecision(State state)
    {
        if (state.OpponentDecisions.Count == 0) return true;
        if (state.OpponentDecisions.Count < _n) return state.OpponentDecisions.Last();

        var isAnyOpponentsFalse = state.OpponentDecisions.TakeLast(_n).Any(d => d == false);
        if (isAnyOpponentsFalse)
            return state.MyDecisions.TakeLast(_n).Count(d => !d) < _n
                ? false
                : true;

        return true;
    }
}

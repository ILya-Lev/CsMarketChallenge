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

        return state.OpponentDecisions.Last() && Random.Shared.Next(0, 100) > 50;
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
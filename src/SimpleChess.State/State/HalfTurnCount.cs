using System;

namespace SimpleChess.State.State;

/// <summary>
/// Represents the halfmove clock used for the fifty-move rule.
/// Valid range: 0 to 150 (gameState is automatically drawn at 75 moves/150 half-moves under FIDE Laws of Chess 2014+).
/// </summary>
internal readonly record struct HalfTurnCount
{
    private const int MaxHalfMoves = 150;
    private readonly int _value;

    private HalfTurnCount(int value)
    {
        _value = value;
    }

    public static HalfTurnCount FromFen(FenGameState.FenSegment<FenGameState.HalfTurnCounterKind> halfTurnCounterFen)
    {
        if (!int.TryParse(halfTurnCounterFen, out int count) || !TryCreate(count, out HalfTurnCount result))
        {
            throw new InvalidOperationException("Value provided is not valid for a half turn counter. If you get here there's an error in the FenGameState validation logic.");
        }

        return result;
    }

    private static bool TryCreate(int value, out HalfTurnCount result)
    {
        if (value is < 0 or > MaxHalfMoves)
        {
            result = default;
            return false;
        }
        result = new(value);
        return true;
    }

    public static implicit operator int(HalfTurnCount count) => count._value;

    public static explicit operator HalfTurnCount(int clock)
    {
        return TryCreate(clock, out HalfTurnCount result) ? result : throw new InvalidCastException("Value provided is not valid");
    }
}

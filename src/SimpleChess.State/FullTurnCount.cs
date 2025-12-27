using System;

namespace SimpleChess.State;

/// <summary>
/// Represents the full move number in a chess gameState.
/// Valid range: 1 to 8840 (maximum full moves in a legal chess gameState under FIDE Laws of Chess 2014+).
/// See: https://wismuth.com/chess/longest-gameState.html
/// </summary>
public readonly record struct FullTurnCount
{
    private const int MaxFullMoves = 8840;
    private readonly int _value;

    private FullTurnCount(int value)
    {
        _value = value;
    }

    internal static FullTurnCount FromFen(FenGameState.FenSegment<FenGameState.FullTurnCounterKind> fullTurnCountFen)
    {
        if (!int.TryParse(fullTurnCountFen, out int count) || !TryCreate(count, out FullTurnCount result))
        {
            throw new InvalidOperationException("Value provided is not valid for a half turn counter. If you get here there's an error in the FenGameState validation logic.");
        }

        return result;
    }


    private static bool TryCreate(int value, out FullTurnCount result)
    {
        if (value is < 1 or > MaxFullMoves)
        {
            result = default;
            return false;
        }
        result = new(value);
        return true;
    }

    public static implicit operator int(FullTurnCount count) => count._value;

    public static explicit operator FullTurnCount(int clock)
    {
        return TryCreate(clock, out FullTurnCount result) ? result : throw new InvalidCastException("Value provided is not valid");
    }
}

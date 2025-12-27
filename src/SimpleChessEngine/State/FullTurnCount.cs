using System;

namespace SimpleChessEngine.State;

/// <summary>
/// Represents the full move number in a chess game.
/// Valid range: 1 to 8840 (maximum full moves in a legal chess game under FIDE Laws of Chess 2014+).
/// See: https://wismuth.com/chess/longest-game.html
/// </summary>
internal readonly record struct FullTurnCount
{
    private const int MaxFullMoves = 8840;
    private readonly int _value;

    private FullTurnCount(int value)
    {
        _value = value;
    }

    public static bool TryCreate(int value, out FullTurnCount result)
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
        if (!TryCreate(clock, out FullTurnCount result))
        {
            throw new InvalidCastException("Value provided is not valid");
        }

        return result;
    }
}

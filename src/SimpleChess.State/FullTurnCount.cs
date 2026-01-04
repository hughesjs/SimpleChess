using System;

namespace SimpleChess.State;

/// <summary>
/// Represents the full move number in a chess game.
/// Valid range: 1 to 8840 (maximum full moves in a legal chess game under FIDE Laws of Chess 2014+).
/// See: https://wismuth.com/chess/longest-game.html
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
            throw new InvalidOperationException("Value provided is not valid for a full turn counter. If you get here there's an error in the FenGameState validation logic.");
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

    /// <summary>
    /// Implicitly converts a <see cref="FullTurnCount"/> to an integer.
    /// </summary>
    /// <param name="count">The fullmove count to convert.</param>
    /// <returns>The integer value of the fullmove number.</returns>
    public static implicit operator int(FullTurnCount count) => count._value;

    /// <summary>
    /// Explicitly converts an integer to a <see cref="FullTurnCount"/>.
    /// </summary>
    /// <param name="clock">The integer value to convert (must be between 1 and 8840).</param>
    /// <returns>A fullmove count with the specified value.</returns>
    /// <exception cref="InvalidCastException">Thrown when the value is outside the valid range (1-8840).</exception>
    public static explicit operator FullTurnCount(int clock)
    {
        return TryCreate(clock, out FullTurnCount result) ? result : throw new InvalidCastException("Value provided is not valid");
    }

    /// <summary>
    /// Increments the halfmove clock by one.
    /// </summary>
    /// <returns>A new <see cref="HalfTurnCount"/> with the value incremented by one.</returns>
    /// <exception cref="InvalidOperationException">Thrown when incrementing would exceed the maximum value of 150.</exception>
    public FullTurnCount Increment()
    {
        int newValue = _value + 1;
        if (newValue > MaxFullMoves)
        {
            throw new InvalidOperationException($"Cannot increment beyond maximum half-move count of {MaxFullMoves}");
        }
        return new FullTurnCount(newValue);
    }
}

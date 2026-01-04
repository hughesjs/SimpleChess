using System;

namespace SimpleChess.State;

/// <summary>
/// Represents the halfmove clock used for the fifty-move rule.
/// Valid range: 0 to 150.
/// </summary>
public readonly record struct HalfTurnCount
{
    private const int MaxHalfMoves = 150;
    private readonly int _value;

    private HalfTurnCount(int value)
    {
        _value = value;
    }

    internal static HalfTurnCount FromFen(FenGameState.FenSegment<FenGameState.HalfTurnCounterKind> halfTurnCounterFen)
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

    /// <summary>
    /// Implicitly converts a <see cref="HalfTurnCount"/> to an integer.
    /// </summary>
    /// <param name="count">The halfmove count to convert.</param>
    /// <returns>The integer value of the halfmove counter.</returns>
    public static implicit operator int(HalfTurnCount count) => count._value;

    /// <summary>
    /// Explicitly converts an integer to a <see cref="HalfTurnCount"/>.
    /// </summary>
    /// <param name="clock">The integer value to convert (must be between 0 and 150).</param>
    /// <returns>A halfmove count with the specified value.</returns>
    /// <exception cref="InvalidCastException">Thrown when the value is outside the valid range (0-150).</exception>
    public static explicit operator HalfTurnCount(int clock)
    {
        return TryCreate(clock, out HalfTurnCount result) ? result : throw new InvalidCastException("Value provided is not valid");
    }

    /// <summary>
    /// Increments the halfmove clock by one.
    /// </summary>
    /// <returns>A new <see cref="HalfTurnCount"/> with the value incremented by one.</returns>
    /// <exception cref="InvalidOperationException">Thrown when incrementing would exceed the maximum value of 150.</exception>
    public HalfTurnCount Increment()
    {
        int newValue = _value + 1;
        if (newValue > MaxHalfMoves)
        {
            throw new InvalidOperationException($"Cannot increment beyond maximum half-move count of {MaxHalfMoves}");
        }
        return new HalfTurnCount(newValue);
    }
}

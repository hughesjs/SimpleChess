namespace SimpleChess.Engine;

/// <summary>
/// Defines the vector a piece should move in the reference frame of the player
/// </summary>
public readonly struct MoveVector
{
    public required int Ranks { get; init; }
    public required int Files { get; init; }

    public static MoveVector operator *(MoveVector vector, int multiplier)
    {
        return new(){Ranks = vector.Ranks * multiplier, Files = vector.Files * multiplier};
    }
}

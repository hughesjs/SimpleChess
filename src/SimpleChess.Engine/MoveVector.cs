namespace SimpleChess.Engine;

/// <summary>
/// Defines the vector a piece should move in the reference frame of the player
/// </summary>
public readonly ref struct MoveVector
{
    public required int Ranks { get; init; }
    public required int Files { get; init; }
}

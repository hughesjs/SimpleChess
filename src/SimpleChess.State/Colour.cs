namespace SimpleChess.State;

/// <summary>
/// Represents the colour of a chess piece or the active player.
/// </summary>
/// <remarks>
/// This enum serves dual purposes: identifying piece colours (White or Black pieces)
/// and indicating which player has the next turn. The None value represents an empty square
/// or an unspecified colour.
/// </remarks>
public enum Colour
{
    None,
    Black,
    White
}

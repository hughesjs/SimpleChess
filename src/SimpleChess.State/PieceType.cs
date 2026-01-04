namespace SimpleChess.State;

/// <summary>
/// Represents the type of a chess piece.
/// </summary>
/// <remarks>
/// This enum identifies the six standard chess piece types: Pawn, Rook, Bishop, Knight, Queen, and King.
/// The None value represents an empty square or an unspecified piece type.
/// </remarks>
public enum PieceType: byte
{
    None,
    Pawn,
    Rook,
    Bishop,
    Knight,
    Queen,
    King
}

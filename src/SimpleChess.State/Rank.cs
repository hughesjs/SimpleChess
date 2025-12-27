namespace SimpleChess.State;

/// <summary>
/// Represents the ranks (rows) on a chess board, labelled 1 through 8.
/// </summary>
/// <remarks>
/// Ranks represent the Y-axis of the board. Rank.One (value 0) corresponds to White's back row,
/// and Rank.Eight (value 7) corresponds to Black's back row. This uses zero-based indexing internally
/// for efficient array access, so Rank.One = 0, Rank.Two = 1, and so on.
/// </remarks>
public enum Rank
{
    One = 0, // This is not a mistake. It converts from chess' one-based index to our zero based index
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight
}

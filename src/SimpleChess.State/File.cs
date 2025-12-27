namespace SimpleChess.State;

/// <summary>
/// Represents the files (columns) on a chess board, labelled A through H.
/// </summary>
/// <remarks>
/// Files represent the X-axis of the board, running from left to right from White's perspective.
/// File A (value 0) is the leftmost column, and File H (value 7) is the rightmost column.
/// This uses zero-based indexing internally for efficient array access.
/// </remarks>
public enum File
{
    A = 0,
    B,
    C,
    D,
    E,
    F,
    G,
    H
}

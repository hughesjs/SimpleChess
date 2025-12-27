using System;
using System.Text;

namespace SimpleChess.State;

/// <summary>
/// Represents a coordinate on the chess board using file (column) and rank (row).
/// </summary>
/// <remarks>
/// Squares are identified using algebraic notation, where files are labelled A-H (left to right from White's perspective)
/// and ranks are labelled 1-8 (White's back row to Black's back row). For example, "e4" represents the square
/// at file E, rank 4.
/// </remarks>
public readonly struct Square
{
    /// <summary>
    /// Gets the file (column) of this square.
    /// </summary>
    /// <remarks>
    /// Files range from A (leftmost) to H (rightmost) from White's perspective.
    /// </remarks>
    public File File { get; }

    /// <summary>
    /// Gets the rank (row) of this square.
    /// </summary>
    /// <remarks>
    /// Ranks range from 1 (White's back row) to 8 (Black's back row).
    /// </remarks>
    public Rank Rank { get; }

    private Square(File file, Rank rank)
    {
        File = file;
        Rank = rank;
    }

    internal static Square? FromFen(FenGameState.FenSegment<FenGameState.EnPassantStateKind> enPassantState)
    {
        if (enPassantState.Length == 1 && enPassantState[0] == '-')
        {
            return null;
        }

        // Theoretically, these upper case checks aren't needed
        File file = enPassantState[0] switch
        {
            'a' or 'A' => File.A,
            'b' or 'B' => File.B,
            'c' or 'C' => File.C,
            'd' or 'D' => File.D,
            'e' or 'E' => File.E,
            'f' or 'F' => File.F,
            'g' or 'G' => File.G,
            'h' or 'H' => File.H,
            _ => throw new ArgumentException($"Invalid file: {enPassantState[0]}")
        };

        Rank rank = enPassantState[1] switch
        {
            '1' => Rank.One,
            '2' => Rank.Two,
            '3' => Rank.Three,
            '4' => Rank.Four,
            '5' => Rank.Five,
            '6' => Rank.Six,
            '7' => Rank.Seven,
            '8' => Rank.Eight,
            _ => throw new ArgumentException($"Invalid rank: {enPassantState[1]}")
        };

        return new(file, rank);
    }

    internal static void ToFen(Square? square, StringBuilder builder)
    {
        if (square is null)
        {
            builder.Append('-');
            return;
        }

        char fileChar = square.Value.File switch
        {
            File.A => 'a',
            File.B => 'b',
            File.C => 'c',
            File.D => 'd',
            File.E => 'e',
            File.F => 'f',
            File.G => 'g',
            File.H => 'h',
            _ => throw new ArgumentException($"Invalid file: {square.Value.File}")
        };

        char rankChar = square.Value.Rank switch
        {
            Rank.One => '1',
            Rank.Two => '2',
            Rank.Three => '3',
            Rank.Four => '4',
            Rank.Five => '5',
            Rank.Six => '6',
            Rank.Seven => '7',
            Rank.Eight => '8',
            _ => throw new ArgumentException($"Invalid rank: {square.Value.Rank}")
        };

        builder.Append(fileChar);
        builder.Append(rankChar);
    }
}

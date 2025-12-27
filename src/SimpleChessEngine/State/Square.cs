using System;
using System.Text;

namespace SimpleChessEngine.State;

internal readonly struct Square
{
    public readonly File File { get; }
    public readonly Rank Rank { get; }

    private Square(File file, Rank rank)
    {
        File = file;
        Rank = rank;
    }

    public static Square? FromFen(FenGameState.FenSegment<FenGameState.EnPassantStateKind> enPassantState)
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

    public static void ToFen(Square? square, StringBuilder builder)
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

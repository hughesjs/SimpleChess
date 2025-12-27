using System;

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
}

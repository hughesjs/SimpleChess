using System;

namespace SimpleChessEngine.State;

internal readonly struct Square
{
    public readonly Rank Rank { get; }
    public readonly File File { get; }

    private Square(Rank rank, File file)
    {
        Rank = rank;
        File = file;
    }

    public static Square? FromFen(FenGameState.FenSegment<FenGameState.EnPassantStateKind> enPassantState)
    {
        if (enPassantState.Length == 1 && enPassantState[0] == '-')
        {
            return null;
        }

        // Theoretically, these upper case checks aren't needed
        Rank rank = enPassantState[0] switch
        {
            'a' or 'A' => Rank.A,
            'b' or 'B' => Rank.B,
            'c' or 'C' => Rank.C,
            'd' or 'D' => Rank.D,
            'e' or 'E' => Rank.E,
            'f' or 'F' => Rank.F,
            'g' or 'G' => Rank.G,
            'h' or 'H' => Rank.H,
            _ => throw new ArgumentException($"Invalid rank: {enPassantState[0]}")
        };

        File file = enPassantState[1] switch
        {
            '1' => File.One,
            '2' => File.Two,
            '3' => File.Three,
            '4' => File.Four,
            '5' => File.Five,
            '6' => File.Six,
            '7' => File.Seven,
            '8' => File.Eight,
            _ => throw new ArgumentException($"Invalid file: {enPassantState[1]}")
        };

        return new(rank, file);
    }
}

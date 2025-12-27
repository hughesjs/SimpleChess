using System;

namespace SimpleChess.State;

public record struct Piece(Colour Colour, PieceType PieceType)
{
    internal static Piece FromFenCode(char code)
    {
        return code switch
        {
            'r' => new(Colour.Black, PieceType.Rook),
            'n' => new(Colour.Black, PieceType.Knight),
            'b' => new(Colour.Black, PieceType.Bishop),
            'q' => new(Colour.Black, PieceType.Queen),
            'k' => new(Colour.Black, PieceType.King),
            'p' => new(Colour.Black, PieceType.Pawn),
            'R' => new(Colour.White, PieceType.Rook),
            'N' => new(Colour.White, PieceType.Knight),
            'B' => new(Colour.White, PieceType.Bishop),
            'Q' => new(Colour.White, PieceType.Queen),
            'K' => new(Colour.White, PieceType.King),
            'P' => new(Colour.White, PieceType.Pawn),
            _ => throw new ArgumentOutOfRangeException(nameof(code), code, null)
        };
    }

    internal static char ToFen(Piece piece)
    {
        return (piece.Colour, piece.PieceType) switch
        {
            (Colour.Black, PieceType.Rook) => 'r',
            (Colour.Black, PieceType.Knight) => 'n',
            (Colour.Black, PieceType.Bishop) => 'b',
            (Colour.Black, PieceType.Queen) => 'q',
            (Colour.Black, PieceType.King) => 'k',
            (Colour.Black, PieceType.Pawn) => 'p',
            (Colour.White, PieceType.Rook) => 'R',
            (Colour.White, PieceType.Knight) => 'N',
            (Colour.White, PieceType.Bishop) => 'B',
            (Colour.White, PieceType.Queen) => 'Q',
            (Colour.White, PieceType.King) => 'K',
            (Colour.White, PieceType.Pawn) => 'P',
            (Colour.None, PieceType.None) => throw new InvalidOperationException("Cannot convert empty square to FEN code. Empty squares are represented by numbers in FEN notation."),
            _ => throw new InvalidOperationException($"Invalid piece: Colour={piece.Colour}, PieceType={piece.PieceType}")
        };
    }
}

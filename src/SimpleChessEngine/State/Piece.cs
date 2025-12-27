using System;

namespace SimpleChessEngine.State;

internal record struct Piece(Colour Colour, PieceType PieceType)
{
    public static Piece FromFenCode(char code)
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
}

internal enum PieceType
{
    None,
    Pawn,
    Rook,
    Bishop,
    Knight,
    Queen,
    King
}

internal enum Colour
{
    None,
    Black,
    White
}

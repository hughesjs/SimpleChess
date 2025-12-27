using System;
using System.Threading.Tasks;

namespace SimpleChess.State.Tests.State;

public class PieceTests
{
    [Test]
    public async Task DefaultPieceIsEmpty()
    {
        Piece piece = default;
        Piece expected = new(Colour.None, PieceType.None);

        await Assert.That(piece).IsEqualTo(expected);
    }

    [Test]
    [Arguments(Colour.Black, PieceType.Rook, 'r')]
    [Arguments(Colour.Black, PieceType.Knight, 'n')]
    [Arguments(Colour.Black, PieceType.Bishop, 'b')]
    [Arguments(Colour.Black, PieceType.Queen, 'q')]
    [Arguments(Colour.Black, PieceType.King, 'k')]
    [Arguments(Colour.Black, PieceType.Pawn, 'p')]
    [Arguments(Colour.White, PieceType.Rook, 'R')]
    [Arguments(Colour.White, PieceType.Knight, 'N')]
    [Arguments(Colour.White, PieceType.Bishop, 'B')]
    [Arguments(Colour.White, PieceType.Queen, 'Q')]
    [Arguments(Colour.White, PieceType.King, 'K')]
    [Arguments(Colour.White, PieceType.Pawn, 'P')]
    internal async Task ToFenReturnsCorrectCharacter(Colour colour, PieceType pieceType, char expectedFen)
    {
        Piece piece = new(colour, pieceType);
        char result = Piece.ToFen(piece);

        await Assert.That(result).IsEqualTo(expectedFen);
    }

    [Test]
    public async Task ToFenThrowsForEmptyPiece()
    {
        Piece piece = new(Colour.None, PieceType.None);

        await Assert.That(() => Piece.ToFen(piece))
            .Throws<InvalidOperationException>()
            .WithMessage("Cannot convert empty square to FEN code. Empty squares are represented by numbers in FEN notation.");
    }

    [Test]
    public async Task FromFenCodeAndToFenAreInverses()
    {
        char[] fenCodes = { 'r', 'n', 'b', 'q', 'k', 'p', 'R', 'N', 'B', 'Q', 'K', 'P' };

        foreach (char fenCode in fenCodes)
        {
            Piece piece = Piece.FromFenCode(fenCode);
            char result = Piece.ToFen(piece);

            await Assert.That(result).IsEqualTo(fenCode);
        }
    }
}

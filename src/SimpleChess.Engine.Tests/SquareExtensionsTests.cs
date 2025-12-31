using System.Threading.Tasks;
using SimpleChess.State;

namespace SimpleChess.Engine.Tests;

public class SquareExtensionsTests
{
    [Test]
    // White pawn moves
    [Arguments(Colour.White, File.E, Rank.Two, 0, 1, File.E, Rank.Three)]
    [Arguments(Colour.White, File.E, Rank.Two, 0, 2, File.E, Rank.Four)]
    // White diagonal moves
    [Arguments(Colour.White, File.E, Rank.Four, 1, 1, File.F, Rank.Five)]
    [Arguments(Colour.White, File.E, Rank.Four, -1, 1, File.D, Rank.Five)]
    [Arguments(Colour.White, File.E, Rank.Four, 1, -1, File.F, Rank.Three)]
    [Arguments(Colour.White, File.E, Rank.Four, -1, -1, File.D, Rank.Three)]
    // White knight moves
    [Arguments(Colour.White, File.E, Rank.Four, 2, 1, File.G, Rank.Five)]
    [Arguments(Colour.White, File.E, Rank.Four, 2, -1, File.G, Rank.Three)]
    [Arguments(Colour.White, File.E, Rank.Four, -2, 1, File.C, Rank.Five)]
    [Arguments(Colour.White, File.E, Rank.Four, -2, -1, File.C, Rank.Three)]
    [Arguments(Colour.White, File.E, Rank.Four, 1, 2, File.F, Rank.Six)]
    [Arguments(Colour.White, File.E, Rank.Four, 1, -2, File.F, Rank.Two)]
    [Arguments(Colour.White, File.E, Rank.Four, -1, 2, File.D, Rank.Six)]
    [Arguments(Colour.White, File.E, Rank.Four, -1, -2, File.D, Rank.Two)]
    // Black pawn moves (reversed vectors)
    [Arguments(Colour.Black, File.E, Rank.Seven, 0, 1, File.E, Rank.Six)]
    [Arguments(Colour.Black, File.E, Rank.Seven, 0, 2, File.E, Rank.Five)]
    // Black diagonal moves (reversed vectors)
    [Arguments(Colour.Black, File.E, Rank.Five, 1, 1, File.D, Rank.Four)]
    [Arguments(Colour.Black, File.E, Rank.Five, -1, 1, File.F, Rank.Four)]
    [Arguments(Colour.Black, File.E, Rank.Five, 1, -1, File.D, Rank.Six)]
    [Arguments(Colour.Black, File.E, Rank.Five, -1, -1, File.F, Rank.Six)]
    // Black knight moves (reversed vectors)
    [Arguments(Colour.Black, File.E, Rank.Five, 2, 1, File.C, Rank.Four)]
    [Arguments(Colour.Black, File.E, Rank.Five, 1, 2, File.D, Rank.Three)]
    // Zero vector (no move)
    [Arguments(Colour.White, File.A, Rank.One, 0, 0, File.A, Rank.One)]
    [Arguments(Colour.Black, File.H, Rank.Eight, 0, 0, File.H, Rank.Eight)]
    // Corner moves
    [Arguments(Colour.White, File.A, Rank.One, 1, 1, File.B, Rank.Two)]
    [Arguments(Colour.Black, File.H, Rank.Eight, 1, 1, File.G, Rank.Seven)]
    // Large vectors
    [Arguments(Colour.White, File.E, Rank.One, 0, 7, File.E, Rank.Eight)]
    [Arguments(Colour.Black, File.E, Rank.Eight, 0, 7, File.E, Rank.One)]
    [Arguments(Colour.White, File.A, Rank.One, 7, 7, File.H, Rank.Eight)]
    [Arguments(Colour.Black, File.H, Rank.Eight, 7, 7, File.A, Rank.One)]
    // Lateral moves
    [Arguments(Colour.White, File.E, Rank.Four, 1, 0, File.F, Rank.Four)]
    [Arguments(Colour.White, File.E, Rank.Four, -1, 0, File.D, Rank.Four)]
    [Arguments(Colour.Black, File.E, Rank.Four, 1, 0, File.D, Rank.Four)]
    [Arguments(Colour.Black, File.E, Rank.Four, -1, 0, File.F, Rank.Four)]
    public async Task ValidMoveReturnsExpectedSquare(Colour colour, File startFile, Rank startRank, int fileOffset, int rankOffset, File expectedFile, Rank expectedRank)
    {
        Square source = Square.FromRankAndFile(startFile, startRank);
        MoveVector vector = new() { Files = fileOffset, Ranks = rankOffset };

        bool result = source.TryApplyMoveVector(colour, vector, out Square? targetSquare);

        using (Assert.Multiple())
        {
            await Assert.That(result).IsTrue();
            await Assert.That(targetSquare).IsNotNull();
            await Assert.That(targetSquare!.Value.File).IsEqualTo(expectedFile);
            await Assert.That(targetSquare!.Value.Rank).IsEqualTo(expectedRank);
        }
    }

    [Test]
    // White moving off top of board
    [Arguments(Colour.White, File.E, Rank.Eight, 0, 1)]
    [Arguments(Colour.White, File.E, Rank.Seven, 0, 2)]
    [Arguments(Colour.White, File.E, Rank.Eight, 1, 1)]
    // Black moving off bottom of board
    [Arguments(Colour.Black, File.E, Rank.One, 0, 1)]
    [Arguments(Colour.Black, File.E, Rank.Two, 0, 2)]
    [Arguments(Colour.Black, File.E, Rank.One, -1, 1)]
    // White moving off right side (H-file)
    [Arguments(Colour.White, File.H, Rank.Four, 1, 0)]
    [Arguments(Colour.White, File.H, Rank.Four, 1, 1)]
    [Arguments(Colour.White, File.G, Rank.Four, 2, 0)]
    [Arguments(Colour.White, File.H, Rank.Four, 2, 1)]
    // Black moving off left side (A-file reversed perspective)
    [Arguments(Colour.Black, File.H, Rank.Four, -1, 0)]
    [Arguments(Colour.Black, File.H, Rank.Four, -1, 1)]
    [Arguments(Colour.Black, File.G, Rank.Four, -2, 0)]
    // White moving off left side (A-file)
    [Arguments(Colour.White, File.A, Rank.Four, -1, 0)]
    [Arguments(Colour.White, File.A, Rank.Four, -1, 1)]
    [Arguments(Colour.White, File.B, Rank.Four, -2, 0)]
    [Arguments(Colour.White, File.A, Rank.Four, -2, 1)]
    // Black moving off right side (H-file reversed perspective)
    [Arguments(Colour.Black, File.A, Rank.Four, 1, 0)]
    [Arguments(Colour.Black, File.A, Rank.Four, 1, 1)]
    [Arguments(Colour.Black, File.B, Rank.Four, 2, 0)]
    // Corner cases going off board
    [Arguments(Colour.White, File.H, Rank.Eight, 1, 1)]
    [Arguments(Colour.Black, File.A, Rank.One, -1, 1)]
    [Arguments(Colour.White, File.A, Rank.One, -1, -1)]
    [Arguments(Colour.Black, File.H, Rank.Eight, 1, -1)]
    // Large vectors going off board
    [Arguments(Colour.White, File.E, Rank.One, 0, 8)]
    [Arguments(Colour.Black, File.E, Rank.Eight, 0, 8)]
    [Arguments(Colour.White, File.A, Rank.One, 8, 0)]
    [Arguments(Colour.Black, File.H, Rank.Eight, -8, 0)]
    // Knight moves off board
    [Arguments(Colour.White, File.A, Rank.One, -2, 1)]
    [Arguments(Colour.White, File.H, Rank.Eight, 2, 1)]
    [Arguments(Colour.Black, File.A, Rank.One, 2, 1)]
    [Arguments(Colour.Black, File.H, Rank.Eight, -2, 1)]
    public async Task InvalidMoveReturnsFalse(Colour colour, File startFile, Rank startRank, int fileOffset, int rankOffset)
    {
        Square source = Square.FromRankAndFile(startFile, startRank);
        MoveVector vector = new() { Files = fileOffset, Ranks = rankOffset };

        bool result = source.TryApplyMoveVector(colour, vector, out Square? targetSquare);

        using (Assert.Multiple())
        {
            await Assert.That(result).IsFalse();
            await Assert.That(targetSquare).IsNull();
        }
    }
}

using System.Threading.Tasks;
using SimpleChessEngine.State;

namespace SimpleChessEngine.Tests.State;

public class SquareTests
{
    [Test]
    public async Task DashReturnsNull()
    {
        FenGameState.FenSegment<FenGameState.EnPassantStateKind> segment = new("-");
        Square? result = Square.FromFen(segment);

        await Assert.That(result).IsNull();
    }

    [Test]
    [Arguments("a3", File.A, Rank.Three)]
    [Arguments("b3", File.B, Rank.Three)]
    [Arguments("c3", File.C, Rank.Three)]
    [Arguments("d3", File.D, Rank.Three)]
    [Arguments("e3", File.E, Rank.Three)]
    [Arguments("f3", File.F, Rank.Three)]
    [Arguments("g3", File.G, Rank.Three)]
    [Arguments("h3", File.H, Rank.Three)]
    [Arguments("a6", File.A, Rank.Six)]
    [Arguments("b6", File.B, Rank.Six)]
    [Arguments("c6", File.C, Rank.Six)]
    [Arguments("d6", File.D, Rank.Six)]
    [Arguments("e6", File.E, Rank.Six)]
    [Arguments("f6", File.F, Rank.Six)]
    [Arguments("g6", File.G, Rank.Six)]
    [Arguments("h6", File.H, Rank.Six)]
    public async Task ValidSquaresParsesCorrectly(string fenSquare, File expectedFile, Rank expectedRank)
    {
        FenGameState.FenSegment<FenGameState.EnPassantStateKind> segment = new(fenSquare);
        Square? result = Square.FromFen(segment);

        using (Assert.Multiple())
        {
            await Assert.That(result).IsNotNull();
            await Assert.That(result!.Value.File).IsEqualTo(expectedFile);
            await Assert.That(result!.Value.Rank).IsEqualTo(expectedRank);
        }
    }
}

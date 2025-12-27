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
    [Arguments("a3", Rank.A, File.Three)]
    [Arguments("b3", Rank.B, File.Three)]
    [Arguments("c3", Rank.C, File.Three)]
    [Arguments("d3", Rank.D, File.Three)]
    [Arguments("e3", Rank.E, File.Three)]
    [Arguments("f3", Rank.F, File.Three)]
    [Arguments("g3", Rank.G, File.Three)]
    [Arguments("h3", Rank.H, File.Three)]
    [Arguments("a6", Rank.A, File.Six)]
    [Arguments("b6", Rank.B, File.Six)]
    [Arguments("c6", Rank.C, File.Six)]
    [Arguments("d6", Rank.D, File.Six)]
    [Arguments("e6", Rank.E, File.Six)]
    [Arguments("f6", Rank.F, File.Six)]
    [Arguments("g6", Rank.G, File.Six)]
    [Arguments("h6", Rank.H, File.Six)]
    public async Task ValidSquaresParsesCorrectly(string fenSquare, Rank expectedRank, File expectedFile)
    {
        FenGameState.FenSegment<FenGameState.EnPassantStateKind> segment = new(fenSquare);
        Square? result = Square.FromFen(segment);

        using (Assert.Multiple())
        {
            await Assert.That(result).IsNotNull();
            await Assert.That(result!.Value.Rank).IsEqualTo(expectedRank);
            await Assert.That(result!.Value.File).IsEqualTo(expectedFile);
        }
    }
}

using System.Text;
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

    [Test]
    public async Task ToFenHandlesNullSquare()
    {
        Square? square = null;
        StringBuilder builder = new();
        Square.ToFen(square, builder);
        string result = builder.ToString();

        await Assert.That(result).IsEqualTo("-");
    }

    [Test]
    [Arguments("a3")]
    [Arguments("b3")]
    [Arguments("c3")]
    [Arguments("d3")]
    [Arguments("e3")]
    [Arguments("f3")]
    [Arguments("g3")]
    [Arguments("h3")]
    [Arguments("a6")]
    [Arguments("b6")]
    [Arguments("c6")]
    [Arguments("d6")]
    [Arguments("e6")]
    [Arguments("f6")]
    [Arguments("g6")]
    [Arguments("h6")]
    public async Task ToFenGeneratesCorrectNotation(string expected)
    {
        // Note: Square constructor is private, so create via FromFen
        Square? square = Square.FromFen(new(expected));
        StringBuilder builder = new();
        Square.ToFen(square, builder);
        string result = builder.ToString();

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments("-")]
    [Arguments("a3")]
    [Arguments("b3")]
    [Arguments("c3")]
    [Arguments("d3")]
    [Arguments("e3")]
    [Arguments("f3")]
    [Arguments("g3")]
    [Arguments("h3")]
    [Arguments("a6")]
    [Arguments("b6")]
    [Arguments("c6")]
    [Arguments("d6")]
    [Arguments("e6")]
    [Arguments("f6")]
    [Arguments("g6")]
    [Arguments("h6")]
    public async Task FromFenAndToFenAreInverses(string fenString)
    {
        Square? original = Square.FromFen(new(fenString));
        StringBuilder builder = new();
        Square.ToFen(original, builder);
        string serialised = builder.ToString();
        Square? roundTrip = Square.FromFen(new(serialised));

        await Assert.That(roundTrip).IsEqualTo(original);
    }
}

using System.Text;
using System.Threading.Tasks;

namespace SimpleChess.State.Tests.State;

public class CastlingRightsTests
{
    [Test]
    public async Task NoCastlingRights()
    {
        CastlingRights rights = CastlingRights.FromFen(new("-"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsFalse();
            await Assert.That(rights.WhiteQueenside).IsFalse();
            await Assert.That(rights.BlackKingside).IsFalse();
            await Assert.That(rights.BlackQueenside).IsFalse();
        }
    }

    [Test]
    public async Task AllCastlingRights()
    {
        CastlingRights rights = CastlingRights.FromFen(new("KQkq"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsTrue();
            await Assert.That(rights.WhiteQueenside).IsTrue();
            await Assert.That(rights.BlackKingside).IsTrue();
            await Assert.That(rights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task WhiteKingsideOnly()
    {
        CastlingRights rights = CastlingRights.FromFen(new("K"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsTrue();
            await Assert.That(rights.WhiteQueenside).IsFalse();
            await Assert.That(rights.BlackKingside).IsFalse();
            await Assert.That(rights.BlackQueenside).IsFalse();
        }
    }

    [Test]
    public async Task WhiteQueensideOnly()
    {
        CastlingRights rights = CastlingRights.FromFen(new("Q"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsFalse();
            await Assert.That(rights.WhiteQueenside).IsTrue();
            await Assert.That(rights.BlackKingside).IsFalse();
            await Assert.That(rights.BlackQueenside).IsFalse();
        }
    }

    [Test]
    public async Task BlackKingsideOnly()
    {
        CastlingRights rights = CastlingRights.FromFen(new("k"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsFalse();
            await Assert.That(rights.WhiteQueenside).IsFalse();
            await Assert.That(rights.BlackKingside).IsTrue();
            await Assert.That(rights.BlackQueenside).IsFalse();
        }
    }

    [Test]
    public async Task BlackQueensideOnly()
    {
        CastlingRights rights = CastlingRights.FromFen(new("q"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsFalse();
            await Assert.That(rights.WhiteQueenside).IsFalse();
            await Assert.That(rights.BlackKingside).IsFalse();
            await Assert.That(rights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task WhiteBothSides()
    {
        CastlingRights rights = CastlingRights.FromFen(new("KQ"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsTrue();
            await Assert.That(rights.WhiteQueenside).IsTrue();
            await Assert.That(rights.BlackKingside).IsFalse();
            await Assert.That(rights.BlackQueenside).IsFalse();
        }
    }

    [Test]
    public async Task BlackBothSides()
    {
        CastlingRights rights = CastlingRights.FromFen(new("kq"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsFalse();
            await Assert.That(rights.WhiteQueenside).IsFalse();
            await Assert.That(rights.BlackKingside).IsTrue();
            await Assert.That(rights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task WhiteKingsideBlackKingside()
    {
        CastlingRights rights = CastlingRights.FromFen(new("Kk"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsTrue();
            await Assert.That(rights.WhiteQueenside).IsFalse();
            await Assert.That(rights.BlackKingside).IsTrue();
            await Assert.That(rights.BlackQueenside).IsFalse();
        }
    }

    [Test]
    public async Task WhiteKingsideBlackQueenside()
    {
        CastlingRights rights = CastlingRights.FromFen(new("Kq"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsTrue();
            await Assert.That(rights.WhiteQueenside).IsFalse();
            await Assert.That(rights.BlackKingside).IsFalse();
            await Assert.That(rights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task WhiteQueensideBlackKingside()
    {
        CastlingRights rights = CastlingRights.FromFen(new("Qk"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsFalse();
            await Assert.That(rights.WhiteQueenside).IsTrue();
            await Assert.That(rights.BlackKingside).IsTrue();
            await Assert.That(rights.BlackQueenside).IsFalse();
        }
    }

    [Test]
    public async Task WhiteQueensideBlackQueenside()
    {
        CastlingRights rights = CastlingRights.FromFen(new("Qq"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsFalse();
            await Assert.That(rights.WhiteQueenside).IsTrue();
            await Assert.That(rights.BlackKingside).IsFalse();
            await Assert.That(rights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task WhiteBothBlackKingside()
    {
        CastlingRights rights = CastlingRights.FromFen(new("KQk"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsTrue();
            await Assert.That(rights.WhiteQueenside).IsTrue();
            await Assert.That(rights.BlackKingside).IsTrue();
            await Assert.That(rights.BlackQueenside).IsFalse();
        }
    }

    [Test]
    public async Task WhiteBothBlackQueenside()
    {
        CastlingRights rights = CastlingRights.FromFen(new("KQq"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsTrue();
            await Assert.That(rights.WhiteQueenside).IsTrue();
            await Assert.That(rights.BlackKingside).IsFalse();
            await Assert.That(rights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task WhiteKingsideBlackBoth()
    {
        CastlingRights rights = CastlingRights.FromFen(new("Kkq"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsTrue();
            await Assert.That(rights.WhiteQueenside).IsFalse();
            await Assert.That(rights.BlackKingside).IsTrue();
            await Assert.That(rights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task WhiteQueensideBlackBoth()
    {
        CastlingRights rights = CastlingRights.FromFen(new("Qkq"));

        using (Assert.Multiple())
        {
            await Assert.That(rights.WhiteKingside).IsFalse();
            await Assert.That(rights.WhiteQueenside).IsTrue();
            await Assert.That(rights.BlackKingside).IsTrue();
            await Assert.That(rights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task OrderDoesNotMatter()
    {
        CastlingRights rights1 = CastlingRights.FromFen(new("KQkq"));
        CastlingRights rights2 = CastlingRights.FromFen(new("qkQK"));
        CastlingRights rights3 = CastlingRights.FromFen(new("kKqQ"));

        using (Assert.Multiple())
        {
            await Assert.That(rights1).IsEqualTo(rights2);
            await Assert.That(rights1).IsEqualTo(rights3);
            await Assert.That(rights2).IsEqualTo(rights3);
        }
    }

    [Test]
    public async Task DuplicatesAreIgnored()
    {
        CastlingRights rights1 = CastlingRights.FromFen(new("KK"));
        CastlingRights rights2 = CastlingRights.FromFen(new("K"));

        await Assert.That(rights1).IsEqualTo(rights2);
    }

    [Test]
    public async Task ToFenGeneratesNoCastlingRights()
    {
        CastlingRights rights = default; // default struct, no rights
        StringBuilder builder = new();
        CastlingRights.ToFen(rights, builder);
        string result = builder.ToString();

        await Assert.That(result).IsEqualTo("-");
    }

    [Test]
    [Arguments("-")]
    [Arguments("K")]
    [Arguments("Q")]
    [Arguments("k")]
    [Arguments("q")]
    [Arguments("KQ")]
    [Arguments("Kk")]
    [Arguments("Kq")]
    [Arguments("Qk")]
    [Arguments("Qq")]
    [Arguments("kq")]
    [Arguments("KQk")]
    [Arguments("KQq")]
    [Arguments("Kkq")]
    [Arguments("Qkq")]
    [Arguments("KQkq")]
    public async Task ToFenGeneratesCorrectNotation(string expected)
    {
        // Note: CastlingRights constructor is private, so create via FromFen
        CastlingRights rights = CastlingRights.FromFen(new(expected));
        StringBuilder builder = new();
        CastlingRights.ToFen(rights, builder);
        string result = builder.ToString();

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments("-")]
    [Arguments("K")]
    [Arguments("Q")]
    [Arguments("k")]
    [Arguments("q")]
    [Arguments("KQ")]
    [Arguments("Kk")]
    [Arguments("Kq")]
    [Arguments("Qk")]
    [Arguments("Qq")]
    [Arguments("kq")]
    [Arguments("KQk")]
    [Arguments("KQq")]
    [Arguments("Kkq")]
    [Arguments("Qkq")]
    [Arguments("KQkq")]
    public async Task FromFenAndToFenAreInverses(string fenString)
    {
        CastlingRights original = CastlingRights.FromFen(new(fenString));
        StringBuilder builder = new();
        CastlingRights.ToFen(original, builder);
        string serialised = builder.ToString();
        CastlingRights roundTrip = CastlingRights.FromFen(new(serialised));

        await Assert.That(roundTrip).IsEqualTo(original);
    }
}

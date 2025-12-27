using System.Threading.Tasks;
using SimpleChessEngine.State;

namespace SimpleChessEngine.Tests.State;

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
}

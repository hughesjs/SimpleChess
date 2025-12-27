using System.Threading.Tasks;
using SimpleChessEngine.State;

namespace SimpleChessEngine.Tests.State;

public class PieceTests
{
    [Test]
    public async Task DefaultPieceIsEmpty()
    {
        Piece piece = default;
        Piece expected = new(Colour.None, PieceType.None);

        await Assert.That(piece).IsEqualTo(expected);
    }
}

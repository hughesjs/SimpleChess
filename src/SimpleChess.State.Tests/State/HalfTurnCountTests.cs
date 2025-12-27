using System.Threading.Tasks;
using SimpleChess.State.State;

namespace SimpleChess.State.Tests.State;

public class HalfTurnCountTests
{
    [Test]
    [Arguments("0", 0)]
    [Arguments("1", 1)]
    [Arguments("50", 50)]
    [Arguments("100", 100)]
    [Arguments("149", 149)]
    [Arguments("150", 150)]
    public async Task FromFenParsesValidValues(string fenString, int expectedValue)
    {
        FenGameState.FenSegment<FenGameState.HalfTurnCounterKind> segment = new(fenString);
        HalfTurnCount count = HalfTurnCount.FromFen(segment);
        int actualValue = count; // implicit conversion

        await Assert.That(actualValue).IsEqualTo(expectedValue);
    }

    [Test]
    public async Task FromFenHandlesBoundaryValues()
    {
        HalfTurnCount min = HalfTurnCount.FromFen(new("0"));
        HalfTurnCount max = HalfTurnCount.FromFen(new("150"));

        using (Assert.Multiple())
        {
            await Assert.That((int)min).IsEqualTo(0);
            await Assert.That((int)max).IsEqualTo(150);
        }
    }
}

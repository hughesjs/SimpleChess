using System.Threading.Tasks;

namespace SimpleChess.State.Tests.State;

public class FullTurnCountTests
{
    [Test]
    [Arguments("1", 1)]
    [Arguments("5", 5)]
    [Arguments("100", 100)]
    [Arguments("1000", 1000)]
    [Arguments("8839", 8839)]
    [Arguments("8840", 8840)]
    public async Task FromFenParsesValidValues(string fenString, int expectedValue)
    {
        FenGameState.FenSegment<FenGameState.FullTurnCounterKind> segment = new(fenString);
        FullTurnCount count = FullTurnCount.FromFen(segment);
        int actualValue = count; // implicit conversion

        await Assert.That(actualValue).IsEqualTo(expectedValue);
    }

    [Test]
    public async Task FromFenHandlesBoundaryValues()
    {
        FullTurnCount min = FullTurnCount.FromFen(new("1"));
        FullTurnCount max = FullTurnCount.FromFen(new("8840"));

        using (Assert.Multiple())
        {
            await Assert.That((int)min).IsEqualTo(1);
            await Assert.That((int)max).IsEqualTo(8840);
        }
    }
}

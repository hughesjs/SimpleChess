using SimpleChessEngine.Notation;

namespace SimpleChessEngine.Tests.Notation;

public class FenBoardStateTests
{
    [Test]
    public async Task TryParseStartingPositionReturnsTrue()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task TryParseMiddlegamePositionReturnsTrue()
    {
        const string fen = "r1bqkb1r/pppp1ppp/2n2n2/4p3/2B1P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 4 5";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task TryParseNoCastlingRightsReturnsTrue()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task TryParseEnPassantAvailableReturnsTrue()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task TryParseWhiteCastlingOnlyReturnsTrue()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQ - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task TryParseBlackCastlingOnlyReturnsTrue()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w kq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task TryParseHighMoveNumbersReturnsTrue()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 50 100";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task TryParseComplexPositionReturnsTrue()
    {
        const string fen = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task TryParseAllEmptyRanksReturnsTrue()
    {
        const string fen = "8/8/8/8/8/8/8/8 w - - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 extra")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR  w  KQkq  -  0  1")]
    public async Task TryParseInvalidPartCountReturnsFalse(string fen)
    {
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseOneRankReturnsFalse()
    {
        const string fen = "rnbqkbnr w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseSevenRanksReturnsFalse()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseNineRanksReturnsFalse()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/8/8/PPPPPPPP w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseRankSumsToSevenReturnsFalse()
    {
        const string fen = "rnbqkbn/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseRankSumsToNineReturnsFalse()
    {
        const string fen = "rnbqkbnrr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseDigitNineReturnsFalse()
    {
        const string fen = "9/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseCombinationExceedsEightReturnsFalse()
    {
        const string fen = "4rnbqk/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseEmptyRankReturnsFalse()
    {
        const string fen = "rnbqkbnr/pppppppp//8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseInvalidCharacterXReturnsFalse()
    {
        const string fen = "rnbqkbnx/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseInvalidCharacterZeroReturnsFalse()
    {
        const string fen = "rnbqkbn0/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseSpecialCharacterReturnsFalse()
    {
        const string fen = "rnbq@bnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("w")]
    [Arguments("b")]
    public async Task TryParseValidActiveColourReturnsTrue(string colour)
    {
        string fen = $"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR {colour} KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("W")]
    [Arguments("B")]
    [Arguments("x")]
    [Arguments("white")]
    [Arguments("black")]
    public async Task TryParseInvalidActiveColourReturnsFalse(string colour)
    {
        string fen = $"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR {colour} KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
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
    public async Task TryParseValidCastlingRightsReturnsTrue(string castling)
    {
        string fen = $"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w {castling} - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("QK")]
    [Arguments("qk")]
    [Arguments("KK")]
    [Arguments("KQX")]
    [Arguments("kKQq")]
    [Arguments("KqQk")]
    [Arguments("Qq k")]
    public async Task TryParseInvalidCastlingRightsReturnsFalse(string castling)
    {
        string fen = $"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w {castling} - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("-")]
    [Arguments("a3")]
    [Arguments("h3")]
    [Arguments("a6")]
    [Arguments("h6")]
    [Arguments("e3")]
    [Arguments("e6")]
    [Arguments("d3")]
    [Arguments("d6")]
    [Arguments("b3")]
    [Arguments("c6")]
    public async Task TryParseValidEnPassantSquareReturnsTrue(string enPassant)
    {
        string fen = $"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq {enPassant} 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("e1")]
    [Arguments("e2")]
    [Arguments("e4")]
    [Arguments("e5")]
    [Arguments("e7")]
    [Arguments("e8")]
    [Arguments("i3")]
    [Arguments("E3")]
    [Arguments("e33")]
    [Arguments("e")]
    [Arguments("3")]
    public async Task TryParseInvalidEnPassantSquareReturnsFalse(string enPassant)
    {
        string fen = $"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq {enPassant} 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments(0, 1)]
    [Arguments(1, 1)]
    [Arguments(50, 25)]
    [Arguments(100, 50)]
    [Arguments(999, 999)]
    public async Task TryParseValidMoveCountersReturnsTrue(int halfmove, int fullmove)
    {
        string fen = $"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - {halfmove} {fullmove}";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments(-1, 1)]
    [Arguments(-50, 1)]
    [Arguments(0, 0)]
    [Arguments(1, 0)]
    [Arguments(0, -1)]
    [Arguments(-1, -1)]
    public async Task TryParseInvalidMoveCountersReturnsFalse(int halfmove, int fullmove)
    {
        string fen = $"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - {halfmove} {fullmove}";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("abc", "1")]
    [Arguments("1.5", "1")]
    [Arguments("0", "xyz")]
    [Arguments("0", "2.5")]
    [Arguments("abc", "xyz")]
    public async Task TryParseMoveCountersNonNumericReturnsFalse(string halfmove, string fullmove)
    {
        string fen = $"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - {halfmove} {fullmove}";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseWhitespaceOnlyReturnsFalse()
    {
        const string fen = "     ";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseTabSeparatedReturnsFalse()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR\tw\tKQkq\t-\t0\t1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseTrailingSpaceReturnsFalse()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 ";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryParseLeadingSpaceReturnsFalse()
    {
        const string fen = " rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        bool result = FenBoardState.TryParse(fen, out FenBoardState _);
        await Assert.That(result).IsFalse();
    }
}

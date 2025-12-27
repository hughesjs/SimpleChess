using System.Threading.Tasks;
using SimpleChessEngine.State;

namespace SimpleChessEngine.Tests.State;

public class GameStateTests
{
    [Test]
    public async Task GameStateHasValueSemantics()
    {
        GameState gameState = GameState.FromFen(FenGameState.DefaultGame);
        GameState gameState2 =  GameState.FromFen(FenGameState.DefaultGame);

        await Assert.That(gameState).IsEqualTo(gameState2);
    }

    [Test]
    public async Task NewGameHasAllCastlingRights()
    {
        GameState gameState = GameState.NewGameState;

        using (Assert.Multiple())
        {
            await Assert.That(gameState.CastlingRights.WhiteKingside).IsTrue();
            await Assert.That(gameState.CastlingRights.WhiteQueenside).IsTrue();
            await Assert.That(gameState.CastlingRights.BlackKingside).IsTrue();
            await Assert.That(gameState.CastlingRights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task NewGameHasNoEnPassantTarget()
    {
        GameState gameState = GameState.NewGameState;

        await Assert.That(gameState.EnPassantTarget).IsNull();
    }

    [Test]
    public async Task GameWithNoCastlingRightsHasNoCastlingRights()
    {
        FenGameState fen = FenGameState.DefaultGame;
        bool parsed = FenGameState.TryParse("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - - 0 1", out fen);
        GameState gameState = GameState.FromFen(fen);

        using (Assert.Multiple())
        {
            await Assert.That(parsed).IsTrue();
            await Assert.That(gameState.CastlingRights.WhiteKingside).IsFalse();
            await Assert.That(gameState.CastlingRights.WhiteQueenside).IsFalse();
            await Assert.That(gameState.CastlingRights.BlackKingside).IsFalse();
            await Assert.That(gameState.CastlingRights.BlackQueenside).IsFalse();
        }
    }

    [Test]
    public async Task GameWithOnlyWhiteKingsideCastlingHasCorrectRights()
    {
        FenGameState fen = FenGameState.DefaultGame;
        bool parsed = FenGameState.TryParse("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w K - 0 1", out fen);
        GameState gameState = GameState.FromFen(fen);

        using (Assert.Multiple())
        {
            await Assert.That(parsed).IsTrue();
            await Assert.That(gameState.CastlingRights.WhiteKingside).IsTrue();
            await Assert.That(gameState.CastlingRights.WhiteQueenside).IsFalse();
            await Assert.That(gameState.CastlingRights.BlackKingside).IsFalse();
            await Assert.That(gameState.CastlingRights.BlackQueenside).IsFalse();
        }
    }

    [Test]
    public async Task GameAfterE4HasEnPassantTargetE3()
    {
        FenGameState fen = FenGameState.DefaultGame;
        bool parsed = FenGameState.TryParse("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", out fen);
        GameState gameState = GameState.FromFen(fen);

        using (Assert.Multiple())
        {
            await Assert.That(parsed).IsTrue();
            await Assert.That(gameState.EnPassantTarget).IsNotNull();
            await Assert.That(gameState.EnPassantTarget!.Value.File).IsEqualTo(File.E);
            await Assert.That(gameState.EnPassantTarget!.Value.Rank).IsEqualTo(Rank.Three);
        }
    }

    [Test]
    public async Task GameAfterE4E5HasEnPassantTargetE6()
    {
        FenGameState fen = FenGameState.DefaultGame;
        bool parsed = FenGameState.TryParse("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e6 0 2", out fen);
        GameState gameState = GameState.FromFen(fen);

        using (Assert.Multiple())
        {
            await Assert.That(parsed).IsTrue();
            await Assert.That(gameState.EnPassantTarget).IsNotNull();
            await Assert.That(gameState.EnPassantTarget!.Value.File).IsEqualTo(File.E);
            await Assert.That(gameState.EnPassantTarget!.Value.Rank).IsEqualTo(Rank.Six);
        }
    }

    [Test]
    [Arguments("KQkq")]
    [Arguments("KQ")]
    [Arguments("Kk")]
    [Arguments("Qq")]
    [Arguments("K")]
    [Arguments("q")]
    [Arguments("-")]
    public async Task GameFromFenPreservesCastlingRights(string castlingRights)
    {
        FenGameState fen = FenGameState.DefaultGame;
        string fenString = $"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w {castlingRights} - 0 1";
        bool parsed = FenGameState.TryParse(fenString, out fen);
        GameState gameState = GameState.FromFen(fen);

        CastlingRights expected = CastlingRights.FromFen(new(castlingRights));

        using (Assert.Multiple())
        {
            await Assert.That(parsed).IsTrue();
            await Assert.That(gameState.CastlingRights).IsEqualTo(expected);
        }
    }

    [Test]
    [Arguments("-", null, null)]
    [Arguments("e3", File.E, Rank.Three)]
    [Arguments("e6", File.E, Rank.Six)]
    [Arguments("a3", File.A, Rank.Three)]
    [Arguments("h6", File.H, Rank.Six)]
    public async Task GameFromFenPreservesEnPassantTarget(string enPassantFen, File? expectedFile, Rank? expectedRank)
    {
        FenGameState fen = FenGameState.DefaultGame;
        string fenString = $"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq {enPassantFen} 0 1";
        bool parsed = FenGameState.TryParse(fenString, out fen);
        GameState gameState = GameState.FromFen(fen);

        using (Assert.Multiple())
        {
            await Assert.That(parsed).IsTrue();

            if (expectedRank == null && expectedFile == null)
            {
                await Assert.That(gameState.EnPassantTarget).IsNull();
            }
            else
            {
                await Assert.That(gameState.EnPassantTarget).IsNotNull();
                await Assert.That(gameState.EnPassantTarget!.Value.File).IsEqualTo(expectedFile!.Value);
                await Assert.That(gameState.EnPassantTarget!.Value.Rank).IsEqualTo(expectedRank!.Value);
            }
        }
    }

    [Test]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
    [Arguments("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1")]
    [Arguments("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e6 0 2")]
    [Arguments("r1bqkb1r/pppp1ppp/2n2n2/4p3/2B1P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 4 5")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - - 0 1")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w K - 0 1")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Kq - 0 1")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1")]
    [Arguments("8/8/8/8/8/8/8/8 w - - 0 1")]
    [Arguments("4k3/8/8/8/8/8/8/4K3 w - - 0 1")]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 50 100")]
    public async Task FromFenAndToFenAreInverses(string fenString)
    {
        bool parsed = FenGameState.TryParse(fenString, out FenGameState originalFen);
        GameState gameState = GameState.FromFen(originalFen);
        FenGameState roundTripFen = GameState.ToFen(gameState);
        string roundTripString = roundTripFen.ToString();
        GameState roundTripGameState = GameState.FromFen(roundTripFen);

        using (Assert.Multiple())
        {
            await Assert.That(parsed).IsTrue();
            await Assert.That(roundTripString).IsEqualTo(fenString);
            await Assert.That(roundTripGameState).IsEqualTo(gameState);
        }
    }
}

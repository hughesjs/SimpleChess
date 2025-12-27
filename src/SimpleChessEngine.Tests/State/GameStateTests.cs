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
            await Assert.That(gameState.EnPassantTarget!.Value.Rank).IsEqualTo(Rank.E);
            await Assert.That(gameState.EnPassantTarget!.Value.File).IsEqualTo(File.Three);
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
            await Assert.That(gameState.EnPassantTarget!.Value.Rank).IsEqualTo(Rank.E);
            await Assert.That(gameState.EnPassantTarget!.Value.File).IsEqualTo(File.Six);
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
    [Arguments("e3", Rank.E, File.Three)]
    [Arguments("e6", Rank.E, File.Six)]
    [Arguments("a3", Rank.A, File.Three)]
    [Arguments("h6", Rank.H, File.Six)]
    public async Task GameFromFenPreservesEnPassantTarget(string enPassantFen, Rank? expectedRank, File? expectedFile)
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
                await Assert.That(gameState.EnPassantTarget!.Value.Rank).IsEqualTo(expectedRank!.Value);
                await Assert.That(gameState.EnPassantTarget!.Value.File).IsEqualTo(expectedFile!.Value);
            }
        }
    }
}

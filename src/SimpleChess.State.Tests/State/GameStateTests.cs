using System.Threading.Tasks;

namespace SimpleChess.State.Tests.State;

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

    // ===== ApplyMove() Tests =====

    // Turn Alternation Tests

    [Test]
    public async Task ApplyMoveAlternatesTurnsFromWhiteToBlack()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.E, Rank.Two),
            Square.FromRankAndFile(File.E, Rank.Four)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.NextToPlay).IsEqualTo(Colour.White);
            await Assert.That(newState.NextToPlay).IsEqualTo(Colour.Black);
        }
    }

    [Test]
    public async Task ApplyMoveAlternatesTurnsFromBlackToWhite()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.E, Rank.Seven),
            Square.FromRankAndFile(File.E, Rank.Five)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.NextToPlay).IsEqualTo(Colour.Black);
            await Assert.That(newState.NextToPlay).IsEqualTo(Colour.White);
        }
    }

    // Turn Counter Tests

    [Test]
    public async Task ApplyMoveIncrementsHalfTurnCounter()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 5 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.B, Rank.One),
            Square.FromRankAndFile(File.C, Rank.Three)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That((int)state.HalfTurnCounter).IsEqualTo(5);
            await Assert.That((int)newState.HalfTurnCounter).IsEqualTo(6);
        }
    }

    [Test]
    public async Task ApplyMoveIncrementsFullTurnCounterAfterBlackMove()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.E, Rank.Seven),
            Square.FromRankAndFile(File.E, Rank.Five)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That((int)state.FullTurnCounter).IsEqualTo(1);
            await Assert.That((int)newState.FullTurnCounter).IsEqualTo(2);
        }
    }

    [Test]
    public async Task ApplyMoveDoesNotIncrementFullTurnCounterAfterWhiteMove()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.E, Rank.Two),
            Square.FromRankAndFile(File.E, Rank.Four)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That((int)state.FullTurnCounter).IsEqualTo(1);
            await Assert.That((int)newState.FullTurnCounter).IsEqualTo(1);
        }
    }

    // En Passant Target Tests

    [Test]
    [Arguments(File.A)]
    [Arguments(File.D)]
    [Arguments(File.E)]
    [Arguments(File.H)]
    public async Task ApplyMoveSetsEnPassantTargetForWhitePawnDouble(File file)
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.PawnDouble(
            Square.FromRankAndFile(file, Rank.Two),
            Square.FromRankAndFile(file, Rank.Four)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.EnPassantTarget).IsNull();
            await Assert.That(newState.EnPassantTarget).IsNotNull();
            await Assert.That(newState.EnPassantTarget!.Value.File).IsEqualTo(file);
            await Assert.That(newState.EnPassantTarget!.Value.Rank).IsEqualTo(Rank.Four);
        }
    }

    [Test]
    [Arguments(File.A)]
    [Arguments(File.D)]
    [Arguments(File.E)]
    [Arguments(File.H)]
    public async Task ApplyMoveSetsEnPassantTargetForBlackPawnDouble(File file)
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.PawnDouble(
            Square.FromRankAndFile(file, Rank.Seven),
            Square.FromRankAndFile(file, Rank.Five)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.EnPassantTarget).IsNull();
            await Assert.That(newState.EnPassantTarget).IsNotNull();
            await Assert.That(newState.EnPassantTarget!.Value.File).IsEqualTo(file);
            await Assert.That(newState.EnPassantTarget!.Value.Rank).IsEqualTo(Rank.Five);
        }
    }

    [Test]
    public async Task ApplyMoveClearsEnPassantTargetForNonPawnDoubleMoves()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.E, Rank.Seven),
            Square.FromRankAndFile(File.E, Rank.Six)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.EnPassantTarget).IsNotNull();
            await Assert.That(newState.EnPassantTarget).IsNull();
        }
    }

    [Test]
    public async Task ApplyMoveDoesNotSetEnPassantTargetForNormalPawnMove()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.E, Rank.Two),
            Square.FromRankAndFile(File.E, Rank.Three)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.EnPassantTarget).IsNull();
            await Assert.That(newState.EnPassantTarget).IsNull();
        }
    }

    // Castling Rights Removal Tests

    [Test]
    public async Task ApplyMoveRemovesWhiteCastlingRightsWhenKingMoves()
    {
        const string fen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.E, Rank.One),
            Square.FromRankAndFile(File.E, Rank.Two)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.CastlingRights.WhiteKingside).IsTrue();
            await Assert.That(state.CastlingRights.WhiteQueenside).IsTrue();
            await Assert.That(newState.CastlingRights.WhiteKingside).IsFalse();
            await Assert.That(newState.CastlingRights.WhiteQueenside).IsFalse();
            await Assert.That(newState.CastlingRights.BlackKingside).IsTrue();
            await Assert.That(newState.CastlingRights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task ApplyMoveRemovesBlackCastlingRightsWhenKingMoves()
    {
        const string fen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R b KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.E, Rank.Eight),
            Square.FromRankAndFile(File.E, Rank.Seven)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.CastlingRights.BlackKingside).IsTrue();
            await Assert.That(state.CastlingRights.BlackQueenside).IsTrue();
            await Assert.That(newState.CastlingRights.BlackKingside).IsFalse();
            await Assert.That(newState.CastlingRights.BlackQueenside).IsFalse();
            await Assert.That(newState.CastlingRights.WhiteKingside).IsTrue();
            await Assert.That(newState.CastlingRights.WhiteQueenside).IsTrue();
        }
    }

    [Test]
    public async Task ApplyMoveRemovesWhiteKingsideCastlingWhenH1RookMoves()
    {
        const string fen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.H, Rank.One),
            Square.FromRankAndFile(File.H, Rank.Two)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.CastlingRights.WhiteKingside).IsTrue();
            await Assert.That(newState.CastlingRights.WhiteKingside).IsFalse();
            await Assert.That(newState.CastlingRights.WhiteQueenside).IsTrue();
            await Assert.That(newState.CastlingRights.BlackKingside).IsTrue();
            await Assert.That(newState.CastlingRights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task ApplyMoveRemovesWhiteQueensideCastlingWhenA1RookMoves()
    {
        const string fen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.A, Rank.One),
            Square.FromRankAndFile(File.A, Rank.Two)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.CastlingRights.WhiteQueenside).IsTrue();
            await Assert.That(newState.CastlingRights.WhiteQueenside).IsFalse();
            await Assert.That(newState.CastlingRights.WhiteKingside).IsTrue();
            await Assert.That(newState.CastlingRights.BlackKingside).IsTrue();
            await Assert.That(newState.CastlingRights.BlackQueenside).IsTrue();
        }
    }

    [Test]
    public async Task ApplyMoveRemovesBlackKingsideCastlingWhenH8RookMoves()
    {
        const string fen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R b KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.H, Rank.Eight),
            Square.FromRankAndFile(File.H, Rank.Seven)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.CastlingRights.BlackKingside).IsTrue();
            await Assert.That(newState.CastlingRights.BlackKingside).IsFalse();
            await Assert.That(newState.CastlingRights.BlackQueenside).IsTrue();
            await Assert.That(newState.CastlingRights.WhiteKingside).IsTrue();
            await Assert.That(newState.CastlingRights.WhiteQueenside).IsTrue();
        }
    }

    [Test]
    public async Task ApplyMoveRemovesBlackQueensideCastlingWhenA8RookMoves()
    {
        const string fen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R b KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.A, Rank.Eight),
            Square.FromRankAndFile(File.A, Rank.Seven)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.CastlingRights.BlackQueenside).IsTrue();
            await Assert.That(newState.CastlingRights.BlackQueenside).IsFalse();
            await Assert.That(newState.CastlingRights.BlackKingside).IsTrue();
            await Assert.That(newState.CastlingRights.WhiteKingside).IsTrue();
            await Assert.That(newState.CastlingRights.WhiteQueenside).IsTrue();
        }
    }

    // Castling Rights Preservation Tests

    [Test]
    public async Task ApplyMovePreservesCastlingRightsForNonKingNonRookMoves()
    {
        const string fen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.D, Rank.Two),
            Square.FromRankAndFile(File.D, Rank.Four)
        );

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(newState.CastlingRights.WhiteKingside).IsTrue();
            await Assert.That(newState.CastlingRights.WhiteQueenside).IsTrue();
            await Assert.That(newState.CastlingRights.BlackKingside).IsTrue();
            await Assert.That(newState.CastlingRights.BlackQueenside).IsTrue();
        }
    }

    // Board State Immutability Tests

    [Test]
    public async Task ApplyMoveDoesNotMutateOriginalGameState()
    {
        const string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Normal(
            Square.FromRankAndFile(File.E, Rank.Two),
            Square.FromRankAndFile(File.E, Rank.Four)
        );

        Board originalBoard = state.CurrentBoard;
        Colour originalNextToPlay = state.NextToPlay;
        HalfTurnCount originalHalfTurn = state.HalfTurnCounter;

        GameState newState = state.ApplyMove(move);

        using (Assert.Multiple())
        {
            await Assert.That(state.CurrentBoard).IsEqualTo(originalBoard);
            await Assert.That(state.NextToPlay).IsEqualTo(originalNextToPlay);
            await Assert.That(state.HalfTurnCounter).IsEqualTo(originalHalfTurn);
            await Assert.That(newState.NextToPlay).IsEqualTo(Colour.Black);
        }
    }

    // Sequential Move Tests

    [Test]
    public async Task ApplyMultipleMovesUpdatesStateCorrectly()
    {
        GameState state = GameState.NewGameState;

        // Move 1: e4
        Move move1 = Move.PawnDouble(
            Square.FromRankAndFile(File.E, Rank.Two),
            Square.FromRankAndFile(File.E, Rank.Four)
        );
        state = state.ApplyMove(move1);

        using (Assert.Multiple())
        {
            await Assert.That(state.NextToPlay).IsEqualTo(Colour.Black);
            await Assert.That((int)state.FullTurnCounter).IsEqualTo(1);
            await Assert.That(state.EnPassantTarget).IsNotNull();
        }

        // Move 2: e5
        Move move2 = Move.PawnDouble(
            Square.FromRankAndFile(File.E, Rank.Seven),
            Square.FromRankAndFile(File.E, Rank.Five)
        );
        state = state.ApplyMove(move2);

        using (Assert.Multiple())
        {
            await Assert.That(state.NextToPlay).IsEqualTo(Colour.White);
            await Assert.That((int)state.FullTurnCounter).IsEqualTo(2);
            await Assert.That(state.EnPassantTarget).IsNotNull();
        }

        // Move 3: Nf3 (clears en passant)
        Move move3 = Move.Normal(
            Square.FromRankAndFile(File.G, Rank.One),
            Square.FromRankAndFile(File.F, Rank.Three)
        );
        state = state.ApplyMove(move3);

        using (Assert.Multiple())
        {
            await Assert.That(state.NextToPlay).IsEqualTo(Colour.Black);
            await Assert.That((int)state.FullTurnCounter).IsEqualTo(2);
            await Assert.That(state.EnPassantTarget).IsNull();
        }
    }

    // Special Move Type Tests

    [Test]
    public async Task ApplyMoveHandlesPromotionMoves()
    {
        const string fen = "8/4P3/8/8/8/8/8/4K2k w - - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Promotion(
            Square.FromRankAndFile(File.E, Rank.Seven),
            Square.FromRankAndFile(File.E, Rank.Eight),
            PieceType.Queen
        );

        GameState newState = state.ApplyMove(move);
        Piece promotedPiece = newState.CurrentBoard.GetPieceAt(Rank.Eight, File.E);

        using (Assert.Multiple())
        {
            await Assert.That(promotedPiece.PieceType).IsEqualTo(PieceType.Queen);
            await Assert.That(promotedPiece.Colour).IsEqualTo(Colour.White);
            await Assert.That(newState.NextToPlay).IsEqualTo(Colour.Black);
            await Assert.That((int)newState.HalfTurnCounter).IsEqualTo(1);
        }
    }

    [Test]
    public async Task ApplyMoveHandlesEnPassantCapture()
    {
        const string fen = "8/8/8/3pP3/8/8/8/4K2k w - d6 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Square capturedPawnSquare = Square.FromRankAndFile(File.D, Rank.Five);
        Move move = Move.EnPassant(
            Square.FromRankAndFile(File.E, Rank.Five),
            Square.FromRankAndFile(File.D, Rank.Six),
            capturedPawnSquare
        );

        GameState newState = state.ApplyMove(move);
        Piece capturedSquare = newState.CurrentBoard.GetPieceAt(capturedPawnSquare);
        Piece destinationSquare = newState.CurrentBoard.GetPieceAt(Rank.Six, File.D);

        using (Assert.Multiple())
        {
            await Assert.That(capturedSquare).IsEqualTo(Piece.None);
            await Assert.That(destinationSquare.PieceType).IsEqualTo(PieceType.Pawn);
            await Assert.That(destinationSquare.Colour).IsEqualTo(Colour.White);
            await Assert.That(newState.EnPassantTarget).IsNull();
        }
    }

    [Test]
    public async Task ApplyMoveHandlesCastlingMove()
    {
        const string fen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w KQkq - 0 1";
        FenGameState.TryParse(fen, out FenGameState fenState);
        GameState state = GameState.FromFen(fenState);

        Move move = Move.Castling(
            Square.FromRankAndFile(File.E, Rank.One),
            Square.FromRankAndFile(File.G, Rank.One),
            Square.FromRankAndFile(File.H, Rank.One),
            Square.FromRankAndFile(File.F, Rank.One)
        );

        GameState newState = state.ApplyMove(move);

        Piece kingSquare = newState.CurrentBoard.GetPieceAt(Rank.One, File.G);
        Piece rookSquare = newState.CurrentBoard.GetPieceAt(Rank.One, File.F);
        Piece oldKingSquare = newState.CurrentBoard.GetPieceAt(Rank.One, File.E);
        Piece oldRookSquare = newState.CurrentBoard.GetPieceAt(Rank.One, File.H);

        using (Assert.Multiple())
        {
            await Assert.That(kingSquare.PieceType).IsEqualTo(PieceType.King);
            await Assert.That(rookSquare.PieceType).IsEqualTo(PieceType.Rook);
            await Assert.That(oldKingSquare).IsEqualTo(Piece.None);
            await Assert.That(oldRookSquare).IsEqualTo(Piece.None);
            await Assert.That(newState.CastlingRights.WhiteKingside).IsFalse();
            await Assert.That(newState.CastlingRights.WhiteQueenside).IsFalse();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SimpleChess.State;

namespace SimpleChess.Engine.Tests;

/// <summary>
/// Tests the individual stages of the BasicMoveFinder. The implementation chains these together, so they're hard to test individually using the public API.
/// As such, I'm using reflection to test the private methods.
/// </summary>
public class BasicMoveFinderTests
{

    private static IEnumerable<Move> InvokeGetPawnBasicMoves(Square pieceSquare, Board board, Piece piece)
    {
        MethodInfo? method = typeof(BasicMoveFinder)
            .GetMethod("GetPawnBasicMoves", BindingFlags.NonPublic | BindingFlags.Static);

        return (IEnumerable<Move>)method!.Invoke(null, [pieceSquare, board, piece])!;
    }

    private static IEnumerable<Move> InvokeGetDoubleMove(Square pieceSquare, Board board, Piece piece)
    {
        MethodInfo? method = typeof(BasicMoveFinder)
            .GetMethod("GetPawnDoubleMove", BindingFlags.NonPublic | BindingFlags.Static);

        return (IEnumerable<Move>)method!.Invoke(null, [pieceSquare, board, piece])!;
    }

    private static IEnumerable<Move> InvokeGetRookBasicMoves(Square pieceSquare, Board board, Piece piece)
    {
        MethodInfo? method = typeof(BasicMoveFinder)
            .GetMethod("GetRookBasicMoves", BindingFlags.NonPublic | BindingFlags.Static);

        return (IEnumerable<Move>)method!.Invoke(null, [pieceSquare, board, piece])!;
    }

    private static IEnumerable<Move> InvokeGetBishopBasicMoves(Square pieceSquare, Board board, Piece piece)
    {
        MethodInfo? method = typeof(BasicMoveFinder)
            .GetMethod("GetBishopBasicMoves", BindingFlags.NonPublic | BindingFlags.Static);

        return (IEnumerable<Move>)method!.Invoke(null, [pieceSquare, board, piece])!;
    }

    private static IEnumerable<Move> InvokeGetQueenBasicMoves(Square pieceSquare, Board board, Piece piece)
    {
        MethodInfo? method = typeof(BasicMoveFinder)
            .GetMethod("GetQueenBasicMoves", BindingFlags.NonPublic | BindingFlags.Static);

        return (IEnumerable<Move>)method!.Invoke(null, [pieceSquare, board, piece])!;
    }

    private static IEnumerable<Move> InvokeGetKnightBasicMoves(Square pieceSquare, Board board, Piece piece)
    {
        MethodInfo? method = typeof(BasicMoveFinder)
            .GetMethod("GetKnightBasicMoves", BindingFlags.NonPublic | BindingFlags.Static);

        return (IEnumerable<Move>)method!.Invoke(null, [pieceSquare, board, piece])!;
    }

    private static IEnumerable<Move> InvokeGetKingBasicMoves(Square pieceSquare, Board board, Piece piece)
    {
        MethodInfo? method = typeof(BasicMoveFinder)
            .GetMethod("GetKingBasicMoves", BindingFlags.NonPublic | BindingFlags.Static);

        return (IEnumerable<Move>)method!.Invoke(null, [pieceSquare, board, piece])!;
    }

    private static IEnumerable<Move> InvokeGetEnPassantMoves(Square pieceSquare, Piece piece, Square? enPassantTarget)
    {
        MethodInfo? method = typeof(BasicMoveFinder)
            .GetMethod("GetEnPassantMoves", BindingFlags.NonPublic | BindingFlags.Static);

        return (IEnumerable<Move>)method!.Invoke(null, [pieceSquare, piece, enPassantTarget])!;
    }

    private static IEnumerable<Move> InvokeGetCastlingMoves(Square pieceSquare, Piece piece, CastlingRights castlingRights)
    {
        MethodInfo? method = typeof(BasicMoveFinder)
            .GetMethod("GetCastlingMoves", BindingFlags.NonPublic | BindingFlags.Static);

        return (IEnumerable<Move>)method!.Invoke(null, [pieceSquare, piece, castlingRights])!;
    }

    private static IEnumerable<Move> InvokeGetPromotionMoves(Square pieceSquare, Board board, Piece piece)
    {
        MethodInfo? method = typeof(BasicMoveFinder)
            .GetMethod("GetPromotionMoves", BindingFlags.NonPublic | BindingFlags.Static);

        return (IEnumerable<Move>)method!.Invoke(null, [pieceSquare, board, piece])!;
    }

    private static Board CreateBoardFromFen(string fenString)
    {
        bool success = FenGameState.TryParse(fenString, out FenGameState fenState);
        if (!success)
        {
            throw new ArgumentException($"Invalid FEN: {fenString}");
        }
        GameState gameState = GameState.FromFen(fenState);
        return gameState.CurrentBoard;
    }

    private static CastlingRights CreateCastlingRightsFromFen(string fenString)
    {
        bool success = FenGameState.TryParse(fenString, out FenGameState fenState);
        if (!success)
        {
            throw new ArgumentException($"Invalid FEN: {fenString}");
        }
        GameState gameState = GameState.FromFen(fenState);
        return gameState.CastlingRights;
    }

    [Test]
    public async Task WhitePawnMovesForwardToEmptySquare()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/4P3/8 w - - 0 1");
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Square e3 = Square.FromRankAndFile(File.E, Rank.Three);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPawnBasicMoves(e2, board, whitePawn);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e2);
            await Assert.That(movesArray[0].Destination).IsEqualTo(e3);
        }
    }

    [Test]
    public async Task BlackPawnMovesForwardToEmptySquare()
    {
        Board board = CreateBoardFromFen("8/4p3/8/8/8/8/8/8 w - - 0 1");
        Square e7 = Square.FromRankAndFile(File.E, Rank.Seven);
        Square e6 = Square.FromRankAndFile(File.E, Rank.Six);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPawnBasicMoves(e7, board, blackPawn);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e7);
            await Assert.That(movesArray[0].Destination).IsEqualTo(e6);
        }
    }

    [Test]
    public async Task PawnBlockedByPieceReturnsNoMoves()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/4p3/4P3/8 w - - 0 1");
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPawnBasicMoves(e2, board, whitePawn);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task WhitePawnCapturesDiagonallyBothSides()
    {
        Board board = CreateBoardFromFen("8/8/8/3p1p2/4P3/8/8/8 w - - 0 1");
        Square e4 = Square.FromRankAndFile(File.E, Rank.Four);
        Square d5 = Square.FromRankAndFile(File.D, Rank.Five);
        Square e5 = Square.FromRankAndFile(File.E, Rank.Five);
        Square f5 = Square.FromRankAndFile(File.F, Rank.Five);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPawnBasicMoves(e4, board, whitePawn);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(3);
            await Assert.That(movesArray.Any(m => m.Destination.Equals(e5))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(d5))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(f5))).IsTrue();
        }
    }

    [Test]
    public async Task PawnCannotCaptureOwnPiece()
    {
        Board board = CreateBoardFromFen("8/8/8/3N1N2/4P3/8/8/8 w - - 0 1");
        Square e4 = Square.FromRankAndFile(File.E, Rank.Four);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPawnBasicMoves(e4, board, whitePawn);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Destination.Rank).IsEqualTo(Rank.Five);
            await Assert.That(movesArray[0].Destination.File).IsEqualTo(File.E);
        }
    }

    [Test]
    public async Task WhitePawnOnRankSevenDoesNotMoveToRankEight()
    {
        Board board = CreateBoardFromFen("8/4P3/8/8/8/8/8/8 w - - 0 1");
        Square e7 = Square.FromRankAndFile(File.E, Rank.Seven);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPawnBasicMoves(e7, board, whitePawn);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task BlackPawnOnRankTwoDoesNotMoveToRankOne()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/4p3/8 w - - 0 1");
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPawnBasicMoves(e2, board, blackPawn);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task WhitePawnCannotCaptureToPromotionRank()
    {
        Board board = CreateBoardFromFen("4n3/3P4/8/8/8/8/8/8 w - - 0 1");
        Square d7 = Square.FromRankAndFile(File.D, Rank.Seven);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPawnBasicMoves(d7, board, whitePawn);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task WhitePawnOnRankTwoCanDoubleMove()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/4P3/8 w - - 0 1");
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Square e4 = Square.FromRankAndFile(File.E, Rank.Four);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetDoubleMove(e2, board, whitePawn);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e2);
            await Assert.That(movesArray[0].Destination).IsEqualTo(e4);
        }
    }

    [Test]
    public async Task BlackPawnOnRankSevenCanDoubleMove()
    {
        Board board = CreateBoardFromFen("8/4p3/8/8/8/8/8/8 w - - 0 1");
        Square e7 = Square.FromRankAndFile(File.E, Rank.Seven);
        Square e5 = Square.FromRankAndFile(File.E, Rank.Five);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetDoubleMove(e7, board, blackPawn);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e7);
            await Assert.That(movesArray[0].Destination).IsEqualTo(e5);
        }
    }

    [Test]
    public async Task PawnNotOnStartingRankCannotDoubleMove()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/4P3/8/8 w - - 0 1");
        Square e3 = Square.FromRankAndFile(File.E, Rank.Three);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetDoubleMove(e3, board, whitePawn);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task DoubleMoveBlockedByPieceAtDestination()
    {
        Board board = CreateBoardFromFen("8/8/8/8/4p3/8/4P3/8 w - - 0 1");
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetDoubleMove(e2, board, whitePawn);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task RookOnEmptyBoardHasFullRangeOfMovement()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3R4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece whiteRook = new() { Colour = Colour.White, PieceType = PieceType.Rook };

        IEnumerable<Move> moves = InvokeGetRookBasicMoves(d4, board, whiteRook);

        Move[] movesArray = moves.ToArray();
        await Assert.That(movesArray).Count().IsEqualTo(14);
    }

    [Test]
    public async Task RookBlockedByOwnPiecesInAllDirections()
    {
        Board board = CreateBoardFromFen("8/8/8/3P4/2PRP3/3P4/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece whiteRook = new() { Colour = Colour.White, PieceType = PieceType.Rook };

        IEnumerable<Move> moves = InvokeGetRookBasicMoves(d4, board, whiteRook);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task RookCapturesEnemyPieceAndStops()
    {
        Board board = CreateBoardFromFen("8/8/8/3p4/3R4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Square d5 = Square.FromRankAndFile(File.D, Rank.Five);
        Piece whiteRook = new() { Colour = Colour.White, PieceType = PieceType.Rook };

        IEnumerable<Move> moves = InvokeGetRookBasicMoves(d4, board, whiteRook);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(11);
            await Assert.That(movesArray.Any(m => m.Destination.Equals(d5))).IsTrue();
        }
    }

    [Test]
    public async Task RookCapturesInMultipleDirections()
    {
        Board board = CreateBoardFromFen("8/8/8/3p4/1p1R1p2/8/3p4/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Square d5 = Square.FromRankAndFile(File.D, Rank.Five);
        Square b4 = Square.FromRankAndFile(File.B, Rank.Four);
        Square f4 = Square.FromRankAndFile(File.F, Rank.Four);
        Square d2 = Square.FromRankAndFile(File.D, Rank.Two);
        Piece whiteRook = new() { Colour = Colour.White, PieceType = PieceType.Rook };

        IEnumerable<Move> moves = InvokeGetRookBasicMoves(d4, board, whiteRook);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(7);
            await Assert.That(movesArray.Any(m => m.Destination.Equals(d5))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(b4))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(f4))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(d2))).IsTrue();
        }
    }

    [Test]
    public async Task RookOnBoardEdgeHasReducedMovement()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/8/R7 w - - 0 1");
        Square a1 = Square.FromRankAndFile(File.A, Rank.One);
        Piece whiteRook = new() { Colour = Colour.White, PieceType = PieceType.Rook };

        IEnumerable<Move> moves = InvokeGetRookBasicMoves(a1, board, whiteRook);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(14);
    }

    [Test]
    public async Task BlackRookMovementIsSameAsWhite()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3r4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece blackRook = new() { Colour = Colour.Black, PieceType = PieceType.Rook };

        IEnumerable<Move> moves = InvokeGetRookBasicMoves(d4, board, blackRook);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(14);
    }

    [Test]
    public async Task BishopOnEmptyBoardHasFullDiagonalRange()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3B4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece whiteBishop = new() { Colour = Colour.White, PieceType = PieceType.Bishop };

        IEnumerable<Move> moves = InvokeGetBishopBasicMoves(d4, board, whiteBishop);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(13);
    }

    [Test]
    public async Task BishopBlockedByOwnPiecesInAllDiagonals()
    {
        Board board = CreateBoardFromFen("8/8/8/2P1P3/3B4/2P1P3/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece whiteBishop = new() { Colour = Colour.White, PieceType = PieceType.Bishop };

        IEnumerable<Move> moves = InvokeGetBishopBasicMoves(d4, board, whiteBishop);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task BishopCapturesEnemyPieceAndStops()
    {
        Board board = CreateBoardFromFen("8/8/5p2/8/3B4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Square e5 = Square.FromRankAndFile(File.E, Rank.Five);
        Square f6 = Square.FromRankAndFile(File.F, Rank.Six);
        Piece whiteBishop = new() { Colour = Colour.White, PieceType = PieceType.Bishop };

        IEnumerable<Move> moves = InvokeGetBishopBasicMoves(d4, board, whiteBishop);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(11);
            await Assert.That(movesArray.Any(m => m.Destination.Equals(e5))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(f6))).IsTrue();
        }
    }

    [Test]
    public async Task BishopCapturesInMultipleDiagonals()
    {
        Board board = CreateBoardFromFen("8/8/5p2/2p5/3B4/2p5/5p2/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Square e5 = Square.FromRankAndFile(File.E, Rank.Five);
        Square f6 = Square.FromRankAndFile(File.F, Rank.Six);
        Square c5 = Square.FromRankAndFile(File.C, Rank.Five);
        Square c3 = Square.FromRankAndFile(File.C, Rank.Three);
        Square e3 = Square.FromRankAndFile(File.E, Rank.Three);
        Square f2 = Square.FromRankAndFile(File.F, Rank.Two);
        Piece whiteBishop = new() { Colour = Colour.White, PieceType = PieceType.Bishop };

        IEnumerable<Move> moves = InvokeGetBishopBasicMoves(d4, board, whiteBishop);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(6);
            await Assert.That(movesArray.Any(m => m.Destination.Equals(e5))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(f6))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(c5))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(c3))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(e3))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(f2))).IsTrue();
        }
    }

    [Test]
    public async Task BishopInCornerHasMinimalMovement()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/8/B7 w - - 0 1");
        Square a1 = Square.FromRankAndFile(File.A, Rank.One);
        Piece whiteBishop = new() { Colour = Colour.White, PieceType = PieceType.Bishop };

        IEnumerable<Move> moves = InvokeGetBishopBasicMoves(a1, board, whiteBishop);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(7);
    }

    [Test]
    public async Task BishopOnCentralSquareHasMaximumMovement()
    {
        Board board = CreateBoardFromFen("8/8/8/8/4B3/8/8/8 w - - 0 1");
        Square e4 = Square.FromRankAndFile(File.E, Rank.Four);
        Piece whiteBishop = new() { Colour = Colour.White, PieceType = PieceType.Bishop };

        IEnumerable<Move> moves = InvokeGetBishopBasicMoves(e4, board, whiteBishop);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(13);
    }

    [Test]
    public async Task BlackBishopMovementIsSameAsWhite()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3b4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece blackBishop = new() { Colour = Colour.Black, PieceType = PieceType.Bishop };

        IEnumerable<Move> moves = InvokeGetBishopBasicMoves(d4, board, blackBishop);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(13);
    }

    [Test]
    public async Task QueenOnEmptyBoardHasFullRangeInAllDirections()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3Q4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece whiteQueen = new() { Colour = Colour.White, PieceType = PieceType.Queen };

        IEnumerable<Move> moves = InvokeGetQueenBasicMoves(d4, board, whiteQueen);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(27);
    }

    [Test]
    public async Task QueenBlockedByOwnPiecesInAllDirections()
    {
        Board board = CreateBoardFromFen("8/8/5P2/2PPP3/2PQP3/2PPP3/5P2/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece whiteQueen = new() { Colour = Colour.White, PieceType = PieceType.Queen };

        IEnumerable<Move> moves = InvokeGetQueenBasicMoves(d4, board, whiteQueen);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task QueenCapturesInMixedDirections()
    {
        Board board = CreateBoardFromFen("8/8/8/2P1p3/1p1Q2p1/2p1P3/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Square e5 = Square.FromRankAndFile(File.E, Rank.Five);
        Square c3 = Square.FromRankAndFile(File.C, Rank.Three);
        Square b4 = Square.FromRankAndFile(File.B, Rank.Four);
        Square g4 = Square.FromRankAndFile(File.G, Rank.Four);
        Piece whiteQueen = new() { Colour = Colour.White, PieceType = PieceType.Queen };

        IEnumerable<Move> moves = InvokeGetQueenBasicMoves(d4, board, whiteQueen);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray.Any(m => m.Destination.Equals(e5))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(c3))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(b4))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(g4))).IsTrue();
        }
    }

    [Test]
    public async Task QueenInCornerHasThreeDirections()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/8/Q7 w - - 0 1");
        Square a1 = Square.FromRankAndFile(File.A, Rank.One);
        Piece whiteQueen = new() { Colour = Colour.White, PieceType = PieceType.Queen };

        IEnumerable<Move> moves = InvokeGetQueenBasicMoves(a1, board, whiteQueen);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(21);
    }

    [Test]
    public async Task QueenOnCentralSquareHasMaximumMovement()
    {
        Board board = CreateBoardFromFen("8/8/8/8/4Q3/8/8/8 w - - 0 1");
        Square e4 = Square.FromRankAndFile(File.E, Rank.Four);
        Piece whiteQueen = new() { Colour = Colour.White, PieceType = PieceType.Queen };

        IEnumerable<Move> moves = InvokeGetQueenBasicMoves(e4, board, whiteQueen);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(27);
    }

    [Test]
    public async Task QueenWithPartialBlockingInSomeDirections()
    {
        Board board = CreateBoardFromFen("8/3P4/8/8/3Q4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Square d7 = Square.FromRankAndFile(File.D, Rank.Seven);
        Piece whiteQueen = new() { Colour = Colour.White, PieceType = PieceType.Queen };

        IEnumerable<Move> moves = InvokeGetQueenBasicMoves(d4, board, whiteQueen);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray.Any(m => m.Destination.Equals(d7))).IsFalse();
            await Assert.That(movesArray).Count().IsEqualTo(25);
        }
    }

    [Test]
    public async Task QueenCapturesAtDifferentRanges()
    {
        Board board = CreateBoardFromFen("3p4/8/8/8/p2Q3p/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Square d8 = Square.FromRankAndFile(File.D, Rank.Eight);
        Square a4 = Square.FromRankAndFile(File.A, Rank.Four);
        Square h4 = Square.FromRankAndFile(File.H, Rank.Four);
        Piece whiteQueen = new() { Colour = Colour.White, PieceType = PieceType.Queen };

        IEnumerable<Move> moves = InvokeGetQueenBasicMoves(d4, board, whiteQueen);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray.Any(m => m.Destination.Equals(d8))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(a4))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(h4))).IsTrue();
        }
    }

    [Test]
    public async Task BlackQueenMovementIsSameAsWhite()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3q4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece blackQueen = new() { Colour = Colour.Black, PieceType = PieceType.Queen };

        IEnumerable<Move> moves = InvokeGetQueenBasicMoves(d4, board, blackQueen);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(27);
    }

    [Test]
    public async Task KnightOnEmptyBoardHasAllEightMoves()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3N4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece whiteKnight = new() { Colour = Colour.White, PieceType = PieceType.Knight };

        IEnumerable<Move> moves = InvokeGetKnightBasicMoves(d4, board, whiteKnight);

        Move[] movesArray = moves.ToArray();
        await Assert.That(movesArray).Count().IsEqualTo(8);
    }

    [Test]
    public async Task KnightInCornerHasMinimalMoves()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/8/N7 w - - 0 1");
        Square a1 = Square.FromRankAndFile(File.A, Rank.One);
        Square b3 = Square.FromRankAndFile(File.B, Rank.Three);
        Square c2 = Square.FromRankAndFile(File.C, Rank.Two);
        Piece whiteKnight = new() { Colour = Colour.White, PieceType = PieceType.Knight };

        IEnumerable<Move> moves = InvokeGetKnightBasicMoves(a1, board, whiteKnight);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(2);
            await Assert.That(movesArray.Any(m => m.Destination.Equals(b3))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(c2))).IsTrue();
        }
    }

    [Test]
    public async Task KnightOnEdgeHasReducedMoves()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/8/3N4 w - - 0 1");
        Square d1 = Square.FromRankAndFile(File.D, Rank.One);
        Piece whiteKnight = new() { Colour = Colour.White, PieceType = PieceType.Knight };

        IEnumerable<Move> moves = InvokeGetKnightBasicMoves(d1, board, whiteKnight);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(4);
    }

    [Test]
    public async Task KnightBlockedByOwnPiecesInAllDirections()
    {
        Board board = CreateBoardFromFen("8/8/2P1P3/1P3P2/3N4/1P3P2/2P1P3/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece whiteKnight = new() { Colour = Colour.White, PieceType = PieceType.Knight };

        IEnumerable<Move> moves = InvokeGetKnightBasicMoves(d4, board, whiteKnight);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task KnightCapturesEnemyPiece()
    {
        Board board = CreateBoardFromFen("8/8/4p3/8/3N4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Square e6 = Square.FromRankAndFile(File.E, Rank.Six);
        Piece whiteKnight = new() { Colour = Colour.White, PieceType = PieceType.Knight };

        IEnumerable<Move> moves = InvokeGetKnightBasicMoves(d4, board, whiteKnight);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(8);
            await Assert.That(movesArray.Any(m => m.Destination.Equals(e6))).IsTrue();
        }
    }

    [Test]
    public async Task KnightWithMixedPiecesAround()
    {
        Board board = CreateBoardFromFen("8/8/2p1p3/1P3p2/3N4/1p3P2/2P1p3/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Square c6 = Square.FromRankAndFile(File.C, Rank.Six);
        Square e6 = Square.FromRankAndFile(File.E, Rank.Six);
        Square f5 = Square.FromRankAndFile(File.F, Rank.Five);
        Square b3 = Square.FromRankAndFile(File.B, Rank.Three);
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Piece whiteKnight = new() { Colour = Colour.White, PieceType = PieceType.Knight };

        IEnumerable<Move> moves = InvokeGetKnightBasicMoves(d4, board, whiteKnight);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(5);
            await Assert.That(movesArray.Any(m => m.Destination.Equals(c6))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(e6))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(f5))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(b3))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(e2))).IsTrue();
        }
    }

    [Test]
    public async Task BlackKnightMovementIsSameAsWhite()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3n4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece blackKnight = new() { Colour = Colour.Black, PieceType = PieceType.Knight };

        IEnumerable<Move> moves = InvokeGetKnightBasicMoves(d4, board, blackKnight);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(8);
    }

    [Test]
    public async Task KingOnEmptyBoardHasAllEightMoves()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3K4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece whiteKing = new() { Colour = Colour.White, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetKingBasicMoves(d4, board, whiteKing);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(8);
    }

    [Test]
    public async Task KingInCornerHasThreeMoves()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/8/K7 w - - 0 1");
        Square a1 = Square.FromRankAndFile(File.A, Rank.One);
        Square a2 = Square.FromRankAndFile(File.A, Rank.Two);
        Square b1 = Square.FromRankAndFile(File.B, Rank.One);
        Square b2 = Square.FromRankAndFile(File.B, Rank.Two);
        Piece whiteKing = new() { Colour = Colour.White, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetKingBasicMoves(a1, board, whiteKing);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(3);
            await Assert.That(movesArray.Any(m => m.Destination.Equals(a2))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(b1))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(b2))).IsTrue();
        }
    }

    [Test]
    public async Task KingOnEdgeHasFiveMoves()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/8/3K4 w - - 0 1");
        Square d1 = Square.FromRankAndFile(File.D, Rank.One);
        Piece whiteKing = new() { Colour = Colour.White, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetKingBasicMoves(d1, board, whiteKing);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(5);
    }

    [Test]
    public async Task KingBlockedByOwnPiecesInAllDirections()
    {
        Board board = CreateBoardFromFen("8/8/8/8/2PPP3/2PKP3/2PPP3/8 w - - 0 1");
        Square d3 = Square.FromRankAndFile(File.D, Rank.Three);
        Piece whiteKing = new() { Colour = Colour.White, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetKingBasicMoves(d3, board, whiteKing);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task KingCapturesEnemyPiece()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3p4/3K4/8/8 w - - 0 1");
        Square d3 = Square.FromRankAndFile(File.D, Rank.Three);
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece whiteKing = new() { Colour = Colour.White, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetKingBasicMoves(d3, board, whiteKing);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(8);
            await Assert.That(movesArray.Any(m => m.Destination.Equals(d4))).IsTrue();
        }
    }

    [Test]
    public async Task KingWithMixedPiecesAround()
    {
        Board board = CreateBoardFromFen("8/8/8/8/2pPp3/2pKP3/2ppp3/8 w - - 0 1");
        Square d3 = Square.FromRankAndFile(File.D, Rank.Three);
        Square c4 = Square.FromRankAndFile(File.C, Rank.Four);
        Square c3 = Square.FromRankAndFile(File.C, Rank.Three);
        Square c2 = Square.FromRankAndFile(File.C, Rank.Two);
        Square d2 = Square.FromRankAndFile(File.D, Rank.Two);
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Square e4 = Square.FromRankAndFile(File.E, Rank.Four);
        Piece whiteKing = new() { Colour = Colour.White, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetKingBasicMoves(d3, board, whiteKing);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(6);
            await Assert.That(movesArray.Any(m => m.Destination.Equals(c4))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(c3))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(c2))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(d2))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(e2))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Destination.Equals(e4))).IsTrue();
        }
    }

    [Test]
    public async Task BlackKingMovementIsSameAsWhite()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3k4/8/8/8 w - - 0 1");
        Square d4 = Square.FromRankAndFile(File.D, Rank.Four);
        Piece blackKing = new() { Colour = Colour.Black, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetKingBasicMoves(d4, board, blackKing);

        await Assert.That(moves.ToArray()).Count().IsEqualTo(8);
    }

    [Test]
    public async Task EnPassantWithNoTargetReturnsNoMoves()
    {
        Board board = CreateBoardFromFen("8/8/8/4P3/8/8/8/8 w - - 0 1");
        Square e5 = Square.FromRankAndFile(File.E, Rank.Five);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetEnPassantMoves(e5, whitePawn, null);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task WhitePawnCanCaptureEnPassantOnLeft()
    {
        Board board = CreateBoardFromFen("8/8/8/4Pp2/8/8/8/8 w - f6 0 1");
        Square e5 = Square.FromRankAndFile(File.E, Rank.Five);
        Square f6 = Square.FromRankAndFile(File.F, Rank.Six);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetEnPassantMoves(e5, whitePawn, f6);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e5);
            await Assert.That(movesArray[0].Destination).IsEqualTo(f6);
        }
    }

    [Test]
    public async Task WhitePawnCanCaptureEnPassantOnRight()
    {
        Board board = CreateBoardFromFen("8/8/8/3pP3/8/8/8/8 w - d6 0 1");
        Square e5 = Square.FromRankAndFile(File.E, Rank.Five);
        Square d6 = Square.FromRankAndFile(File.D, Rank.Six);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetEnPassantMoves(e5, whitePawn, d6);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e5);
            await Assert.That(movesArray[0].Destination).IsEqualTo(d6);
        }
    }

    [Test]
    public async Task BlackPawnCanCaptureEnPassantOnLeft()
    {
        Board board = CreateBoardFromFen("8/8/8/8/4pP2/8/8/8 w - f3 0 1");
        Square e4 = Square.FromRankAndFile(File.E, Rank.Four);
        Square f3 = Square.FromRankAndFile(File.F, Rank.Three);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetEnPassantMoves(e4, blackPawn, f3);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e4);
            await Assert.That(movesArray[0].Destination).IsEqualTo(f3);
        }
    }

    [Test]
    public async Task BlackPawnCanCaptureEnPassantOnRight()
    {
        Board board = CreateBoardFromFen("8/8/8/8/3Pp3/8/8/8 w - d3 0 1");
        Square e4 = Square.FromRankAndFile(File.E, Rank.Four);
        Square d3 = Square.FromRankAndFile(File.D, Rank.Three);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetEnPassantMoves(e4, blackPawn, d3);

        Move[] movesArray = moves.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e4);
            await Assert.That(movesArray[0].Destination).IsEqualTo(d3);
        }
    }

    [Test]
    public async Task PawnCannotCaptureEnPassantWhenTargetNotDiagonallyAdjacent()
    {
        Board board = CreateBoardFromFen("8/8/8/4P3/8/8/8/8 w - b6 0 1");
        Square e5 = Square.FromRankAndFile(File.E, Rank.Five);
        Square b6 = Square.FromRankAndFile(File.B, Rank.Six);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetEnPassantMoves(e5, whitePawn, b6);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task PawnOnWrongRankCannotCaptureEnPassant()
    {
        Board board = CreateBoardFromFen("8/8/8/8/4P3/8/8/8 w - d6 0 1");
        Square e4 = Square.FromRankAndFile(File.E, Rank.Four);
        Square d6 = Square.FromRankAndFile(File.D, Rank.Six);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetEnPassantMoves(e4, whitePawn, d6);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task WhiteKingCanCastleKingsideWhenRightsAvailable()
    {
        CastlingRights rights = CreateCastlingRightsFromFen("8/8/8/8/8/8/8/8 w K - 0 1");
        Square e1 = Square.FromRankAndFile(File.E, Rank.One);
        Square g1 = Square.FromRankAndFile(File.G, Rank.One);
        Piece whiteKing = new() { Colour = Colour.White, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetCastlingMoves(e1, whiteKing, rights);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e1);
            await Assert.That(movesArray[0].Destination).IsEqualTo(g1);
        }
    }

    [Test]
    public async Task WhiteKingCanCastleQueensideWhenRightsAvailable()
    {
        CastlingRights rights = CreateCastlingRightsFromFen("8/8/8/8/8/8/8/8 w Q - 0 1");
        Square e1 = Square.FromRankAndFile(File.E, Rank.One);
        Square c1 = Square.FromRankAndFile(File.C, Rank.One);
        Piece whiteKing = new() { Colour = Colour.White, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetCastlingMoves(e1, whiteKing, rights);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e1);
            await Assert.That(movesArray[0].Destination).IsEqualTo(c1);
        }
    }

    [Test]
    public async Task BlackKingCanCastleKingsideWhenRightsAvailable()
    {
        CastlingRights rights = CreateCastlingRightsFromFen("8/8/8/8/8/8/8/8 w k - 0 1");
        Square e8 = Square.FromRankAndFile(File.E, Rank.Eight);
        Square g8 = Square.FromRankAndFile(File.G, Rank.Eight);
        Piece blackKing = new() { Colour = Colour.Black, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetCastlingMoves(e8, blackKing, rights);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e8);
            await Assert.That(movesArray[0].Destination).IsEqualTo(g8);
        }
    }

    [Test]
    public async Task BlackKingCanCastleQueensideWhenRightsAvailable()
    {
        CastlingRights rights = CreateCastlingRightsFromFen("8/8/8/8/8/8/8/8 w q - 0 1");
        Square e8 = Square.FromRankAndFile(File.E, Rank.Eight);
        Square c8 = Square.FromRankAndFile(File.C, Rank.Eight);
        Piece blackKing = new() { Colour = Colour.Black, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetCastlingMoves(e8, blackKing, rights);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(1);
            await Assert.That(movesArray[0].Source).IsEqualTo(e8);
            await Assert.That(movesArray[0].Destination).IsEqualTo(c8);
        }
    }

    [Test]
    public async Task KingCannotCastleWhenNoCastlingRightsAvailable()
    {
        CastlingRights rights = CreateCastlingRightsFromFen("8/8/8/8/8/8/8/8 w - - 0 1");
        Square e1 = Square.FromRankAndFile(File.E, Rank.One);
        Piece whiteKing = new() { Colour = Colour.White, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetCastlingMoves(e1, whiteKing, rights);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task KingCannotCastleWhenOnlyOpponentHasCastlingRights()
    {
        CastlingRights rights = CreateCastlingRightsFromFen("8/8/8/8/8/8/8/8 w kq - 0 1");
        Square e1 = Square.FromRankAndFile(File.E, Rank.One);
        Piece whiteKing = new() { Colour = Colour.White, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetCastlingMoves(e1, whiteKing, rights);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task KingWithBothCastlingRightsReturnsBothMoves()
    {
        CastlingRights rights = CreateCastlingRightsFromFen("8/8/8/8/8/8/8/8 w KQ - 0 1");
        Square e1 = Square.FromRankAndFile(File.E, Rank.One);
        Square g1 = Square.FromRankAndFile(File.G, Rank.One);
        Square c1 = Square.FromRankAndFile(File.C, Rank.One);
        Piece whiteKing = new() { Colour = Colour.White, PieceType = PieceType.King };

        IEnumerable<Move> moves = InvokeGetCastlingMoves(e1, whiteKing, rights);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(2);
            await Assert.That(movesArray.Any(m => m.Source.Equals(e1) && m.Destination.Equals(g1))).IsTrue();
            await Assert.That(movesArray.Any(m => m.Source.Equals(e1) && m.Destination.Equals(c1))).IsTrue();
        }
    }

    [Test]
    public async Task WhitePawnOnRank7PromotesWithForwardMove()
    {
        Board board = CreateBoardFromFen("8/4P3/8/8/8/8/8/8 w - - 0 1");
        Square e7 = Square.FromRankAndFile(File.E, Rank.Seven);
        Square e8 = Square.FromRankAndFile(File.E, Rank.Eight);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e7, board, whitePawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(4);
            await Assert.That(movesArray.All(m => m.Source.Equals(e7) && m.Destination.Equals(e8))).IsTrue();
            await Assert.That(movesArray.Any(m => m.PromotionPieceType == PieceType.Queen)).IsTrue();
            await Assert.That(movesArray.Any(m => m.PromotionPieceType == PieceType.Rook)).IsTrue();
            await Assert.That(movesArray.Any(m => m.PromotionPieceType == PieceType.Bishop)).IsTrue();
            await Assert.That(movesArray.Any(m => m.PromotionPieceType == PieceType.Knight)).IsTrue();
        }
    }

    [Test]
    public async Task WhitePawnOnRank7PromotesWithLeftCapture()
    {
        Board board = CreateBoardFromFen("3rR3/4P3/8/8/8/8/8/8 w - - 0 1");
        Square e7 = Square.FromRankAndFile(File.E, Rank.Seven);
        Square d8 = Square.FromRankAndFile(File.D, Rank.Eight);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e7, board, whitePawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(4);
            await Assert.That(movesArray.All(m => m.Source.Equals(e7) && m.Destination.Equals(d8))).IsTrue();
        }
    }

    [Test]
    public async Task WhitePawnOnRank7PromotesWithRightCapture()
    {
        Board board = CreateBoardFromFen("4Rr2/4P3/8/8/8/8/8/8 w - - 0 1");
        Square e7 = Square.FromRankAndFile(File.E, Rank.Seven);
        Square f8 = Square.FromRankAndFile(File.F, Rank.Eight);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e7, board, whitePawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(4);
            await Assert.That(movesArray.All(m => m.Source.Equals(e7) && m.Destination.Equals(f8))).IsTrue();
        }
    }

    [Test]
    public async Task WhitePawnOnRank7PromotesWithBothCaptures()
    {
        Board board = CreateBoardFromFen("3r1r2/4P3/8/8/8/8/8/8 w - - 0 1");
        Square e7 = Square.FromRankAndFile(File.E, Rank.Seven);
        Square d8 = Square.FromRankAndFile(File.D, Rank.Eight);
        Square e8 = Square.FromRankAndFile(File.E, Rank.Eight);
        Square f8 = Square.FromRankAndFile(File.F, Rank.Eight);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e7, board, whitePawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(12);
            await Assert.That(movesArray.Count(m => m.Destination.Equals(e8))).IsEqualTo(4);
            await Assert.That(movesArray.Count(m => m.Destination.Equals(d8))).IsEqualTo(4);
            await Assert.That(movesArray.Count(m => m.Destination.Equals(f8))).IsEqualTo(4);
        }
    }

    [Test]
    public async Task WhitePawnOnRank7BlockedCannotPromote()
    {
        Board board = CreateBoardFromFen("4R3/4P3/8/8/8/8/8/8 w - - 0 1");
        Square e7 = Square.FromRankAndFile(File.E, Rank.Seven);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e7, board, whitePawn);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task WhitePawnOnRank7CaptureBlockedByOwnPiece()
    {
        Board board = CreateBoardFromFen("3R1R2/4P3/8/8/8/8/8/8 w - - 0 1");
        Square e7 = Square.FromRankAndFile(File.E, Rank.Seven);
        Square e8 = Square.FromRankAndFile(File.E, Rank.Eight);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e7, board, whitePawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(4);
            await Assert.That(movesArray.All(m => m.Destination.Equals(e8))).IsTrue();
        }
    }

    [Test]
    public async Task BlackPawnOnRank2PromotesWithForwardMove()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/4p3/8 w - - 0 1");
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Square e1 = Square.FromRankAndFile(File.E, Rank.One);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e2, board, blackPawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(4);
            await Assert.That(movesArray.All(m => m.Source.Equals(e2) && m.Destination.Equals(e1))).IsTrue();
            await Assert.That(movesArray.Any(m => m.PromotionPieceType == PieceType.Queen)).IsTrue();
            await Assert.That(movesArray.Any(m => m.PromotionPieceType == PieceType.Rook)).IsTrue();
            await Assert.That(movesArray.Any(m => m.PromotionPieceType == PieceType.Bishop)).IsTrue();
            await Assert.That(movesArray.Any(m => m.PromotionPieceType == PieceType.Knight)).IsTrue();
        }
    }

    [Test]
    public async Task BlackPawnOnRank2PromotesWithLeftCapture()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/4p3/3Rr3 w - - 0 1");
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Square d1 = Square.FromRankAndFile(File.D, Rank.One);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e2, board, blackPawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(4);
            await Assert.That(movesArray.All(m => m.Source.Equals(e2) && m.Destination.Equals(d1))).IsTrue();
        }
    }

    [Test]
    public async Task BlackPawnOnRank2PromotesWithRightCapture()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/4p3/4rR2 w - - 0 1");
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Square f1 = Square.FromRankAndFile(File.F, Rank.One);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e2, board, blackPawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(4);
            await Assert.That(movesArray.All(m => m.Source.Equals(e2) && m.Destination.Equals(f1))).IsTrue();
        }
    }

    [Test]
    public async Task BlackPawnOnRank2PromotesWithBothCaptures()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/4p3/3R1R2 w - - 0 1");
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Square d1 = Square.FromRankAndFile(File.D, Rank.One);
        Square e1 = Square.FromRankAndFile(File.E, Rank.One);
        Square f1 = Square.FromRankAndFile(File.F, Rank.One);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e2, board, blackPawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(12);
            await Assert.That(movesArray.Count(m => m.Destination.Equals(e1))).IsEqualTo(4);
            await Assert.That(movesArray.Count(m => m.Destination.Equals(d1))).IsEqualTo(4);
            await Assert.That(movesArray.Count(m => m.Destination.Equals(f1))).IsEqualTo(4);
        }
    }

    [Test]
    public async Task BlackPawnOnRank2BlockedCannotPromote()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/4p3/4r3 w - - 0 1");
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e2, board, blackPawn);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task BlackPawnOnRank2CaptureBlockedByOwnPiece()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/4p3/3r1r2 w - - 0 1");
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Square e1 = Square.FromRankAndFile(File.E, Rank.One);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e2, board, blackPawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(4);
            await Assert.That(movesArray.All(m => m.Destination.Equals(e1))).IsTrue();
        }
    }

    [Test]
    public async Task WhitePawnOnCornerRank7HasLimitedPromotionOptions()
    {
        Board board = CreateBoardFromFen("1r6/P7/8/8/8/8/8/8 w - - 0 1");
        Square a7 = Square.FromRankAndFile(File.A, Rank.Seven);
        Square a8 = Square.FromRankAndFile(File.A, Rank.Eight);
        Square b8 = Square.FromRankAndFile(File.B, Rank.Eight);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(a7, board, whitePawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(8);
            await Assert.That(movesArray.Count(m => m.Destination.Equals(a8))).IsEqualTo(4);
            await Assert.That(movesArray.Count(m => m.Destination.Equals(b8))).IsEqualTo(4);
        }
    }

    [Test]
    public async Task BlackPawnOnCornerRank2HasLimitedPromotionOptions()
    {
        Board board = CreateBoardFromFen("8/8/8/8/8/8/p7/1R6 w - - 0 1");
        Square a2 = Square.FromRankAndFile(File.A, Rank.Two);
        Square a1 = Square.FromRankAndFile(File.A, Rank.One);
        Square b1 = Square.FromRankAndFile(File.B, Rank.One);
        Piece blackPawn = new() { Colour = Colour.Black, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(a2, board, blackPawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(8);
            await Assert.That(movesArray.Count(m => m.Destination.Equals(a1))).IsEqualTo(4);
            await Assert.That(movesArray.Count(m => m.Destination.Equals(b1))).IsEqualTo(4);
        }
    }

    [Test]
    public async Task PawnOnWrongRankDoesNotPromote()
    {
        Board board = CreateBoardFromFen("8/8/8/4P3/8/8/8/8 w - - 0 1");
        Square e5 = Square.FromRankAndFile(File.E, Rank.Five);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e5, board, whitePawn);

        await Assert.That(moves.ToArray()).IsEmpty();
    }

    [Test]
    public async Task PawnOnRank7CannotCaptureOwnPiece()
    {
        Board board = CreateBoardFromFen("3R4/4P3/8/8/8/8/8/8 w - - 0 1");
        Square e7 = Square.FromRankAndFile(File.E, Rank.Seven);
        Square e8 = Square.FromRankAndFile(File.E, Rank.Eight);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e7, board, whitePawn);
        Move[] movesArray = moves.ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(4);
            await Assert.That(movesArray.All(m => m.Destination.Equals(e8))).IsTrue();
        }
    }

    [Test]
    public async Task PromotionMovesHaveCorrectPromotionPieceTypeSet()
    {
        Board board = CreateBoardFromFen("8/4P3/8/8/8/8/8/8 w - - 0 1");
        Square e7 = Square.FromRankAndFile(File.E, Rank.Seven);
        Piece whitePawn = new() { Colour = Colour.White, PieceType = PieceType.Pawn };

        IEnumerable<Move> moves = InvokeGetPromotionMoves(e7, board, whitePawn);
        Move[] movesArray = moves.ToArray();

        PieceType[] expectedPromotionTypes = [PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight];

        using (Assert.Multiple())
        {
            await Assert.That(movesArray).Count().IsEqualTo(4);
            await Assert.That(movesArray.All(m => m.PromotionPieceType.HasValue)).IsTrue();
            await Assert.That(movesArray.All(m => expectedPromotionTypes.Contains(m.PromotionPieceType!.Value))).IsTrue();
            await Assert.That(movesArray.Select(m => m.PromotionPieceType).Distinct()).Count().IsEqualTo(4);
        }
    }
}

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
}

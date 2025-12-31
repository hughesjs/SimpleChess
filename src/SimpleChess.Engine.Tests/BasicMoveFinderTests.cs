using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SimpleChess.State;

namespace SimpleChess.Engine.Tests;

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
            .GetMethod("GetDoubleMove", BindingFlags.NonPublic | BindingFlags.Static);

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
}
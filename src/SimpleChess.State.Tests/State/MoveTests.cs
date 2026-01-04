using System;
using System.Threading.Tasks;

namespace SimpleChess.State.Tests.State;

public class MoveTests
{
    [Test]
    [Arguments(File.E, Rank.Two, File.E, Rank.Four)]
    [Arguments(File.A, Rank.One, File.H, Rank.Eight)]
    [Arguments(File.H, Rank.Eight, File.A, Rank.One)]
    [Arguments(File.D, Rank.Four, File.D, Rank.Five)]
    public async Task NormalMovePacksAndUnpacksCorrectly(File sourceFile, Rank sourceRank, File destFile, Rank destRank)
    {
        Square source = Square.FromRankAndFile(sourceFile, sourceRank);
        Square destination = Square.FromRankAndFile(destFile, destRank);

        Move move = Move.Normal(source, destination);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source.File).IsEqualTo(sourceFile);
            await Assert.That(move.Source.Rank).IsEqualTo(sourceRank);
            await Assert.That(move.Destination.File).IsEqualTo(destFile);
            await Assert.That(move.Destination.Rank).IsEqualTo(destRank);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.Normal);
        }
    }

    [Test]
    public async Task NormalMoveAllCornerSquares()
    {
        Square a1 = Square.FromRankAndFile(File.A, Rank.One);
        Square h1 = Square.FromRankAndFile(File.H, Rank.One);
        Square a8 = Square.FromRankAndFile(File.A, Rank.Eight);
        Square h8 = Square.FromRankAndFile(File.H, Rank.Eight);

        Move move1 = Move.Normal(a1, h8);
        Move move2 = Move.Normal(h8, a1);
        Move move3 = Move.Normal(a8, h1);
        Move move4 = Move.Normal(h1, a8);

        using (Assert.Multiple())
        {
            await Assert.That(move1.Source).IsEqualTo(a1);
            await Assert.That(move1.Destination).IsEqualTo(h8);
            await Assert.That(move2.Source).IsEqualTo(h8);
            await Assert.That(move2.Destination).IsEqualTo(a1);
            await Assert.That(move3.Source).IsEqualTo(a8);
            await Assert.That(move3.Destination).IsEqualTo(h1);
            await Assert.That(move4.Source).IsEqualTo(h1);
            await Assert.That(move4.Destination).IsEqualTo(a8);
        }
    }

    [Test]
    [Arguments(File.E, Rank.Two, File.E, Rank.Four)]
    [Arguments(File.A, Rank.Two, File.A, Rank.Four)]
    [Arguments(File.H, Rank.Two, File.H, Rank.Four)]
    public async Task PawnDoubleMovePacksAndUnpacksCorrectly(File sourceFile, Rank sourceRank, File destFile, Rank destRank)
    {
        Square source = Square.FromRankAndFile(sourceFile, sourceRank);
        Square destination = Square.FromRankAndFile(destFile, destRank);

        Move move = Move.PawnDouble(source, destination);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source.File).IsEqualTo(sourceFile);
            await Assert.That(move.Source.Rank).IsEqualTo(sourceRank);
            await Assert.That(move.Destination.File).IsEqualTo(destFile);
            await Assert.That(move.Destination.Rank).IsEqualTo(destRank);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.PawnDouble);
        }
    }

    [Test]
    [Arguments(File.E, Rank.Seven, File.E, Rank.Five)]
    [Arguments(File.D, Rank.Seven, File.D, Rank.Five)]
    [Arguments(File.F, Rank.Seven, File.F, Rank.Five)]
    public async Task BlackPawnDoubleMovePacksAndUnpacksCorrectly(File sourceFile, Rank sourceRank, File destFile, Rank destRank)
    {
        Square source = Square.FromRankAndFile(sourceFile, sourceRank);
        Square destination = Square.FromRankAndFile(destFile, destRank);

        Move move = Move.PawnDouble(source, destination);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source.File).IsEqualTo(sourceFile);
            await Assert.That(move.Source.Rank).IsEqualTo(sourceRank);
            await Assert.That(move.Destination.File).IsEqualTo(destFile);
            await Assert.That(move.Destination.Rank).IsEqualTo(destRank);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.PawnDouble);
        }
    }

    [Test]
    public async Task PawnDoubleMovePreservesSourceAndDestination()
    {
        Square e2 = Square.FromRankAndFile(File.E, Rank.Two);
        Square e4 = Square.FromRankAndFile(File.E, Rank.Four);
        Square d7 = Square.FromRankAndFile(File.D, Rank.Seven);
        Square d5 = Square.FromRankAndFile(File.D, Rank.Five);

        Move whiteMove = Move.PawnDouble(e2, e4);
        Move blackMove = Move.PawnDouble(d7, d5);

        using (Assert.Multiple())
        {
            await Assert.That(whiteMove.Source).IsEqualTo(e2);
            await Assert.That(whiteMove.Destination).IsEqualTo(e4);
            await Assert.That(whiteMove.MoveType).IsEqualTo(MoveType.PawnDouble);
            await Assert.That(blackMove.Source).IsEqualTo(d7);
            await Assert.That(blackMove.Destination).IsEqualTo(d5);
            await Assert.That(blackMove.MoveType).IsEqualTo(MoveType.PawnDouble);
        }
    }

    [Test]
    public async Task WhiteKingsideCastlingPacksAndUnpacksCorrectly()
    {
        Square kingSource = Square.FromRankAndFile(File.E, Rank.One);
        Square kingDest = Square.FromRankAndFile(File.G, Rank.One);
        Square rookSource = Square.FromRankAndFile(File.H, Rank.One);
        Square rookDest = Square.FromRankAndFile(File.F, Rank.One);

        Move move = Move.Castling(kingSource, kingDest, rookSource, rookDest);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source).IsEqualTo(kingSource);
            await Assert.That(move.Destination).IsEqualTo(kingDest);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.Castling);
            await Assert.That(move.GetRookSource()).IsEqualTo(rookSource);
            await Assert.That(move.GetRookDestination()).IsEqualTo(rookDest);
        }
    }

    [Test]
    public async Task WhiteQueensideCastlingPacksAndUnpacksCorrectly()
    {
        Square kingSource = Square.FromRankAndFile(File.E, Rank.One);
        Square kingDest = Square.FromRankAndFile(File.C, Rank.One);
        Square rookSource = Square.FromRankAndFile(File.A, Rank.One);
        Square rookDest = Square.FromRankAndFile(File.D, Rank.One);

        Move move = Move.Castling(kingSource, kingDest, rookSource, rookDest);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source).IsEqualTo(kingSource);
            await Assert.That(move.Destination).IsEqualTo(kingDest);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.Castling);
            await Assert.That(move.GetRookSource()).IsEqualTo(rookSource);
            await Assert.That(move.GetRookDestination()).IsEqualTo(rookDest);
        }
    }

    [Test]
    public async Task BlackKingsideCastlingPacksAndUnpacksCorrectly()
    {
        Square kingSource = Square.FromRankAndFile(File.E, Rank.Eight);
        Square kingDest = Square.FromRankAndFile(File.G, Rank.Eight);
        Square rookSource = Square.FromRankAndFile(File.H, Rank.Eight);
        Square rookDest = Square.FromRankAndFile(File.F, Rank.Eight);

        Move move = Move.Castling(kingSource, kingDest, rookSource, rookDest);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source).IsEqualTo(kingSource);
            await Assert.That(move.Destination).IsEqualTo(kingDest);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.Castling);
            await Assert.That(move.GetRookSource()).IsEqualTo(rookSource);
            await Assert.That(move.GetRookDestination()).IsEqualTo(rookDest);
        }
    }

    [Test]
    public async Task BlackQueensideCastlingPacksAndUnpacksCorrectly()
    {
        Square kingSource = Square.FromRankAndFile(File.E, Rank.Eight);
        Square kingDest = Square.FromRankAndFile(File.C, Rank.Eight);
        Square rookSource = Square.FromRankAndFile(File.A, Rank.Eight);
        Square rookDest = Square.FromRankAndFile(File.D, Rank.Eight);

        Move move = Move.Castling(kingSource, kingDest, rookSource, rookDest);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source).IsEqualTo(kingSource);
            await Assert.That(move.Destination).IsEqualTo(kingDest);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.Castling);
            await Assert.That(move.GetRookSource()).IsEqualTo(rookSource);
            await Assert.That(move.GetRookDestination()).IsEqualTo(rookDest);
        }
    }

    [Test]
    [Arguments(PieceType.Queen)]
    [Arguments(PieceType.Rook)]
    [Arguments(PieceType.Bishop)]
    [Arguments(PieceType.Knight)]
    public async Task WhitePawnPromotionPacksAndUnpacksCorrectly(PieceType promotionPieceType)
    {
        Square source = Square.FromRankAndFile(File.E, Rank.Seven);
        Square destination = Square.FromRankAndFile(File.E, Rank.Eight);

        Move move = Move.Promotion(source, destination, promotionPieceType);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source).IsEqualTo(source);
            await Assert.That(move.Destination).IsEqualTo(destination);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.Promotion);
            await Assert.That(move.GetPromotionPieceType()).IsEqualTo(promotionPieceType);
        }
    }

    [Test]
    [Arguments(PieceType.Queen)]
    [Arguments(PieceType.Rook)]
    [Arguments(PieceType.Bishop)]
    [Arguments(PieceType.Knight)]
    public async Task BlackPawnPromotionPacksAndUnpacksCorrectly(PieceType promotionPieceType)
    {
        Square source = Square.FromRankAndFile(File.D, Rank.Two);
        Square destination = Square.FromRankAndFile(File.D, Rank.One);

        Move move = Move.Promotion(source, destination, promotionPieceType);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source).IsEqualTo(source);
            await Assert.That(move.Destination).IsEqualTo(destination);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.Promotion);
            await Assert.That(move.GetPromotionPieceType()).IsEqualTo(promotionPieceType);
        }
    }

    [Test]
    public async Task PromotionWithCapturePacksCorrectly()
    {
        Square source = Square.FromRankAndFile(File.E, Rank.Seven);
        Square destination = Square.FromRankAndFile(File.D, Rank.Eight);

        Move move = Move.Promotion(source, destination, PieceType.Queen);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source).IsEqualTo(source);
            await Assert.That(move.Destination).IsEqualTo(destination);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.Promotion);
            await Assert.That(move.GetPromotionPieceType()).IsEqualTo(PieceType.Queen);
        }
    }

    [Test]
    public async Task WhitePawnEnPassantPacksAndUnpacksCorrectly()
    {
        Square source = Square.FromRankAndFile(File.E, Rank.Five);
        Square destination = Square.FromRankAndFile(File.D, Rank.Six);
        Square enPassantTarget = Square.FromRankAndFile(File.D, Rank.Five);

        Move move = Move.EnPassant(source, destination, enPassantTarget);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source).IsEqualTo(source);
            await Assert.That(move.Destination).IsEqualTo(destination);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.EnPassant);
            await Assert.That(move.GetEnPassantTarget()).IsEqualTo(enPassantTarget);
        }
    }

    [Test]
    public async Task BlackPawnEnPassantPacksAndUnpacksCorrectly()
    {
        Square source = Square.FromRankAndFile(File.D, Rank.Four);
        Square destination = Square.FromRankAndFile(File.E, Rank.Three);
        Square enPassantTarget = Square.FromRankAndFile(File.E, Rank.Four);

        Move move = Move.EnPassant(source, destination, enPassantTarget);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source).IsEqualTo(source);
            await Assert.That(move.Destination).IsEqualTo(destination);
            await Assert.That(move.MoveType).IsEqualTo(MoveType.EnPassant);
            await Assert.That(move.GetEnPassantTarget()).IsEqualTo(enPassantTarget);
        }
    }

    [Test]
    [Arguments(File.A, File.B)]
    [Arguments(File.B, File.A)]
    [Arguments(File.G, File.H)]
    [Arguments(File.H, File.G)]
    public async Task EnPassantEdgeFilesPackCorrectly(File sourceFile, File destFile)
    {
        Square source = Square.FromRankAndFile(sourceFile, Rank.Five);
        Square destination = Square.FromRankAndFile(destFile, Rank.Six);
        Square enPassantTarget = Square.FromRankAndFile(destFile, Rank.Five);

        Move move = Move.EnPassant(source, destination, enPassantTarget);

        using (Assert.Multiple())
        {
            await Assert.That(move.Source).IsEqualTo(source);
            await Assert.That(move.Destination).IsEqualTo(destination);
            await Assert.That(move.GetEnPassantTarget()).IsEqualTo(enPassantTarget);
        }
    }

    [Test]
    public async Task GetPromotionPieceTypeThrowsForNonPromotionMove()
    {
        Square source = Square.FromRankAndFile(File.E, Rank.Two);
        Square destination = Square.FromRankAndFile(File.E, Rank.Four);
        Move move = Move.Normal(source, destination);

        await Assert.That(() => move.GetPromotionPieceType()).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task GetRookSourceThrowsForNonCastlingMove()
    {
        Square source = Square.FromRankAndFile(File.E, Rank.Two);
        Square destination = Square.FromRankAndFile(File.E, Rank.Four);
        Move move = Move.Normal(source, destination);

        await Assert.That(() => move.GetRookSource()).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task GetRookDestinationThrowsForNonCastlingMove()
    {
        Square source = Square.FromRankAndFile(File.E, Rank.Two);
        Square destination = Square.FromRankAndFile(File.E, Rank.Four);
        Move move = Move.Normal(source, destination);

        await Assert.That(() => move.GetRookDestination()).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task GetEnPassantTargetThrowsForNonEnPassantMove()
    {
        Square source = Square.FromRankAndFile(File.E, Rank.Two);
        Square destination = Square.FromRankAndFile(File.E, Rank.Four);
        Move move = Move.Normal(source, destination);

        await Assert.That(() => move.GetEnPassantTarget()).Throws<InvalidOperationException>();
    }
}
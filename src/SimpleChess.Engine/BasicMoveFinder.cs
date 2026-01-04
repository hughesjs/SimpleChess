using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SimpleChess.State;

namespace SimpleChess.Engine;

public class BasicMoveFinder : IMoveFinder
{
    private static readonly MoveVector PawnMove = new() { Ranks = 1, Files = 0 };
    private static readonly MoveVector PawnDoubleMove = new() { Ranks = 2, Files = 0 };
    private static readonly MoveVector[] PawnAttacks = [new() { Ranks = 1, Files = 1 }, new() { Ranks = 1, Files = -1 }];
    private static readonly MoveVector[] RookUnitVectors = [new() { Ranks = 1, Files = 0 }, new() { Ranks = 0, Files = 1 }, new() { Ranks = -1, Files = 0 }, new() { Ranks = 0, Files = -1 }];
    private static readonly MoveVector[] BishopUnitVectors = [new() { Ranks = 1, Files = 1 }, new() { Ranks = 1, Files = -1 }, new() { Ranks = -1, Files = -1 }, new() { Ranks = -1, Files = 1 }];
    private static readonly MoveVector[] RoyalMoveVectors = BishopUnitVectors.Concat(RookUnitVectors).ToArray();
    private static readonly MoveVector[] KnightMoveVectors = [new() { Ranks = 2, Files = 1 }, new() { Ranks = 2, Files = -1 }, new() { Ranks = -2, Files = 1 }, new() { Ranks = -2, Files = -1 }, new() { Ranks = 1, Files = 2 }, new() { Ranks = 1, Files = -2 }, new() { Ranks = -1, Files = 2 }, new() { Ranks = -1, Files = -2 }];
    private static readonly PieceType[] PromotionPieceTypes = [PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight];

    public IEnumerable<Move> GetLegalMovesForAllPieces(GameState state) => state.CurrentBoard.EnumerateOccupiedSquares().SelectMany(s => GetLegalMovesForPiece(s, state));

    [Pure]
    public IEnumerable<Move> GetLegalMovesForPiece(Square pieceSquare, GameState state)
    {
        Board board = state.CurrentBoard;
        Piece piece = board.GetPieceAt(pieceSquare);

        if (piece.Colour != state.NextToPlay)
        {
            return [];
        }

        IEnumerable<Move> moves = (piece.PieceType) switch
        {
            PieceType.None => [],
            PieceType.Pawn => GetPawnBasicMoves(pieceSquare, board, piece)
                .Concat(GetEnPassantMoves(pieceSquare, piece, state.EnPassantTarget))
                .Concat(GetPromotionMoves(pieceSquare, board, piece))
                .Concat(GetPawnDoubleMove(pieceSquare, board, piece)),
            PieceType.Rook => GetRookBasicMoves(pieceSquare, board, piece),
            PieceType.Bishop => GetBishopBasicMoves(pieceSquare, board, piece),
            PieceType.Knight => GetKnightBasicMoves(pieceSquare, board, piece),
            PieceType.Queen => GetQueenBasicMoves(pieceSquare, board, piece),
            PieceType.King => GetKingBasicMoves(pieceSquare, board, piece)
                .Concat(GetCastlingMoves(pieceSquare, piece, state.CastlingRights)),
            _ => throw new ArgumentOutOfRangeException(nameof(pieceSquare), pieceSquare, "Piece at pieceSquare square is invalid")
        };

        return moves.Where(m => !MoveWouldLeaveSelfInCheck(m));
    }


    [Pure]
    private static bool MoveWouldLeaveSelfInCheck(Move move)
    {
        // Use king as super piece approach - can't implement without board piece application
        throw new NotImplementedException();
    }

    [Pure]
    private static IEnumerable<Move> GetCastlingMoves(Square pieceSquare, Piece piece, CastlingRights stateCastlingRights)
    {
        if (piece.Colour == Colour.Black && stateCastlingRights.BlackKingside)
        {
            Square kingDest = Square.FromRankAndFile(File.G, Rank.Eight);
            Square rookSource = Square.FromRankAndFile(File.H, Rank.Eight);
            Square rookDest = Square.FromRankAndFile(File.F, Rank.Eight);
            yield return Move.Castling(pieceSquare, kingDest, rookSource, rookDest);
        }

        if (piece.Colour == Colour.Black && stateCastlingRights.BlackQueenside)
        {
            Square kingDest = Square.FromRankAndFile(File.C, Rank.Eight);
            Square rookSource = Square.FromRankAndFile(File.A, Rank.Eight);
            Square rookDest = Square.FromRankAndFile(File.D, Rank.Eight);
            yield return Move.Castling(pieceSquare, kingDest, rookSource, rookDest);
        }

        if (piece.Colour == Colour.White && stateCastlingRights.WhiteKingside)
        {
            Square kingDest = Square.FromRankAndFile(File.G, Rank.One);
            Square rookSource = Square.FromRankAndFile(File.H, Rank.One);
            Square rookDest = Square.FromRankAndFile(File.F, Rank.One);
            yield return Move.Castling(pieceSquare, kingDest, rookSource, rookDest);
        }

        if (piece.Colour == Colour.White && stateCastlingRights.WhiteQueenside)
        {
            Square kingDest = Square.FromRankAndFile(File.C, Rank.One);
            Square rookSource = Square.FromRankAndFile(File.A, Rank.One);
            Square rookDest = Square.FromRankAndFile(File.D, Rank.One);
            yield return Move.Castling(pieceSquare, kingDest, rookSource, rookDest);
        }
    }

    [Pure]
    private static IEnumerable<Move> GetPromotionMoves(Square pieceSquare, Board board, Piece piece)
    {
        if (pieceSquare.Rank is not (Rank.Seven or Rank.Two))
        {
            yield break;
        }

        if (pieceSquare.TryApplyMoveVector(piece.Colour, PawnMove, out Square? targetMoveSquare) && board.GetPieceAt(targetMoveSquare.Value) == Piece.None)
        {
            foreach (Move move in GenerateEachPromotionMove(pieceSquare, targetMoveSquare.Value))
            {
                yield return move;
            }
        }

        foreach (MoveVector attack in PawnAttacks)
        {
            if (!pieceSquare.TryApplyMoveVector(piece.Colour, attack, out Square? targetAttackSquare))
            {
                continue;
            }

            Piece pieceAtDestination = board.GetPieceAt(targetAttackSquare.Value);
            if (pieceAtDestination == Piece.None || pieceAtDestination.Colour == piece.Colour)
            {
                continue;
            }

            foreach (Move move in GenerateEachPromotionMove(pieceSquare, targetAttackSquare.Value))
            {
                yield return move;
            }
        }

        yield break;

        IEnumerable<Move> GenerateEachPromotionMove(Square source, Square destination)
        {
            foreach (PieceType promotionPiece in PromotionPieceTypes)
            {
                yield return Move.Promotion(source, destination, promotionPiece);
            }
        }
    }

    [Pure]
    private static IEnumerable<Move> GetEnPassantMoves(Square pieceSquare, Piece piece, Square? stateEnPassantTarget)
    {
        if (!stateEnPassantTarget.HasValue)
        {
            yield break;
        }

        foreach (MoveVector pawnAttack in PawnAttacks)
        {
            if (pieceSquare.TryApplyMoveVector(piece.Colour, pawnAttack, out Square? targetSquare) && targetSquare == stateEnPassantTarget.Value)
            {
                // The captured pawn is at (pieceSquare.Rank, targetSquare.File)
                Square enPassantTargetPawn = Square.FromRankAndFile(targetSquare.Value.File, pieceSquare.Rank);
                yield return Move.EnPassant(pieceSquare, targetSquare.Value, enPassantTargetPawn);
                yield break;
            }
        }
    }

    [Pure]
    private static IEnumerable<Move> GetKingBasicMoves(Square pieceSquare, Board board, Piece piece)
    {
        foreach (MoveVector moveVector in RoyalMoveVectors)
        {
            if (!pieceSquare.TryApplyMoveVector(piece.Colour, moveVector, out Square? targetSquare))
            {
                continue;
            }

            Piece pieceAtDestination = board.GetPieceAt(targetSquare.Value);

            if (pieceAtDestination == Piece.None || pieceAtDestination.Colour != piece.Colour)
            {
                yield return Move.Normal(pieceSquare, targetSquare.Value);
            }
        }
    }

    [Pure]
    private static IEnumerable<Move> GetKnightBasicMoves(Square pieceSquare, Board board, Piece piece)
    {
        foreach (MoveVector moveVector in KnightMoveVectors)
        {
            if (!pieceSquare.TryApplyMoveVector(piece.Colour, moveVector, out Square? targetSquare))
            {
                continue;
            }

            Piece pieceAtDestination = board.GetPieceAt(targetSquare.Value);

            if (pieceAtDestination == Piece.None || pieceAtDestination.Colour != piece.Colour)
            {
                yield return Move.Normal(pieceSquare, targetSquare.Value);
            }
        }
    }

    [Pure]
    private static IEnumerable<Move> GetQueenBasicMoves(Square pieceSquare, Board board, Piece piece) =>
        RoyalMoveVectors.SelectMany(unitVector => ProjectAlongDirection(pieceSquare, board, piece.Colour, unitVector));

    [Pure]
    private static IEnumerable<Move> GetBishopBasicMoves(Square pieceSquare, Board board, Piece piece) =>
        BishopUnitVectors.SelectMany(unitVector => ProjectAlongDirection(pieceSquare, board, piece.Colour, unitVector));

    [Pure]
    private static IEnumerable<Move> GetRookBasicMoves(Square pieceSquare, Board board, Piece piece) =>
        RookUnitVectors.SelectMany(unitVector => ProjectAlongDirection(pieceSquare, board, piece.Colour, unitVector));

    private static IEnumerable<Move> ProjectAlongDirection(Square pieceSquare, Board board, Colour pieceColour, MoveVector unitVector) =>
        ProjectAlongDirection(pieceSquare, pieceSquare, board, pieceColour, unitVector);

    private static IEnumerable<Move> ProjectAlongDirection(Square pieceSquare, Square searchSquare, Board board, Colour pieceColour, MoveVector unitVector)
    {
        if (!searchSquare.TryApplyMoveVector(pieceColour, unitVector, out Square? targetSquare))
        {
            yield break;
        }

        Piece targetPiece = board.GetPieceAt(targetSquare.Value);

        if (targetPiece == Piece.None)
        {
            yield return Move.Normal(pieceSquare, targetSquare.Value);
        }
        else if (targetPiece.Colour == pieceColour)
        {
            yield break;
        }
        else
        {
            yield return Move.Normal(pieceSquare, targetSquare.Value);
            yield break;
        }

        foreach (Move move in ProjectAlongDirection(pieceSquare, targetSquare.Value, board, pieceColour, unitVector))
        {
            yield return move;
        }
    }

    [Pure]
    private static IEnumerable<Move> GetPawnBasicMoves(Square pieceSquare, Board board, Piece piece)
    {
        if (pieceSquare.TryApplyMoveVector(piece.Colour, PawnMove, out Square? targetMoveSquare))
        {
            if (targetMoveSquare.Value.Rank is not (Rank.Eight or Rank.One) // Filter promotions
                && board.GetPieceAt(targetMoveSquare.Value) == Piece.None)
            {
                yield return Move.Normal(pieceSquare, targetMoveSquare.Value);
            }
        }

        foreach (MoveVector attack in PawnAttacks)
        {
            if (!pieceSquare.TryApplyMoveVector(piece.Colour, attack, out Square? targetAttackSquare) ||
                targetAttackSquare.Value.Rank is Rank.Eight or Rank.One) // Filter promotions
            {
                continue;
            }

            Piece pieceAtDestination = board.GetPieceAt(targetAttackSquare.Value);
            if (pieceAtDestination != Piece.None && pieceAtDestination.Colour != piece.Colour)
            {
                yield return Move.Normal(pieceSquare, targetAttackSquare.Value);
            }
        }
    }

    [Pure]
    private static IEnumerable<Move> GetPawnDoubleMove(Square pieceSquare, Board board, Piece piece)
    {
        if (pieceSquare.Rank is (Rank.Seven or Rank.Two) && pieceSquare.TryApplyMoveVector(piece.Colour, PawnDoubleMove, out Square? targetMoveSquare) &&
            board.GetPieceAt(targetMoveSquare.Value) == Piece.None)
        {
            yield return Move.PawnDouble(pieceSquare, targetMoveSquare.Value);
        }
    }
}

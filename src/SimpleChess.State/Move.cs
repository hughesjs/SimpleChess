using System;

namespace SimpleChess.State;

/// <summary>
/// Represents a chess move using bit-packed encoding for optimal memory usage.
/// </summary>
/// <remarks>
/// Move data is packed into a 32-bit uint (4 bytes total):
/// - Bits 0-5: source square (6 bits = 0-63)
/// - Bits 6-11: destination square (6 bits = 0-63)
/// - Bits 12-14: move type (3 bits = 8 types)
/// - Bits 15-17: promotion piece type (3 bits, only used for promotion moves)
/// - Bits 18-23: Rook Source OR EnPassant Target
/// - Bits 24-29: Rook Destination
/// </remarks>
public readonly struct Move
{
    // Bit layout constants
    private const int SourceSquareShift = 0;
    private const int DestinationSquareShift = 6;
    private const int MoveTypeShift = 12;
    private const int PromotionPieceTypeShift = 15;
    private const int RookSourceOrEnPassantTargetShift = 18;
    private const int RookDestinationShift = 24;

    private const uint SquareMask = 0x3F;        // 6 bits
    private const uint MoveTypeMask = 0x7;       // 3 bits
    private const uint PromotionPieceTypeMask = 0x7;  // 3 bits

    private readonly uint _data;

    private Move(uint data)
    {
        _data = data;
    }

    /// <summary>
    /// Creates a normal move from source to destination.
    /// </summary>
    /// <param name="source">The source square.</param>
    /// <param name="destination">The destination square.</param>
    /// <returns>A move representing a normal piece movement.</returns>
    public static Move Normal(Square source, Square destination)
    {
        uint data = PackSquare(source, SourceSquareShift)
            | PackSquare(destination, DestinationSquareShift)
            | ((uint)MoveType.Normal << MoveTypeShift);
        return new Move(data);
    }

    /// <summary>
    /// Creates a pawn double move (pawn moving two squares forward from starting position).
    /// </summary>
    /// <param name="source">The source square (must be pawn's starting rank).</param>
    /// <param name="destination">The destination square (two ranks forward).</param>
    /// <returns>A move representing a pawn double move that creates an en passant target.</returns>
    public static Move PawnDouble(Square source, Square destination)
    {
        uint data = PackSquare(source, SourceSquareShift)
            | PackSquare(destination, DestinationSquareShift)
            | ((uint)MoveType.PawnDouble << MoveTypeShift);
        return new Move(data);
    }

    public static Move Castling(Square source, Square destination, Square rookSource, Square rookDestination)
    {
        uint data = PackSquare(source, SourceSquareShift)
            | PackSquare(destination, DestinationSquareShift)
            | ((uint)MoveType.Castling << MoveTypeShift)
            | PackSquare(rookSource, RookSourceOrEnPassantTargetShift)
            | PackSquare(rookDestination, RookDestinationShift);
        return new Move(data);
    }

    public static Move Promotion(Square source, Square destination, PieceType promotionPieceType)
    {
        uint data = PackSquare(source, SourceSquareShift)
            | PackSquare(destination, DestinationSquareShift)
            | ((uint)MoveType.Promotion << MoveTypeShift)
            | ((uint)promotionPieceType << PromotionPieceTypeShift);
        return new Move(data);
    }

    public static Move EnPassant(Square source, Square destination, Square enPassantTarget)
    {
        uint data = PackSquare(source, SourceSquareShift)
            | PackSquare(destination, DestinationSquareShift)
            | ((uint)MoveType.EnPassant << MoveTypeShift)
            | PackSquare(enPassantTarget, RookSourceOrEnPassantTargetShift);
        return new Move(data);
    }

    private static uint PackSquare(Square square, int shift)
    {
        int index = (int)square.Rank * 8 + (int)square.File;
        return (uint)index << shift;
    }

    private static Square UnpackSquare(uint data, int shift)
    {
        int index = (int)((data >> shift) & SquareMask);
        File file = (File)(index % 8);
        Rank rank = (Rank)(index / 8);
        return Square.FromRankAndFile(file, rank);
    }

    public Square Source => UnpackSquare(_data, SourceSquareShift);

    public Square Destination => UnpackSquare(_data, DestinationSquareShift);

    public MoveType MoveType => (MoveType)((_data >> MoveTypeShift) & MoveTypeMask);

    public PieceType GetPromotionPieceType()
    {
        if (MoveType != MoveType.Promotion)
        {
            throw new InvalidOperationException($"Cannot get promotion piece type for {MoveType} move");
        }
        return (PieceType)((_data >> PromotionPieceTypeShift) & PromotionPieceTypeMask);
    }

    public Square GetRookSource()
    {
        if (MoveType != MoveType.Castling)
        {
            throw new InvalidOperationException($"Cannot get rook source for {MoveType} move");
        }
        return UnpackSquare(_data, RookSourceOrEnPassantTargetShift);
    }

    public Square GetRookDestination()
    {
        if (MoveType != MoveType.Castling)
        {
            throw new InvalidOperationException($"Cannot get rook destination for {MoveType} move");
        }
        return UnpackSquare(_data, RookDestinationShift);
    }

    public Square GetEnPassantTarget()
    {
        if (MoveType != MoveType.EnPassant)
        {
            throw new InvalidOperationException($"Cannot get en passant target for {MoveType} move");
        }
        return UnpackSquare(_data, RookSourceOrEnPassantTargetShift);
    }
}

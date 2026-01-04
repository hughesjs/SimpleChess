using System;

namespace SimpleChess.State;

/// <summary>
/// Represents a chess move using bit-packed encoding for optimal memory usage.
/// </summary>
/// <remarks>
/// Move data is packed into a 32-bit uint (4 bytes total):
/// - Bits 0-5: source square (6 bits = 0-63)
/// - Bits 6-11: destination square (6 bits = 0-63)
/// - Bits 12-13: move type (2 bits = 4 types)
/// - Bits 14-16: promotion piece type (3 bits, only used for promotion moves)
/// - Bits 17-22: Rook Source OR EnPassant Target
/// - Bits 23-28: Rook Destination
/// </remarks>
public readonly struct Move
{
    // Bit layout constants
    private const int SourceSquareShift = 0;
    private const int DestinationSquareShift = 6;
    private const int MoveTypeShift = 12;
    private const int PromotionPieceTypeShift = 14;
    private const int RookSourceOrEnPassantTargetShift = 17;
    private const int RookDestinationShift = 23;

    private const uint SquareMask = 0x3F;        // 6 bits
    private const uint MoveTypeMask = 0x3;       // 2 bits
    private const uint PromotionPieceTypeMask = 0x7;  // 3 bits

    private readonly uint _data;

    private Move(uint data)
    {
        _data = data;
    }

    public static Move Normal(Square source, Square destination)
    {
        uint data = PackSquare(source, SourceSquareShift)
            | PackSquare(destination, DestinationSquareShift)
            | ((uint)MoveType.Normal << MoveTypeShift);
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

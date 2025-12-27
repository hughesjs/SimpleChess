using System;
using System.Text;

namespace SimpleChess.State;

/// <summary>
/// Represents the castling rights available to both players in a chess game.
/// </summary>
/// <remarks>
/// Castling is a special move involving the king and a rook. A player may castle kingside (with the h-file rook)
/// or queenside (with the a-file rook), provided certain conditions are met. This type tracks which castling
/// moves are still legally available for each player.
/// <para>
/// Castling rights are lost when: the king moves, the relevant rook moves, or the rook is captured.
/// When all four properties are false, no castling is available for either player.
/// </para>
/// </remarks>
public readonly record struct CastlingRights
{
    /// <summary>
    /// Gets whether White can castle kingside (with the rook on h1).
    /// </summary>
    public bool WhiteKingside => _flags.HasFlag(CastlingRightsFlags.WhiteKingside);

    /// <summary>
    /// Gets whether White can castle queenside (with the rook on a1).
    /// </summary>
    public bool WhiteQueenside => _flags.HasFlag(CastlingRightsFlags.WhiteQueenside);

    /// <summary>
    /// Gets whether Black can castle kingside (with the rook on h8).
    /// </summary>
    public bool BlackKingside => _flags.HasFlag(CastlingRightsFlags.BlackKingside);

    /// <summary>
    /// Gets whether Black can castle queenside (with the rook on a8).
    /// </summary>
    public bool BlackQueenside => _flags.HasFlag(CastlingRightsFlags.BlackQueenside);

    private readonly CastlingRightsFlags _flags;

    private CastlingRights(CastlingRightsFlags flags)
    {
        _flags = flags;
    }

    [Flags]
    private enum CastlingRightsFlags
    {
        None = 0,
        WhiteKingside = 1,
        WhiteQueenside = 2,
        BlackKingside = 4,
        BlackQueenside = 8
    }


    internal static CastlingRights FromFen(FenGameState.FenSegment<FenGameState.CastlingStateKind> castlingRightsFen)
    {
        CastlingRightsFlags flags = CastlingRightsFlags.None;

        foreach (char c in castlingRightsFen)
        {
            flags |= c switch
            {
                'K' => CastlingRightsFlags.WhiteKingside,
                'Q' => CastlingRightsFlags.WhiteQueenside,
                'k' => CastlingRightsFlags.BlackKingside,
                'q' => CastlingRightsFlags.BlackQueenside,
                '-' => CastlingRightsFlags.None,
                // Theoretically, we should never get here. The FEN string should already be validated
                _ => throw new ArgumentException($"'{c}' is not a valid character in a FEN castling string. If you get here, there's a bug in FenGameState.")
            };
        }

        return new(flags);
    }

    internal static void ToFen(CastlingRights castlingRights, StringBuilder builder)
    {
        int lengthBefore = builder.Length;

        if (castlingRights.WhiteKingside)
        {
            builder.Append('K');
        }

        if (castlingRights.WhiteQueenside)
        {
            builder.Append('Q');
        }

        if (castlingRights.BlackKingside)
        {
            builder.Append('k');
        }

        if (castlingRights.BlackQueenside)
        {
            builder.Append('q');
        }

        if (builder.Length == lengthBefore)
        {
            builder.Append('-');
        }
    }
}

using System;
using System.Text;

namespace SimpleChess.State.State;

public readonly record struct CastlingRights
{
    public bool WhiteKingside => _flags.HasFlag(CastlingRightsFlags.WhiteKingside);
    public bool WhiteQueenside => _flags.HasFlag(CastlingRightsFlags.WhiteQueenside);
    public bool BlackKingside => _flags.HasFlag(CastlingRightsFlags.BlackKingside);
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

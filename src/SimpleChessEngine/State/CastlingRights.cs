using System;

namespace SimpleChessEngine.State;

internal readonly record struct CastlingRights
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


    public static CastlingRights FromFen(FenGameState.FenSegment<FenGameState.CastlingStateKind> castlingRightsFen)
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
}

using System;
using System.Diagnostics.CodeAnalysis;
using SimpleChess.State;

namespace SimpleChess.Engine;

public static class SquareExtensions
{

    public static bool TryApplyMoveVector(this Square square, Colour colour, MoveVector vector, [NotNullWhen(true)] out Square? targetSquare)
    {
        int fileOffset = colour == Colour.White ? vector.Files : -vector.Files;
        int rankOffset = colour == Colour.White ? vector.Ranks : -vector.Ranks;

        Rank newRank = square.Rank + rankOffset;
        File newFile = square.File + fileOffset;

        if (!(Enum.IsDefined(newRank) && Enum.IsDefined(newFile)))
        {
            targetSquare = null;
            return false;
        }

        targetSquare = Square.FromRankAndFile(newFile, newRank);

        return true;
    }
}


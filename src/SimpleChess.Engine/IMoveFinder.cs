using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SimpleChess.State;

namespace SimpleChess.Engine;

public interface IMoveFinder
{
    [Pure]
    public IEnumerable<Move> GetLegalMovesForPiece(Square pieceSquare, GameState state);
}

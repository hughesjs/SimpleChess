

namespace SimpleChessEngine.State;

internal class Game
{
    public Board CurrentBoard { get; }
    public Colour NextToPlay { get; }
    public HalfTurnCount HalfTurnCounter { get; }
    public FullTurnCount FullTurnCounter { get; }
    public CastlingRights CastlingRights { get; }

    private Game(Board currentBoard, Colour nextToPlay, HalfTurnCount halfTurnCounter, FullTurnCount fullTurnCounter, CastlingRights castlingRights)
    {
        CurrentBoard = currentBoard;
        NextToPlay = nextToPlay;
        HalfTurnCounter = halfTurnCounter;
        FullTurnCounter = fullTurnCounter;
        CastlingRights = castlingRights;
    }

    public static Game NewGame => FromFen(FenGameState.DefaultGame);

    public static Game FromFen(FenGameState fen)
    {
        Board board = Board.FromFen(fen.PieceLayout);
        Colour nextToPlay = fen.NextToPlay.ToString() is "w" ? Colour.White : Colour.Black;
        CastlingRights castlingRights = CastlingRights.FromFen(fen.CastlingState);
        HalfTurnCount halfTurnCounter = HalfTurnCount.FromFen(fen.HalfTurnCounter);
        FullTurnCount fullTurnCounter = FullTurnCount.FromFen(fen.FullTurnCounter);
        return new Game(board, nextToPlay, halfTurnCounter, fullTurnCounter, castlingRights);
    }
}



namespace SimpleChessEngine.State;

internal class Game
{
    public Board CurrentBoard { get; }
    public Colour NextToPlay { get; }
    public HalfTurnCount HalfTurnCounter { get; }
    public FullTurnCount FullTurnCounter { get; }

    private Game(Board currentBoard, Colour nextToPlay, HalfTurnCount halfTurnCounter, FullTurnCount fullTurnCounter)
    {
        CurrentBoard = currentBoard;
        NextToPlay = nextToPlay;
        HalfTurnCounter = halfTurnCounter;
        FullTurnCounter = fullTurnCounter;
    }

    public static Game NewGame => FromFen(FenGameState.DefaultGame);

    public static Game FromFen(FenGameState fen)
    {
        Board board = Board.FromFenNotation(fen.PieceLayout);
        Colour nextToPlay = fen.NextToPlay.ToString() is "w" ? Colour.White : Colour.Black;

        // These should be safe as FenBoardState validates the input
        HalfTurnCount.TryCreate(int.Parse(fen.HalfTurnCounter.ToString()), out HalfTurnCount halfTurnCounter);
        FullTurnCount.TryCreate(int.Parse(fen.FullTurnCounter.ToString()), out FullTurnCount fullTurnCounter);

        return new Game(board, nextToPlay, halfTurnCounter, fullTurnCounter);
    }
}

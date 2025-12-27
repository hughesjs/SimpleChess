namespace SimpleChessEngine.State;

internal record struct GameState
{
    public Board CurrentBoard { get; }
    public Colour NextToPlay { get; }
    public HalfTurnCount HalfTurnCounter { get; }
    public FullTurnCount FullTurnCounter { get; }
    public CastlingRights CastlingRights { get; }
    public Square? EnPassantTarget { get; }

    private GameState(Board currentBoard, Colour nextToPlay, HalfTurnCount halfTurnCounter, FullTurnCount fullTurnCounter, CastlingRights castlingRights,  Square? enPassantTarget)
    {
        CurrentBoard = currentBoard;
        NextToPlay = nextToPlay;
        HalfTurnCounter = halfTurnCounter;
        FullTurnCounter = fullTurnCounter;
        CastlingRights = castlingRights;
        EnPassantTarget = enPassantTarget;
    }

    public static GameState NewGameState => FromFen(FenGameState.DefaultGame);

    public static GameState FromFen(FenGameState fen)
    {
        Board board = Board.FromFen(fen.PieceLayout);
        Colour nextToPlay = fen.NextToPlay.ToString() is "w" ? Colour.White : Colour.Black;
        CastlingRights castlingRights = CastlingRights.FromFen(fen.CastlingState);
        HalfTurnCount halfTurnCounter = HalfTurnCount.FromFen(fen.HalfTurnCounter);
        FullTurnCount fullTurnCounter = FullTurnCount.FromFen(fen.FullTurnCounter);
        Square? enPassantTarget = Square.FromFen(fen.EnPassantState);
        return new(board, nextToPlay, halfTurnCounter, fullTurnCounter, castlingRights, enPassantTarget);
    }

    // public static FenGameState ToFen(GameState fen)
    // {
    //     string boardState = fen.CurrentBoard.
    // }
}

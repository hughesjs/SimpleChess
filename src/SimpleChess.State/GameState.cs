using System;
using System.Text;

namespace SimpleChess.State;

/// <summary>
/// Represents the complete state of a chess game at a specific point in time.
/// </summary>
/// <remarks>
/// This is the primary public API for working with chess game state. It contains all information
/// necessary to represent a position including piece placement, whose turn it is, castling rights,
/// en passant opportunities, and move counters for the fifty-move rule.
/// <para>
/// Game states can be created from FEN (Forsyth-Edwards Notation) strings using <see cref="FromFen"/>,
/// or converted back to FEN using <see cref="ToFen"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Create a new game in the starting position
/// GameState game = GameState.NewGameState;
///
/// // Or parse from FEN notation
/// if (FenGameState.TryParse("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", out FenGameState fen))
/// {
///     GameState customGame = GameState.FromFen(fen);
/// }
///
/// // Access game state information
/// Piece piece = game.CurrentBoard.GetPieceAt(Rank.Two, File.E);
/// Colour activePlayer = game.NextToPlay;
/// </code>
/// </example>
public record struct GameState
{
    /// <summary>
    /// Gets the current board configuration with all piece positions.
    /// </summary>
    public Board CurrentBoard { get; }

    /// <summary>
    /// Gets which player (White or Black) has the next turn.
    /// </summary>
    public Colour NextToPlay { get; }

    /// <summary>
    /// Gets the halfmove clock used for the fifty-move rule.
    /// </summary>
    /// <remarks>
    /// This counter is incremented after each player's move and reset to zero after a pawn move or capture.
    /// </remarks>
    public HalfTurnCount HalfTurnCounter { get; }

    /// <summary>
    /// Gets the full move number, starting at 1 and incremented after Black's move.
    /// </summary>
    public FullTurnCount FullTurnCounter { get; }

    /// <summary>
    /// Gets the castling rights available for both players.
    /// </summary>
    public CastlingRights CastlingRights { get; }

    /// <summary>
    /// Gets the target square for en passant capture, if available.
    /// </summary>
    /// <remarks>
    /// This is <c>null</c> when no en passant capture is possible. When a pawn moves two squares forward,
    /// this contains the square it passed over, which can be captured by an enemy pawn via en passant.
    /// </remarks>
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

    /// <summary>
    /// Gets a new game in the standard starting position.
    /// </summary>
    /// <remarks>
    /// This represents the initial setup with all pieces in their starting positions,
    /// White to move, all castling rights available, no en passant, and move counters at their initial values.
    /// </remarks>
    public static GameState NewGameState => FromFen(FenGameState.DefaultGame);

    /// <summary>
    /// Creates a <see cref="GameState"/> from a validated FEN (Forsyth-Edwards Notation) string.
    /// </summary>
    /// <param name="fen">A validated FEN game state. Use <see cref="FenGameState.TryParse"/> to validate FEN strings.</param>
    /// <returns>A game state representing the position described by the FEN string.</returns>
    /// <remarks>
    /// This method assumes the FEN string has already been validated. Invalid FEN strings should be caught
    /// by <see cref="FenGameState.TryParse"/> before calling this method.
    /// </remarks>
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

    /// <summary>
    /// Converts a <see cref="GameState"/> to FEN (Forsyth-Edwards Notation) format.
    /// </summary>
    /// <param name="state">The game state to convert.</param>
    /// <returns>A validated FEN string representing the game state.</returns>
    /// <exception cref="InvalidGameStateException">
    /// Thrown if the game state cannot be represented as a valid FEN string.
    /// </exception>
    /// <remarks>
    /// This method constructs a FEN string from the game state and validates it.
    /// Under normal circumstances, all valid game states should produce valid FEN strings.
    /// </remarks>
    public static FenGameState ToFen(GameState state)
    {
        StringBuilder builder = new(128);
        Board.ToFen(state.CurrentBoard, builder);
        builder.Append(' ');
        builder.Append(state.NextToPlay is Colour.White ? 'w' : 'b');
        builder.Append(' ');
        CastlingRights.ToFen(state.CastlingRights, builder);
        builder.Append(' ');
        Square.ToFen(state.EnPassantTarget, builder);
        builder.Append(' ');
        builder.Append(state.HalfTurnCounter);
        builder.Append(' ');
        builder.Append(state.FullTurnCounter);

        return FenGameState.TryParse(builder.ToString(), out FenGameState fen)? fen : throw new InvalidGameStateException("Could not represent game state as valid FEN string");
    }

    public static GameState ApplyMove(Move move)
    {
        throw new NotImplementedException();
    }
}

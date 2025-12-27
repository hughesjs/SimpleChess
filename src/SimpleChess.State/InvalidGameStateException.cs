using System;

namespace SimpleChess.State;

/// <summary>
/// The exception that is thrown when a game state cannot be represented as valid FEN notation.
/// </summary>
/// <remarks>
/// This exception is typically thrown by <see cref="GameState.ToFen"/> when the game state
/// cannot be serialised to a valid FEN string. Under normal circumstances, all valid game states
/// should be convertible to FEN, so this exception indicates an internal error or corruption.
/// </remarks>
public class InvalidGameStateException: Exception
{
    /// <summary>
    /// Initialises a new instance of the <see cref="InvalidGameStateException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidGameStateException(string message) : base(message){}
}

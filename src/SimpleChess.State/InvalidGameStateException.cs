using System;

namespace SimpleChess.State.State;

public class InvalidGameStateException: Exception
{
    public InvalidGameStateException(string message) : base(message){}
}

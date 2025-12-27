using System;

namespace SimpleChessEngine.State;

public class InvalidGameStateException: Exception
{
    public InvalidGameStateException(string message) : base(message){}
}

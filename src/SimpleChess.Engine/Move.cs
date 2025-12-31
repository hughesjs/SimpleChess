using SimpleChess.State;

namespace SimpleChess.Engine;

public record struct Move
{
    public required Square Source { get; init; }
    public required Square Destination { get; init; }
    public PieceType? PromotionPieceType { get; init; }
}

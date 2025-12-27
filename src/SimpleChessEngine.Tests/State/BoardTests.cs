using System.Threading.Tasks;

using SimpleChessEngine.State;
using FenGameState = SimpleChessEngine.State.FenGameState;

namespace SimpleChessEngine.Tests.State;

public class BoardTests
{
    [Test]
    public async Task CanParseDefaultBoardFromFen()
    {
        FenGameState fen = FenGameState.DefaultGame;

        Board board = Board.FromFenNotation(fen.PieceLayout);

        using (Assert.Multiple())
        {
            // Black back rank (rank 8)
            await Assert.That(board.GetPieceAt(Rank.A, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.B, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.C, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.D, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.E, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.F, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.G, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.H, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));

            // Black pawns (rank 7)
            await Assert.That(board.GetPieceAt(Rank.A, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.B, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.C, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.D, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.E, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.F, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.G, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.H, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));

            // All the blank squares
            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    await Assert.That(board.GetPieceAt((Rank)j, (File)i)).IsDefault();
                }
            }

            // White pawns (rank 2)
            await Assert.That(board.GetPieceAt(Rank.A, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.B, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.C, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.D, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.E, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.F, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.G, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.H, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));

            // White back rank (rank 1)
            await Assert.That(board.GetPieceAt(Rank.A, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.B, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.C, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.D, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.E, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.F, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.G, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.H, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));
        }
    }

    [Test]
    public async Task GeneratesCorrectDefaultBoard()
    {
        Board board = Board.DefaultBoard;

        using (Assert.Multiple())
        {
            // Black back rank (rank 8)
            await Assert.That(board.GetPieceAt(Rank.A, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.B, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.C, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.D, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.E, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.F, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.G, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.H, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));

            // Black pawns (rank 7)
            await Assert.That(board.GetPieceAt(Rank.A, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.B, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.C, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.D, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.E, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.F, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.G, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.H, File.Seven)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));

            // All the blank squares
            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    await Assert.That(board.GetPieceAt((Rank)j, (File)i)).IsDefault();
                }
            }

            // White pawns (rank 2)
            await Assert.That(board.GetPieceAt(Rank.A, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.B, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.C, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.D, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.E, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.F, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.G, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.H, File.Two)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));

            // White back rank (rank 1)
            await Assert.That(board.GetPieceAt(Rank.A, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.B, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.C, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.D, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.E, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.F, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.G, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.H, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));
        }
    }

    [Test]
    public async Task CanParseItalianGamePositionFromFen()
    {
        // Italian Game after 1.e4 e5 2.Nf3 Nc6 3.Bc4 Nf6
        const string fenString = "r1bqkb1r/pppp1ppp/2n2n2/4p3/2B1P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 4 5";
        _ = FenGameState.TryParse(fenString, out FenGameState fen);

        Board board = Board.FromFenNotation(fen.PieceLayout);

        using (Assert.Multiple())
        {
            // Black pieces - back rank with gaps
            await Assert.That(board.GetPieceAt(Rank.A, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.B, File.Eight)).IsDefault(); // empty square
            await Assert.That(board.GetPieceAt(Rank.C, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.D, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.E, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.F, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.G, File.Eight)).IsDefault(); // empty square
            await Assert.That(board.GetPieceAt(Rank.H, File.Eight)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));

            // Black knights on c6 and f6
            await Assert.That(board.GetPieceAt(Rank.C, File.Six)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.F, File.Six)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));

            // Black pawn on e5
            await Assert.That(board.GetPieceAt(Rank.E, File.Five)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));

            // White bishop on c4
            await Assert.That(board.GetPieceAt(Rank.C, File.Four)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));

            // White pawn on e4
            await Assert.That(board.GetPieceAt(Rank.E, File.Four)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));

            // White knight on f3
            await Assert.That(board.GetPieceAt(Rank.F, File.Three)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));

            // White back rank - unmoved pieces
            await Assert.That(board.GetPieceAt(Rank.A, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.B, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.C, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.D, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.E, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.F, File.One)).IsDefault(); // empty - knight moved
            await Assert.That(board.GetPieceAt(Rank.G, File.One)).IsDefault(); // empty
            await Assert.That(board.GetPieceAt(Rank.H, File.One)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));

            // Some empty squares in the center
            await Assert.That(board.GetPieceAt(Rank.D, File.Four)).IsDefault();
            await Assert.That(board.GetPieceAt(Rank.D, File.Five)).IsDefault();
        }
    }
}

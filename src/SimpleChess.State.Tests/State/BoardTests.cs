using System.Text;
using System.Threading.Tasks;

namespace SimpleChess.State.Tests.State;

public class BoardTests
{
    [Test]
    public async Task CanParseDefaultBoardFromFen()
    {
        FenGameState fen = FenGameState.DefaultGame;

        Board board = Board.FromFen(fen.PieceLayout);

        using (Assert.Multiple())
        {
            // Black back rank (rank 8)
            await Assert.That(board.GetPieceAt(Rank.Eight, File.A)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.B)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.C)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.D)).IsEqualTo(new Piece(Colour.Black, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.E)).IsEqualTo(new Piece(Colour.Black, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.F)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.G)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.H)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));

            // Black pawns (rank 7)
            await Assert.That(board.GetPieceAt(Rank.Seven, File.A)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.B)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.C)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.D)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.E)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.F)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.G)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.H)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));

            // All the blank squares
            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    await Assert.That(board.GetPieceAt((Rank)i, (File)j)).IsDefault();
                }
            }

            // White pawns (rank 2)
            await Assert.That(board.GetPieceAt(Rank.Two, File.A)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.B)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.C)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.D)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.E)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.F)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.G)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.H)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));

            // White back rank (rank 1)
            await Assert.That(board.GetPieceAt(Rank.One, File.A)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.One, File.B)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.One, File.C)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.One, File.D)).IsEqualTo(new Piece(Colour.White, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.One, File.E)).IsEqualTo(new Piece(Colour.White, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.One, File.F)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.One, File.G)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.One, File.H)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));
            
        }
    }

    [Test]
    public async Task GeneratesCorrectDefaultBoard()
    {
        Board board = Board.DefaultBoard;

        using (Assert.Multiple())
        {
            // Black back rank (rank 8)
            await Assert.That(board.GetPieceAt(Rank.Eight, File.A)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.B)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.C)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.D)).IsEqualTo(new Piece(Colour.Black, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.E)).IsEqualTo(new Piece(Colour.Black, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.F)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.G)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.H)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));

            // Black pawns (rank 7)
            await Assert.That(board.GetPieceAt(Rank.Seven, File.A)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.B)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.C)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.D)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.E)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.F)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.G)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Seven, File.H)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));

            // All the blank squares
            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    await Assert.That(board.GetPieceAt((Rank)i, (File)j)).IsDefault();
                }
            }

            // White pawns (rank 2)
            await Assert.That(board.GetPieceAt(Rank.Two, File.A)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.B)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.C)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.D)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.E)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.F)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.G)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));
            await Assert.That(board.GetPieceAt(Rank.Two, File.H)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));

            // White back rank (rank 1)
            await Assert.That(board.GetPieceAt(Rank.One, File.A)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.One, File.B)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.One, File.C)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.One, File.D)).IsEqualTo(new Piece(Colour.White, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.One, File.E)).IsEqualTo(new Piece(Colour.White, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.One, File.F)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.One, File.G)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.One, File.H)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));
        }
    }

    [Test]
    public async Task CanParseItalianGamePositionFromFen()
    {
        // Italian GameState after 1.e4 e5 2.Nf3 Nc6 3.Bc4 Nf6
        const string fenString = "r1bqkb1r/pppp1ppp/2n2n2/4p3/2B1P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 4 5";
        _ = FenGameState.TryParse(fenString, out FenGameState fen);

        Board board = Board.FromFen(fen.PieceLayout);

        using (Assert.Multiple())
        {
            // Black pieces - back rank with gaps
            await Assert.That(board.GetPieceAt(Rank.Eight, File.A)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.B)).IsDefault(); // empty square
            await Assert.That(board.GetPieceAt(Rank.Eight, File.C)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.D)).IsEqualTo(new Piece(Colour.Black, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.E)).IsEqualTo(new Piece(Colour.Black, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.F)).IsEqualTo(new Piece(Colour.Black, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.Eight, File.G)).IsDefault(); // empty square
            await Assert.That(board.GetPieceAt(Rank.Eight, File.H)).IsEqualTo(new Piece(Colour.Black, PieceType.Rook));

            // Black knights on c6 and f6
            await Assert.That(board.GetPieceAt(Rank.Six, File.C)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.Six, File.F)).IsEqualTo(new Piece(Colour.Black, PieceType.Knight));

            // Black pawn on e5
            await Assert.That(board.GetPieceAt(Rank.Five, File.E)).IsEqualTo(new Piece(Colour.Black, PieceType.Pawn));

            // White bishop on c4
            await Assert.That(board.GetPieceAt(Rank.Four, File.C)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));

            // White pawn on e4
            await Assert.That(board.GetPieceAt(Rank.Four, File.E)).IsEqualTo(new Piece(Colour.White, PieceType.Pawn));

            // White knight on f3
            await Assert.That(board.GetPieceAt(Rank.Three, File.F)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));

            // White back rank - unmoved pieces
            await Assert.That(board.GetPieceAt(Rank.One, File.A)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));
            await Assert.That(board.GetPieceAt(Rank.One, File.B)).IsEqualTo(new Piece(Colour.White, PieceType.Knight));
            await Assert.That(board.GetPieceAt(Rank.One, File.C)).IsEqualTo(new Piece(Colour.White, PieceType.Bishop));
            await Assert.That(board.GetPieceAt(Rank.One, File.D)).IsEqualTo(new Piece(Colour.White, PieceType.Queen));
            await Assert.That(board.GetPieceAt(Rank.One, File.E)).IsEqualTo(new Piece(Colour.White, PieceType.King));
            await Assert.That(board.GetPieceAt(Rank.One, File.F)).IsDefault(); // empty - knight moved
            await Assert.That(board.GetPieceAt(Rank.One, File.G)).IsDefault(); // empty
            await Assert.That(board.GetPieceAt(Rank.One, File.H)).IsEqualTo(new Piece(Colour.White, PieceType.Rook));

            // Some empty squares in the center
            await Assert.That(board.GetPieceAt(Rank.Four, File.D)).IsDefault();
            await Assert.That(board.GetPieceAt(Rank.Five, File.D)).IsDefault();
        }
    }

    [Test]
    public async Task ToFenGeneratesCorrectStartingPosition()
    {
        Board board = Board.DefaultBoard;
        StringBuilder builder = new();
        Board.ToFen(board, builder);
        string fen = builder.ToString();

        await Assert.That(fen).IsEqualTo("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
    }

    [Test]
    public async Task ToFenHandlesItalianGamePosition()
    {
        // Italian GameState after 1.e4 e5 2.Nf3 Nc6 3.Bc4 Nf6
        const string fenString = "r1bqkb1r/pppp1ppp/2n2n2/4p3/2B1P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 4 5";
        _ = FenGameState.TryParse(fenString, out FenGameState fen);

        Board board = Board.FromFen(fen.PieceLayout);
        StringBuilder builder = new();
        Board.ToFen(board, builder);
        string resultFen = builder.ToString();

        await Assert.That(resultFen).IsEqualTo("r1bqkb1r/pppp1ppp/2n2n2/4p3/2B1P3/5N2/PPPP1PPP/RNBQK2R");
    }

    [Test]
    public async Task ToFenHandlesEmptyBoard()
    {
        const string emptyBoardFen = "8/8/8/8/8/8/8/8 w - - 0 1";
        _ = FenGameState.TryParse(emptyBoardFen, out FenGameState fen);

        Board board = Board.FromFen(fen.PieceLayout);
        StringBuilder builder = new();
        Board.ToFen(board, builder);
        string resultFen = builder.ToString();

        await Assert.That(resultFen).IsEqualTo("8/8/8/8/8/8/8/8");
    }

    [Test]
    [Arguments("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR")]
    [Arguments("r1bqkb1r/pppp1ppp/2n2n2/4p3/2B1P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 4 5", "r1bqkb1r/pppp1ppp/2n2n2/4p3/2B1P3/5N2/PPPP1PPP/RNBQK2R")]
    [Arguments("8/8/8/8/8/8/8/8 w - - 0 1", "8/8/8/8/8/8/8/8")]
    [Arguments("4k3/8/8/8/8/8/8/4K3 w - - 0 1", "4k3/8/8/8/8/8/8/4K3")]
    public async Task FromFenAndToFenAreInverses(string fullFen, string expectedBoardFen)
    {
        _ = FenGameState.TryParse(fullFen, out FenGameState fen);
        Board board = Board.FromFen(fen.PieceLayout);
        StringBuilder builder = new();
        Board.ToFen(board, builder);
        string resultFen = builder.ToString();

        await Assert.That(resultFen).IsEqualTo(expectedBoardFen);
    }
}

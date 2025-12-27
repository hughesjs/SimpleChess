using System;


namespace SimpleChessEngine.State;


internal sealed class Board
{
    private readonly Piece[] _pieces;

    private Board(Piece[] pieces)
    {
        _pieces = pieces;
    }

    /// <summary>
    /// Gets a piece using its rank and file.
    /// </summary>
    /// <param name="rank">X axis with the leftmost rook at A</param>
    /// <param name="file">Y axis with the white's back row at One</param>
    /// <returns></returns>
    public Piece GetPieceAt(Rank rank, File file) => _pieces[((int)file * 8) + (int)rank];

    public static Board DefaultBoard => FromFenNotation(FenGameState.DefaultGame.PieceLayout);

    public static Board FromFenNotation(ReadOnlySpan<char> piecesFenSection)
    {
        Piece[] pieces = new Piece[64];

        int pieceArrayIndex = 56; // 8A
        foreach (char c in piecesFenSection)
        {
            if (c == '/')
            {
                pieceArrayIndex -= 16; // Drop down a row and reset to leftmost
                continue;
            }

            if (char.IsDigit(c))
            {
                pieceArrayIndex += c - '0';
                continue;
            }

            pieces[pieceArrayIndex] = Piece.FromFenCode(c);
            pieceArrayIndex++;
        }

        return new(pieces);
    }
}

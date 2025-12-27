using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace SimpleChessEngine.State;

internal readonly struct Board : IEquatable<Board> // Note: Not a record struct because the standard IEquatable<T> implementation doesn't work with InlineArray
{
    private readonly PieceBuffer _pieces;

    private Board(PieceBuffer pieces)
    {
        _pieces = pieces;
    }

    /// <summary>
    /// Gets a piece using its file and rank.
    /// </summary>
    /// <param name="rank">Y axis (row) with White's back row at One</param>
    /// <param name="file">X axis (column) with the leftmost file at A</param>
    /// <returns></returns>
    public Piece GetPieceAt(Rank rank, File file) => _pieces[((int)rank * 8) + (int)file];

    public Piece GetPieceAt(Square square) => GetPieceAt(square.Rank, square.File);

    public static Board DefaultBoard => FromFen(FenGameState.DefaultGame.PieceLayout);

    public static Board FromFen(FenGameState.FenSegment<FenGameState.PieceLayoutKind> piecesFenSection)
    {
        PieceBuffer pieces = new();

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

    public static void ToFen(Board board, StringBuilder builder)
    {
        for (int rank = 7; rank >= 0; rank--)
        {
            int consecutiveEmpties = 0;
            for (int file = 0; file < 8; file++)
            {
                Piece piece = board.GetPieceAt((Rank)rank, (File)file);

                if (piece == default)
                {
                    consecutiveEmpties++;
                    continue;
                }

                if (consecutiveEmpties != 0)
                {
                    builder.Append(consecutiveEmpties);
                    consecutiveEmpties = 0;
                }

                builder.Append(Piece.ToFen(piece));
            }

            if (consecutiveEmpties > 0)
            {
                builder.Append(consecutiveEmpties);
            }

            if (rank != 0)
            {
                builder.Append('/');
            }
        }
    }

    /// <summary>
    /// Gives us a value type representing the Piece[]
    /// </summary>
    [InlineArray(64)]
    private struct PieceBuffer
    {
        private Piece _element0;
    }

    #region equality members

    public bool Equals(Board other)
    {
        // Need to do this manually for an inline array
        for (int i = 0; i < 64; i++)
        {
            if (_pieces[i] != other._pieces[i])
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        return obj is Board other && Equals(other);
    }

    public override int GetHashCode()
    {
        HashCode hash = new();
        for (int i = 0; i < 64; i++)
        {
            hash.Add(_pieces[i]);
        }

        return hash.ToHashCode();
    }

    public static bool operator ==(Board left, Board right) => left.Equals(right);
    public static bool operator !=(Board left, Board right) => !left.Equals(right);

    #endregion
}

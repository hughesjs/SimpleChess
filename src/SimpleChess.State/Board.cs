using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SimpleChess.State;

/// <summary>
/// Represents the 64-square chess board with piece positions.
/// </summary>
/// <remarks>
/// The board uses an efficient inline array for piece storage, providing optimal performance
/// for piece lookups and modifications. The coordinate system uses <see cref="Rank"/> for rows (1-8)
/// and <see cref="File"/> for columns (A-H), with Rank.One being White's back row.
/// <para>
/// Empty squares are represented by default <see cref="Piece"/> values (with Colour.None and PieceType.None).
/// </para>
/// </remarks>
public readonly struct Board : IEquatable<Board> // Note: Not a record struct because the standard IEquatable<T> implementation doesn't work with InlineArray
{
    public IEnumerable<Square> EnumerateOccupiedSquares()
    {
        for (int i = 0; i < 64; i++)
        {
            if (_pieces[i] != default)
            {
                yield return Square.FromRankAndFile((File)(i % 8), (Rank)(i / 8));
            }
        }
    }

    private readonly PieceBuffer _pieces;

    private Board(PieceBuffer pieces)
    {
        _pieces = pieces;
    }

    /// <summary>
    /// Gets a piece at the specified rank and file coordinates.
    /// </summary>
    /// <param name="rank">The rank (row), where Rank.One is White's back row and Rank.Eight is Black's back row.</param>
    /// <param name="file">The file (column), where File.A is the leftmost column and File.H is the rightmost.</param>
    /// <returns>The piece at the specified position, or a default piece (Colour.None, PieceType.None) if the square is empty.</returns>
    public Piece GetPieceAt(Rank rank, File file) => _pieces[((int)rank * 8) + (int)file];

    /// <summary>
    /// Gets a piece at the specified square.
    /// </summary>
    /// <param name="square">The square coordinates.</param>
    /// <returns>The piece at the specified position, or a default piece (Colour.None, PieceType.None) if the square is empty.</returns>
    public Piece GetPieceAt(Square square) => GetPieceAt(square.Rank, square.File);

    /// <summary>
    /// Gets a board in the standard starting position for a new chess game.
    /// </summary>
    /// <remarks>
    /// This represents the traditional setup with pawns on ranks 2 and 7,
    /// and major/minor pieces on ranks 1 and 8 in their standard positions.
    /// </remarks>
    public static Board DefaultBoard => FromFen(FenGameState.DefaultGame.PieceLayout);

    internal static Board FromFen(FenGameState.FenSegment<FenGameState.PieceLayoutKind> piecesFenSection)
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

    internal static void ToFen(Board board, StringBuilder builder)
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

    internal Board WithMove(Move move)
    {
        PieceBuffer pieces = new();

        // Copy old board
        for (int i = 0; i < 64; i++)
        {
            if (_pieces[i] != Piece.None)
            {
                pieces[i] = _pieces[i];
            }
        }

        int sourceIndex = (int)move.Source.Rank * 8 + (int)move.Source.File;
        int destIndex = (int)move.Destination.Rank * 8 + (int)move.Destination.File;

        switch (move.MoveType)
        {
            case MoveType.Normal:
            {
                pieces[destIndex] = _pieces[sourceIndex];
                pieces[sourceIndex] = Piece.None;
                break;
            }
            case MoveType.PawnDouble:
            {
                pieces[destIndex] = _pieces[sourceIndex];
                pieces[sourceIndex] = Piece.None;
                break;
            }
            case MoveType.Promotion:
            {
                pieces[destIndex] = new(_pieces[sourceIndex].Colour, move.GetPromotionPieceType());
                pieces[sourceIndex] = Piece.None;
                break;
            }
            case MoveType.EnPassant:
            {
                Square enPassantTarget = move.GetEnPassantTarget();
                int enPassantIndex = (int)enPassantTarget.Rank * 8 + (int)enPassantTarget.File;

                pieces[destIndex] = _pieces[sourceIndex];
                pieces[enPassantIndex] = Piece.None;
                pieces[sourceIndex] = Piece.None;
                break;
            }
            case MoveType.Castling:
            {
                Square rookDestination = move.GetRookDestination();
                int rookDestinationIndex = (int)rookDestination.Rank * 8 + (int)rookDestination.File;

                Square rookSource = move.GetRookSource();
                int rookSourceIndex = (int)rookSource.Rank * 8 + (int)rookSource.File;

                pieces[destIndex] = _pieces[sourceIndex];
                pieces[rookDestinationIndex] = _pieces[rookSourceIndex];
                pieces[rookSourceIndex] = Piece.None;
                pieces[sourceIndex] = Piece.None;
                break;
            }
        }

        return new(pieces);
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

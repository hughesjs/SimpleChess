using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleChess.State;



/// <summary>
/// A fully validated FEN string representing a game state.
/// <example>
/// The FEN string representing a new game is:
/// rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
/// </example>
/// </summary>
public ref partial struct FenGameState
{
    internal FenSegment<PieceLayoutKind> PieceLayout;
    internal FenSegment<NextToPlayKind> NextToPlay;
    internal FenSegment<CastlingStateKind> CastlingState;
    internal FenSegment<EnPassantStateKind> EnPassantState;
    internal FenSegment<HalfTurnCounterKind> HalfTurnCounter;
    internal FenSegment<FullTurnCounterKind> FullTurnCounter;

    /// <summary>
    /// Represents the game state at the start of a new game.
    /// <code>
    /// rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
    /// </code>
    /// </summary>
    public static FenGameState DefaultGame => new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

    private static readonly HashSet<char> ValidPieceCharacters = [..("pnbrqkPNBRQK12345678")];
    private static readonly HashSet<string> ValidCastles = ["-", "K", "Q", "k", "q", "KQ", "Kk", "Kq", "Qk", "Qq", "kq", "KQk", "KQq", "Kkq", "Qkq", "KQkq"];

    private readonly string _fen;

    private FenGameState(string rawFen)
    {
        _fen = rawFen;

        ReadOnlySpan<char> fenSpan = rawFen.AsSpan();
        int space1 = rawFen.IndexOf(' ');
        int space2 = rawFen.IndexOf(' ', space1 + 1);
        int space3 = rawFen.IndexOf(' ', space2 + 1);
        int space4 = rawFen.IndexOf(' ', space3 + 1);
        int space5 = rawFen.IndexOf(' ', space4 + 1);

        PieceLayout = new(fenSpan[..space1]);
        NextToPlay = new(fenSpan[(space1 + 1)..space2]);
        CastlingState = new(fenSpan[(space2 + 1)..space3]);
        EnPassantState = new(fenSpan[(space3 + 1)..space4]);
        HalfTurnCounter = new(fenSpan[(space4 + 1)..space5]);
        FullTurnCounter = new(fenSpan[(space5 + 1)..]);
    }

    /// <summary>
    /// Returns the FEN string representation of this game state.
    /// </summary>
    /// <returns>The validated FEN string.</returns>
    public override string ToString() => _fen;

    /// <summary>
    /// Attempts to parse and validate a FEN (Forsyth-Edwards Notation) string.
    /// </summary>
    /// <param name="rawFen">The FEN string to parse.</param>
    /// <param name="fen">When this method returns, contains the parsed and validated FEN game state if successful; otherwise, the default value.</param>
    /// <returns><c>true</c> if the FEN string was successfully parsed and validated; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method validates all six components of a FEN string:
    /// <list type="number">
    /// <item><description>Piece placement: Must have 8 ranks separated by slashes, each rank totalling 8 files</description></item>
    /// <item><description>Active colour: Must be 'w' (White) or 'b' (Black)</description></item>
    /// <item><description>Castling rights: Must be valid combination of K, Q, k, q, or '-' for none</description></item>
    /// <item><description>En passant target: Must be '-' or a valid square in algebraic notation (e.g., 'e3')</description></item>
    /// <item><description>Halfmove clock: Must be a non-negative integer (0-150)</description></item>
    /// <item><description>Fullmove number: Must be a positive integer (1-8840)</description></item>
    /// </list>
    /// </remarks>
    public static bool TryParse(string rawFen, out FenGameState fen)
    {
        fen = default;
        string[] parts = rawFen.Split(' ');

        if (parts.Length != 6)
        {
            return false;
        }

        string pieceLayout = parts[0];

        string[] ranks = pieceLayout.Split('/');

        if (ranks.Length != 8)
        {
            return false;
        }

        bool allRanksHaveEightFiles = ranks.All(rank => rank.Sum(p => char.IsDigit(p) ? p - '0' : 1) == 8);

        if (!allRanksHaveEightFiles)
        {
            return false;
        }

        bool allPiecesAreValidCodes = ranks.All(rank => rank.All(p => ValidPieceCharacters.Contains(p)));

        if (!allPiecesAreValidCodes)
        {
            return false;
        }

        string colourToMove = parts[1];

        if (colourToMove is not ("w" or "b"))
        {
            return false;
        }

        string castling = parts[2];

        if (!ValidCastles.Contains(castling))
        {
            return false;
        }

        string enPassant = parts[3];

        if (!EnPassantRegex().IsMatch(enPassant))
        {
            return false;
        }

        string halfMove = parts[4];
        string fullMove = parts[5];

        if (!(int.TryParse(halfMove, out int halfMoveValue) && halfMoveValue >= 0))
        {
            return false;
        }

        if (!(int.TryParse(fullMove, out int fullMoveValue) && fullMoveValue > 0))
        {
            return false;
        }


        fen = new(rawFen);
        return true;
    }

    /// <summary>
    /// Represents a pre-validated segment of a FenGameState
    /// </summary>
    /// <typeparam name="TKind">The segment it refers to</typeparam>
    internal readonly ref struct FenSegment<TKind> where TKind : IFenSegmentKind
    {
        private readonly ReadOnlySpan<char> _value;

        /// <summary>
        /// DO NOT USE THIS OUTSIDE OF FenGameState (or test code)
        /// Unfortunately, there's no access modifier that lets me define a ctor only accessible from the containing type and nowhere else
        /// If you use this anywhere else, all validation guarantees go out of the window.
        /// </summary>
        /// <param name="value"></param>
        internal FenSegment(ReadOnlySpan<char> value) => _value = value;
        public ReadOnlySpan<char> Value => _value;
        public int Length => _value.Length;
        public bool IsEmpty => _value.IsEmpty;
        public char this[int index] => _value[index];
        public override string ToString() => _value.ToString();
        public ReadOnlySpan<char>.Enumerator GetEnumerator() => _value.GetEnumerator();
        public ReadOnlySpan<char> Slice(int start) => _value[start..];
        public ReadOnlySpan<char> Slice(int start, int length) => _value.Slice(start, length);
        public static implicit operator ReadOnlySpan<char>(FenSegment<TKind> segment) => segment._value;
    }

    internal interface IFenSegmentKind;
    internal readonly struct PieceLayoutKind: IFenSegmentKind;
    internal readonly struct NextToPlayKind: IFenSegmentKind;
    internal readonly struct CastlingStateKind: IFenSegmentKind;
    internal readonly struct EnPassantStateKind: IFenSegmentKind;
    internal readonly struct HalfTurnCounterKind: IFenSegmentKind;
    internal readonly struct FullTurnCounterKind: IFenSegmentKind;

    [GeneratedRegex(@"^(-|[a-h][36])$")]
    private static partial Regex EnPassantRegex();
}

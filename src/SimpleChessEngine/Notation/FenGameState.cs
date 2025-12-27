using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleChessEngine.Notation;

/// <summary>
/// A fully validated FEN string representing a board state.
/// <example>
/// The FEN string representing a new game is:
/// rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
/// </example>
/// </summary>
internal ref partial struct FenGameState
{
    public ReadOnlySpan<char> PieceLayout;
    public ReadOnlySpan<char> CurrentTurn;
    public ReadOnlySpan<char> CastlingState;
    public ReadOnlySpan<char> EnPassantState;
    public ReadOnlySpan<char> HalfTurnCounter;
    public ReadOnlySpan<char> FullTurnCounter;

    /// <summary>
    /// Represents the board at the start of a game.
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

        PieceLayout = fenSpan[..space1];
        CurrentTurn = fenSpan[(space1 + 1)..space2];
        CastlingState = fenSpan[(space2 + 1)..space3];
        EnPassantState = fenSpan[(space3 + 1)..space4];
        HalfTurnCounter = fenSpan[(space4 + 1)..space5];
        FullTurnCounter = fenSpan[(space5 + 1)..];
    }



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

    [GeneratedRegex(@"^(-|[a-h][36])$")]
    private static partial Regex EnPassantRegex();
}

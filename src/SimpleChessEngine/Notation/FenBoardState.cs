using System.Text.RegularExpressions;

namespace SimpleChessEngine.Notation;

internal partial record struct FenBoardState
{
    private static readonly HashSet<char> ValidPieceCharacters = [..("pnbrqkPNBRQK12345678")];
    private static readonly HashSet<string> ValidCastles = ["-", "K", "Q", "k", "q", "KQ", "Kk", "Kq", "Qk", "Qq", "kq", "KQk", "KQq", "Kkq", "Qkq", "KQkq"];

    private readonly string _fen;

    private FenBoardState(string rawFen)
    {
        _fen = rawFen;
    }

    public static bool TryParse(string rawFen, out FenBoardState fen)
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

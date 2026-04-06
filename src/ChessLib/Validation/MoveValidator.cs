using ChessLib.Entities;
using ChessLib.Figures;
using ChessLib.Utils;

namespace ChessLib.Validation;

public static class MoveValidator
{
    public static bool IsValidMove(int a, int b, int x, int y, Color playerColor, Field field)
    {
        return IsValidCoordinates(x, y) 
            && IsValidCoordinates(a, b) 
            && CheckMovementOnField(a, b, x, y, playerColor, field);
    }

    public static bool CheckMovementOnField(int a, int b, int x, int y, Color movingPlayerColor, Field field)
    {
        if (!IsValidCoordinates(x, y) || !IsValidCoordinates(a, b)) return false;
        Figure? figure = field.GetCell(a, b);
        return figure != null && IsPlayersFigure(figure, movingPlayerColor) && figure.GetMoveAction(x, y, field) != null;
    }

    public static bool CellIsSafe(int x, int y, Color color, Field field)
    {
        if (!IsValidCoordinates(x, y)) return false;
        foreach (Figure? f in field.GetField())
        {
            if (f != null && color != f.Color && f.CheckTake(x, y))
            {
                List<(int x, int y)> cells = PathAlgorithm.GetPath(f.A, f.B, x, y);
                if (cells.Count == 0 || cells.All(cell => field.GetCell(cell.x, cell.y) == null))
                    return false;
            }
        }
        return true;
    }

    public static bool PathIsClear(List<(int x, int y)> cells, Field field)
    {
        return cells.All(cell => field.GetCell(cell.x, cell.y) == null);
    }

    public static bool PathIsSafe(List<(int x, int y)> cells, Color color, Field field)
    {
        return cells.All(cell => CellIsSafe(cell.x, cell.y, color, field));
    }

    public static bool FigureIsSafeAfterMove(int x, int y, Figure figure, Field field)
    {
        if (!IsValidCoordinates(x, y)) return false;
        foreach (Figure? f in field.GetField())
        {
            if (f != null && figure.Color != f.Color && f.CheckTake(x, y))
            {
                List<(int x, int y)> cells = PathAlgorithm.GetPath(f.A, f.B, x, y);
                if (cells.Count == 0 || cells.All(cell => field.GetCell(cell.x, cell.y) == null || field.GetCell(cell.x, cell.y) == figure))
                    return false;
            }
        }
        return true;
    }

    public static bool CellIsSafeAfterMove(int x, int y, Figure figure, int a, int b, Field field)
    {
        if (!IsValidCoordinates(x, y) || !IsValidCoordinates(a, b)) return false;
        foreach (Figure? f in field.GetField())
        {
            if (f != null && figure.Color != f.Color && f.CheckTake(x, y) && f != field.GetCell(a, b))
            {
                List<(int x, int y)> cells = PathAlgorithm.GetPath(f.A, f.B, x, y);
                if (cells.Count == 0 || cells.All(cell => (field.GetCell(cell.x, cell.y) == null || field.GetCell(cell.x, cell.y) == figure) && (cell.x != a || cell.y != b)))
                    return false;
            }
        }
        return true;
    }

    public static TerminalPositionType? IsEndOfGame(Player defendingPlayer, Field field)
    {
        foreach (Figure f in defendingPlayer.GetFigures())
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (CheckMovementOnField(f.A, f.B, i, j, f.Color, field)) 
                        return null;
        }
        Figure king = field.GetKing(defendingPlayer.Color);
        if (CellIsSafe(king.A, king.B, king.Color, field)) return TerminalPositionType.StaleMate;
        else return TerminalPositionType.CheckMate;
    }

    public static bool IsValidCastling(King king, int x, Field field)
    {
        if (!IsValidCoordinates(x)) return false;
        int direction = (king.A < x) ? 1 : -1;
        int side = (direction == 1) ? 3 : -4;
        Figure? f = field.GetCell(king.A + side, king.B);
        if (f != null && f.Title == FigureType.Rook && king.Color == f.Color)
        {
            Rook rook = (Rook)f;
            if (rook.AmountMovesOfFigure == 0 && IsValidMove(rook.A, rook.B, king.A + direction, king.B, rook.Color, field))
                return true;
        }
        return false;
    }

    public static bool IsValidTakeOnPassage(Pawn p, int x, Field field)
    {
        if (!IsValidCoordinates(x)) return false;
        Figure? f = (p.A < x) ? field.GetCell(p.A + 1, p.B) : field.GetCell(p.A - 1, p.B);
        if (f != null && f.Title == FigureType.Pawn && f.AmountMovesOfFigure == 1)
        {
            ChessMove lastMove = field.moves[field.AmountMovesOnField];
            (int a, int b, int x, int y) coordinatesOfPlayerLastMove = (lastMove.blackMoves.Count == 0) ? lastMove.whiteMoves[0] : lastMove.blackMoves[0];
            if (coordinatesOfPlayerLastMove.x == x && coordinatesOfPlayerLastMove.y == p.B)
                return true;
        }
        return false;
    }
    
    public static bool IsValidCoordinates(int x, int y = 0) => x >= 0 && x < 8 && y >= 0 && y < 8;

    public static bool IsPlayersFigure(Figure figure, Color playerColor) => figure.Color == playerColor;
}
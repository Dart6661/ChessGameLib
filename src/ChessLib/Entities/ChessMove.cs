using ChessLib.MoveActions;
using ChessLib.MoveOptions;

namespace ChessLib.Entities;

public class ChessMove
{
    internal MoveCoordinates? whiteMove;
    internal MoveCoordinates? blackMove;
    internal MoveAction? whiteMoveAction;
    internal MoveAction? blackMoveAction;

    public Color ColorOfMovingPlayer() => (whiteMove == null || whiteMove != null && blackMove != null) ? Color.White : Color.Black;
    
    public MoveCoordinates? GetWhiteMove() => whiteMove;

    public MoveCoordinates? GetBlackMove() => blackMove;

    public MoveOption[] GetWhiteMoveOptions() => whiteMoveAction != null ? whiteMoveAction.GetOptions() : [];

    public MoveOption[] GetBlackMoveOptions() => blackMoveAction != null ? blackMoveAction.GetOptions() : [];

    internal void SetMove(MoveCoordinates move, MoveAction moveAction)
    {
        if (whiteMove == null)
        {
            whiteMove = move;
            whiteMoveAction = moveAction;
        }
        else
        {
            blackMove = move;
            blackMoveAction = moveAction;
        }
    }
}
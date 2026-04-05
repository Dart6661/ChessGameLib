using ChessLib.Entities;
using ChessLib.Exceptions;
using ChessLib.Figures;
using ChessLib.MoveActions;
using ChessLib.MoveOptions;
using ChessLib.Validation;

namespace ChessLib.GameLogic;

public class GameHandler
{
    public readonly Player whitePlayer;
    public readonly Player blackPlayer;
    public readonly Field field;

    public GameHandler()
    {
        whitePlayer = new(Color.White);
        blackPlayer = new(Color.Black);
        field = new(whitePlayer, blackPlayer);
    }

    public void MakeMove(int a, int b, int x, int y, params MoveOption[] moveOptions)
    {
        try
        {
            Player movingPlayer = GetMovingPlayer();
            Player defendingPlayer = GetDefendingPlayer();
            MoveValidator.IsValidMove(a, b, x, y, movingPlayer.Color, field);
            Figure f = field.GetCell(a, b)!;
            Move(f, x, y, [..moveOptions]);
            movingPlayer.AmountMovesOfPlayer++;
            MoveValidator.IsEndOfGame(defendingPlayer, field);
        }
        catch (CheckMateException) { throw; }
        catch (StaleMateException) { throw; }
    }

    public Player GetMovingPlayer() => (whitePlayer.AmountMovesOfPlayer == blackPlayer.AmountMovesOfPlayer) ? whitePlayer : blackPlayer;

    public Player GetDefendingPlayer() => (whitePlayer.AmountMovesOfPlayer > blackPlayer.AmountMovesOfPlayer) ? whitePlayer : blackPlayer;

    private void Move(Figure figure, int x, int y, params MoveOption[] moveOptions)
    {
        MoveAction? moveAction = figure.GetMoveAction(x, y, field);
        moveAction?.ExecuteMove(false, [..moveOptions]);
    }
}

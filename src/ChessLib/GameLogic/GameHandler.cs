using ChessLib.Entities;
using ChessLib.Figures;
using ChessLib.MoveActions;
using ChessLib.MoveOptions;
using ChessLib.Validation;

namespace ChessLib.GameLogic;

public class GameHandler
{
    public Player WhitePlayer { get; private set; }
    public Player BlackPlayer { get; private set; }
    public Field Field { get; private set; }

    public GameHandler()
    {
        WhitePlayer = new(Color.White);
        BlackPlayer = new(Color.Black);
        Field = new(WhitePlayer, BlackPlayer);
    }

    public MoveResult MakeMove(int a, int b, int x, int y, params MoveOption[] moveOptions)
    {
        Player movingPlayer = GetMovingPlayer();
        Player defendingPlayer = GetDefendingPlayer();
        if (!MoveValidator.IsValidMove(a, b, x, y, movingPlayer.Color, Field)) 
            return new MoveResult(false);
        Figure f = Field.GetCell(a, b)!;
        Move(f, x, y, [..moveOptions]);
        movingPlayer.AmountMovesOfPlayer++;
        TerminalPositionType? position = MoveValidator.IsEndOfGame(defendingPlayer, Field);
        return new MoveResult(true, position);
    }

    public Player GetMovingPlayer() => (WhitePlayer.AmountMovesOfPlayer == BlackPlayer.AmountMovesOfPlayer) ? WhitePlayer : BlackPlayer;

    public Player GetDefendingPlayer() => (WhitePlayer.AmountMovesOfPlayer > BlackPlayer.AmountMovesOfPlayer) ? WhitePlayer : BlackPlayer;

    private void Move(Figure figure, int x, int y, params MoveOption[] moveOptions)
    {
        MoveAction? moveAction = figure.GetMoveAction(x, y, Field);
        moveAction?.ExecuteMove(false, [..moveOptions]);
    }
}
namespace ChessLib.Entities;

public class MoveResult(bool isValid, TerminalPositionType? type = null)
{
    public bool IsValid { get; private set; } = isValid;
    public TerminalPositionType? TerminalType { get; private set; } = type;
}
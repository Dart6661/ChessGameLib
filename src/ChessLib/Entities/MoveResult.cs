namespace ChessLib.Entities;

public class MoveResult(bool isValid, TerminalPositionType? type = null)
{
    public bool IsValid { get; } = isValid;
    public TerminalPositionType? TerminalType { get; } = type;
}
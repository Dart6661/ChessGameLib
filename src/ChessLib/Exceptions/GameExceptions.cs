namespace ChessLib.Exceptions;

public class CheckMateException(string message) : Exception(message);

public class StaleMateException(string message) : Exception(message);
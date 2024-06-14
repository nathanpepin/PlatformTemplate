namespace Shared;

public record StatusMessage(Status Status, string Message, DateTime Logged)
{
    public override string ToString()
    {
        return $"[{Status}] - {Logged} - {Message}";
    }
}
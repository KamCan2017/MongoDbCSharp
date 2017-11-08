namespace Core
{
    public interface IBusyIndicator
    {
        bool Busy { get; set; }

        string Message { get; set; }
    }
}

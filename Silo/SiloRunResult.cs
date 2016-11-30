namespace Silo
{
    public class SiloRunResult
    {
        public SiloRunStatus status;
    }

    public enum SiloRunStatus
    {
        Ready,
        Error
    }
}

namespace RDR2DelayedPhotographyHelper.Abstract
{
    public interface ICountdownTimer
    {
        void Start();
        void Stop();
        void StopAndDispose();
    }
}
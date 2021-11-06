using System;
using System.Diagnostics.CodeAnalysis;
using System.Timers;
using RDR2DelayedPhotographyHelper.Abstract;

namespace RDR2DelayedPhotographyHelper.Timers
{
    public class CountdownTimer:ICountdownTimer,IDisposable
    {
        private bool _disposed=false;
        private Timer _timer;
        public ElapsedEventHandler OnTimeEndEvent{get;init;}
        protected CountdownTimer()
        {
            
        }
        public CountdownTimer(TimeSpan delayedTime,[NotNull]ElapsedEventHandler timeEndEvent):this()
        {
            OnTimeEndEvent=timeEndEvent;
            _timer=new Timer(delayedTime.TotalMilliseconds);
            _timer.Elapsed+=OnTimeEndEvent;
            _timer.AutoReset=false;//only execute once
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void StopAndDispose()
        {
            _timer.Stop();
            Dispose();
        }

        public void Dispose()
        {
            if(!_disposed)
            {
                _disposed=true;
                _timer.Dispose();
            }
        }
    }
}
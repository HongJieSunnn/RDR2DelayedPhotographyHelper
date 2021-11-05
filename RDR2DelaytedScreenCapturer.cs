using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using RDR2DelayedPhotographyHelper.Abstract;
using RDR2DelayedPhotographyHelper.Timers;
using TimeEndHandler = System.Timers.ElapsedEventHandler;

namespace RDR2DelayedPhotographyHelper
{
    public class RDR2DelayedScreenCapturer : IDelayedScreenCapturer,IDisposable,IAsyncDisposable
    {
        private bool _disposed=false;
        private Timer _timer;
        private CountdownTimer _countdownTimer;
        private readonly object _mutex=new object();
        private readonly IList<Image> _delayedPhotographies;
        private readonly IScreenCapturer _screenCapturer;
        private readonly AutoResetEvent _stopCountdownEvent;
        public TimeSpan DelayedTime { get;set; }
        public TimerCallback TimerCallback {get;set;}
        public TimeEndHandler TimeEndEvent { get;init; }

        /// <summary>
        /// dut time to invoke callback
        /// </summary>
        /// <value></value>
        public TimeSpan DueTime { get; set; }

        /// <summary>
        /// Period time to invoke callback again
        /// </summary>
        /// <value></value>
        public TimeSpan Period { get; set; }

        protected RDR2DelayedScreenCapturer()
        {
            _screenCapturer=new FullScreenCapturer();
            _stopCountdownEvent=new AutoResetEvent(false);
        }
        public RDR2DelayedScreenCapturer(Action<RDR2DelayedScreenCapturer> options):this()
        {
            options(this);
            //TODO 使用 C# 10 特性 required properties
            _delayedPhotographies=new List<Image>((int)(DelayedTime/Period)+1);

            TimerCallback=(state)=>
            {
                var screenShot=_screenCapturer.CaptureScreen();
                _delayedPhotographies.Add(screenShot);
            };
            
            TimeEndEvent=(source,args)=>
            {
                Dispose();//when countdowntime ends dispose _timer
                _stopCountdownEvent.Set();//To notify to stop and dispose countdownTimer
            };
        }

        public async Task<IEnumerable<Image>> GetDelayedPhotographiesAsync()
        {
            _countdownTimer=new CountdownTimer(DelayedTime,TimeEndEvent);

            _countdownTimer.Start();
            _timer=new Timer(TimerCallback,null,DueTime,Period);

            _stopCountdownEvent.WaitOne();
            _countdownTimer.StopAndDispose();

            await Task.Yield();
            return _delayedPhotographies;
        }

        public async Task SaveDelayedPhotographiesAsync()
        {
            _screenCapturer.Saved=true;
            TimerCallback=(state)=>
            {
                _screenCapturer.CaptureScreen();
            };

            _countdownTimer=new CountdownTimer(DelayedTime,TimeEndEvent);

            _countdownTimer.Start();
            _timer=new Timer(TimerCallback,null,DueTime,Period);

            _stopCountdownEvent.WaitOne();
            _countdownTimer.StopAndDispose();

            await Task.Yield();
        }

        public void Dispose()
        {
            if(!_disposed)
            {
                _disposed=true;
                lock(_mutex)
                {
                    _timer.Dispose();
                }
            }
        }

        public ValueTask DisposeAsync()
        {
            if(!_disposed)
            {
                _disposed=true;
                return _timer.DisposeAsync();
            }
            return ValueTask.CompletedTask;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using TimeEndHandler = System.Timers.ElapsedEventHandler;

namespace RDR2DelayedPhotographyHelper.Abstract
{
    public interface IDelayedScreenCapturer
    {
        TimeSpan DelayedTime { get; set; }
        TimerCallback TimerCallback{get;set;}
        TimeEndHandler TimeEndEvent{get;init;}
        Task<IEnumerable<Image>> GetDelayedPhotographiesAsync();
        /// <summary>
        /// capture screen each period time in delaytime
        /// </summary>
        /// <returns>directory to save images</returns>
        Task<string> SaveDelayedPhotographiesAsync();
    }
}
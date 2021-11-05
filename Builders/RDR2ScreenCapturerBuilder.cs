using System;
using RDR2DelayedPhotographyHelper.Abstract;

namespace RDR2DelayedPhotographyHelper.Builders
{
    public class RDR2ScreenCapturerBuilder
    {
        public static IDelayedScreenCapturer BuildDefault()
        {
            var defaultInstance=new RDR2DelayedScreenCapturer((options)=>
            {
                options.DelayedTime=TimeSpan.FromMinutes(30);
                options.DueTime=TimeSpan.Zero;
                options.Period=TimeSpan.FromSeconds(15);
            });
            return defaultInstance;
        }
    }
}
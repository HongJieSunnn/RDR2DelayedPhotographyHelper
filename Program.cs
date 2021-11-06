using System;
using System.Threading;
using System.Threading.Tasks;
using RDR2DelayedPhotographyHelper.Abstract;
using RDR2DelayedPhotographyHelper.Builders;
using RDR2DelayedPhotographyHelper.DependencyInjectionHelpers;
using RDR2DelayedPhotographyHelper.LocalOperators;
using RDR2DelayedPhotographyHelper.VideoSynthesizers;

namespace RDR2DelayedPhotographyHelper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DependencyInjectionHelper.BuildServiceProvider();
            var cancellationToken=new CancellationToken();

            // System.Console.WriteLine("press to start");
            // Console.ReadKey();
            // var capturer=new RDR2DelayedScreenCapturer((options)=>
            // {
            //     options.DelayedTime=TimeSpan.FromMinutes(2);
            //     options.DueTime=TimeSpan.Zero;
            //     options.Period=TimeSpan.FromSeconds(0.5);
            // });
            // var imagePath= await capturer.SaveDelayedPhotographiesAsync();

            //var capturer=RDR2ScreenCapturerBuilder.BuildDefault();
            
            var videoSync=new DelayedPhotographyVideoSynthesizer();
            var imageOperator=new LocalImagesOperator(@"G:\RDR2Screenshot\images\2021-11-06 06-11-30");
            await videoSync.SyntheticVideoFromSourcePathAsync(imageOperator.GetLocalFilesNameByPattern("*.png"),cancellationToken);

            // var capturer=new FullScreenCapturer();
            // capturer.CaptureScreen();
        }
    }
}

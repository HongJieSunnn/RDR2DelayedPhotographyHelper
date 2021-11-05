using System;
using System.Threading;
using System.Threading.Tasks;
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

            var localImagesOperator=new LocalImagesOperator();
            var delayedPhotographyVideoSynthesizer=new DelayedPhotographyVideoSynthesizer();
            var cancellationToken=new CancellationToken();
            var fileNames=localImagesOperator.GetLocalImageFilenames();

            await delayedPhotographyVideoSynthesizer.SyntheticVideoFromSourcePathAsync(fileNames,cancellationToken);
        }
    }
}

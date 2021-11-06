using Microsoft.Extensions.DependencyInjection;
using RDR2DelayedPhotographyHelper.DependencyInjectionHelpers;
using Serilog;
using Xabe.FFmpeg.Events;

namespace RDR2DelayedPhotographyHelper.VideoSynthesizers
{
    public sealed class ConversionProgressEvents
    {
        private static ILogger _logger=DependencyInjectionHelper.ServiceProvider.GetService<ILogger>();
        public static ConversionProgressEventHandler OnVideoSyntheticing=(sender,args)=>
        {
            _logger.Information($"Video is syntheticing - Process id:{args.ProcessId} {args.Percent}%");
        };

        public static ConversionProgressEventHandler OnVideoModifying=(sender,args)=>
        {
            _logger.Information($"Video is modifying from source file to dest file - Process id:{args.ProcessId} {args.Percent}%");
        };
    }
}
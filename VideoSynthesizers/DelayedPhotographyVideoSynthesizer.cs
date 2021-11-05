using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using RDR2DelayedPhotographyHelper.Abstract;
using RDR2DelayedPhotographyHelper.DependencyInjectionHelpers;
using Serilog;
using Xabe.FFmpeg;

namespace RDR2DelayedPhotographyHelper.VideoSynthesizers
{
    public class DelayedPhotographyVideoSynthesizer : IVideoSynthesizer
    {
        private readonly IConversion _videoConversion;
        private readonly ILogger _logger;
        public DelayedPhotographyVideoSynthesizer()
        {
            _videoConversion=new Xabe.FFmpeg.Conversion();
            _logger=DependencyInjectionHelper.ServiceProvider.GetService<ILogger>();
        }

        public async Task SyntheticVideoFromSourcePathAsync(IEnumerable<string> sources, CancellationToken cancellationToken)
        {
            _videoConversion.OnProgress+=(sender,args)=>
            {
                _logger.Information($"Video is syntheticing - {args.Percent}%");
            };
            //TODO create video file that named by datetime
            var fileInfos=sources.Select(path=>new ImageInfo(path)).Where(info=>info.Height==1080&&info.Width==1920).ToArray();
            FFMpeg.JoinImageSequence($@"G:\RDR2Screenshot\output.mp4", frameRate: 20,fileInfos);
            //
            var videoInfo=await Xabe.FFmpeg.FFmpeg.GetMediaInfo($@"G:\RDR2Screenshot\output.mp4");
            var videoStream= videoInfo.VideoStreams.First().ChangeSpeed(0.5);
            await _videoConversion.AddStream(videoStream).SetOutput($@"G:\RDR2Screenshot\output1.mp4").Start();
        }

        public Task SyntheticVideoFromSourcesAsync<T>(IEnumerable<T> sources, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
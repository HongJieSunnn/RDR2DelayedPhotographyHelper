using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        private readonly ILocalOperator _localOperator;
        private readonly ILogger _logger;
        private const string OutputPath=@"G:\RDR2Screenshot\";
        public DelayedPhotographyVideoSynthesizer()
        {
            _videoConversion=DependencyInjectionHelper.ServiceProvider.GetService<IConversion>();
            _logger=DependencyInjectionHelper.ServiceProvider.GetService<ILogger>();
            _localOperator=DependencyInjectionHelper.ServiceProvider.GetService<ILocalOperator>();
        }

        public async Task<IConversionResult> SyntheticVideoFromSourcePathAsync(IEnumerable<string> sources, CancellationToken cancellationToken)
        {
            _videoConversion.OnProgress+=ConversionProgressEvents.OnVideoSyntheticing;

            var fileName=$"{DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss")}-initial.mp4";
            var createWrittenVideoTask=CreateWrittenVideoFileAsync(fileName);
            
            //Reading files' info by sources
            _logger.Information($"Reading files' info by sources");
            var fileInfos=sources.Select(path=>new ImageInfo(path)).ToArray();//TODO
            _logger.Information($"Read files' info by sources successfully");
            
            //Create video to write
            await createWrittenVideoTask;

            //synthetic video by images store in disk
            _logger.Information($"Converting images into {fileName}");
            var isSucc=FFMpeg.JoinImageSequence($@"{OutputPath}{fileName}", frameRate: 24,fileInfos);
            if(isSucc)
                _logger.Information($"Convert images into {fileName} successfully");
            else
                _logger.Error($"Convert images into {fileName} failed");

            await ModifyVideoFileAsync($@"{OutputPath}{fileName}",$@"{OutputPath}{fileName.Replace("mp4","avi").Replace("initial","final")}",(info)=>
            {
                var videoStream=info.VideoStreams.First();
                return videoStream.SetBitrate(videoStream.Bitrate);
            });
            //change video speed 
            var copyResult=await ModifyVideoFileAsync($@"{OutputPath}{fileName}",$@"{OutputPath}{fileName.Replace("avi","mp4").Replace("initial","final")}",(info)=>
            {
                var videoStream=info.VideoStreams.First();
                return videoStream.SetBitrate(videoStream.Bitrate).ChangeSpeed(0.5);
            });
            return copyResult;
        }

        public Task<IConversionResult> SyntheticVideoFromSourcesAsync<T>(IEnumerable<T> sources, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Create a empty video file from empty.mp4 and then convert images to this file
        /// </summary>
        /// <returns></returns>
        private Task CreateWrittenVideoFileAsync([NotNull]string fileName)
        {
            _logger.Information($"Creating wirtten video - name:{fileName}");

            _localOperator.CreateLocalFile($@"{OutputPath}{fileName}");

            _logger.Information($"Create wirtten video successfully - name:{fileName}");

            return Task.CompletedTask;
        }
        /// <summary>
        /// Modify video file from source file to dest file by modifyOptions.Also,this method can copy file.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="copyOptions"></param>
        /// <returns></returns>
        private async Task<IConversionResult> ModifyVideoFileAsync(string source,string dest,Func<IMediaInfo,IVideoStream> modifyOptions)
        {
            //dest file should not be existed or will not end while output video.
            if(File.Exists(dest))
            {
                File.Delete(dest);
            }
            //TODO check source and dest format
            var videoInfo=await Xabe.FFmpeg.FFmpeg.GetMediaInfo(source);
            var viedeoStream=modifyOptions(videoInfo);
            _logger.Information($"Start modifying source video to dest video - soruce:{source} dest:{dest}");
            return await _videoConversion.AddStream(viedeoStream).SetOutput(dest).Start();
        }
    }
}
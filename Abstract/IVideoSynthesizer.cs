using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace RDR2DelayedPhotographyHelper.Abstract
{
    public interface IVideoSynthesizer
    {
        Task<IConversionResult> SyntheticVideoFromSourcesAsync<T>(IEnumerable<T> sources,CancellationToken cancellationToken);
        Task<IConversionResult> SyntheticVideoFromSourcePathAsync(IEnumerable<string> sources,CancellationToken cancellationToken);
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RDR2DelayedPhotographyHelper.Abstract
{
    public interface IVideoSynthesizer
    {
        Task SyntheticVideoFromSourcesAsync<T>(IEnumerable<T> sources,CancellationToken cancellationToken);
        Task SyntheticVideoFromSourcePathAsync(IEnumerable<string> sources,CancellationToken cancellationToken);
    }
}
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace RDR2DelayedPhotographyHelper.Abstract
{
    public interface ILocalImagesOperator
    {
        IEnumerable<Image> ReadLocalImages();
        Task<IEnumerable<Image>> ReadLocalImagesAsync();
        IEnumerable<string> GetLocalImageFilenames();
        Task<IEnumerable<string>> GetLocalImageFilenamesAsync();
    }
}
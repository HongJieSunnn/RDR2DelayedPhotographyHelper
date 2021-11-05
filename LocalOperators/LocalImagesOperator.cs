using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RDR2DelayedPhotographyHelper.Abstract;

namespace RDR2DelayedPhotographyHelper.LocalOperators
{
    public class LocalImagesOperator : ILocalImagesOperator
    {
        private string _imagePath;
        public LocalImagesOperator(string path=@"G:\RDR2Screenshot\images")
        {
            _imagePath=path;
        }

        public IEnumerable<Image> ReadLocalImages()
        {
            var files=GetLocalImageFilenames();
            var images=new List<Image>(files.Count());
            foreach (var file in files)
            {
                images.Add(Image.FromFile(file));
            }
            return images;
        }

        public async Task<IEnumerable<Image>> ReadLocalImagesAsync()
        {
            await Task.Yield();
            return ReadLocalImages();
        }

        public IEnumerable<string> GetLocalImageFilenames()
        {
            return Directory.EnumerateFiles(_imagePath,"*.png");
        }

        public async Task<IEnumerable<string>> GetLocalImageFilenamesAsync()
        {
            await Task.Yield();
            return GetLocalImageFilenames();
        }
    }
}
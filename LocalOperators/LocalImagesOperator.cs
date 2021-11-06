using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RDR2DelayedPhotographyHelper.Abstract;

namespace RDR2DelayedPhotographyHelper.LocalOperators
{
    public class LocalImagesOperator : ILocalOperator
    {
        public string Path{get;set;}
        public LocalImagesOperator(string path=@"G:\RDR2Screenshot\images")
        {
            Path=path;
        }

        public IEnumerable<Image> ReadLocalFiles()
        {
            var files=GetLocalFilesName();
            var images=new List<Image>(files.Count());
            foreach (var file in files)
            {
                images.Add(Image.FromFile(file));
            }
            return images;
        }

        public async Task<IEnumerable<Image>> ReadLocalFilesAsync()
        {
            await Task.Yield();
            return ReadLocalFiles();
        }

        public IEnumerable<string> GetLocalFilesName()
        {
            return Directory.EnumerateFiles(Path);
        }

        public async Task<IEnumerable<string>> GetLocalFilesNameAsync()
        {
            await Task.Yield();
            return GetLocalFilesName();
        }

        public IEnumerable<string> GetLocalFilesNameByPattern(string pattern)
        {
            return Directory.EnumerateFiles(Path,pattern);
        }

        public void CreateLocalFile(string fileNamePath)
        {
            throw new System.NotImplementedException();
        }
    }
}
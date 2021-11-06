using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using RDR2DelayedPhotographyHelper.Abstract;

namespace RDR2DelayedPhotographyHelper.LocalOperators
{
    public class LocalVideoOperator : ILocalOperator
    {
        public string Path { get; set; }
        public LocalVideoOperator(string path=@"G:\RDR2Screenshot\")
        {
            Path=path;
        }

        public void CreateLocalFile(string fileNamePath)
        {
            if(!File.Exists(fileNamePath))
            {
                using (File.Create(fileNamePath))
                {
                     
                }
            }
        }

        public IEnumerable<string> GetLocalFilesName()
        {
            return Directory.EnumerateFiles(Path);
        }

        public IEnumerable<string> GetLocalFilesNameByPattern(string pattern)
        {
            return Directory.EnumerateFiles(Path,pattern);
        }

        public IEnumerable<Image> ReadLocalFiles()
        {
            throw new System.NotImplementedException();
        }
    }
}
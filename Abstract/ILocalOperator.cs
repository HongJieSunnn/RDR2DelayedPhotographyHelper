using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace RDR2DelayedPhotographyHelper.Abstract
{
    public interface ILocalOperator
    {
        IEnumerable<Image> ReadLocalFiles();
        IEnumerable<string> GetLocalFilesName();
        IEnumerable<string> GetLocalFilesNameByPattern(string pattern);
        void CreateLocalFile(string fileNamePath);
    }
}
using System.Drawing;
using System.Threading.Tasks;

namespace RDR2DelayedPhotographyHelper.Abstract
{
    public interface IScreenCapturer
    {
        bool Saved { get; set; }
        string ImagePath{get;}
        Image CaptureScreen();
        Task<Image> CaptureScreenAsync();
    }
}
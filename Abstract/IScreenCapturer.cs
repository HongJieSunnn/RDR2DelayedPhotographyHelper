using System.Drawing;
using System.Threading.Tasks;

namespace RDR2DelayedPhotographyHelper.Abstract
{
    public interface IScreenCapturer
    {
        bool Saved { get; set; }
        Image CaptureScreen();
        Task<Image> CaptureScreenAsync();
    }
}
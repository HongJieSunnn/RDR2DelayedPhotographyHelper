using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Management;
using System.Threading.Tasks;
using RDR2DelayedPhotographyHelper.Abstract;

namespace RDR2DelayedPhotographyHelper
{
    public class FullScreenCapturer : IScreenCapturer
    {
        private const string ImagePath=@"G:\RDR2Screenshot\images";
        private const string SeachStatement="SELECT CurrentHorizontalResolution, CurrentVerticalResolution FROM Win32_VideoController";
        private readonly ManagementObjectSearcher _searcher;
        public bool Saved { get; set; }
        public FullScreenCapturer()
        {
            _searcher=new ManagementObjectSearcher(SeachStatement);
        }

        public Image CaptureScreen()
        {
            var fullScreenParams=GetScreenResolution();
            Image fullScreenImage=new Bitmap(fullScreenParams.width,fullScreenParams.height);
            
            using (Graphics g=Graphics.FromImage(fullScreenImage))
            {
                g.CopyFromScreen(0,0,0,0,fullScreenImage.Size);
            }
            if(Saved)
                SaveImageAsync(fullScreenImage);

            return fullScreenImage;
        }

        public async Task<Image> CaptureScreenAsync()
        {
            await Task.Yield();
            return CaptureScreen();
        }

        private void SaveImageAsync(Image img)
        {
            if(!Directory.Exists(ImagePath))
            {
                Directory.CreateDirectory(ImagePath);
            }
            var fileName=DateTime.Now.ToString();
            img.Save($"{ImagePath}{fileName}",ImageFormat.Jpeg);
        }

        /// <summary>
        /// To get full screen resolution
        /// </summary>
        /// <param name="width"></param>
        /// <param name="GetScreenResolution("></param>
        /// <returns></returns>
        private (int width,int height) GetScreenResolution()
        {
            int width=0;
            int height=0;
            foreach (var record in _searcher.Get())
            {
                width=int.Parse(record.GetPropertyValue("CurrentHorizontalResolution").ToString());
                height=int.Parse(record.GetPropertyValue("CurrentVerticalResolution").ToString());
            }
            return (width,height);
        }
    }
}
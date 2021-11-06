using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Management;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RDR2DelayedPhotographyHelper.Abstract;
using RDR2DelayedPhotographyHelper.DependencyInjectionHelpers;
using RDR2DelayedPhotographyHelper.Win32APIs;
using Serilog;

namespace RDR2DelayedPhotographyHelper
{
    public class FullScreenCapturer : IScreenCapturer
    {
        private const string SeachStatement="SELECT CurrentHorizontalResolution, CurrentVerticalResolution FROM Win32_VideoController";
        private int _width=1920;
        private int _height=1080;
        private readonly ManagementObjectSearcher _searcher;
        private readonly ILogger _logger;
        public bool Saved { get; set; }
        public string ImagePath{get;private set;}=$@"G:\RDR2Screenshot\images\";
        public FullScreenCapturer()
        {
            _searcher=new ManagementObjectSearcher(SeachStatement);
            _logger=DependencyInjectionHelper.ServiceProvider.GetService<ILogger>();
            ImagePath+=$@"{DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss")}\";
            (_width,_height)=GetScreenResolution();
        }

        public Image CaptureScreen()
        {   
            MakeConsoleForeground();
            var fullScreenImage=CaptureScreenByGraphics();
            if(Saved)
                SaveImage(fullScreenImage);

            return fullScreenImage;
        }

        private Image CaptureScreenByGraphics()
        {
            Image fullScreenImage=new Bitmap(_width,_height);
            using (Graphics g=Graphics.FromImage(fullScreenImage))
            {
                g.CopyFromScreen(0,0,0,0,fullScreenImage.Size);
            }
            return fullScreenImage;
        }

        private void MakeConsoleForeground()
        {
            IntPtr handle = Win32APIInvoker.GetConsoleWindow();
            Console.WriteLine(Win32APIInvoker.SetForegroundWindow(handle));
        }

        public async Task<Image> CaptureScreenAsync()
        {
            await Task.Yield();
            return CaptureScreen();
        }

        private void SaveImage(Image img)
        {
            if(!Directory.Exists(ImagePath))
            {
                Directory.CreateDirectory(ImagePath);
            }
            var timeForName=DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
            var fileName=$@"{ImagePath}{timeForName}.png";
            img.Save(fileName,ImageFormat.Png);

            _logger.Information($"Capature screen and save image successfully - file:{fileName}");
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
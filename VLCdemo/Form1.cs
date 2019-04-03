using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VLCdemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //this.vlcControl1.SetMedia(new Uri("rtsp://admin:Admin12345@192.168.200.33/h265/ch1/main/av_stream"), new string[] { "-vv" });
            //this.vlcControl1.SetMedia(new Uri("rtsp://admin:admin12345@192.168.44.31/video1"), new string[] { "-vv" });
            //this.vlcControl1.SetMedia(new Uri("rtsp://192.168.10.32:5554/eye3"), new string[] { "-vv" });
            this.vlcControl1.SetMedia(new Uri("http://192.168.81.125:5050/video_feed"), new string[] { "-vv" });
            //this.vlcControl1.SetMedia(new Uri("rtsp://admin:admin12345@192.168.11.12/MediaInput/h264"), new string[] { "-vv" });
            //this.vlcControl1.Paint += VlcControl1_Paint;
        }

 

        //private void VlcControl1_Paint(object sender, PaintEventArgs e)
        //{
        //    var g = e.Graphics;
        //    g.DrawLine(new Pen(Color.Red), 0, 0, 100, 200);
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
           
            var state = this.vlcControl1.State.ToString();
            Console.WriteLine(state);    
            this.vlcControl1.Play(); // (new Uri("rtsp://192.168.44.20/MediaInput/h264"), new string[] { "-vv" });

            Record();
        }
        Vlc.DotNet.Core.VlcMediaPlayer mediaPlayer;
        private void Record()
        {
            var libDirectory = this.vlcControl1.VlcLibDirectory;

            var options = new string[]
            {
                "-vv"
                // VLC options can be given here. Please refer to the VLC command line documentation.
            };

            mediaPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(libDirectory);

            var mediaOptions = new string[]
            {
                ":sout=#file{dst="+Path.Combine(Environment.CurrentDirectory, "output.mov")+"}",
                ":sout-keep"
            };

            mediaPlayer.SetMedia(new Uri("http://192.168.81.125:5050/video_feed"), mediaOptions);


            mediaPlayer.Play();
            /*
             * https://github.com/ZeBobo5/Vlc.DotNet/wiki/Getting-started
             * 
             */

        }

        private void vlcControl1_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            // Default installation path of VideoLAN.LibVLC.Windows
            e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            /*
            作者：小徐不知道
            链接：https://www.jianshu.com/p/83f17f658a6a
            來源：简书
            简书著作权归作者所有，任何形式的转载都请联系作者获得授权并注明出处。
            */
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.mediaPlayer.Stop();
        }
    }
}

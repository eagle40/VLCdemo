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
            this.vlcControl1.SetMedia(new Uri("rtsp://admin:Admin12345@192.168.200.33/h265/ch1/main/av_stream"), new string[] { "-vv" });
            //this.vlcControl1.SetMedia(new Uri("rtsp://admin:admin12345@192.168.44.31/video1"), new string[] { "-vv" });
            //this.vlcControl1.SetMedia(new Uri("rtsp://192.168.10.32:5554/eye3"), new string[] { "-vv" });
            //this.vlcControl1.SetMedia(new Uri("http://192.168.81.125:5000/video_feed"), new string[] { "-vv" });
            this.timer1.Tick += Timer1_Tick;
            //this.vlcControl1.Paint += VlcControl1_Paint;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.Text = (this.vlcControl1.State.ToString());

        }

        private void VlcControl1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawLine(new Pen(Color.Red), 0, 0, 100, 200);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var state = this.vlcControl1.State.ToString();
            Console.WriteLine(state);    
            this.vlcControl1.Play(); // (new Uri("rtsp://192.168.44.20/MediaInput/h264"), new string[] { "-vv" });

            this.timer1.Start();
                   
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
        Point basepoint;
        bool m_down = false;

        private void vlcControl1_MouseDown(object sender, MouseEventArgs e)
        {
            basepoint = e.Location;
            m_down = true;
        }

        private void vlcControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_down)
            {
                //实例化一个和窗口一样大的位图
                var i = new Bitmap(this.Width, this.Height);
                //创建位图的gdi对象
                var g = Graphics.FromImage(i);
                //创建画笔
                var p = new Pen(Color.Red, 2.0f);
                //指定线条的样式为划线段
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                //根据当前位置画图，使用math的abs()方法求绝对值
                if (e.X < basepoint.X && e.Y < basepoint.Y)
                    g.DrawRectangle(p, e.X, e.Y, System.Math.Abs(e.X - basepoint.X), System.Math.Abs(e.Y - basepoint.Y));
                else if (e.X > basepoint.X && e.Y < basepoint.Y)
                    g.DrawRectangle(p, basepoint.X, e.Y, System.Math.Abs(e.X - basepoint.X), System.Math.Abs(e.Y - basepoint.Y));
                else if (e.X < basepoint.X && e.Y > basepoint.Y)
                    g.DrawRectangle(p, e.X, basepoint.Y, System.Math.Abs(e.X - basepoint.X), System.Math.Abs(e.Y - basepoint.Y));
                else
                    g.DrawRectangle(p, basepoint.X, basepoint.Y, System.Math.Abs(e.X - basepoint.X), System.Math.Abs(e.Y - basepoint.Y));

                //将位图贴到窗口上
                this.BackgroundImage = i;
                //释放gid和pen资源
                g.Dispose();
                p.Dispose();

            }


        }

        private void vlcControl1_MouseUp(object sender, MouseEventArgs e)
        {
            var i = new Bitmap(this.Width, this.Height);
            var g = Graphics.FromImage(i);
            g.Clear(Color.Transparent);
            this.BackgroundImage = i;
            g.Dispose();

            //标志位置低
            m_down = false;

            /* ---------------------
            作者：哀歌与世无争
            来源：CSDN
            原文：https://blog.csdn.net/yxy244/article/details/78439848 
            版权声明：本文为博主原创文章，转载请附上博文链接！ */
        }
    }
}

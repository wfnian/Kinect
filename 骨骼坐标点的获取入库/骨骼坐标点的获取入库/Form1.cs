using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using Microsoft.Kinect;
using System.Data.SqlClient;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;
using CCWin;
using System.Threading;

namespace 骨骼坐标点的获取入库     // 不好意思命名我用了汉字。
{

    public partial class Form1 : CCSkinMain
    {
        private int click_times = 0;
        private delegate void FlushClient();

        private const string pos_Pic = "F:\\kinect\\Kinect\\pic\\pic.jpg";   // 注意路径
        private String connsql = "server=.;database=bone_pos;integrated security=SSPI";
        private Image<Bgr, Byte> skeletonImage;
        
        int depthWidth, depthHeight;
        private double hipcenter_handleft1 = 0, hipright_handright2 = 0, handright_kneeright3 = 0, handleft_kneeleft4 = 0, elbowleft_hipleft5 = 0;
        private double elbowright_hipright6 = 0, footleft_footright7 = 0, handleft_footleft8 = 0, handright_footright9 = 0, handleft_handright10 = 0;
        private double handleft_head11 = 0, handright_head12 = 0, elbowright_kneeleft13 = 0, elbowleft_kneeright14 = 0;

        private string[] pos = { "起势", "左右野马分鬃", "白鹤亮翅", "左右搂膝拗步", "手挥琵琶",
            "左右倒卷肱", "左揽雀尾", "右拦雀尾", "单鞭", "云手", "高探马", "右蹬脚", "双峰贯耳",
            "转身左蹬脚", "左下式独立", "左下式独立", "左右穿梭", "海底针", "闪通臂", "转身搬拦捶",
            "如封似闭", "十字手", "预备势" };

        private Skeleton[] skeletonData;// 按理说是识别六人，size = 6
        private MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_COMPLEX, 0.3, 0.3);
        // 数据缓冲存储空间
        ColorImageFormat colorImageFormat;
        DepthImageFormat depthImageFormat;

        private KinectSensor sensor;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Load(pos_Pic);
            this.Location = (Point)new Size(100, 1);

            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }//连接设备;
            if (null != this.sensor)
            {   // 初始化 Kinect 设置     
                colorImageFormat = ColorImageFormat.RgbResolution640x480Fps30;
                depthImageFormat = DepthImageFormat.Resolution640x480Fps30;
                this.sensor.SkeletonStream.Enable();//设备骨骼可用;

                this.sensor.ColorStream.Enable(colorImageFormat);
                this.sensor.DepthStream.Enable(depthImageFormat);
                depthWidth = this.sensor.DepthStream.FrameWidth;
                depthHeight = this.sensor.DepthStream.FrameHeight;
                skeletonImage = new Image<Bgr, byte>(depthWidth, depthHeight);
                skeletonImage.Draw(new Rectangle(0, 0, depthWidth, depthHeight), new Bgr(0.0, 0.0, 0.0), -1);
                imageBox1.Image = skeletonImage;
                this.skeletonData = new Skeleton[this.sensor.SkeletonStream.FrameSkeletonArrayLength];

                this.sensor.SkeletonFrameReady += SensorSkeletonFrameReady;//事件处理;
                try
                {
                    this.sensor.Start();
                }
                catch
                {
                    ;
                }

                this.toolStripStatusLabel1.Text = " " + DateTime.Now.ToString("yyyy-MM-dd hh:mm ");
                this.toolStripStatusLabel2.Text = "\tKinect设备已连接，工作正常.";

            }
            else
            {
                MessageBox.Show("设备未就绪！", "未连接");
                skinButton2.Enabled = false;
            }

        }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {


            bool received = false;

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletonFrame.CopySkeletonDataTo(this.skeletonData);
                    received = true;
                }
            }

            if (received)
            {
                // 重绘整个画面,冲掉原有骨骼图像
                skeletonImage.Draw(new Rectangle(0, 0, skeletonImage.Width, skeletonImage.Height), new Bgr(0.0, 0.0, 0.0), -1);

                DrawSkeletons(skeletonImage, 0);
                imageBox1.Image = skeletonImage;


                FlushClient fc = new FlushClient(ShowIn12Label);//多线程
                fc.BeginInvoke(null, null);
            }
            
            

        }

        private void ShowIn12Label()
        {
            // 调用采用多线程实现实时数据展现。
            if (DateTime.Now.Second % 5 == 0)
            {

                foreach (Skeleton skeleton in this.skeletonData)
                {
                    if (skeleton == null)
                        continue;
                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        hipcenter_handleft1 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HipCenter].Position.X - skeleton.Joints[JointType.HandLeft].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HipCenter].Position.Y - skeleton.Joints[JointType.HandLeft].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HipCenter].Position.Z - skeleton.Joints[JointType.HandLeft].Position.Z), 2));

                        hipright_handright2 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HipRight].Position.X - skeleton.Joints[JointType.HandRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HipRight].Position.Y - skeleton.Joints[JointType.HandRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HipRight].Position.Z - skeleton.Joints[JointType.HandRight].Position.Z), 2));

                        handright_kneeright3 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HandRight].Position.X - skeleton.Joints[JointType.KneeRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandRight].Position.Y - skeleton.Joints[JointType.KneeRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandRight].Position.Z - skeleton.Joints[JointType.KneeRight].Position.Z), 2));

                        handleft_kneeleft4 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HandLeft].Position.X - skeleton.Joints[JointType.KneeLeft].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Y - skeleton.Joints[JointType.KneeLeft].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Z - skeleton.Joints[JointType.KneeLeft].Position.Z), 2));

                        elbowleft_hipleft5 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.ElbowLeft].Position.X - skeleton.Joints[JointType.HipLeft].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowLeft].Position.Y - skeleton.Joints[JointType.HipLeft].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowLeft].Position.Z - skeleton.Joints[JointType.HipLeft].Position.Z), 2));

                        elbowright_hipright6 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.ElbowRight].Position.X - skeleton.Joints[JointType.HipRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowRight].Position.Y - skeleton.Joints[JointType.HipRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowRight].Position.Z - skeleton.Joints[JointType.HipRight].Position.Z), 2));

                        footleft_footright7 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.FootLeft].Position.X - skeleton.Joints[JointType.FootRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.FootLeft].Position.Y - skeleton.Joints[JointType.FootRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.FootLeft].Position.Z - skeleton.Joints[JointType.FootRight].Position.Z), 2));

                        handleft_footleft8 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.FootLeft].Position.X - skeleton.Joints[JointType.HandLeft].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.FootLeft].Position.Y - skeleton.Joints[JointType.HandLeft].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.FootLeft].Position.Z - skeleton.Joints[JointType.HandLeft].Position.Z), 2));

                        handright_footright9 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HandRight].Position.X - skeleton.Joints[JointType.FootRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandRight].Position.Y - skeleton.Joints[JointType.FootRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandRight].Position.Z - skeleton.Joints[JointType.FootRight].Position.Z), 2));

                        handleft_handright10 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HandLeft].Position.X - skeleton.Joints[JointType.HandRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Y - skeleton.Joints[JointType.HandRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Z - skeleton.Joints[JointType.HandRight].Position.Z), 2));

                        handleft_head11 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HandLeft].Position.X - skeleton.Joints[JointType.Head].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Y - skeleton.Joints[JointType.Head].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Z - skeleton.Joints[JointType.Head].Position.Z), 2));

                        handright_head12 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.Head].Position.X - skeleton.Joints[JointType.HandRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.Head].Position.Y - skeleton.Joints[JointType.HandRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.Head].Position.Z - skeleton.Joints[JointType.HandRight].Position.Z), 2));

                        label21.Text = hipcenter_handleft1.ToString();
                        label22.Text = hipright_handright2.ToString();
                        label23.Text = handright_kneeright3.ToString();
                        label24.Text = handleft_kneeleft4.ToString();
                        label25.Text = elbowleft_hipleft5.ToString();
                        label26.Text = elbowright_hipright6.ToString();
                        label27.Text = footleft_footright7.ToString();
                        label28.Text = handleft_footleft8.ToString();
                        label29.Text = handright_footright9.ToString();
                        label30.Text = handleft_handright10.ToString();
                        label31.Text = handleft_head11.ToString();
                        label32.Text = handright_head12.ToString();
                        break;
                    }
                }
            }
        }

        private void DrawSkeletons(Image<Bgr, Byte> img, int depthOrColor)
        {
            //绘制所有正确Tracked的骨骼

            foreach (Skeleton skeleton in this.skeletonData)
            {
                if (skeleton == null) continue;
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    DrawTrackedSkeletonJoints(img, skeleton.Joints, depthOrColor);
                }


            }

        }

        private void DrawTrackedSkeletonJoints(Image<Bgr, Byte> img, JointCollection jointCollection, int depthOrColor)
        {
            // Render Head and Shoulders
            DrawBone(img, jointCollection[JointType.Head], jointCollection[JointType.ShoulderCenter], depthOrColor);
            DrawBone(img, jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderRight], depthOrColor);

            // Render Left Arm
            DrawBone(img, jointCollection[JointType.ShoulderLeft], jointCollection[JointType.ElbowLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.ElbowLeft], jointCollection[JointType.WristLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.WristLeft], jointCollection[JointType.HandLeft], depthOrColor);

            // Render Right Arm
            DrawBone(img, jointCollection[JointType.ShoulderRight], jointCollection[JointType.ElbowRight], depthOrColor);
            DrawBone(img, jointCollection[JointType.ElbowRight], jointCollection[JointType.WristRight], depthOrColor);
            DrawBone(img, jointCollection[JointType.WristRight], jointCollection[JointType.HandRight], depthOrColor);

            // Render other bones...
            DrawBone(img, jointCollection[JointType.ShoulderCenter], jointCollection[JointType.Spine], depthOrColor);

            DrawBone(img, jointCollection[JointType.Spine], jointCollection[JointType.HipRight], depthOrColor);
            DrawBone(img, jointCollection[JointType.KneeRight], jointCollection[JointType.HipRight], depthOrColor);
            DrawBone(img, jointCollection[JointType.KneeRight], jointCollection[JointType.AnkleRight], depthOrColor);
            DrawBone(img, jointCollection[JointType.FootRight], jointCollection[JointType.AnkleRight], depthOrColor);

            DrawBone(img, jointCollection[JointType.Spine], jointCollection[JointType.HipLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.KneeLeft], jointCollection[JointType.HipLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.KneeLeft], jointCollection[JointType.AnkleLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.FootLeft], jointCollection[JointType.AnkleLeft], depthOrColor);
        }

        private void DrawBone(Image<Bgr, Byte> img, Joint jointFrom, Joint jointTo, int depthOrColor)
        {
            if (jointFrom.TrackingState == JointTrackingState.NotTracked ||
            jointTo.TrackingState == JointTrackingState.NotTracked)
            {
                return; // nothing to draw, one of the joints is not tracked
            }

            if (jointFrom.TrackingState == JointTrackingState.Inferred ||
            jointTo.TrackingState == JointTrackingState.Inferred)
            {
                DrawBoneLine(img, jointFrom.Position, jointTo.Position, 1, depthOrColor);
            }

            if (jointFrom.TrackingState == JointTrackingState.Tracked &&
            jointTo.TrackingState == JointTrackingState.Tracked)
            {
                DrawBoneLine(img, jointFrom.Position, jointTo.Position, 3, depthOrColor);
            }
        }

        private void DrawBoneLine(Image<Bgr, Byte> img, SkeletonPoint p1, SkeletonPoint p2, int lineWidth, int depthOrColor)
        {
            System.Drawing.Point p_1, p_2;

            //depthOrColor = 0;
            if (depthOrColor == 0)
            {
                p_1 = SkeletonPointToDepthScreen(p1);
                p_2 = SkeletonPointToDepthScreen(p2);
            }
            else
            {
                p_1 = SkeletonPointToColorScreen(p1);
                p_2 = SkeletonPointToColorScreen(p2);
            }

            img.Draw(new LineSegment2D(p_1, p_2), new Bgr(255, 255, 0), lineWidth);
            img.Draw(new CircleF(p_1,5), new Bgr(0, 0, 255), -1);

            StringBuilder str = new StringBuilder();
            str.AppendFormat("({0},{1},{2})", p1.X.ToString("0.0"), p1.Y.ToString("0.0"), p1.Z.ToString("0.0"));

            img.Draw(str.ToString(), ref font, p_1, new Bgr(0, 255, 0));
            img.Draw(new CircleF(p_2, 5), new Bgr(0, 0, 255), -1);

            str.Clear();
            str.AppendFormat("({0},{1},{2})", p2.X.ToString("0.0"), p2.Y.ToString("0.0"), p2.Z.ToString("0.0"));
            img.Draw(str.ToString(), ref font, p_2, new Bgr(0, 255, 0));
        }
                
        private void skinButton2_Click(object sender, EventArgs e)
        {
            // 主要是数据采集以及数据传入 ,核心


            foreach (Skeleton skeleton in this.skeletonData)
            {
                if (skeleton == null) continue;
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {

                    using (SqlConnection conn = new SqlConnection(connsql))
                    {
                        conn.Open();//打开数据库

                        SqlCommand cmd = conn.CreateCommand();
                        
                        string lable = null;
                        hipcenter_handleft1 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HipCenter].Position.X - skeleton.Joints[JointType.HandLeft].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HipCenter].Position.Y - skeleton.Joints[JointType.HandLeft].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HipCenter].Position.Z - skeleton.Joints[JointType.HandLeft].Position.Z), 2));

                        hipright_handright2 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HipRight].Position.X - skeleton.Joints[JointType.HandRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HipRight].Position.Y - skeleton.Joints[JointType.HandRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HipRight].Position.Z - skeleton.Joints[JointType.HandRight].Position.Z), 2));

                        handright_kneeright3 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HandRight].Position.X - skeleton.Joints[JointType.KneeRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandRight].Position.Y - skeleton.Joints[JointType.KneeRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandRight].Position.Z - skeleton.Joints[JointType.KneeRight].Position.Z), 2));

                        handleft_kneeleft4 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HandLeft].Position.X - skeleton.Joints[JointType.KneeLeft].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Y - skeleton.Joints[JointType.KneeLeft].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Z - skeleton.Joints[JointType.KneeLeft].Position.Z), 2));

                        elbowleft_hipleft5 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.ElbowLeft].Position.X - skeleton.Joints[JointType.HipLeft].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowLeft].Position.Y - skeleton.Joints[JointType.HipLeft].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowLeft].Position.Z - skeleton.Joints[JointType.HipLeft].Position.Z), 2));

                        elbowright_hipright6 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.ElbowRight].Position.X - skeleton.Joints[JointType.HipRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowRight].Position.Y - skeleton.Joints[JointType.HipRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowRight].Position.Z - skeleton.Joints[JointType.HipRight].Position.Z), 2));

                        footleft_footright7 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.FootLeft].Position.X - skeleton.Joints[JointType.FootRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.FootLeft].Position.Y - skeleton.Joints[JointType.FootRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.FootLeft].Position.Z - skeleton.Joints[JointType.FootRight].Position.Z), 2));

                        handleft_footleft8 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.FootLeft].Position.X - skeleton.Joints[JointType.HandLeft].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.FootLeft].Position.Y - skeleton.Joints[JointType.HandLeft].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.FootLeft].Position.Z - skeleton.Joints[JointType.HandLeft].Position.Z), 2));

                        handright_footright9 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HandRight].Position.X - skeleton.Joints[JointType.FootRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandRight].Position.Y - skeleton.Joints[JointType.FootRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandRight].Position.Z - skeleton.Joints[JointType.FootRight].Position.Z), 2));

                        handleft_handright10 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HandLeft].Position.X - skeleton.Joints[JointType.HandRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Y - skeleton.Joints[JointType.HandRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Z - skeleton.Joints[JointType.HandRight].Position.Z), 2));

                        handleft_head11 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.HandLeft].Position.X - skeleton.Joints[JointType.Head].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Y - skeleton.Joints[JointType.Head].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.HandLeft].Position.Z - skeleton.Joints[JointType.Head].Position.Z), 2));

                        handright_head12 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.Head].Position.X - skeleton.Joints[JointType.HandRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.Head].Position.Y - skeleton.Joints[JointType.HandRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.Head].Position.Z - skeleton.Joints[JointType.HandRight].Position.Z), 2));

                        elbowright_kneeleft13 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.ElbowRight].Position.X - skeleton.Joints[JointType.KneeLeft].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowRight].Position.Y - skeleton.Joints[JointType.KneeLeft].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowRight].Position.Z - skeleton.Joints[JointType.KneeLeft].Position.Z), 2));

                        elbowleft_kneeright14 = 1000 * Math.Sqrt(Math.Pow((skeleton.Joints[JointType.ElbowLeft].Position.X - skeleton.Joints[JointType.KneeRight].Position.X), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowLeft].Position.Y - skeleton.Joints[JointType.KneeRight].Position.Y), 2) +
                        Math.Pow((skeleton.Joints[JointType.ElbowLeft].Position.Z - skeleton.Joints[JointType.KneeRight].Position.Z), 2));

                        lable = textBox1.Text;

                        string All = "insert into distance3 values(" + hipcenter_handleft1 + "," + hipright_handright2 + "," + handright_kneeright3
                            + "," + handleft_kneeleft4 + "," + elbowleft_hipleft5 + "," + elbowright_hipright6 + "," + footleft_footright7
                            + "," + handleft_footleft8 + "," + handright_footright9 + "," + handleft_handright10 + "," + handleft_head11
                            + "," + handright_head12 + "," + elbowright_kneeleft13 + "," + elbowleft_kneeright14 +","+ lable + ")";

                        string cons = "F:\\kinect\\Kinect\\neural_network\\application.py " + hipcenter_handleft1 + "," + hipright_handright2 + "," + handright_kneeright3
                            + "," + handleft_kneeleft4 + "," + elbowleft_hipleft5 + "," + elbowright_hipright6 + "," + footleft_footright7
                            + "," + handleft_footleft8 + "," + handright_footright9 + "," + handleft_handright10 + "," + handleft_head11
                            + "," + handright_head12 + "," + elbowright_kneeleft13 + "," + elbowleft_kneeright14;

                        //Console.WriteLine(cons);
                        //cmd.CommandText = All;
                        //cmd.ExecuteNonQuery();



                        Predicate(cons);// 动作预测.
                    }
                }

            }
        }

        private System.Drawing.Point SkeletonPointToColorScreen(SkeletonPoint skelpoint)
        {
            ColorImagePoint colorPoint = this.sensor.CoordinateMapper.MapSkeletonPointToColorPoint(skelpoint, colorImageFormat);
            return new System.Drawing.Point(colorPoint.X, colorPoint.Y);
        }

        private System.Drawing.Point SkeletonPointToDepthScreen(SkeletonPoint skelpoint)
        {
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, depthImageFormat);
            return new System.Drawing.Point(depthPoint.X, depthPoint.Y);
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            //  使用多线程，防止按钮卡顿。
            click_times++;
            Thread thread1 = new Thread(TrainNetwork1);
            Thread thread2 = new Thread(TrainNetwork2);
            Thread thread3 = new Thread(ProcessBar);
            Control.CheckForIllegalCrossThreadCalls = false;
            if (click_times % 2 == 0)//这个会造成读写冲突，因此复制一份,交叉执行。
            {

                thread1.Start();
                thread2.Abort();
                thread3.Start();
            }
            else
            {
                thread2.Start();
                thread1.Abort();
                thread3.Start();
            }


        }

        private void TrainNetwork2()
        {
            string strInput = "F:\\kinect\\Kinect\\neural_network\\backup\\23Classification.py";

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(strInput + "&exit");
            p.StandardInput.AutoFlush = true;
            string strOuput = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();

            skinPictureBox1.Load(@"F:\\kinect\\Kinect\\neural_network\\backup\\loss.png");  // 注意路径
            skinPictureBox2.Load(@"F:\\kinect\\Kinect\\neural_network\\backup\\acc.png");  // 注意路径
        }

        private void TrainNetwork1()
        {

            string strInput = "F:\\kinect\\Kinect\\neural_network\\23Classification.py";  // 注意路径

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(strInput + "&exit");
            p.StandardInput.AutoFlush = true;
            string strOuput = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();

            skinPictureBox1.Load(@"F:\\kinect\\Kinect\\neural_network\\train_loss_acc_pic\\loss.png");  // 注意路径
            skinPictureBox2.Load(@"F:\\kinect\\Kinect\\neural_network\\train_loss_acc_pic\\acc.png");  // 注意路径
        }

        private void Predicate(string cmdd)
        {
            string strInput = cmdd;
            int post = 0;
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(strInput + "&exit");
            p.StandardInput.AutoFlush = true;
            string strOuput = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            
            
            post = int.Parse(strOuput.Split('\n')[strOuput.Split('\n').Length - 2]); //对python执行结果进行提取.
            label3.Text = pos[post-1];

        }

        private void ProcessBar()
        {
            while (true)
            {
                if (toolStripProgressBar1.Value < 100)
                {
                    toolStripProgressBar1.Value += 4;
                    Thread.Sleep(500);
                }
                else
                {
                    break;
                }
            }
            Thread.Sleep(4000);
            toolStripProgressBar1.Value = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {
            
        }

    }
}


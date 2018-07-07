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



namespace 骨骼坐标点的获取入库
{

    public partial class Form1 : Form
    {
        private const string V = "F:\\kinect\\12.jpg";
        private String connsql = "server=.;database=bone_pos;integrated security=SSPI";

        private KinectSensor sensor;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Load(V);
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }//连接设备;
            if (null != this.sensor)
            {
                this.sensor.SkeletonStream.Enable();//设备骨骼可用;
                this.sensor.SkeletonFrameReady += SensorSkeletonFrameReady;//事件处理;
                try
                {
                    this.sensor.Start();
                }
                catch
                {
                    ;
                }
            }
            else
            {
                MessageBox.Show("设备未就绪！", "未连接");
            }

        }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                skeletonFrame.CopySkeletonDataTo(skeletons);

            }


        }

        private void DatabaseOp_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connsql))
            {
                conn.Open();//打开数据库

                SqlCommand cmd = conn.CreateCommand();
                //创建查询语句
                cmd.CommandText = "SELECT * FROM pos1";
                //从数据库中读取数据流存入reader中
                SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine(conn.State);
                while (reader.Read())
                {
                    string name = reader.GetString(reader.GetOrdinal("x1"));
                    //int age = reader.GetInt32(reader.GetOrdinal("age"));
                    Console.WriteLine(name);
                }
                reader.Close();//报bug，必须要关掉才可以执行。
                String insert = "insert into pos1 values(5,3,6)";
                cmd.CommandText = insert;
                cmd.ExecuteNonQuery();

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

